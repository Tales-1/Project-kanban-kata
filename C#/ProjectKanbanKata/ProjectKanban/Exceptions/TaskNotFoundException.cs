namespace ProjectKanban.Exceptions;

public class TaskNotFoundException(string? message) : DomainException(!string.IsNullOrEmpty(message) ? message : MESSAGE)
{
    const string MESSAGE = "Task not found";
}
