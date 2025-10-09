using DOM.Presentation.Entities.test_db;
using DOM.Presentation.Implementation.Interfaces;
using DOM.Presentation.Models.Entities.Post;
using Microsoft.AspNetCore.Mvc;

namespace DOM.Presentation.Controllers.Api
{
    [Route("v1/api/post")]
    [ApiController]
    public class PostApiController : ControllerBase
    {
        private readonly ILogger<PostApiController> _logger;

        private readonly IDbService _dbService;

        public PostApiController(
                ILogger<PostApiController> logger,
                IDbService dbService
            )
        {
            _logger = logger;
            _dbService = dbService;
        }

        [HttpGet]
        public ActionResult Get()
        {
            var Response = _dbService.Select<Post>("SELECT * FROM test_db.dbo.[Post]");

            return Ok(Response);
        }

        [HttpGet("{uid}")]
        public ActionResult GetByUid(string uid)
        {
            var Response = _dbService.Select<Text>($"SELECT * FROM test_db.dbo.[Post] WHERE Uid = '{uid}'").FirstOrDefault();

            return Ok(Response);
        }

        [HttpPost("")]
        public ActionResult Post([FromBody] PostInput postInput)
        {
            var Response = _dbService.Execute($"INSERT test_db.dbo.[Post] (Uid,Name,Description, UrlImage) VALUES (NEWID(),'{postInput.Name}','{postInput.Description}','{postInput.UrlImage}')");

            if (Response > 0)
                return Ok();

            return BadRequest();
        }

        [HttpPut("{uid}")]
        public ActionResult Put(string uid, [FromBody] PostInput postInput)
        {
            var Response = _dbService.Execute($"UPDATE test_db.dbo.[Post] SET Name = '{postInput.Name}', Description = '{postInput.Description}', UrlImage = '{postInput.UrlImage}'  WHERE Uid = '{uid}'");

            if (Response > 0)
                return Ok();

            return BadRequest();
        }

        [HttpDelete("{uid}")]
        public ActionResult Delete(string uid)
        {
            var Response = _dbService.Execute($"DELETE FROM test_db.dbo.[Post] WHERE [Uid] = '{uid}'");

            if (Response > 0)
                return Ok();

            return BadRequest();
        }
    }
}