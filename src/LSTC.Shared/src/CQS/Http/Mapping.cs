using System.Reflection;
using Newtonsoft.Json.Serialization;

namespace LSTC.Shared.CQS.Http;

public class Mapping
{
    public PropertyInfo Property { get;}

    public string Name { get; }

    private string CamelCase(string name)
    {
        if (string.IsNullOrEmpty(name) || char.IsLower(name[0]))
            return name;
        return char.ToLower(name[0]) + name.Substring(1);
    }

    public Mapping(PropertyInfo property, string? name)
    {
        Property = property;
        Name = name ?? CamelCase(property.Name);
    }
}
