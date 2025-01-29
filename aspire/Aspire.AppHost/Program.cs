var builder = DistributedApplication.CreateBuilder(args);

var projectA = builder.AddProject<Projects.ProjectA>("serviceA").WithExternalHttpEndpoints();

builder.AddProject<Projects.ProjectB>("serviceB")
    .WithExternalHttpEndpoints()
    .WithReference(projectA)
    .WaitFor(projectA);

builder.Build().Run();