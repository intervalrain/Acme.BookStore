namespace EdgeSync.Common.Net.Options;

/// <summary>
/// Options for configuring EdgeSync exception handling behavior
/// </summary>
public class EdgeSyncExceptionHandlingOptions
{
    /// <summary>
    /// Gets or sets the base URL for problem type URIs
    /// Default: "https://example.com/probs/"
    /// </summary>
    public string ProblemTypeBaseUrl { get; set; } = "https://example.com/probs/";
    
    /// <summary>
    /// Gets or sets whether to include exception details in problem responses
    /// Default: false (for production safety)
    /// </summary>
    public bool IncludeExceptionDetails { get; set; } = false;
    
    /// <summary>
    /// Gets or sets whether to handle all exceptions or only EdgeSync exceptions
    /// Default: false (only handle EdgeSync exceptions)
    /// </summary>
    public bool HandleAllExceptions { get; set; } = false;
    
    /// <summary>
    /// Gets or sets custom problem type mappings for specific exception types
    /// </summary>
    public Dictionary<Type, string> CustomProblemTypeMappings { get; set; } = new();
}