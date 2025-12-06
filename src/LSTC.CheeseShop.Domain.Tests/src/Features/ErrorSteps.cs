using Reqnroll;

namespace LSTC.CheeseShop.Domain.Tests.Steps
{
    [Binding]
    public class ErrorSteps
    {
        const string ERRORS = "errors";
        const string ERRORS_EXPECTED = "expected";
        const string ERRORS_EXCEPTION = "exception";

        private readonly ScenarioContext _scenario;

        public ErrorSteps(ScenarioContext scenario)
        {
            _scenario = scenario;
        }

        [Given(@"an error is expected")]
        public void Given_an_error_is_expected()
        {
            _scenario.SetProperty(ERRORS, ERRORS_EXPECTED, true);
        }

        [Then(@"an error was encountered")]
        public void Then_an_error_was_trapped()
        {
            var e = _scenario.GetProperty<Exception>(ERRORS, ERRORS_EXCEPTION);
            Assert.NotNull(e);
        }

        [Then(@"an error was not encountered")]
        public void Then_an_error_was_not_trapped()
        {
            var e = _scenario.GetProperty<Exception>(ERRORS, ERRORS_EXCEPTION);
            Assert.Null(e);
        }

        public static async Task Trap(ScenarioContext scenario, Func<Task> action)
        {
            try
            {
                await action();
            }
            catch (Exception e)
            {
                scenario.SetProperty(ERRORS, ERRORS_EXCEPTION, e);
                if (!scenario.GetProperty(ERRORS, ERRORS_EXPECTED, false))
                    throw;
            }
        }
    }
}
