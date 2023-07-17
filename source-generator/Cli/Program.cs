using Cli;

var projectName = args[0];
var input = args[1];
var output = args[2];

ProjectFactory projectFactory = new(projectName);

projectFactory.GetClass().Do(input, output);
