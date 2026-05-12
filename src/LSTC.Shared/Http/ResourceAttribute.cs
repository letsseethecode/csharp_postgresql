namespace LSTC.Shared.Http;

public class ResourceAttribute : Attribute
{
    public string Name { get; set; }

    public ResourceAttribute(string name)
    {
        this.Name = name;
    }
}
