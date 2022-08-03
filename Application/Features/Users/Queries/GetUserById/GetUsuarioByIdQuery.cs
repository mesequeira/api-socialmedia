using Application.DTOs.Users;
using Application.Interfaces;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Users.Queries.GetUserById
{
    public class GetUsuarioByIdQuery : IRequest<Response<UserDto>>
    {
        public int Id { get; set; }
        public class GetUsuarioByIdQueryQueryHandler : IRequestHandler<GetUsuarioByIdQuery, Response<UserDto>>
        {
            private readonly IRepositoryAsync<User> _repositoryAsync;
            private readonly IMapper _mapper;

            public GetUsuarioByIdQueryQueryHandler(IRepositoryAsync<User> repositoryAsync, IMapper mapper)
            {
                _repositoryAsync = repositoryAsync;
                _mapper = mapper;
            }

            public async Task<Response<UserDto>> Handle(GetUsuarioByIdQuery request, CancellationToken cancellationToken)
            {
                var user = await _repositoryAsync.GetByIdAsync(request.Id);                

                if (user == null)
                    throw new KeyNotFoundException($"The user with the id {request.Id} could not be found.");

                var userDto = _mapper.Map<UserDto>(user);

                return new Response<UserDto>(userDto);
            }
        }
    }
}
