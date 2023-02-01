using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRUD.ViewModels
{
    public class StudentVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PhoneNo { get; set; }
        public int? StateId { get; set; }
        public int? CityId { get; set; }
        
    }
}