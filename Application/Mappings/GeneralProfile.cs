using Application.DTOs.Users;
using Application.Features.Users.Commands.CreateUserCommand;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            #region Commands
            CreateMap<CreateUserCommand, User>();
            #endregion

            #region DTOs
            CreateMap<User, UserDto>();
            #endregion
        }
    }
}
