## FusionAuth C# Client ![semver 2.0.0 compliant](http://img.shields.io/badge/semver-2.0.0-brightgreen.svg?style=flat-square)
If you're integrating FusionAuth with a C# (.Net) application, this library will speed up your development time.

For additional information and documentation on FusionAuth refer to [https://fusionauth.io](https://fusionauth.io).

> **Read this first: **: This librrary has been superceded by the .NET Core FusionAuth library. See https://github.com/FusionAuth/fusionauth-netcore-client

**Note:** This project uses a slightly different file layout than other C# and Visual Studio projects. This is a Java convention that we carried over to all of our client libraries to keep consistency.

**Note:** This project is using .Net 2.0 and versions of various libraries that are compatible with .Net 2.0. This is done so that this library can be used with Unity.

Find me on NuGet [FusionAuth.Client](https://www.nuget.org/packages/FusionAuth.Client/)

To get started, you can import the C# project into your solution. The C# project file is located here:

```bash
src/main/csharp/FusionAuthCSharpClient.csproj
```

This project file contains the necessary references to the DLLs required to use the FusionAuth C# client. If you need to rebuild or refresh any of the packages, here's the list of dependencies:

* Newtonsoft.Json (version 8.0.3)
* Inversoft.Restify
* System

