namespace LSTC.Shared.Http;

public class FromPathAttribute : Attribute
{
    public string Name { get; set; }
    
    public FromPathAttribute(string name)
    {
        this.Name = name;
    }
    
    public FromPathAttribute() : this(string.Empty)
    {
    }
}
