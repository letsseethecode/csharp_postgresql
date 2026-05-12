namespace LSTC.Shared.Http;

public class FromQueryStringAttribute : Attribute
{
    public string Name { get; set; }

    public FromQueryStringAttribute(string name)
    {
        this.Name = name;
    }

    public FromQueryStringAttribute() : this(string.Empty)
    {
    }
}
