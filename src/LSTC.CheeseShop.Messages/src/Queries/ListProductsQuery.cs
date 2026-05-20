using LSTC.Shared.CQS.Queries;

namespace LSTC.CheeseShop.Messages.Queries;

public class ListProductsQuery
{
    public class Args : IQueryArgs
    {
        public string Id { get; set; } = string.Empty;
    }

    public class Product
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }

    public class Results : IQueryResults
    {
        public Args? Args { get; set; } = null;
        public Product[] Products { get; set; } = [];
    }
}