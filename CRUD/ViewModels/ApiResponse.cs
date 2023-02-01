using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRUD.ViewModels
{
    public class ApiResponse
    {
        public bool IsSuccess { get; set; }
        public dynamic ResponseData { get; set; }
        public string Message { get; set; }
        public int? TotalRecord { get; set; }
        public int? RecordFilter { get; set; }

    }
}