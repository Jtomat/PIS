using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PIS_Project.Models.DataControllers
{
    public class ValidationResponse
    {
        public bool Result { get; set; }
        public object ValidData { get; set; }
        public string Information { get; set; }
    }
}