using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace LSTC.Shared.Http;

public class Response
{
    /// <summary>
    /// Turns exceptions into IResult of ApiResponse with appropriate status
    /// codes and error messages.
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public static async Task<IResult?> TrapErrors(Func<Task> action)
    {
        try
        {
            await action();
            return null;
        }
        catch (Exception ex) when (ex is FormatException or ValidationException)
        {
            return Results.BadRequest(
                new ApiResponse("Bad Request")
                    .AddError(ex.Message, ex.StackTrace)
            );
        }
        catch (AggregateException ex) when (ex.InnerExceptions.All(e => e is ValidationException))
        {
            var errors = ex.InnerExceptions
                .Select(e => new ApiResponse.Error(e.Message, e.StackTrace))
                .ToArray();
            return Results.BadRequest(
                new ApiResponse("Bad Request", errors)
            );
        }
        catch (InvalidOperationException ex)
        {
            return Results.UnprocessableEntity(
                new ApiResponse("Unprocessable Entity")
                    .AddError(ex.Message, ex.StackTrace)
            );
        }
        catch (Exception ex)
        {
            return Results.InternalServerError(
                new ApiResponse("Internal Server Error")
                    .AddError(ex.Message, ex.StackTrace)
            );
        }
    }
}
