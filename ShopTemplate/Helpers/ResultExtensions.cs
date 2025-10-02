using FluentResults;
using Microsoft.AspNetCore.Mvc;
using ShopTemplate.ResponseTypes;

namespace ShopTemplate.Helpers;

public static class ResultExtensions
{
    public static ActionResult<T> ToActionResult<T>(this Result<T> result)
    {
            var error = result.Errors.First();

            var failureType = error.Metadata.TryGetValue("Type", out var typeObj) && typeObj is FailureTypes type
                ? type
                : default;

            var errorResponse = new
            {
                message = error.Message,
                code = failureType.ToString()
            };

            return failureType switch
            {
                FailureTypes.NotFound => new NotFoundObjectResult(errorResponse),
                FailureTypes.InvalidPassword => new UnauthorizedObjectResult(errorResponse),
                FailureTypes.InvalidOrderStatus => new BadRequestObjectResult(errorResponse),
                FailureTypes.PaymentFailed => new BadRequestObjectResult(errorResponse),
                FailureTypes.AlreadyExists => new ConflictObjectResult(errorResponse),
                _ => new BadRequestObjectResult(errorResponse)
            };
    }
}