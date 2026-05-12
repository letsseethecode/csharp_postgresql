namespace LSTC.Shared.Http;

public class ContextAttribute : Attribute
{
    public string Name { get; set; }

    public ContextAttribute(string name)
    {
        this.Name = name;
    }
}