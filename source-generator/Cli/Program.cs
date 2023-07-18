using Cli;

var commandName = args[0];
var input = args[1];
var output = args[2];

CommandFactory commandFactory = new(commandName);

commandFactory.Create().Execute(input, output);
