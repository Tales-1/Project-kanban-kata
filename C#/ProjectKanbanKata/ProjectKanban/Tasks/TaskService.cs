using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ProjectKanban.Controllers.Tasks;
using ProjectKanban.Controllers.Tasks.Models;
using ProjectKanban.Controllers.Tasks.Responses;
using ProjectKanban.Extensions;
using ProjectKanban.Tasks.Dtos;
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
                Id = taskRecord.Id
            };
        }

        public GetAllTasksResponse GetAll(Session session)
        {
            var taskRecords = _taskRepository.GetAll();

            var response = new GetAllTasksResponse{Tasks = new List<TaskModel>()};

            foreach (var task in taskRecords)
            {
                var taskModel = new TaskModel
                {
                    Id = task.Id,
                    Status = task.Status,
                    EstimatedDevDays = task.EstimatedDevDays,
                    Description = task.Description,
                };
                taskModel.AssignedUsers = new List<TaskAssignedUserModel>();
                var assigned = _taskRepository.GetAssignedFor(task.Id);
                foreach (var assignee in assigned)
                {
                    var user = _userRepository.GetAll().First(x => x.Id == assignee.UserId);
                    taskModel.AssignedUsers.Add(new TaskAssignedUserModel { Username = user.Username });
                }
                response.Tasks.Add(taskModel);
            }

            return response;
        }
    }
}