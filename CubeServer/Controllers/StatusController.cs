namespace CubeServer.Controllers
{
    using System.Web.Http;
    using System.Reflection;


    public class StatusController : ApiController
    {
        
        [Route("status")]
        public string Get()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
    }
}
