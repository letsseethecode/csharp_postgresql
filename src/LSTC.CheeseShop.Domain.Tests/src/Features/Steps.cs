using TechTalk.SpecFlow;
using Xunit;

namespace LSTC.CheeseShop.Domain.Tests.Steps
{
    public class Calculator
    {
        public int Add(int a, int b)
        {
            return a + b;
        }
    }

    [Binding]
    public class CalculatorSteps
    {
        private int _result;
        private Calculator _calculator;

        [Given(@"I have a calculator")]
        public void GivenIHaveACalculator()
        {
            _calculator = new Calculator();
        }

        [When(@"I add (.*) and (.*)")]
        public void WhenIAddNumbers(int number1, int number2)
        {
            _result = _calculator.Add(number1, number2);
        }

        [Then(@"the result should be (.*)")]
        public void ThenTheResultShouldBe(int expected)
        {
            Assert.Equal(expected, _result);
        }
    }
}
