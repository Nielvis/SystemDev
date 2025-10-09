using DOM.Presentation.Entities.test_db;
using DOM.Presentation.Implementation.Interfaces;
using DOM.Presentation.Models.Entities.Test;
using DOM.Presentation.Models.Entities.Text;
using DOM.Presentation.Models.Entities.User;
using Microsoft.AspNetCore.Mvc;


namespace DOM.Presentation.Controllers.Api
{
    [Route("v1/api/user")]
    [ApiController]
    public class UserApiController : ControllerBase
    {
        private readonly ILogger<UserApiController> _logger;

        private readonly IDbService _dbService;

        public UserApiController(
                IDbService dbService
,
                ILogger<UserApiController> logger)
        {
            _logger = logger;
            _dbService = dbService;
        }

        [HttpGet]
        public ActionResult Get()
        {
            var Response = _dbService.Select<Entities.test_db.User>("SELECT * FROM test_db.dbo.[User]");

            return Ok(Response);
        }

        [HttpGet("{uid}")]
        public ActionResult GetByUid(string uid)
        {
            var Response = _dbService.Select<Entities.test_db.User>($"SELECT * FROM test_db.dbo.[User] WHERE Uid = '{uid}'").FirstOrDefault();

            return Ok(Response);
        }

        [HttpPost("")]
        public ActionResult Post([FromBody] Models.Entities.User.UserInput userInput)
        {
            var Response = _dbService.Execute($"INSERT test_db.dbo.[User] (Uid, Name, Email, Password, Gender, Birthdate) VALUES (NEWID(),'{userInput.Name}','{userInput.Email}','{userInput.Password}', '{userInput.Gender}', '{userInput.BirthDate}')");

            if (Response > 0)
                return Ok();

            return BadRequest();
        }

        [HttpPut("{uid}")]
        public ActionResult Put(string uid, [FromBody] Models.Entities.User.UserInput userInput)
        {
            var Response = _dbService.Execute($"UPDATE test_db.dbo.[User] SET Name = '{userInput.Name}', Email = '{userInput.Email}', Password = '{userInput.Password}', Gender = '{userInput.Gender}', BirthDate = '{userInput.BirthDate}'  WHERE Uid = '{uid}'");

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
