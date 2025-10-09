    using DOM.Presentation.Entities.test_db;
using DOM.Presentation.Implementation.Interfaces;
using DOM.Presentation.Models.Entities.Test;
using DOM.Presentation.Models.Entities.Text;
using DOM.Presentation.Models.Entities.Product;
using Microsoft.AspNetCore.Mvc;

namespace DOM.Presentation.Controllers.Api
{
    [Route("v1/api/product")]
    [ApiController]
    public class ProductApiController : ControllerBase
    {
        private readonly ILogger<ProductApiController> _logger;

        private readonly IDbService _dbService;

        public ProductApiController(
                ILogger<ProductApiController> logger,
                IDbService dbService
            ) 
        {
            _logger = logger;
            _dbService = dbService;
        }

        [HttpGet]
        public ActionResult Get()
        {
            var Response = _dbService.Select<Product>("SELECT * FROM test_db.dbo.[Product]");

            return Ok(Response);
        }

        [HttpGet("{uid}")]
        public ActionResult GetByUid(string uid)
        {
            var Response = _dbService.Select<Product>($"SELECT * FROM test_db.dbo.[Product] WHERE Uid = '{uid}'").FirstOrDefault();

            return Ok(Response);
        }

        [HttpPost("")]
        public ActionResult Post([FromBody] ProductInput productInput)
        {
            var Response = _dbService.Execute($"INSERT test_db.dbo.[Product] (Uid,Name,Description, UrlImage, Price) VALUES (NEWID(),'{productInput.Name}','{productInput.Description}','{productInput.UrlImage}','{productInput.Price}')");

            if (Response > 0)
                return Ok();

            return BadRequest();
        }

        [HttpPut("{uid}")]
        public ActionResult Put(string uid, [FromBody] ProductInput productInput)
        {
            var Response = _dbService.Execute($"UPDATE test_db.dbo.[Product] SET Name = '{productInput.Name}', Description = '{productInput.Description}', UrlImage = '{productInput.UrlImage}', Price = '{productInput.Price}'  WHERE Uid = '{uid}'");

            if (Response > 0)
                return Ok();

            return BadRequest();
        }

        [HttpDelete("{uid}")]
        public ActionResult Delete(string uid)
        {
            var Response = _dbService.Execute($"DELETE FROM test_db.dbo.[Product] WHERE [Uid] = '{uid}'");

            if(Response > 0)
                return Ok();

            return BadRequest();
        }
    }
}
