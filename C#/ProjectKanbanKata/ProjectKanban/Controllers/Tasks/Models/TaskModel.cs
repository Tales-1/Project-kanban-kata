using System.Collections.Generic;

namespace ProjectKanban.Controllers.Tasks.Models;

public class TaskModel
{
    public int Id { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
    public int EstimatedDevDays { get; set; }
    public List<TaskAssignedUserModel> AssignedUsers { get; set; }
}
