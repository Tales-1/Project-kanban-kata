using Dapper;
using ProjectKanban.Data;
using ProjectKanban.Exceptions;
using ProjectKanban.Tasks.Dtos;
using System.Collections.Generic;
using System.Linq;

namespace ProjectKanban.Tasks;

// In an ideal world these queries are asynchronous - not changing it as it would require me changing the unit test code
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
                ?? throw new NotFoundException("task", id);

            return taskRecord;
        }
    }

    public TaskRecord Create(TaskRecord taskRecord)
    {
        using (var connection = _database.Connect())
        {
            connection.Open();

            using var transaction = connection.BeginTransaction();

            taskRecord.Id = connection.Insert("insert into task(client_id, status, description, estimated_dev_days) VALUES (@ClientId, @Status, @Description, @EstimatedDevDays);", taskRecord);

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

            var taskRecords = connection.Query<TaskRecord>(@"
                SELECT * from task 
                ORDER BY 
                (case 
                    when status = @Done then 0
                    when status = @SignOff then 1
                    when status = @InProgress then 2
                    when status = @Backlog then 3
                    else status 
                end)", new { 
                Done = TaskStatus.DONE, 
                SignOff = TaskStatus.IN_SIGNOFF, 
                InProgress = TaskStatus.IN_PROGRESS, 
                Backlog = TaskStatus.BACKLOG}).ToList();

            return taskRecords;
        }
    }

    public List<TaskAssignedRecord> GetAssignedFor(int taskId)
    {
        using (var connection = _database.Connect())
        {
            connection.Open();

            using var transaction = connection.BeginTransaction();

            var taskRecords = connection.Query<TaskAssignedRecord>("SELECT * from task_assigned where task_id = @TaskId;", 
                new { TaskId = taskId }).ToList();

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

public struct TaskStatus
{
    public const string BACKLOG = "Backlog";
    public const string IN_PROGRESS = "In Progress";
    public const string IN_SIGNOFF = "In Signoff";
    public const string DONE = "Done";
}