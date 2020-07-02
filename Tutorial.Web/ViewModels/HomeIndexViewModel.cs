using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Tutorial.Web.Model;

namespace Tutorial.Web.ViewModels
{
    public class HomeIndexViewModel
    {
        public IEnumerable<StudentViewModel> Students { get; set; }
    }
}
