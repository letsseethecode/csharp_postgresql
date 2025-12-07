namespace LSTC.CheeseShop.Domain.Tests.Steps
{
    using Reqnroll;
    using Reqnroll.Formatters.PayloadProcessing.Cucumber;

    public static class ScenarioExtensions
    {
        public const string DEFAULT_NAME = "(default)";

        public static Dictionary<string, object> GetDict(this ScenarioContext context, Collection collection)
        {
            if (!context.TryGetValue<Dictionary<string, object>>(collection.ToString(), out var d))
                context[collection.ToString()] = d = new();
            return d;
        }

        public static void SetProperty(this ScenarioContext context, Collection collection, object? value)
        {
            SetProperty(context, collection, DEFAULT_NAME, value);
        }

        public static void SetProperty(this ScenarioContext context, Collection collection, string name, object? value)
        {
            var d = GetDict(context, collection);
            if (value == null)
                d.Remove(name);
            else
                d[name] = value;
        }

        public static T GetProperty<T>(this ScenarioContext context, Collection collection)
        {
            return GetProperty<T>(context, collection, DEFAULT_NAME);
        }

        public static T GetProperty<T>(this ScenarioContext context, Collection collection, string name)
        {
            return (T)GetDict(context, collection)[name];
        }

        public static T GetProperty<T>(this ScenarioContext context, Collection collection, string name, T defaultValue)
        {
            return GetDict(context, collection).TryGetValue(name, out var v) ? (T)v : defaultValue;
        }

        // -------------------------------------------------------------------------

        public static void SetValue(this ScenarioContext context, string name, object value)
        {
            SetProperty(context, Collection.Value, name, value);
        }

        public static T GetValue<T>(this ScenarioContext context, string name)
        {
            return GetProperty<T>(context, Collection.Value, name);
        }
    }
}