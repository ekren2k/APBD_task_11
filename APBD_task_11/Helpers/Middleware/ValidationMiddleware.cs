using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using APBD_task_11.DTOs.Device;
using APBD_task_11.Repositories.Interfaces;

namespace APBD_task_11.Helpers.Middleware;

public class ValidationMiddleware
{
    
    private readonly RequestDelegate _next;
    private readonly ILogger<ValidationMiddleware> _logger;
    private readonly List<ValidationRuleSet> _ruleSets;
    private IDeviceRepository _deviceRepository;

    public ValidationMiddleware(RequestDelegate next, ILogger<ValidationMiddleware> logger)
    {
        _next = next;
        _logger = logger;
        
        var json = File.ReadAllText("ValidationRules.json");
        
        var root = JsonSerializer.Deserialize<ValidationRoot>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        _ruleSets = root?.Validations ?? new();
        _logger.LogInformation("Loaded {Count} validation rule sets", _ruleSets.Count);
    }

    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Method != HttpMethods.Post && context.Request.Method != HttpMethods.Put)
        {
            await _next(context);
            return;
        }

        if (context.Request.ContentLength == null || context.Request.ContentLength == 0)
        {
            await _next(context);
            return;
        }
        
        var serviceProvider = context.RequestServices;

        using var scope = serviceProvider.CreateScope();
        var deviceRepository = scope.ServiceProvider.GetRequiredService<IDeviceRepository>();
        _logger.LogInformation("Starting request validation");
        context.Request.EnableBuffering();
        context.Request.Body.Position = 0;
        
        using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true);
        var body = await reader.ReadToEndAsync();
        _logger.LogInformation("Request body: {body}", body);
        context.Request.Body.Position = 0;

        try
        {
            var dto = JsonSerializer.Deserialize<DeviceCreateOrUpdateDto>(body, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (dto == null)
            {
                _logger.LogInformation("Cannot validate request, not a valid device dto");
                throw new Exception("Request body is not a valid device dto");
            }
            var deviceType = await _deviceRepository.GetDeviceTypeByIdAsync(dto.TypeId);
            if (deviceType == null)
                throw new Exception($"Invalid device type ID: {dto.TypeId}");

            var deviceTypeName = deviceType.Name;
            
            var matchingRuleSet = _ruleSets.FirstOrDefault(rule => 
                rule.Type.Equals(deviceTypeName, StringComparison.OrdinalIgnoreCase) &&
                rule.PreRequestName.Equals("IsEnabled", StringComparison.OrdinalIgnoreCase) &&
                dto.IsEnabled.ToString().Equals(rule.PreRequestValue, StringComparison.OrdinalIgnoreCase)
               );
            
            if (matchingRuleSet == null)
            {
                _logger.LogWarning("No validation rules matched for DeviceTypeName '{DeviceTypeName}' with IsEnabled '{IsEnabled}'",
                   deviceTypeName, dto.IsEnabled);
            }
            else
            {
                _logger.LogInformation("Found {Count} rules for DeviceTypeName '{DeviceTypeName}'", matchingRuleSet.Rules.Count, deviceTypeName);
            }

            if (matchingRuleSet != null)
            {
                var additionalProperties = JsonSerializer.Deserialize<Dictionary<string, string>>(dto.AdditionalProperties.GetRawText());

                foreach (var rule in matchingRuleSet.Rules)
                {
                    if (!additionalProperties.TryGetValue(rule.ParamName, out var actualValue))
                        throw new Exception($"Missing additional property {rule.ParamName}");
                    
                    var regexElement = rule.Regex;

                    if (regexElement.ValueKind == JsonValueKind.Array)
                    {
                        var validSetList = regexElement.EnumerateArray().Select(x => x.ToString()).ToList();
                        if (!validSetList.Contains(actualValue))
                        {
                            _logger.LogWarning("Failed validation for {param} with value {value}", rule.ParamName, actualValue);
                            throw new Exception($"Validation failed for {rule.ParamName}: {actualValue} value is not allowed");
                        }

                    }
                    else if (regexElement.ValueKind == JsonValueKind.String)
                    {
                        var pattern = regexElement.GetString()?.Trim('/');
                        if (!Regex.IsMatch(actualValue, pattern ?? ""))
                        {
                            _logger.LogWarning("Failed regex validation for {param}: value '{value}' did not match pattern '{pattern}'",
                                rule.ParamName, actualValue, pattern);
                            throw new Exception($"Validation failed for {rule.ParamName}: {actualValue} value doesn't match pattern {pattern}");
                        }

                    }

                }
                
            }
            
            _logger.LogInformation("Validation completed successfully");

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "$Validation failed: {Message}", ex.Message);
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync(ex.Message);
            return;
        }
        
        await _next(context);
    }
}

public class ValidationRoot
{
    public List<ValidationRuleSet> Validations { get; set; }
}

public class ValidationRuleSet
{
    public string Type { get; set; }
    public string PreRequestName { get; set; }
    public string PreRequestValue { get; set; }
    public List<ValidationRule> Rules { get; set; }
}

public class ValidationRule
{
    public string ParamName { get; set; }
    
    public JsonElement Regex { get; set; }
}