using System.Buffers;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json.Linq;

namespace LSTC.Shared.CQS.Http;

public abstract class HttpBaseMap<TEntity> where TEntity : new()
{
    public string Path { get; private set; } = string.Empty;
    public string? Summary { get; private set; } = string.Empty;
    public string? Description { get; private set; } = string.Empty;
    public IList<Mapping> Headers { get; } = new List<Mapping>();
    public IList<Mapping> PathParameters { get; } = new List<Mapping>();
    public IList<Mapping> BodyParameters { get; } = new List<Mapping>();
    public IList<Mapping> QueryStringParameters { get; } = new List<Mapping>();

    private PropertyInfo GetProperty<TValue>(Expression<Func<TEntity, TValue>> expr)
    {
        if (expr.Body is not MemberExpression memberExpr || memberExpr.Member is not PropertyInfo property)
        {
            throw new ArgumentException("Expression must be a property access", nameof(expr));
        }
        return property;
    }

    protected void Route(string path)
    {
        Path = path;
    }

    protected void FromHeader<TValue>(Expression<Func<TEntity, TValue>> expr, string? name = null)
    {
        var p = GetProperty(expr);
        Headers.Add(new Mapping(p, name));
    }

    protected void FromPath<TValue>(Expression<Func<TEntity, TValue>> expr, string? name = null)
    {
        var p = GetProperty(expr);
        PathParameters.Add(new Mapping(p, name));
    }

    protected void FromBody<TValue>(Expression<Func<TEntity, TValue>> expr, string? name = null)
    {
        var p = GetProperty(expr);
        BodyParameters.Add(new Mapping(p, name));
    }

    protected void FromQueryString<TValue>(Expression<Func<TEntity, TValue>> expr, string? name = null)
    {
        var p = GetProperty(expr);
        QueryStringParameters.Add(new Mapping(p, name));
    }

    public async Task<TEntity> CreateAsync(HttpRequest request, RouteValueDictionary routeValues)
    {
        var errors = new List<ValidationException>();
        var json = await ReadJson(request, errors);
        var result = new TEntity();
        ReadHeaders(request, errors, result);
        ReadPath(routeValues, errors, result);
        ReadBody(json!, errors, result);
        ReadQueryString(request, errors, result);
        ThrowErrors(errors);
        return result;
    }

    private static async Task<JObject?> ReadJson(HttpRequest request, List<ValidationException> errors)
    {
        JObject? json = null;
        var contentType = request.ContentType;
        if (contentType == null)
            return null;
        if (!contentType.Contains("application/json", StringComparison.OrdinalIgnoreCase))
            errors.Add(new ValidationException($"Unsupported content type: {contentType}"));
        else
        {
            request.EnableBuffering();
            request.Body.Position = 0;
            var result = await request.BodyReader.ReadAsync();
            var body = Encoding.UTF8.GetString(result.Buffer.ToArray());
            json = JObject.Parse(body);
        }

        return json;
    }

    private static void ThrowErrors(List<ValidationException> errors)
    {
        var e = errors.GetEnumerator();
        if (e.MoveNext())
        {
            var first = e.Current;
            if (!e.MoveNext())
                throw first;
            else
                throw new AggregateException("Validation Errors", errors);
        }
    }

    private static object ChangeType(string value, Type targetType)
    {
        if (targetType == typeof(Guid) || targetType == typeof(Guid?))
            return Guid.Parse(value);

        return Convert.ChangeType(value, targetType);
    }

    private void ReadQueryString(HttpRequest request, List<ValidationException> errors, TEntity result)
    {
        foreach (var queryMap in QueryStringParameters)
        {
            var value = request.Query[queryMap.Name].FirstOrDefault();
            if (queryMap.Property.PropertyType.IsValueType && Nullable.GetUnderlyingType(queryMap.Property.PropertyType) == null && value == null)
            {
                errors.Add(new ValidationException($"Missing required query parameter: {queryMap.Name}"));
                continue;
            }
            var v = value == null ? null : ChangeType(value, queryMap.Property.PropertyType);
            queryMap.Property.SetValue(result, v);
        }
    }

    private void ReadBody(JObject json, List<ValidationException> errors, TEntity result)
    {
        foreach (var bodyMap in BodyParameters)
        {
            if (json == null)
            {
                errors.Add(new ValidationException("Missing body content"));
                return;
            }

            var token = json.SelectToken(bodyMap.Name);
            if (bodyMap.Property.PropertyType.IsValueType && Nullable.GetUnderlyingType(bodyMap.Property.PropertyType) == null && token == null)
            {
                errors.Add(new ValidationException($"Missing required body parameter: {bodyMap.Name}"));
                continue;
            }
            var v = token == null ? null : token.ToObject(bodyMap.Property.PropertyType);
            bodyMap.Property.SetValue(result, v);
        }
    }

    private void ReadPath(RouteValueDictionary routeValues, List<ValidationException> errors, TEntity result)
    {
        foreach (var pathMap in PathParameters)
        {
            var value = routeValues[pathMap.Name]?.ToString();
            if (pathMap.Property.PropertyType.IsValueType && Nullable.GetUnderlyingType(pathMap.Property.PropertyType) == null && value == null)
            {
                errors.Add(new ValidationException($"Missing required path parameter: {pathMap.Name}"));
                continue;
            }
            var v = value == null ? null : ChangeType(value, pathMap.Property.PropertyType);
            pathMap.Property.SetValue(result, v);
        }
    }

    private void ReadHeaders(HttpRequest request, List<ValidationException> errors, TEntity result)
    {
        foreach (var headerMap in Headers)
        {
            var value = request.Headers[headerMap.Name].FirstOrDefault();
            if (headerMap.Property.PropertyType.IsValueType && Nullable.GetUnderlyingType(headerMap.Property.PropertyType) == null && value == null)
            {
                errors.Add(new ValidationException($"Missing required header: {headerMap.Name}"));
                continue;
            }
            var v = value == null ? null : ChangeType(value, headerMap.Property.PropertyType);
            headerMap.Property.SetValue(result, v);
        }
    }
}
