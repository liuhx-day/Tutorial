using Microsoft.AspNetCore.Mvc;

namespace Tutorial.Web.Controllers
{
    // /About
    [Route("[controller]")]
    public class AboutController
    {
        // /About/Me
        [Route("")]
        public string Me()
        {
            return "LIU";
        }
        [Route("Company")]
        public string Company()
        {
            return "No Company";
        }
    }
}
