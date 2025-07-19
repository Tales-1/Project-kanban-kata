using System.Collections.Generic;
using System.Linq;
using Dapper;
using ProjectKanban.Data;

namespace ProjectKanban.Tasks
{
    public sealed class TaskRepository
    {
        private readonly IDatabase _database;

        public TaskRepository(IDatabase database)
        {
            _database = database;
        }

        public TaskRecord GetById(int id)
        {
            using (var connection = _database.Connect())
            {
                connection.Open();
                using var transaction = connection.BeginTransaction();

                var taskRecord = connection.QuerySingleOrDefault<TaskRecord>("SELECT * from task where id = @Id;", new { Id = id })
                    ?? throw new TaskNotFoundException($"Task with id: ${id} not found");

                return taskRecord;
            }
        }

        public TaskRecord Create(TaskRecord taskRecord)
        {
            using (var connection = _database.Connect())
            {
                connection.Open();
                using var transaction = connection.BeginTransaction();
                taskRecord.Id = connection.Insert("insert into task(status, description, estimated_dev_days) VALUES (@Status, @Description, @EstimatedDevDays);", taskRecord);
                transaction.Commit();
            }

            return taskRecord;
        }

        public List<TaskRecord> GetAll()
        {
            using (var connection = _database.Connect())
            {
                connection.Open();
                using var transaction = connection.BeginTransaction();
                var taskRecords = connection.Query<TaskRecord>("SELECT * from task;").ToList();
                return taskRecords;
            }
        }

        public List<TaskAssignedRecord> GetAssignedFor(int taskId)
        {
            using (var connection = _database.Connect())
            {
                connection.Open();
                using var transaction = connection.BeginTransaction();
                var taskRecords = connection.Query<TaskAssignedRecord>("SELECT * from task_assigned where task_id = @TaskId;", new {TaskId = taskId}).ToList();
                return taskRecords;
            }
        }
        
        public void CreateAssigned(TaskAssignedRecord record)
        {
            using (var connection = _database.Connect())
            {
                connection.Open();
                using var transaction = connection.BeginTransaction();
                connection.Execute("INSERT INTO task_assigned (task_id, user_id) VALUES (@TaskId, @UserId);", record);
                transaction.Commit();
            }
        }
    }

    public class TaskRecord
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public int EstimatedDevDays { get; set; }
    }
    
    public class UserRecord
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
    
    public class ClientRecord
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    
    public class TaskAssignedRecord
    {
        public int TaskId { get; set; }
        public int UserId { get; set; }
    }

    public struct TaskStatus
    {
        public const string BACKLOG = "Backlog";
        public const string IN_PROGRESS = "In Progress";
        public const string IN_SIGNOFF = "In Signoff";
        public const string DONE = "Done";
    }
}