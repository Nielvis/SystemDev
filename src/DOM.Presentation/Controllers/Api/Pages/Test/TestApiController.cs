using DOM.Presentation.Entities.test_db;
using DOM.Presentation.Implementation.Interfaces;
using DOM.Presentation.Models.Entities.Test;
using Microsoft.AspNetCore.Mvc;

namespace DOM.Presentation.Controllers.Api
{
    [Route("v1/api/test")]
    [ApiController]
    public class TestApiController : ControllerBase
    {
        private readonly ILogger<TestApiController> _logger;

        private readonly IDbService _dbService;

        public TestApiController(
                ILogger<TestApiController> logger,
                IDbService dbService
            ) 
        {
            _logger = logger;
            _dbService = dbService;
        }

        [HttpGet]
        public ActionResult Get()
        {
            var Response = _dbService.Select<Test>("SELECT * FROM test_db.dbo.[Test]");

            return Ok(Response);
        }

        [HttpGet("{uid}")]
        public ActionResult GetByUid(string uid)
        {
            var Response = _dbService.Select<Test>($"SELECT * FROM test_db.dbo.[Test] WHERE Uid = '{uid}'").FirstOrDefault();

            return Ok(Response);
        }

        [HttpPost("")]
        public ActionResult Post([FromBody] TestInput testInput)
        {
            var Response = _dbService.Execute($"INSERT test_db.dbo.[Test] (Uid,Name,Email) VALUES (NEWID(),'{testInput.Name}','{testInput.Email}')");

            if (Response > 0)
                return Ok();

            return BadRequest();
        }

        [HttpPut("{uid}")]
        public ActionResult Put(string uid, [FromBody] TestInput testInput)
        {
            var Response = _dbService.Execute($"UPDATE test_db.dbo.[Test] SET Name = '{testInput.Name}', Email = '{testInput.Email}' WHERE Uid = '{uid}'");

            if (Response > 0)
                return Ok();

            return BadRequest();
        }

        [HttpDelete("{uid}")]
        public ActionResult Delete(string uid)
        {
            var Response = _dbService.Execute($"DELETE FROM test_db.dbo.[Test] WHERE [Uid] = '{uid}'");

            if(Response > 0)
                return Ok();

            return BadRequest();
        }
    }
}
