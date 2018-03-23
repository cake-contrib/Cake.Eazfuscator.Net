using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Eazfuscator.Net
{
    internal sealed class EazfuscatorNetRunner : Tool<EazfuscatorNetSettings>
    {
        private readonly ICakeEnvironment _environment;

        internal EazfuscatorNetRunner(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner, IToolLocator tools)
            : base(fileSystem, environment, processRunner, tools)
        {
            _environment = environment;
        }

        internal void Run(IEnumerable<FilePath> inputFiles, EazfuscatorNetSettings settings)
        {
            if (inputFiles == null)
            {
                throw new ArgumentNullException(nameof(inputFiles));
            }

            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            Run(settings, GetArguments(inputFiles, settings));
        }

        protected override string GetToolName() => "Eazfuscator.Net";

        protected override IEnumerable<string> GetToolExecutableNames()
        {
            yield return "Eazfuscator.Net.exe";
        }

        private ProcessArgumentBuilder GetArguments(IEnumerable<FilePath> inputFiles, EazfuscatorNetSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            foreach (var inputFile in inputFiles)
            {
                builder.AppendQuoted(inputFile.MakeAbsolute(_environment).FullPath);
            }

            if (settings.NoLogo)
            {
                builder.Append("--nologo");
            }

            if (settings.OutputFile != null)
            {
                builder.Append("--output");
                builder.AppendQuoted(settings.OutputFile.MakeAbsolute(_environment).FullPath);
            }

            if (settings.KeyFile != null)
            {
                builder.Append("--key-file");
                builder.AppendQuoted(settings.KeyFile.MakeAbsolute(_environment).FullPath);
            }

            if (!string.IsNullOrWhiteSpace(settings.KeyContainer))
            {
                builder.Append("--key-container");
                builder.AppendQuoted(settings.KeyContainer);
            }

            if (settings.Quiet)
            {
                builder.Append("--quiet");
            }

            if (!string.IsNullOrWhiteSpace(settings.DecodeStackTraceWithPassword))
            {
                builder.Append("--decode-stack-trace-with-password");
                builder.AppendQuoted(settings.DecodeStackTraceWithPassword);
            }

            if (!string.IsNullOrWhiteSpace(settings.ErrorSandbox))
            {
                builder.Append("--error-sandbox");
                builder.AppendQuoted(settings.ErrorSandbox);
            }

            if (settings.EnsureObfuscated)
            {
                builder.Append("--ensure-obfuscated");
            }

            if (settings.MSBuildProjectPath != null)
            {
                builder.Append("--msbuild-project-path");
                builder.AppendQuoted(settings.MSBuildProjectPath.MakeAbsolute(_environment).FullPath);
            }

            if (!string.IsNullOrWhiteSpace(settings.MSBuildProjectConfiguration))
            {
                builder.Append("--msbuild-project-configuration");
                builder.AppendQuoted(settings.MSBuildProjectConfiguration);
            }

            if (!string.IsNullOrWhiteSpace(settings.MSBuildProjectPlatform))
            {
                builder.Append("--msbuild-project-platform");
                builder.AppendQuoted(settings.MSBuildProjectPlatform);
            }

            if (settings.MSBuildSolutionPath != null)
            {
                builder.Append("--msbuild-solution-path");
                builder.AppendQuoted(settings.MSBuildSolutionPath.MakeAbsolute(_environment).FullPath);
            }

            if (settings.ProtectProject)
            {
                builder.Append("--protect-project");
            }

            if (settings.UnprotectProject)
            {
                builder.Append("--unprotect-project");
            }

            if (!string.IsNullOrWhiteSpace(settings.CompatibilityVersion))
            {
                builder.Append("--compatibility-version");
                builder.AppendQuoted(settings.CompatibilityVersion);
            }

            if (settings.CheckVersion)
            {
                builder.Append("--check-version");
            }

            if (settings.ProbingPaths != null)
            {
                builder.Append("--probing-paths");
                var fullPathsStringArray = new List<string>();

                foreach (var probingPath in settings.ProbingPaths)
                {
                    fullPathsStringArray.Add(probingPath.MakeAbsolute(_environment).FullPath);
                }

                var probingPathsString = string.Join(";", fullPathsStringArray);
                builder.AppendQuoted(probingPathsString);
            }

            if (!string.IsNullOrWhiteSpace(settings.WarningsAsErrors))
            {
                builder.Append("--warnings-as-errors");
                builder.AppendQuoted(settings.WarningsAsErrors);
            }

            if (settings.ConfigurationFile != null)
            {
                builder.Append("--configuration-file");
                builder.AppendQuoted(settings.ConfigurationFile.MakeAbsolute(_environment).FullPath);
            }

            if (settings.Statistics)
            {
                builder.Append("--statistics");
            }

            if (settings.NewlineFlush)
            {
                builder.Append("--newline-flush");
            }

            return builder;
        }
    }
}
