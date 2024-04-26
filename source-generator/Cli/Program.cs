using Cli;

string commandName = args[0];
string input = args[1];
string output = args[2];

CommandFactory commandFactory = new(commandName);

commandFactory.Create().Execute(input, output);