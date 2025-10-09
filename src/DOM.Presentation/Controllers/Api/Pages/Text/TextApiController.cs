using DOM.Presentation.Entities.test_db;
using DOM.Presentation.Implementation.Interfaces;
using DOM.Presentation.Models.Entities.Text;
using Microsoft.AspNetCore.Mvc;

namespace DOM.Presentation.Controllers.Api
{
    [Route("v1/api/text")]
    [ApiController]
    public class TextApiController : ControllerBase
    {
        private readonly ILogger<TextApiController> _logger;

        private readonly IDbService _dbService;

        public TextApiController(
                ILogger<TextApiController> logger,
                IDbService dbService
            ) 
        {
            _logger = logger;
            _dbService = dbService;
        }

        [HttpGet]
        public ActionResult Get()
        {
            var Response = _dbService.Select<Text>("SELECT * FROM test_db.dbo.[Text]");

            return Ok(Response);
        }

        [HttpGet("{uid}")]
        public ActionResult GetByUid(string uid)
        {
            var Response = _dbService.Select<Text>($"SELECT * FROM test_db.dbo.[Text] WHERE Uid = '{uid}'").FirstOrDefault();

            return Ok(Response);
        }

        [HttpPost("")]
        public ActionResult Post([FromBody] TextInput textInput)
        {
            var Response = _dbService.Execute($"INSERT test_db.dbo.[Text] (Uid,Name,Description, UrlImage) VALUES (NEWID(),'{textInput.Name}','{textInput.Description}','{textInput.UrlImage}')");

            if (Response > 0)
                return Ok();

            return BadRequest();
        }

        [HttpPut("{uid}")]
        public ActionResult Put(string uid, [FromBody] TextInput textInput)
        {
            var Response = _dbService.Execute($"UPDATE test_db.dbo.[Text] SET Name = '{textInput.Name}', Description = '{textInput.Description}', UrlImage = '{textInput.UrlImage}'  WHERE Uid = '{uid}'");

            if (Response > 0)
                return Ok();

            return BadRequest();
        }

        [HttpDelete("{uid}")]
        public ActionResult Delete(string uid)
        {
            var Response = _dbService.Execute($"DELETE FROM test_db.dbo.[Text] WHERE [Uid] = '{uid}'");

            if(Response > 0)
                return Ok();

            return BadRequest();
        }
    }
}
