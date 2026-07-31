using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.DTOs.Common
{
    public class QueryRequest
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 5;
        public string? Search {  get; set; }
        public string SortBy { get; set; } = "Id";
        public bool Descending { get; set; } = true;

    }
}
