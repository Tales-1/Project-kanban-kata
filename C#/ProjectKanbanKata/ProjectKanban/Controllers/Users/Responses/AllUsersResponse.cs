using ProjectKanban.Controllers.Users.Models;
using System.Collections.Generic;

namespace ProjectKanban.Controllers.Users.Responses;

public class AllUsersResponse
{
    public List<UserModel> Users { get; set; }
}
