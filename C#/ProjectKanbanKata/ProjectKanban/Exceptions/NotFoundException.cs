using System;

namespace ProjectKanban.Exceptions;

// In a bigger project this exception would be caught by a global error handling middleware that would 
// handle the response.
public class NotFoundException(string name, int key) : Exception($"{name} not found with key {key}");