namespace CodeGen;

public class ServiceModel
{
    public string? Namespace { get; set; }
    public string? TargetNamespace { get; set; }
    public string? Name { get; set; }
    public Operation[]? Operations { get; set; }
}