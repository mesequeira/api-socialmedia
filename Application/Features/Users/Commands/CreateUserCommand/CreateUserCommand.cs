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

namespace Application.Features.Users.Commands.CreateUserCommand
{
    public class DeleteUserCommand : IRequest<Response<int>>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }

    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Response<int>>
    {
        private readonly IRepositoryAsync<User> _repositoryAsync;

        private readonly IMapper _mapper;

        public DeleteUserCommandHandler(IRepositoryAsync<User> repositoryAsync, IMapper mapper)
        {
            _repositoryAsync = repositoryAsync;
            _mapper = mapper;
        }
        public async Task<Response<int>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<User>(request);
            var data = await _repositoryAsync.AddAsync(user);

            return new Response<int>(data.Id);
        }
    }
}
