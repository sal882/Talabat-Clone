using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Talabat.Apis.Controllers
{

    /// <summary>
    /// this is Base Controller for all contrller that will inherit from it
    /// and have common things brtween thos controllers like Route and ApiController Data annotation
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
    }
}
