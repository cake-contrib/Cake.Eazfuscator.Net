#!/usr/bin/env dotnet
#:sdk Cake.Sdk@6.1.1
#:project ../../Source/Cake.Eazfuscator.Net/Cake.Eazfuscator.Net.csproj
#:property Nullable=enable

// Nullable=enable propagates `<Nullable>enable</Nullable>` to the
// SDK's synthetic csproj so the addin's `FilePath?` annotations on
// EazfuscatorNetSettings load in the right context (silences
// CS8632).
//
// NoWarn=CS8603 silences "Possible null reference return" warnings
// emitted from Cake.Sdk's generated `CakeMethodAliases.g.cs` — the
// source generator doesn't currently propagate the addin's `?`
// annotations into the synthesized wrappers, so it reports the
// nullable returns as suspicious. Not actionable from our code;
// upstream issue against Cake.Sdk.
#:property NoWarn=CS8603

// Cake SDK consumer demo for Cake.Eazfuscator.Net. Runs as a
// file-based .NET program (introduced in .NET 10) using the
// Cake.Sdk directives. The #:project directive above lets the SDK
// build the addin from source rather than referencing a published
// nupkg.
//
// To run locally:
//   cd demo/sdk
//   dotnet cake.cs
//
// Runs the same two checks the script and frosting demos run.

using Cake.Eazfuscator.Net;

Task("Default")
    .IsDependentOn("Settings-Roundtrip")
    .IsDependentOn("Alias-ResolvesToToolError");

Task("Settings-Roundtrip")
    .Does(() =>
{
    var settings = new EazfuscatorNetSettings
    {
        NoLogo = true,
        OutputFile = File("./out/obfuscated.dll"),
        KeyFile = File("./key.snk"),
        KeyContainer = "fake-container",
        Quiet = true,
        EnsureObfuscated = true,
        MSBuildProjectPath = File("./demo.csproj"),
        MSBuildProjectConfiguration = "Release",
        MSBuildProjectPlatform = "AnyCPU",
        MSBuildSolutionPath = File("./demo.sln"),
        ProtectProject = false,
        UnprotectProject = false,
        CompatibilityVersion = "5.0",
        CheckVersion = false,
        ProbingPaths = new Cake.Core.IO.DirectoryPath[] { Directory("./bin/Release") },
        WarningsAsErrors = "all",
        ConfigurationFile = File("./eazfuscator.config"),
        Statistics = true,
        NewlineFlush = true,
    };

    AssertThat(settings.NoLogo, "NoLogo roundtrip");
    AssertThat(settings.OutputFile != null && settings.OutputFile.FullPath.EndsWith("obfuscated.dll"), "OutputFile roundtrip");
    AssertThat(settings.KeyFile != null, "KeyFile roundtrip");
    AssertThat(settings.KeyContainer == "fake-container", "KeyContainer roundtrip");
    AssertThat(settings.EnsureObfuscated, "EnsureObfuscated roundtrip");
    AssertThat(settings.CompatibilityVersion == "5.0", "CompatibilityVersion roundtrip");
    AssertThat(settings.ProbingPaths != null, "ProbingPaths roundtrip");

    Information("EazfuscatorNetSettings OK (representative properties round-tripped)");

    var empty = new EazfuscatorNetSettings();
    AssertThat(!empty.NoLogo, "Default NoLogo is false");
    AssertThat(empty.OutputFile == null, "Default OutputFile is null");
    AssertThat(empty.KeyContainer == null, "Default KeyContainer is null");

    Information("EazfuscatorNetSettings default constructor OK");
});

Task("Alias-ResolvesToToolError")
    .Does(() =>
{
    var threw = false;
    try
    {
        EazfuscatorNet(File("./fake-input.dll"));
    }
    catch (Exception ex) when (ex.Message.IndexOf("Eazfuscator", StringComparison.OrdinalIgnoreCase) >= 0
                              || ex.Message.IndexOf("not be found", StringComparison.OrdinalIgnoreCase) >= 0
                              || ex.Message.IndexOf("could not locate", StringComparison.OrdinalIgnoreCase) >= 0)
    {
        threw = true;
        Information("Alias resolved correctly; tool-not-found exception was: {0}", ex.Message);
    }

    AssertThat(threw, "Expected EazfuscatorNet alias to throw a tool-not-found exception (Eazfuscator.Net.exe is licensed and not installed in CI)");
});

RunTarget("Default");

// ----- Helpers (must come AFTER top-level statements per CS8803) -----

static void AssertThat(bool condition, string message)
{
    if (!condition)
    {
        throw new Exception("Assertion failed: " + message);
    }
}
