using LSTC.Shared.CQS.Commands;
using LSTC.Shared.CQS.Http;
using LSTC.Shared.Http;
using Microsoft.OpenApi.Models;

namespace Microsoft.AspNetCore.Builder;

public static class OpenApiHelper
{
    public static OpenApiOperation Generate<TEntity>(OpenApiOperation op, HttpBaseMap<TEntity> map)
        where TEntity : class, new()
    {
        var type = typeof(TEntity);
        op.Summary = map.Summary ?? type.Name;
        op.Description = map.Description ?? type.FullName;

        op.Parameters.Clear();

        foreach (var p in map.PathParameters)
            op.Parameters.Add(GeneratePathParameter(p));

        foreach (var p in map.QueryStringParameters)
            op.Parameters.Add(GenerateQueryStringParameter(p));

        foreach (var p in map.Headers)
            op.Parameters.Add(GenerateHeaderParameter(p));

        if (map.BodyParameters.Any())
            op.RequestBody = GenerateBody(map);

        op.Responses.Clear();

        return op;
    }

    private static OpenApiResponse GenerateReferenceResponse<TResponse>(string description)
    {
        return new OpenApiResponse
        {
            Description = description,
            Content = {
                ["application/json"] = new OpenApiMediaType
                {
                    Schema = new OpenApiSchema
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.Schema,
                            Id = typeof(TResponse).Name
                        }
                    }
                }
            }
        };
    }

    private static OpenApiRequestBody GenerateBody<TEntity>(HttpBaseMap<TEntity> map)
        where TEntity : class, new()
    {
        return new OpenApiRequestBody
        {
            Required = true,
            Content = {
                ["application/json"] = new OpenApiMediaType
                {
                    Schema = new OpenApiSchema
                    {
                        Type = "object",
                        Required = map.BodyParameters.Select(p => p.Name).ToHashSet(),
                        Properties = map.BodyParameters.ToDictionary(
                            p => p.Name,
                            p => new OpenApiSchema { Type = OpenApiSchemaType(p.Property.PropertyType) }
                        )
                    }
                }
            }
        };
    }

    private static OpenApiParameter GenerateHeaderParameter(Mapping p)
    {
        return new OpenApiParameter
        {
            Name = p.Name,
            In = ParameterLocation.Header,
            Required = p.Property.PropertyType.IsValueType && Nullable.GetUnderlyingType(p.Property.PropertyType) == null,
            Schema = new OpenApiSchema { Type = OpenApiSchemaType(p.Property.PropertyType) }
        };
    }

    private static OpenApiParameter GenerateQueryStringParameter(Mapping p)
    {
        return new OpenApiParameter
        {
            Name = p.Name,
            In = ParameterLocation.Query,
            Required = p.Property.PropertyType.IsValueType && Nullable.GetUnderlyingType(p.Property.PropertyType) == null,
            Schema = new OpenApiSchema { Type = OpenApiSchemaType(p.Property.PropertyType) }
        };
    }

    private static OpenApiParameter GeneratePathParameter(Mapping p)
    {
        return new OpenApiParameter
        {
            Name = p.Name,
            In = ParameterLocation.Path,
            Required = true,
            Schema = new OpenApiSchema { Type = OpenApiSchemaType(p.Property.PropertyType) }
        };
    }

    private static string OpenApiSchemaType(Type type)
    {
        type = Nullable.GetUnderlyingType(type) ?? type;
        return type switch
        {
            _ when type == typeof(bool)             => "boolean",
            _ when type == typeof(int)
                || type == typeof(long)             => "integer",
            _ when type == typeof(float) 
                || type == typeof(double)
                || type == typeof(decimal)          => "number",
            _ when type == typeof(Guid)             => "string",
            _ when type == typeof(DateTime)
                || type == typeof(DateTimeOffset)   => "string",
            _                                       => "string"
        };
    }
}
