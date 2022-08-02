using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Parameters
{
    public class RequestParameter
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public RequestParameter()
        {
            this.PageNumber = 1;
            this.PageSize = 10;
        }

        public RequestParameter(int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber < 1 ? 1 : PageNumber;
            this.PageSize = pageSize > 10 ? 10 : PageSize;    
        }
    }
}
