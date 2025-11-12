namespace LSTC.CheeseShop.Domain;

public class Root
{

}

public class Product
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
}

public class Location
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
}
