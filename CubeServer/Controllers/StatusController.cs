namespace CubeServer.Controllers
{
    using System.Web.Http;
    using System.Reflection;

    public class StatusController : ApiController
    {
        
        [Route("status")]
        public IHttpActionResult Get()
        {
            return Ok(Assembly.GetExecutingAssembly().GetName().Version.ToString());
        }
    }
}
