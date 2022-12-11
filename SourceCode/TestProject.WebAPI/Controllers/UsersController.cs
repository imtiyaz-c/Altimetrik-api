using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestProject.WebAPI.DataModels;
using TestProject.WebAPI.Repositories;

namespace TestProject.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _repository;

        public UsersController(IUserRepository context)
        {
            _repository = context;
        }

        // GET: api/Users/GetUsers
        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            return Ok(await _repository.GetAllUsersAsync());
        }

        // POST: api/Users/CreateUser
        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUserAsync([FromBody] User user)
        {
            if (ModelState.IsValid)
            {
                if (await _repository.Exist(user.Email)) return BadRequest("UserName already taken");
                if (user.Salary < 1000 || user.MonthlyExpense < 1000) return BadRequest("User Salary/Monthly Expenses can't be less than 1000");
                return Ok(await _repository.CreateUserAsync(user));

            }

            return BadRequest();
        }
        // POST: api/Users/CreateAccount
        [HttpPost("CreateAccount")]
        public async Task<IActionResult> CreateAccountAsync([FromBody] User user)
        {
            if (ModelState.IsValid)
            {
                if (!await _repository.Exist(user.Email)) return BadRequest("User does not exist");
                return Ok(await _repository.CreateAccountAsync(user));

            }

            return BadRequest();
        }
        // GET: api/Users/GetUser/GetAccounts
        [HttpGet("GetAccounts")]
        public async Task<IActionResult> GetAccountsAsync()
        {
            return Ok(await _repository.GetAllAccountsAsync());
        }
        // GET: api/Users/GetUser/{email}
        [HttpGet("GetUser/{email}")]
        public async Task<IActionResult> GetUserByEmail([FromRoute]string email)
        {
            if (!await _repository.Exist(email)) return BadRequest("User does not exist");
            return Ok(await _repository.GetUserByEmail(email));
        }
    }
}
