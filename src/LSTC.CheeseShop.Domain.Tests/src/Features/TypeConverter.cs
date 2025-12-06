using System.Reflection.Metadata;
using System.Text.RegularExpressions;

namespace LSTC.CheeseShop.Domain.Tests.Steps
{
    public class TypeConverter
    {
        private static RegexOptions _options = RegexOptions.IgnoreCase | RegexOptions.Compiled;
        private static (Regex pattern, Func<string, object> func)[] _patterns =
        {
            (new Regex(@"^(true|false)$", _options), s => bool.Parse(s)),
            (new Regex(@"^[-]?\d+$", _options), s => int.Parse(s)),
            (new Regex(@"^[-]?\d+\.\d+m$", _options), s => decimal.Parse(s[..^1])),
            (new Regex(@"^[-]?\d+\.\d+f$", _options), s => float.Parse(s[..^1])),
            (new Regex(@"^[-]?\d+\.\d+d$", _options), s => double.Parse(s[..^1])),
            (new Regex(@"^(""[^""]+"")+$", _options), (string s) => s.Replace(@"""""", @"""")[1..^1])
        };

        public object? Convert(string value)
        {
            foreach (var (pattern, func) in _patterns)
            {
                if (pattern.IsMatch(value))
                    return func(value);
            }
            return value;
        }
    }

    [Trait("Category", "Unit")]
    public class TypeConverterTests
    {
        [Theory]
        [InlineData("true", true)]
        [InlineData("false", false)]
        [InlineData("0", 0)]
        [InlineData("-1", -1)]
        [InlineData("""Hello""", "Hello")]
        public void TypeConversions(string value, object expected)
        {
            var actual = new TypeConverter().Convert(value);
            Assert.Equal(actual, expected);
        }

        [Fact]
        public void DecialTypeConversions()
        {
            // decimals can't be consts, so won't work with [InlineData]
            var actual = new TypeConverter().Convert("0.1m");
            var expected = 0.1m;
            Assert.Equal(actual, expected);
        }

        [Fact]
        public void NegativeDecialTypeConversions()
        {
            // decimals can't be consts, so won't work with [InlineData]
            var actual = new TypeConverter().Convert("-0.2m");
            var expected = -0.2m;
            Assert.Equal(actual, expected);
        }
    }

}