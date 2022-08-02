using Application.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Users.Queries.GetAllUsers
{
    internal class GetAllUsersParameters : RequestParameter
    {
        public string UserName { get; set; }
    }
}
