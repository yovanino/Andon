namespace Andon.Web.Dtos.Common;

public sealed class ApiError
{
    public string Field { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;

    public string Code { get; set; } = string.Empty;

    public static ApiError Create(string message, string code = "", string field = "")
    {
        return new ApiError
        {
            Field = field,
            Message = message,
            Code = code
        };
    }
}
