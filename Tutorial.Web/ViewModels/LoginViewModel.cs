using System.ComponentModel.DataAnnotations;

namespace Tutorial.Web.ViewModels
{
    public class LoginViewModel
    {
        [Required,Display(Name = "用户名")]
        public string UserName { get; set; }
        [Required,DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
