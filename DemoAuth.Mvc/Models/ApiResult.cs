using System;

namespace DemoAuth.Mvc.Models;

public sealed class ApiResult<T>
{
    public bool IsSuccess { get; private set; }
    public bool IsForbidden { get; private set; }
    public bool IsUnauthorized { get; private set; }
    public T? Data { get; private set; }

    public static ApiResult<T> Success(T data) => new()
    {
        IsSuccess = true,
        Data = data
    };

    public static ApiResult<T> Forbidden() => new()
    {
        IsForbidden = true
    };

    public static ApiResult<T> Unauthorized() => new()
    {
        IsUnauthorized = true
    };
}
