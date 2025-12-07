using System.Text.RegularExpressions;
using Jsonata.Net.Native.Json;
using Reqnroll;

namespace LSTC.CheeseShop.Domain.Tests.Steps
{

    public enum TestOperator
    {
        Lt,
        Lte,
        Eq,
        Neq,
        Gte,
        Gt
    }

    [Binding]
    public class Transforms
    {
        private readonly ScenarioContext _scenario;
        public Transforms(ScenarioContext scenario)
        {
            _scenario = scenario;
        }

        [StepArgumentTransformation(@"(\w+)")]
        public Collection Collection(string value)
        {
            return Enum.Parse<Collection>(value, true);
        }

        [StepArgumentTransformation(@"(<|<=|=|!=|>=|>)")]
        public TestOperator Operator(string value)
        {
            switch (value)
            {
                case "<": return TestOperator.Lt;
                case "<=": return TestOperator.Lte;
                case "=": return TestOperator.Eq;
                case "!=": return TestOperator.Neq;
                case ">=": return TestOperator.Gte;
                case ">": return TestOperator.Gt;
                default: throw new ArgumentException("Invalid Value: {0}", value);
            }
        }

        [StepArgumentTransformation(@"(GET|POST|PUT|DELETE|HEAD|OPTIONS|PATCH|CONNECT|TRACE)")]
        public HttpMethod HttpMethod(string method)
        {
            return new HttpMethod(method);
        }

        [StepArgumentTransformation(@"in (\d+) days")]
        public DateTime InXDaysTransform(int days)
        {
            return DateTime.Today.AddDays(days);
        }

        [StepArgumentTransformation("(.+)")]
        public JToken DecodeJson(string json)
        {
            return Evaluate(_scenario.GetValueOrDefault("values") as Dictionary<string, string>, json);
        }

        public static JToken Evaluate(Dictionary<string, string>? subs, string json)
        {
            if (subs != null)
            {
                json = Regex.Replace(json, @"\$\$[A-Z]+", m => subs[m.Value[2..]]);
            }
            return JToken.Parse(json);
        }
    }

}
