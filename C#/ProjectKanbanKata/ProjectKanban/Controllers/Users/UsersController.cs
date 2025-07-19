using Microsoft.AspNetCore.Mvc;
using ProjectKanban.Controllers.Users.Models;
using ProjectKanban.Controllers.Users.Responses;
using ProjectKanban.Users;
using System.Collections.Generic;

namespace ProjectKanban.Controllers
{
    [Route("api/users")]
    public class UsersController : Controller
    {
        private UserService _userService;

        public UsersController(UserRepository userRepository)
        {
            _userService = new UserService(userRepository);
        }
        
        [HttpGet("{id}")]
        public UserModel Get(int id)
        {
            return new UserModel();
        }
        
        [HttpGet]
        public AllUsersResponse GetAll()
        {
            return _userService.GetAllUsers();
        }

        [HttpPost("login")]
        public Session Login([FromBody] LoginRequest loginRequest)
        {
            return _userService.Login(loginRequest);
        }
    }
}