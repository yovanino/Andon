namespace Andon.Web.Dtos.Common;

public sealed class ApiResult<T>
{
    public bool Success { get; set; }

    public string? Message { get; set; }

    public T? Data { get; set; }

    public List<ApiError> Errors { get; set; } = [];

    public string? CorrelationId { get; set; }

    public static ApiResult<T> Ok(T data, string? message = null, string? correlationId = null)
    {
        return new ApiResult<T>
        {
            Success = true,
            Message = message,
            Data = data,
            CorrelationId = correlationId
        };
    }

    public static ApiResult<T> Fail(
        string message,
        string code = "",
        string field = "",
        string? correlationId = null)
    {
        return new ApiResult<T>
        {
            Success = false,
            Message = message,
            Errors = [ApiError.Create(message, code, field)],
            CorrelationId = correlationId
        };
    }

    public static ApiResult<T> Fail(
        IEnumerable<ApiError> errors,
        string? message = null,
        string? correlationId = null)
    {
        var errorList = errors.ToList();

        return new ApiResult<T>
        {
            Success = false,
            Message = message ?? errorList.FirstOrDefault()?.Message,
            Errors = errorList,
            CorrelationId = correlationId
        };
    }
}
