namespace LSTC.Shared.Http;

public class FromHeaderAttribute : Attribute
{
    public string? Name { get; set; }
    
    public FromHeaderAttribute(string? name)
    {
        this.Name = name;
    }
    
    public FromHeaderAttribute() : this(null)
    {
    }
}
