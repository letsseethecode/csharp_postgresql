namespace LSTC.CheeseShop.Api.Services;

public interface IUserService
{
    void Foo();
}

public class UserService : IUserService
{
    public void Foo()
    {
        Console.WriteLine("Foo!");
    }
}
