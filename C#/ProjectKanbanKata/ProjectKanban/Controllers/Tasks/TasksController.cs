using Microsoft.AspNetCore.Mvc;
using ProjectKanban.Controllers.Tasks.Models;
using ProjectKanban.Controllers.Tasks.Responses;
using ProjectKanban.Tasks;
using ProjectKanban.Users;

namespace ProjectKanban.Controllers.Tasks
{
    [Route("api/tasks")]
    public class TasksController : Controller
    {
        private readonly Session _session;
        private readonly UserRepository _userRepository;
        private TaskService _taskService;

        public TasksController(TaskRepository taskRepository, Session session, UserRepository userRepository)
        {
            _session = session;
            _userRepository = userRepository;
            _taskService = new TaskService(taskRepository, _userRepository);
        }

        [HttpGet("{id}")]
        public TaskModel Get(int id)
        {
            return _taskService.GetById(_session, id);
        }

        public GetAllTasksResponse GetAllTasksResponse()
        {
            return _taskService.GetAll(_session);
        }
    }
}