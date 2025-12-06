using System.Text.RegularExpressions;
using Jsonata.Net.Native.Json;
using Reqnroll;

namespace LSTC.CheeseShop.Domain.Tests.Steps
{
    [Binding]
    public class Transforms
    {
        private readonly ScenarioContext _scenario;
        public Transforms(ScenarioContext scenario)
        {
            _scenario = scenario;
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
