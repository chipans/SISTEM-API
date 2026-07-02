using System.Net;

namespace SistemApi.Domain.Commom;

public class Result<T>
{
    public bool IsSuccess { get; }
    public T? Data { get; }
    public List<string> Errors { get; }
    public HttpStatusCode StatusCode { get; }

    private Result(bool isSuccess, T? data, List<string> errors, HttpStatusCode statusCode)
    {
        IsSuccess = isSuccess;
        Data = data;
        Errors = errors;
        StatusCode = statusCode;
    }

    public static Result<T> Success(T data, HttpStatusCode statusCode = HttpStatusCode.OK)
        => new(true, data, [], statusCode);

    public static Result<T> Failure(List<string> errors, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        => new(false, default, errors, statusCode);
}
