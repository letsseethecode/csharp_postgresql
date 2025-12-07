using System.Text.RegularExpressions;
using System.Xml.Linq;
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
        }

        // ---------------------------------------------------------------------
        // Utility
        // ---------------------------------------------------------------------

        private string Substitute(string value)
        {
            return Regex.Replace(value, @"\$\{(\w+)\}", x =>
            {
                var v = _scenario.GetProperty<string>(Collection.Value, x.Value[2..^1]);
                v = Parse(v)?.ToString() ?? "null";
                v = Substitute(v);
                return v;
            });
        }

        private object? Parse(string value)
        {
            JToken j;
            try
            {
                j = JToken.Parse(value);
            }
            catch
            {
                var path = value.Split(".", StringSplitOptions.RemoveEmptyEntries);
                var subject = _scenario.GetProperty<object>(Collection.Value, path[0]);
                var result = Evaluate(subject, path, 1)?.ToString() ?? "null";
                result = Substitute(result);
                j = JToken.Parse(result);
            }
            return j.Type == JTokenType.Object ? j : j.ToObject<object>();
        }

        private object? Evaluate(object subject, string[] path, int index)
        {
            if (index < path.Length)
                return subject?.GetType().GetProperty(path[index])?.GetValue(subject);
            return subject;
        }

        private void Compare(string lhs, TestOperator op, string rhs)
        {
            var actual = Parse(lhs);
            var expected = Parse(rhs);
            if (op == TestOperator.Eq)
            {
                Assert.Equal(actual, expected);
            }
            else if (op == TestOperator.Neq)
            {
                Assert.NotEqual(actual, expected);
            }
            else
            {
                var res = ((IComparable)actual).CompareTo(expected);
                switch (op)
                {
                    case TestOperator.Lt:
                        Assert.True(res < 0);
                        break;
                    case TestOperator.Lte:
                        Assert.True(res <= 0);
                        break;
                    case TestOperator.Gte:
                        Assert.True(res >= 0);
                        break;
                    case TestOperator.Gt:
                        Assert.True(res > 0);
                        break;
                }
            }
        }

        private void Set(string name, string value)
        {
            _scenario.SetProperty(Collection.Value, name, value);
        }

        private void Query(string data, string query)
        {
            var d = Parse(data) as JToken;
            var q = new JsonataQuery(query);
            var result = q.Eval(d).ToObject<bool>();
            Assert.True(result);
        }

        // ---------------------------------------------------------------------
        // Setting values
        // ---------------------------------------------------------------------

        [Given(@"the value (\w+) is (.+)")]
        public void Given_the_value(string name, string value)
        {
            Set(name, value);
        }

        [Given(@"the value (\w+) is")]
        public void Given_the_value_body(string name, string body)
        {
            Set(name, body);
        }

        [Given(@"the values")]
        public void Given_the_values(DataTable table)
        {
            foreach (var row in table.Rows)
                Set(row[0], row[1]);
        }

        // ---------------------------------------------------------------------
        // Comparing values
        // ---------------------------------------------------------------------

        [Then(@"compare (.+) (<|<=|=|!=|>=|>)")]
        public void Then_compare_body(string lhs, TestOperator op, string body)
        {
            Compare(lhs, op, body);
        }

        [Then(@"compare (.+) (<|<=|=|!=|>=|>) (.+)")]
        public void Then_compare(string lhs, TestOperator op, string rhs)
        {
            Compare(lhs, op, rhs);
        }

        [Then(@"the values")]
        public void Then_the_values(DataTable table)
        {
            var t = new Transforms(_scenario);
            foreach (var row in table.Rows)
                Then_compare(row[0], t.Operator(row[1]), row[2]);
        }

        // ---------------------------------------------------------------------
        // Queries
        // ---------------------------------------------------------------------

        [Then(@"query (.+) matches '(.+)'")]
        public void Query_matches(string data, string query)
        {
            Query(data, query);
        }
    }

}
