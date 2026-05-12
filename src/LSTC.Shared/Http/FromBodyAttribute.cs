namespace LSTC.Shared.Http;

public class FromBodyAttribute : Attribute
{
    public string Name { get; set; }

    public FromBodyAttribute(string name)
    {
        this.Name = name;
    }

    public FromBodyAttribute() : this(string.Empty)
    {
    }
}
