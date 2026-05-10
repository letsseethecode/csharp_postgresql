using LSTC.Shared.CQS.Events;

namespace LSTC.Shared.Domain;

public class Result
{
    /// <summary>
    /// Helper function meaning the TResult type can be inferred from the value parameter, so you don't have to specify
    /// the type when calling the Success method, making the code more concise.
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="value"></param>
    /// <param name="events"></param>
    /// <returns></returns>
    public static Result<TResult> Success<TResult>(TResult value, params IEvent[] events)
    {
        return Result<TResult>.Success(value, events);
    }
}

/// <summary>
/// An immutable class that represents the result of an operation, which can be either a success or a failure. It 
/// contains the value of the successful result, an error message describing the failure, and any events associated with
/// the result.
/// </summary>
/// <typeparam name="TResult"></typeparam>
public class Result<TResult>
{
    public bool IsSuccess { get; }
    public TResult? Value { get; }
    public string? ErrorMessage { get; }
    public IEnumerable<IEvent> Events { get; }

    /// <summary>
    /// Private constructor to create a result. Use the static Success and
    /// Failure methods to create a result.
    /// </summary>
    /// <param name="isSuccess">Indicates whether the result is successful.</param>
    /// <param name="value">The value of the result.</param>
    /// <param name="errorMessage">The error message describing the failure.</param>
    private Result(bool isSuccess, TResult? value, string? errorMessage, params IEvent[] events)
    {
        IsSuccess = isSuccess;
        Value = value;
        ErrorMessage = errorMessage;
        Events = [..events];
    }

    /// <summary>
    /// The result is a success.
    /// </summary>
    /// <param name="value">The value of the successful result.</param>
    /// <param name="events">The events associated with the successful result.</param>
    /// <returns>A successful result containing the specified value and events.</returns>
    public static Result<TResult> Success(TResult value, params IEvent[] events)
    {
        return new Result<TResult>(true, value, null, events);
    }

    /// <summary>
    /// The result is a failure.
    /// </summary>
    /// <param name="errorMessage">The error message describing the failure.</param>
    /// <returns>A failed result containing the specified error message.</returns>
    public static Result<TResult> Failure(string errorMessage)
    {
        return new Result<TResult>(false, default, errorMessage, Array.Empty<IEvent>());
    }

    /// <summary>
    /// Returns an immutable result with the specified events added to the
    /// existing events. This method is useful for chaining additional events to
    /// an existing result without modifying the original result.
    /// </summary>
    /// <param name="events">The events to add to the result.</param>
    /// <returns>The result with the added events.</returns>
    public Result<TResult> WithEvents(params IEvent[] events)
    {
        return new Result<TResult>(IsSuccess, Value, ErrorMessage, [..Events, ..events]);
    }
}