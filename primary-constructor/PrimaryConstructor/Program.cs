using PrimaryConstructor;

var dependency = new Dependency();
var dependent = new Dependent(dependency);

Console.WriteLine(dependent.ShowMessage());
