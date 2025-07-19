using System;

namespace ProjectKanban.Exceptions;

// This allows us to separate domain exceptions from regular ones.
// We can then handle these exceptions in our middleware and display friendly messages to users.
public abstract class DomainException(string message) : Exception(message)
{
}
