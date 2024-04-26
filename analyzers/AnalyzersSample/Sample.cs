namespace AnalyzersSample;

public class Sample
{
    // IDE1006 error will be received when the field name does not start with underscore.
    readonly string _field;

    public Sample()
    {
        // Using `_field = ""` will result in error SA1122.
        _field = string.Empty;
    }
}