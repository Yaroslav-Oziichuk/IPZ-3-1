namespace IndependentWork19;

public class ProfileUpdateException : Exception
{
    public int UserId { get; }
    public string? FieldName { get; }

    public ProfileUpdateException() { }

    public ProfileUpdateException(string message) : base(message) { }

    public ProfileUpdateException(string message, Exception innerException)
        : base(message, innerException) { }

    public ProfileUpdateException(string message, int userId, string fieldName)
        : base(message)
    {
        UserId = userId;
        FieldName = fieldName;
    }
}

public class DatabaseUnavailableException : Exception
{
    public DatabaseUnavailableException() { }
    public DatabaseUnavailableException(string message) : base(message) { }
    public DatabaseUnavailableException(string message, Exception innerException)
        : base(message, innerException) { }
}
