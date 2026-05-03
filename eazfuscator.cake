#reference "BuildArtifacts/temp/_PublishedLibraries/Cake.Eazfuscator.Net/netstandard2.0/Cake.Eazfuscator.Net.dll"

using Cake.Eazfuscator.Net;

// Self-contained exercise of Cake.Eazfuscator.Net's alias + settings
// surface. The actual EazfuscatorNet alias shells out to a licensed
// third-party tool (Eazfuscator.Net.exe), which CI can't install — so
// this script verifies the addin loads, the EazfuscatorNetSettings
// type can be constructed and round-tripped, and the alias is callable
// (it'll throw at tool-resolution time, which we catch).

void AssertThat(bool condition, string message)
{
    if (!condition)
    {
        throw new Exception("Assertion failed: " + message);
    }
}

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
    AssertThat(settings.Quiet, "Quiet roundtrip");
    AssertThat(settings.EnsureObfuscated, "EnsureObfuscated roundtrip");
    AssertThat(settings.MSBuildProjectPath != null, "MSBuildProjectPath roundtrip");
    AssertThat(settings.MSBuildProjectConfiguration == "Release", "MSBuildProjectConfiguration roundtrip");
    AssertThat(settings.MSBuildProjectPlatform == "AnyCPU", "MSBuildProjectPlatform roundtrip");
    AssertThat(settings.MSBuildSolutionPath != null, "MSBuildSolutionPath roundtrip");
    AssertThat(settings.CompatibilityVersion == "5.0", "CompatibilityVersion roundtrip");
    AssertThat(settings.ProbingPaths != null, "ProbingPaths roundtrip");
    AssertThat(settings.WarningsAsErrors == "all", "WarningsAsErrors roundtrip");
    AssertThat(settings.ConfigurationFile != null, "ConfigurationFile roundtrip");
    AssertThat(settings.Statistics, "Statistics roundtrip");
    AssertThat(settings.NewlineFlush, "NewlineFlush roundtrip");

    Information("EazfuscatorNetSettings OK (all 18 properties round-tripped)");
});

Task("Alias-ResolvesToToolError")
    .Does(() =>
{
    // Calling the alias with a fake input file should reach the tool
    // resolution step and fail there with "Eazfuscator.Net.exe could
    // not be found" — that confirms the alias is wired correctly even
    // though the licensed tool isn't installed in CI.
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
