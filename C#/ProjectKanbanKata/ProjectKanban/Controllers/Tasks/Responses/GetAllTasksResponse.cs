using ProjectKanban.Controllers.Tasks.Models;
using System.Collections.Generic;

namespace ProjectKanban.Controllers.Tasks.Responses;

public class GetAllTasksResponse
{
    public List<TaskModel> Tasks { get; set; }
}
