# Native AOT

We researched that we can switch to Native AOT in our projects. And we decided
that it does not satisfy our needs at the moment. You can see the research notes
below.

## To Use

You need to enable `PublishAot` to use it. Then when we publish, the project
will be compiled with AOT. The target platform is important when publishing
because it only works on that platform. For more [detail][publish using cli].

### What does RD.xml do?

AOT compiler might miss some types if the application uses reflection, you can
add an `rd.xml` file to help `ILCompiler` find the types that should be
analyzed. For more [detail][rd.xml].

## Why we didn't use it

Native AOT does not support the json serializers, `dynamic` type, object
relational mapping libraries, `MVC` etc. that we use, so we do not prefer it for
now.

[publish using cli]: https://learn.microsoft.com/en-us/dotnet/core/deploying/native-aot/?tabs=net8plus%2Cwindows#publish-native-aot-using-the-cli
[rd.xml]: https://github.com/dotnet/corert/blob/master/Documentation/using-corert/rd-xml-format.md
