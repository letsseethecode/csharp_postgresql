namespace LSTC.Shared.Http;

public class ApiResponse
{
    public class Error
    {
        public string Message { get; set; } = string.Empty;
        public string? StackTrace { get; set; } = null;

        public Error(string message, string? stackTrace = null)
        {
            Message = message;
            StackTrace = stackTrace;
        }
    }

    public string Message { get; set; } = string.Empty;
    private List<Error> _errors = new List<Error>();
    public IList<Error> Errors { get => _errors; }
    private List<string> _warnings = new List<string>();
    public IList<string> Warnings { get => _warnings; }
    private List<string> _information = new List<string>();
    public IList<string> Information { get => _information; }

    public ApiResponse(
        string message,
        IEnumerable<Error>? errors = null,
        IEnumerable<string>? warnings = null,
        IEnumerable<string>? information = null)
    {
        Message = message;
        if (errors != null)
            errors.ToList().ForEach(error => _errors.Add(error));
        if (warnings != null)
            warnings.ToList().ForEach(warning => _warnings.Add(warning));
        if (information != null)
            information.ToList().ForEach(info => _information.Add(info));
    }

    public ApiResponse AddError(string message, string? stackTrace = null)
    {
        _errors.Add(new Error(message, stackTrace));
        return this;
    }

    public ApiResponse AddWarning(string warning)
    {
        _warnings.Add(warning);
        return this;
    }

    public ApiResponse AddInformation(string info)
    {
        _information.Add(info);
        return this;
    }
}

public class ApiResponse<TData> : ApiResponse
{
    public TData? Data { get; set; }

    public ApiResponse(string message, TData? data = default, IEnumerable<Error>? errors = null, IEnumerable<string>? warnings = null) 
        : base(message, errors, warnings)
    {
        Data = data;
    }
}
