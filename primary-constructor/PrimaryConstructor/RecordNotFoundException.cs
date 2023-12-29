namespace PrimaryConstructor;

public class RecordNotFoundException(Type recordType)
    : Exception(message: $"This {recordType.Name} type record does not exist") { }