namespace ProjectAPI.Core.Exceptions;

public class ProjectNotFoundException : Exception
{
    public ProjectNotFoundException(int id) : base($"Project with ID {id} not found.")
    {
        Id = id;
    }

    public int Id { get; }
} 