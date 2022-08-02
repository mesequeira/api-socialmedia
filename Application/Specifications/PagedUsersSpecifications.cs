using Ardalis.Specification;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Specifications
{
    public class PagedUsersSpecifications : Specification<User>
    {
        public PagedUsersSpecifications(int pageSize, int pageNumber, string username)
        {
            Query.Skip(pageNumber * pageSize)
                .Take(pageSize);

            if (!string.IsNullOrEmpty(username))
                Query.Search(m => m.UserName, $"%{username}%");
        }
    }
}
