namespace Domain;

public class ApplicationModel
{
    public string Id { get; set; }
    public string Name { get; set; }
    public object[] Properties { get; set; }
    public object[] StaticProperties { get; set; }
    public Operation Operations { get; set; }
}

public class Operation
{
    public string OperationName { get; set; }
    public object ReturnValue { get; set; }
}