using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tutorial.Web.Model;
using Tutorial.Web.Services;

namespace Tutorial.Web.Views.ViewComponents
{
    public class WelcomeViewComponent:ViewComponent
    {
        private readonly IRepository<Student> _repository;

        public WelcomeViewComponent(IRepository<Student> repository)
        {
            _repository = repository;
        }

        public IViewComponentResult Invoke()
        {
            var count =  _repository.GetAll().Count().ToString();
            // 如果是字符串那么 View 就会把它当成字符串去寻找这个View
            // 那么就要换一种写法
            return View("Default",count);
        }
    }
}
