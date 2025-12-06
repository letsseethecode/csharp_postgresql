using System.Text.RegularExpressions;
using Jsonata.Net.Native;
using Jsonata.Net.Native.Json;
using Reqnroll;

namespace LSTC.CheeseShop.Domain.Tests.Steps
{

    [Binding]
    public class ResultSteps
    {
        const string DEFAULT = "(default)";
        const string RESULTS = "results";
        const string VALUES = "values";

        private readonly TypeConverter _converter = new TypeConverter();

        private readonly ScenarioContext _scenario;
        private readonly IReqnrollOutputHelper _outputHelper;

        public ResultSteps(IReqnrollOutputHelper outputHelper, ScenarioContext scenario)
        {
            _outputHelper = outputHelper;
            _scenario = scenario;
            _scenario.Add(RESULTS, new Dictionary<string, string>());
            _scenario.Add(VALUES, new Dictionary<string, string>());
        }

        // ---------------------------------------------------------------------
        // Utilities
        // ---------------------------------------------------------------------

        private JToken GetValue(string name)
        {
            var d = (Dictionary<string, string>)_scenario[VALUES];
            if (!d.TryGetValue(name, out var v))
                throw new ArgumentException("Unknown value: {0}", name);
            return JToken.Parse(v);
        }

        private JToken Parse(string s)
        {
            return Regex.IsMatch(s, "^[A-Z]+$")
                ? GetValue(s)
                : JToken.Parse(s);
        }

        // ---------------------------------------------------------------------
        // Results
        // ---------------------------------------------------------------------

        [Then("json (.+) query '(.+)' is (.+)")]
        public void Jsonata_query(JToken data, string query, object expected)
        {
            var qry = new JsonataQuery(query);
            var actual = qry.Eval(data).ToObject<object>();
            expected = Convert.ChangeType(expected, actual.GetType());
            Assert.Equal(actual, expected);
        }

        [Given(@"the result (\w+)")]
        public void Given_the_result_is(string value)
        {
            Given_the_result_name_is(DEFAULT, value);
        }

        [Given(@"the result (\w+) is (.*)")]
        public void Given_the_result_name_is(string name, string value)
        {
            var d = (Dictionary<string, string>)_scenario[RESULTS];
            d[name] = value;
        }

        [Then(@"the result (<|<=|=|!=|>=|>) (\w+)")]
        public void Then_the_result_is(string op, string value)
        {
            Then_the_result_name_is(DEFAULT, op, value);
        }

        [Then(@"the result (\w+) (<|<=|=|!=|>=|>) (.*)")]
        public void Then_the_result_name_is(string name, string op, string value)
        {
            var d = (Dictionary<string, string>)_scenario[RESULTS];
            if (!d.TryGetValue(name, out var v))
                throw new ArgumentException(string.Format("Unknown value {0}", name));
            var expected = JToken.Parse(value).ToObject<object>();
            var actual = JToken.Parse(v).ToObject<object>();
            Assert.Equal(expected, actual);
        }

        // ---------------------------------------------------------------------
        // Values
        // ---------------------------------------------------------------------

        [Given(@"the value (\w+) is (.*)")]
        public void Given_the_value_is(string name, string value)
        {
            var d = (Dictionary<string, string>)_scenario[VALUES];
            d[name] = value;
        }

        [Given(@"the values:")]
        public void Given_the_values(Table table)
        {
            foreach (var row in table.Rows)
            {
                Given_the_value_is(row[0], row[1]);
            }
        }

        [Then(@"the value (\w+) (<|<=|=|!=|>=|>) (.*)")]
        public void Then_the_value_name_is(string name, string op, string value)
        {
            var d = (Dictionary<string, string>)_scenario[VALUES];
            if (!d.TryGetValue(name, out var v))
                throw new ArgumentException(string.Format("Unknown value {0}", name));
            var expected = JToken.Parse(value).ToObject<object>();
            var actual = JToken.Parse(v).ToObject<object>();
            Assert.Equal(expected, actual);
        }

        [Then(@"the values are:")]
        public void Then_the_values_are(Table table)
        {
            foreach (var row in table.Rows)
            {
                Then_the_value_name_is(row[0], row[1], row[2]);
            }
        }

        [Then(@"jsonata (.+) matches (.+)")]
        public void Then_value_a_matches_b(string a, string b)
        {
            var lhs = Parse(a);
            var rhs = Parse(b);
            var query = new JsonataQuery(rhs.ToObject<string>());
            var result = query.Eval(lhs);
            Assert.True(result.ToObject<bool>());
        }

        [Then(@"value (.+) query (.+) is (.+)")]
        public void Then_value_a_matches_b(string data, string query, string result)
        {
            var d = Parse(data);
            var q = Parse(query).ToObject<string>();
            var exp = Parse(result).ToObject<object>();
            var qry = new JsonataQuery(q);
            var res = qry.Eval(d).ToObject<object>();
            Assert.Equal(res, exp);
        }

        [Then(@"value (.+) (<|<=|=|!=|>=|>) (.+)")]
        public void Then_value_a_op_b(string a, string op, string b)
        {
            var lhs = Parse(a);
            var rhs = Parse(b);
            var res = ((IComparable)lhs.ToObject<object>()).CompareTo(rhs.ToObject<object>());
            switch (op)
            {
                case "<":
                    Assert.True(res < 0);
                    break;
                case "<=":
                    Assert.True(res <= 0);
                    break;
                case "=":
                    Assert.True(res == 0);
                    break;
                case "!=":
                    Assert.True(res != 0);
                    break;
                case ">=":
                    Assert.True(res >= 0);
                    break;
                case ">":
                    Assert.True(res > 0);
                    break;
                default:
                    throw new ArgumentException("Unknown operation {0}", op);
            }
        }
    }

}
