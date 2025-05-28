using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using User.Application.Services;
using User.Domain.Models.Response;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/<ValuesController>
        [HttpGet]
        [Authorize]
        public ActionResult<UserResponseDTO> GetUserData()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            try
            {
                var userDto = _userService.GetUserAsync(userId);
                return Ok(userDto);
            }
            catch 
            {
                return NotFound();
            }
        }

        // GET api/<ProductController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetUserById(int id)
        {
            var result = _userService.GetUser(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

    }
}
