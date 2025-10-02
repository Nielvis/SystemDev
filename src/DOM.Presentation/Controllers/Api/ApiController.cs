using Microsoft.AspNetCore.Mvc;

namespace DOM.Presentation.Controllers.Api
{
    [Route("v1/api")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly ILogger<ApiController> _logger;

        public ApiController(
                ILogger<ApiController> logger
            ) 
        {
            _logger = logger;
        }
    }
}
