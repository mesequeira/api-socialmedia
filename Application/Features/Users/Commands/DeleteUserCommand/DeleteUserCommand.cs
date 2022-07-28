using Application.Exceptions;
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

namespace Application.Features.Users.Commands.DeleteUserCommand
{
    public class DeleteUserCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
    }

    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Response<int>>
    {
        private readonly IRepositoryAsync<User> _repositoryAsync;

        public DeleteUserCommandHandler(IRepositoryAsync<User> repositoryAsync)
        {
            _repositoryAsync = repositoryAsync;
        }
        public async Task<Response<int>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _repositoryAsync.GetByIdAsync(request.Id);

            if (user == null)
                throw new KeyNotFoundException($"The user with the id {request.Id} could not be found.");

            await _repositoryAsync.DeleteAsync(user);

            return new Response<int>(user.Id);
        }
    }
}
