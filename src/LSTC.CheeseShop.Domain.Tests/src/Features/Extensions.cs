namespace LSTC.CheeseShop.Domain.Tests.Steps
{
    using Reqnroll;
    using Reqnroll.Formatters.PayloadProcessing.Cucumber;

    public static class ScenarioExtensions
    {
        const string DEFAULT_NAME = "(default)";
        const string VALUES = "values";
        const string RESULTS = "results";

        public static Dictionary<string, object> GetDict(this ScenarioContext context, string name)
        {
            if (!context.TryGetValue<Dictionary<string, object>>(name, out var d))
                context[name] = d = new();
            return d;
        }

        public static void SetProperty(this ScenarioContext context, string dict, string name, object? value)
        {
            var d = GetDict(context, dict);
            if (value == null)
                d.Remove(name);
            else
                d[name] = value;
        }

        public static T GetProperty<T>(this ScenarioContext context, string dict, string name)
        {
            return (T)GetDict(context, dict)[name];
        }

        public static T GetProperty<T>(this ScenarioContext context, string dict, string name, T defaultValue)
        {
            return GetDict(context, dict).TryGetValue(name, out var v) ? (T)v : defaultValue;
        }

        // -------------------------------------------------------------------------

        public static void SetValue(this ScenarioContext context, string name, object value)
        {
            SetProperty(context, VALUES, name, value);
        }

        public static T GetValue<T>(this ScenarioContext context, string name)
        {
            return GetProperty<T>(context, VALUES, name);
        }

        // -------------------------------------------------------------------------

        public static void SetResult(this ScenarioContext context, object value)
        {
            SetProperty(context, RESULTS, DEFAULT_NAME, value);
        }

        public static void SetResult(this ScenarioContext context, string name, object value)
        {
            SetProperty(context, RESULTS, name, value);
        }

        public static T GetResult<T>(this ScenarioContext context)
        {
            return GetProperty<T>(context, RESULTS, DEFAULT_NAME);
        }

        public static T GetResult<T>(this ScenarioContext context, string name)
        {
            return GetProperty<T>(context, RESULTS, name);
        }
    }
}