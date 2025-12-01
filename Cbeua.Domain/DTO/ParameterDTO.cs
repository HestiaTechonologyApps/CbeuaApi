using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Domain.DTO
{
  
    public class LookUpDTO
    {
        public int Id { get; set; }
        public string Text { get; set; } = "";
        public string Code { get; set; } = "";
        public bool IsSelected { get; set; } = false;
    }

    public class PaginatedResult<T>
    {
        public int Total { get; set; }
        public IEnumerable<T> Data { get; set; }
    }
}
