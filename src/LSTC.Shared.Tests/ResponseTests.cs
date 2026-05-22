using Xunit;
using Microsoft.AspNetCore.Http.HttpResults;
using System.ComponentModel.DataAnnotations;

namespace LSTC.Shared.Http.Tests;

public class ResponseTests
{
    [Fact]
    public async Task Success_returns_null()
    {
        var result = await Response.TrapErrors(async () => { });

        Assert.Null(result);
    }

    [Fact]
    public async Task FormatException_returns_bad_request()
    {
        var result = await Response.TrapErrors(async () =>
        {
            throw new FormatException("Invalid format");
        });

        Assert.NotNull(result);
        var badRequestResult = Assert.IsType<BadRequest<ApiResponse>>(result);
        var apiResponse = Assert.IsType<ApiResponse>(badRequestResult.Value);
        Assert.Equal("Bad Request", apiResponse.Message);
        Assert.Single(apiResponse.Errors);
        Assert.Equal("Invalid format", apiResponse.Errors[0].Message);
    }

    [Fact]
    public async Task ValidationException_returns_bad_request()
    {
        var result = await Response.TrapErrors(async () =>
        {
            throw new ValidationException("Validation failed");
        });

        Assert.NotNull(result);
        var badRequestResult = Assert.IsType<BadRequest<ApiResponse>>(result);
        var apiResponse = Assert.IsType<ApiResponse>(badRequestResult.Value);
        Assert.Equal("Bad Request", apiResponse.Message);
        Assert.Single(apiResponse.Errors);
        Assert.Equal("Validation failed", apiResponse.Errors[0].Message);
    }

    [Fact]
    public async Task AggregateException_returns_bad_request()
    {
        var result = await Response.TrapErrors(async () =>
        {
            var ex1 = new ValidationException("Validation failed 1");
            var ex2 = new ValidationException("Validation failed 2");
            throw new AggregateException(ex1, ex2);
        });

        Assert.NotNull(result);
        var badRequestResult = Assert.IsType<BadRequest<ApiResponse>>(result);
        var apiResponse = Assert.IsType<ApiResponse>(badRequestResult.Value);
        Assert.Equal("Bad Request", apiResponse.Message);
        Assert.Equal(2, apiResponse.Errors.Count);
        Assert.Equal("Validation failed 1", apiResponse.Errors[0].Message);
        Assert.Equal("Validation failed 2", apiResponse.Errors[1].Message);
    }

    [Fact]
    public async Task InvalidOperationException_returns_bad_request()
    {
        var result = await Response.TrapErrors(async () =>
        {
            throw new InvalidOperationException("Invalid operation");
        });

        Assert.NotNull(result);
        var badRequestResult = Assert.IsType<UnprocessableEntity<ApiResponse>>(result);
        var apiResponse = Assert.IsType<ApiResponse>(badRequestResult.Value);
        Assert.Equal("Unprocessable Entity", apiResponse.Message);
        Assert.Single(apiResponse.Errors);
        Assert.Equal("Invalid operation", apiResponse.Errors[0].Message);
    }

    public class TestException : Exception
    {
        public TestException(string message) : base(message) { }
    }

    [Fact]
    public async Task UnexpectedException_returns_internal_server_error()
    {
        var result = await Response.TrapErrors(async () =>
        {
            throw new TestException("Unexpected error");
        });

        Assert.NotNull(result);
        var internalServerErrorResult = Assert.IsType<InternalServerError<ApiResponse>>(result);
        var apiResponse = Assert.IsType<ApiResponse>(internalServerErrorResult.Value);
        Assert.Equal("Internal Server Error", apiResponse.Message);
        Assert.Single(apiResponse.Errors);
        Assert.Equal("Unexpected error", apiResponse.Errors[0].Message);
    }
}
