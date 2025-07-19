using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ProjectKanban.Users;

namespace ProjectKanban.Controllers.Users
{
    [Route("api/clients")]
    public class ClientsController : Controller
    {
        private UserService _userService;

        public ClientsController(UserRepository userRepository)
        {
            _userService = new UserService(userRepository);
        }
    }
}