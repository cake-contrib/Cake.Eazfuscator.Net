using Cake.Common.Diagnostics;
using Cake.Common.IO;
using Cake.Core.IO;
using Cake.Eazfuscator.Net;
using Cake.Frosting;

namespace Build.Tasks
{
    [TaskName("Settings-Roundtrip")]
    public sealed class SettingsRoundtripTask : FrostingTask<BuildContext>
    {
        public override void Run(BuildContext context)
        {
            var settings = new EazfuscatorNetSettings
            {
                NoLogo = true,
                OutputFile = context.File("./out/obfuscated.dll"),
                KeyFile = context.File("./key.snk"),
                KeyContainer = "fake-container",
                Quiet = true,
                EnsureObfuscated = true,
                MSBuildProjectPath = context.File("./demo.csproj"),
                MSBuildProjectConfiguration = "Release",
                MSBuildProjectPlatform = "AnyCPU",
                MSBuildSolutionPath = context.File("./demo.sln"),
                ProtectProject = false,
                UnprotectProject = false,
                CompatibilityVersion = "5.0",
                CheckVersion = false,
                ProbingPaths = new DirectoryPath[] { context.Directory("./bin/Release") },
                WarningsAsErrors = "all",
                ConfigurationFile = context.File("./eazfuscator.config"),
                Statistics = true,
                NewlineFlush = true,
            };

            if (!settings.NoLogo)
            {
                throw new System.Exception("NoLogo round-trip failed");
            }

            if (settings.OutputFile == null || !settings.OutputFile.FullPath.EndsWith("obfuscated.dll"))
            {
                throw new System.Exception("OutputFile round-trip failed");
            }

            if (settings.KeyContainer != "fake-container")
            {
                throw new System.Exception("KeyContainer round-trip failed");
            }

            if (settings.CompatibilityVersion != "5.0")
            {
                throw new System.Exception("CompatibilityVersion round-trip failed");
            }

            if (settings.WarningsAsErrors != "all")
            {
                throw new System.Exception("WarningsAsErrors round-trip failed");
            }

            if (settings.ProbingPaths == null)
            {
                throw new System.Exception("ProbingPaths round-trip failed");
            }

            context.Information("EazfuscatorNetSettings OK (representative properties round-tripped)");

            var empty = new EazfuscatorNetSettings();
            if (empty.NoLogo || empty.Quiet || empty.EnsureObfuscated || empty.Statistics || empty.NewlineFlush)
            {
                throw new System.Exception("Default bool properties should all be false");
            }

            if (empty.OutputFile != null || empty.KeyFile != null || empty.KeyContainer != null
                || empty.MSBuildProjectPath != null || empty.MSBuildSolutionPath != null
                || empty.CompatibilityVersion != null || empty.WarningsAsErrors != null
                || empty.ConfigurationFile != null || empty.ProbingPaths != null)
            {
                throw new System.Exception("Default reference properties should all be null");
            }

            context.Information("EazfuscatorNetSettings default constructor OK");
        }
    }
}
