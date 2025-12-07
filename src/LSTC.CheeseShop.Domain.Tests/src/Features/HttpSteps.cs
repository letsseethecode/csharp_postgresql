using System.Text;
using Reqnroll;

namespace LSTC.CheeseShop.Domain.Tests.Steps
{
    public class HttpHeader
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public enum HttpArgType
    {
        Header,
        Query
    }

    public class HttpArgument
    {
        public HttpArgType Type { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }

    [Binding]
    public class HttpSteps
    {
        const string BASE_URL = "baseUrl";

        private readonly ScenarioContext _scenario;

        public HttpSteps(ScenarioContext scenario)
        {
            _scenario = scenario;
        }

        [Given(@"the base url (.*)")]
        public void Given_the_base_url(string baseUrl)
        {
            _scenario.SetProperty(Collection.HttpRequest, BASE_URL, baseUrl);
        }

        [Given(@"the http headers")]
        public void Given_the_http_header(string name, string value)
        {
            _scenario.SetProperty(Collection.HttpHeader, name, value);
        }

        [Given(@"the http headers")]
        public void Given_the_http_headers(DataTable headers)
        {
            foreach (var header in headers.CreateSet<HttpHeader>())
            {
                Given_the_http_header(header.Name, header.Value);
            }
        }

        [When(@"I invoke (.+) (.+)")]
        public async Task When_i_invoke(HttpMethod method, string path)
        {
            await When_i_invoke_with_qs_and_body(method, path, null, null);
        }

        [When(@"I invoke (.+) (.+)")]
        public async Task When_i_invoke_with_qs(HttpMethod method, string path, DataTable query)
        {
            await When_i_invoke_with_qs_and_body(method, path, query, null);
        }

        [When(@"I invoke (.+) (.+)")]
        public async Task When_i_invoke_with_body(HttpMethod method, string path, string body)
        {
            await When_i_invoke_with_qs_and_body(method, path, null, body);
        }

        [When(@"I invoke (.+) (.+)")]
        public async Task When_i_invoke_with_qs_and_body(HttpMethod method, string path, DataTable? arguments, string? body)
        {
            using (var client = new HttpClient())
            {
                var baseUrl = _scenario.GetProperty<string>(Collection.HttpRequest, "baseUrl");
                var headers = _scenario.GetDict(Collection.HttpHeader);

                // Construct the URL
                var uri = string.Format("{0}{1}", baseUrl, path);
                if (arguments != null && arguments.RowCount > 0)
                {
                    uri += "?" + string.Join("&",
                        arguments
                            .CreateSet<HttpArgument>()
                            .Where(x => x.Type == HttpArgType.Query)
                            .Select(x => string.Format("{0}={1}", x.Name, Uri.EscapeDataString(x.Value)))
                    );
                }
                // Construct Request
                var request = new HttpRequestMessage
                {
                    Method = method,
                    RequestUri = new Uri(uri),
                    Content = body != null ? new StringContent(body, Encoding.UTF8, "application/json") : null,
                };
                _scenario.SetProperty(Collection.Subject, request);

                // Add HTTP headers
                foreach (var (key, value) in headers)
                    request.Headers.Add(key, value.ToString());
                if (arguments != null)
                    foreach (var header in arguments.CreateSet<HttpArgument>().Where(x => x.Type == HttpArgType.Header))
                        request.Headers.Add(header.Name, header.Value);

                // Invoke the HTTP request
                await ErrorSteps.Trap(_scenario, async () =>
                {
                    var response = await client.SendAsync(request);
                    _scenario.SetValue("response", response);
                    _scenario.SetValue("result", await response.Content.ReadAsStringAsync());
                });
            }
        }
    }
}
