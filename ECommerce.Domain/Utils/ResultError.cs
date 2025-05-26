namespace ECommerce.Domain.Utils;

public enum ResultErrorType
{
    VALIDATION_ERROR,
    INVALID_OPERATION_ERROR
}

public class ResultError
{
    public ResultError(ResultErrorType errorType, string message)
    {
        ErrorType = errorType;
        Message = message;
    }

    public ResultErrorType ErrorType { get; init; }
    public string Message { get; init; }
}