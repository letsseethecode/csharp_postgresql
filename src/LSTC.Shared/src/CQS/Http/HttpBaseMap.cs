using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json.Linq;

namespace LSTC.Shared.CQS.Http;

public abstract class HttpBaseMap<TEntity> where TEntity : new()
{
    public IDictionary<string, string> Classifications { get; } = new Dictionary<string, string>();
    public IList<Mapping> Headers { get; } = new List<Mapping>();
    public IList<Mapping> PathParameters { get; } = new List<Mapping>();
    public IList<Mapping> BodyParameters { get; } = new List<Mapping>();
    public IList<Mapping> QueryStringParameters { get; } = new List<Mapping>();

    protected void Classify(string key, string value)
    {
        Classifications[key] = value;
    }

    private PropertyInfo GetProperty<TValue>(Expression<Func<TEntity, TValue>> expr)
    {
        if (expr.Body is not MemberExpression memberExpr || memberExpr.Member is not PropertyInfo property)
        {
            throw new ArgumentException("Expression must be a property access", nameof(expr));
        }
        return property;
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
        ReadBody(json, errors, result);
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
            var reader = new StreamReader(request.Body);
            var body = await reader.ReadToEndAsync();
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

    private void ReadQueryString(HttpRequest request, List<ValidationException> errors, TEntity result)
    {
        foreach (var queryMap in QueryStringParameters)
        {
            var value = request.Query[queryMap.Name].FirstOrDefault();
            if (value == null)
            {
                errors.Add(new ValidationException($"Missing required query parameter: {queryMap.Name}"));
                continue;
            }
            queryMap.Property.SetValue(result, Convert.ChangeType(value, queryMap.Property.PropertyType));
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
            if (token == null)
            {
                errors.Add(new ValidationException($"Missing required body parameter: {bodyMap.Name}"));
                continue;
            }
            bodyMap.Property.SetValue(result, token.ToObject(bodyMap.Property.PropertyType));
        }
    }

    private void ReadPath(RouteValueDictionary routeValues, List<ValidationException> errors, TEntity result)
    {
        foreach (var pathMap in PathParameters)
        {
            var value = routeValues[pathMap.Name]?.ToString();
            if (value == null)
            {
                errors.Add(new ValidationException($"Missing required path parameter: {pathMap.Name}"));
                continue;
            }
            pathMap.Property.SetValue(result, Convert.ChangeType(value, pathMap.Property.PropertyType));
        }
    }

    private void ReadHeaders(HttpRequest request, List<ValidationException> errors, TEntity result)
    {
        foreach (var headerMap in Headers)
        {
            var value = request.Headers[headerMap.Name].FirstOrDefault();
            if (value == null)
            {
                errors.Add(new ValidationException($"Missing required header: {headerMap.Name}"));
                continue;
            }
            headerMap.Property.SetValue(result, Convert.ChangeType(value, headerMap.Property.PropertyType));
        }
    }
}
