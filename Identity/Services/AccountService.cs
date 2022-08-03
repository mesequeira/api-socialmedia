using Application.DTOs.Jwt;
using Application.Enums;
using Application.Exceptions;
using Application.Interfaces;
using Application.Wrappers;
using Domain.Settings;
using Identity.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly JWTSettings _jwtSettings;
        private readonly IDateTimeService _dateTimeService;

        public AccountService(IDateTimeService dateTimeService, IOptions<JWTSettings> jwtSettings, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _dateTimeService = dateTimeService;
            _jwtSettings = jwtSettings.Value;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request, string ipAddress)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if(user == null)
                throw new ApiException($"The username or email does not exist.");

            var result = await _signInManager.PasswordSignInAsync(request.UserName, request.Password, false, false);

            if (!result.Succeeded)
                throw new ApiException($"Invalid credentials.");

            var jwtSecurityToken = await GenerateJwtToken(user);

            var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);

            var refreshToken = GenerateRefreshToken(ipAddress);

            var response = new AuthenticationResponse()
            {
                Id = user.Id,
                JwtToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Email = user.Email,
                UserName = user.UserName,
                Roles = rolesList.ToList(),
                IsVerified = user.EmailConfirmed,
                RefreshToken = refreshToken.ToString()
            };

            return new Response<AuthenticationResponse>(response, "User authenticated succesfully.");
        }

        public async Task<Response<string>> RegisterAsync(RegisterRequest request, string origin)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);

            if (user != null)
                throw new ApiException($"The user with the username {request.UserName} already exists.");

            user = await _userManager.FindByEmailAsync(request.Email);

            if (user != null)
                throw new ApiException($"The email {request.Email} already exists.");

            user = new IdentityUser()
            {
                UserName = request.UserName,
                Email = request.Email,
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if(!result.Succeeded)
                throw new ApiException($"An error ocurred: {result.Errors}");

            await _userManager.AddToRoleAsync(user, Roles.Basic.ToString());
            return new Response<string>(user.Id, message: $"The user {request.UserName} was created succesfully.");


        }

        private async Task<JwtSecurityToken> GenerateJwtToken(IdentityUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var roleClaims = new List<Claim>();

            foreach(var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var ipAddres = IpHelper.GetIpAddress();

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id),
                new Claim("ip", ipAddres)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));

            var signInCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,    
                claims: claims,
                signingCredentials: signInCredentials
            );

            return jwtSecurityToken;



        }

        private RefreshToken GenerateRefreshToken(string ipAddress)
        {
            return new RefreshToken
            {
                Token = RandomTokenString(),
                Expires = DateTime.Now.AddDays(7),
                Created = DateTime.Now,
                CreatedByIp = ipAddress
            };

        }

        private string RandomTokenString()
        {
            var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }
    }
}
