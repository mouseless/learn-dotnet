var input = args[0];
var output = args[1];

var generatedCode = File.ReadAllText(input);
//parse json from generated code
File.WriteAllText(output, generatedCode);