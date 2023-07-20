namespace Cli;

public interface ICommand
{
    void Execute(string input, string output);
}