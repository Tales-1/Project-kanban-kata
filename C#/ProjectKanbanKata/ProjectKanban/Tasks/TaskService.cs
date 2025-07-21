using ProjectKanban.Controllers.Tasks.Models;
using ProjectKanban.Controllers.Tasks.Responses;
using ProjectKanban.Extensions;
using ProjectKanban.Users;
using System.Collections.Generic;
using System.Linq;

namespace ProjectKanban.Tasks;

public class TaskService
{
    private readonly TaskRepository _taskRepository;
    private readonly UserRepository _userRepository;

    public TaskService(TaskRepository taskRepository, UserRepository userRepository)
    {
        _taskRepository = taskRepository;
        _userRepository = userRepository;
    }

    // If we're limiting users to only query tasks where the client ids match then we would 
    // pass the client id from the session into the 'GetById()' to run the check on the server
    // Or we could run the check on the here on the client. 
    // I've left it as is since the tests are passing.
    public TaskModel GetById(Session session, int id)
    {
        var taskRecord = _taskRepository.GetById(id);

        return new TaskModel
        {
            Description = taskRecord.Description,
            Status = taskRecord.Status,
            EstimatedDevDays = taskRecord.EstimatedDevDays,
            Id = taskRecord.Id,
            AssignedUsers = GetAssignedUsersToTask(id)
        };
    }

    // We have two options:
    // 1. Pass client id into 'GetAll()' and perform the filter on the 'server'
    // 2. Filter tasks on the client
    // The first option is more efficient, but I picked option 2 for clarity. The method is named 'GetAll', adding the filter clause in the sql may be misleading
    // for developers who expect the query to return ALL tasks regardless of client id.
    // Though if the general understanding is that you must filter by client id then option 1 is fine.
    public GetAllTasksResponse GetAll(Session session)
    {
        var currentUser = _userRepository.GetById(session.UserId);

        var taskRecords = _taskRepository.GetAll()
            .Where(t => t.ClientId == currentUser.ClientId).ToArray();

        var response = new GetAllTasksResponse { Tasks = new List<TaskModel>() };

        foreach (var task in taskRecords)
        {
            var taskModel = new TaskModel
            {
                Id = task.Id,
                Status = task.Status,
                EstimatedDevDays = task.EstimatedDevDays,
                Description = task.Description,
                AssignedUsers = GetAssignedUsersToTask(task.Id)
            };

            response.Tasks.Add(taskModel);
        }

        return response;
    }


    // Method to retrieve users assigned to a task to promote code re-use. 
    private List<TaskAssignedUserModel> GetAssignedUsersToTask(int taskId)
    {
        var assigned = _taskRepository.GetAssignedFor(taskId);

        var assignedUsers = new List<TaskAssignedUserModel>();

        foreach (var assignee in assigned)
        {
            var user = _userRepository.GetById(assignee.UserId);

            assignedUsers.Add(new TaskAssignedUserModel
            {
                Username = user.Username,
                Initials = user.Username.GetInitialsFromUsername()
            });
        }

        return assignedUsers;
    }
}