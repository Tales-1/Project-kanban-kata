using ProjectKanban.Controllers.Tasks.Models;
using ProjectKanban.Controllers.Tasks.Responses;
using ProjectKanban.Extensions;
using ProjectKanban.Users;
using System.Collections.Generic;

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

    public GetAllTasksResponse GetAll(Session session)
    {
        var taskRecords = _taskRepository.GetAll();

        var response = new GetAllTasksResponse { Tasks = new List<TaskModel>() };

        foreach (var task in taskRecords)
        {
            var taskModel = new TaskModel
            {
                Id = task.Id,
                Status = task.Status,
                EstimatedDevDays = task.EstimatedDevDays,
                Description = task.Description,
            };

            taskModel.AssignedUsers = GetAssignedUsersToTask(task.Id);

            response.Tasks.Add(taskModel);
        }

        return response;
    }


    // Method to retrieve users assigned to a task to promote code re-use. 
    public List<TaskAssignedUserModel> GetAssignedUsersToTask(int taskId)
    {
        var assigned = _taskRepository.GetAssignedFor(taskId);

        var assignedUsers = new List<TaskAssignedUserModel>();

        foreach (var assignee in assigned)
        {
            var user = _userRepository.Get(assignee.UserId);

            assignedUsers.Add(new TaskAssignedUserModel
            {
                Username = user.Username,
                Initials = user.Username.GetInitialsFromUsername()
            });
        }

        return assignedUsers;
    }
}