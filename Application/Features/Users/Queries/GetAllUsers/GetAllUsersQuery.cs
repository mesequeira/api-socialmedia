using Application.DTOs.Users;
using Application.Interfaces;
using Application.Specifications;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Users.Queries.GetAllUsers
{
    public class GetAllUsersQuery : IRequest<PagedResponse<List<UserDto>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string UserName { get; set; }

        public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, PagedResponse<List<UserDto>>>
        {
            private readonly IRepositoryAsync<User> _repositoryAsync;

            private readonly IMapper _mapper;

            private readonly IDistributedCache _distributedCache;

            public GetAllUsersQueryHandler(IRepositoryAsync<User> repositoryAsync, IMapper mapper, IDistributedCache distributedCache)
            {
                _repositoryAsync = repositoryAsync;
                _mapper = mapper;
                _distributedCache = distributedCache;
            }

            // Saving the data in cache for 10 minutes to not overload the database with queries
            public async Task<PagedResponse<List<UserDto>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
            {

                var cacheKey = $"usersList_{request.PageSize}_{request.PageNumber}";

                var serializedListUsers = string.Empty;

                var users = new List<User>();

                var redisListUsers = await _distributedCache.GetAsync(cacheKey);

                if (redisListUsers != null)
                {
                    serializedListUsers = Encoding.UTF8.GetString(redisListUsers);
                    users = JsonConvert.DeserializeObject<List<User>>(serializedListUsers);
                }
                else
                {
                    users = await _repositoryAsync.ListAsync(new PagedUsersSpecifications(request.PageSize, request.PageNumber, request.UserName), cancellationToken);
                    serializedListUsers = JsonConvert.SerializeObject(users);
                    redisListUsers = Encoding.UTF8.GetBytes(serializedListUsers);

                    var options = new DistributedCacheEntryOptions()
                                .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                                .SetSlidingExpiration(TimeSpan.FromMinutes(2));

                    await _distributedCache.SetAsync(cacheKey, redisListUsers, options, cancellationToken);
                }

                var usersDto = _mapper.Map<List<UserDto>>(users);

                return new PagedResponse<List<UserDto>>(usersDto, request.PageNumber, request.PageSize);
            }
        }
    }
}
