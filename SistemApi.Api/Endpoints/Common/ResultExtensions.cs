using SistemApi.Domain.Commom;

namespace SistemApi.Api.Endpoints.Common;

public static class ResultExtensions
{
    public static async Task<IResult> ToApiResult<T>(this Task<Result<T>> resultTask)
    {
        var result = await resultTask;

        return Results.Json(
            new { isSuccess = result.IsSuccess, data = result.Data, errors = result.Errors },
            statusCode: (int)result.StatusCode);
    }
}
