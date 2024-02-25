﻿using CSharpFunctionalExtensions;
namespace MyNoteApi.Repositories.Services;
public static class ExtentionMethods
{
    public static object ToResult(this Result result)
    {
        result.TryGetError(out var error);
        return new
        {
            result.IsFailure,
            result.IsSuccess,
            Error = error
        };
    }
}
