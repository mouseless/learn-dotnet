namespace PrimaryConstructor;

public class CustomException(string message)
    : Exception(message) { }