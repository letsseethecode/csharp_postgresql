using LSTC.CheeseShop.Domain;

public interface IStockChecker
{
    /// <summary>
    /// Checks that a location has sufficient stock at a given date/time.
    /// </summary>
    /// <param name="location"></param>
    /// <param name="product"></param>
    /// <param name="quantity"></param>
    /// <param name="date"></param>
    /// <returns></returns>
    public bool HasStock(Location location, Product product, int quantity, DateTime date);
}