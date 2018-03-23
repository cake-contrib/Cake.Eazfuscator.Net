using System.Collections.Generic;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Eazfuscator.Net
{
    /// <summary>
    /// Contains settings used by <see cref="EazfuscatorNetRunner"/>. See <see
    /// href="https://help.gapotchenko.com/eazfuscator.net/45/deployment/command-line-interface">Comman Line Interface</see>
    /// or run
    /// <code>
    /// .\Eazfuscator.Net.exe --help
    /// </code>
    /// for more details.
    /// </summary>
    public sealed class EazfuscatorNetSettings : ToolSettings
    {
        /// <summary>
        /// Gets or sets a property specifing whether or not to suppress logo message.
        /// </summary>
        public bool NoLogo { get; set; }

        /// <summary>
        /// Gets or sets a property specifing the output file for the obfuscated assembly.
        /// </summary>
        /// <remarks>
        /// <para>
        /// If this option is not specified then output assembly overwrites the input file. The option cannot be
        /// specified when multiple input files are given.
        /// </para>
        /// </remarks>
        public FilePath OutputFile { get; set; }

        /// <summary>
        /// Gets or sets a property specifing the key file to use during obfuscation.
        /// </summary>
        /// <remarks>
        /// <para>
        /// If this option is specified then obfuscated assembly will be signed with a key from specified file.
        /// PLEASE NOTE: obfuscated assembly that had a strong name before obfuscation MUST BE resigned to work properly;
        /// otherwise it will not be able to load. Also note that only assemblies with strong name can be resigned -
        /// assemblies without strong name are not affected.
        /// </para>
        /// </remarks>
        public FilePath KeyFile { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use a key from key container during obfuscation.
        /// </summary>
        /// <remarks>
        /// <para>
        /// If this option is specified then obfuscated assembly will be signed with a key from specified container.
        /// This option cannot be used with 'key-file' option.
        /// </para>
        /// </remarks>
        public string KeyContainer { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to print any information and diagnostic messages.
        /// </summary>
        public bool Quiet { get; set; }

        /// <summary>
        /// Gets or sets a value specifing password to use for decryption during obfuscation.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Decodes encrypted stack trace. Password for decryption is given with this option. Encrypted stack
        /// trace must be fed to standard input stream of the application. Decrypted stack trace will be fed to
        /// standard output stream.
        /// </para>
        /// </remarks>
        public string DecodeStackTraceWithPassword { get; set; }

        /// <summary>
        /// Gets or sets a value specifing the error sanddox to be used.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Runs application given as argument in exception sandbox. Every unhandled exception is caught by
        /// sandbox environment. This feature is useful when obfuscated application cannot be started and bails
        /// out with default unexpected error window.
        /// </para>
        /// </remarks>
        public string ErrorSandbox { get; set; }

        /// <summary>
        /// Gets or sets a value specifing whether to check the input file and ensure it is obfuscated.
        /// </summary>
        public bool EnsureObfuscated { get; set; }

        /// <summary>
        /// Gets or sets a value specifing the path to an MSBuild Project.
        /// </summary>
        public FilePath MSBuildProjectPath { get; set; }

        /// <summary>
        /// Gets or sets a value specifying the MSBuild Project Configuration name.
        /// </summary>
        public string MSBuildProjectConfiguration { get; set; }

        /// <summary>
        /// Gets or sets a value specifing the MSBuild Project Platform.
        /// </summary>
        public string MSBuildProjectPlatform { get; set; }

        /// <summary>
        /// Gets or sets a value specifying the path to an MSBuild Solution.
        /// </summary>
        public FilePath MSBuildSolutionPath { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to protect project.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Project is obfuscated by Eazfuscator.NET on every build when protection is active.
        /// This option should be used with 'msbuild-project-path' option.
        /// </para>
        /// </remarks>
        public bool ProtectProject { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to remove project protection.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This option should be used with 'msbuild-project-path' option.
        /// </para>
        /// </remarks>
        public bool UnprotectProject { get; set; }

        /// <summary>
        /// Gets or sets a value specifing a version of Eazfuscator.NET to be compatible with.
        /// </summary>
        /// <value>A value specifing the git tag.</value>
        public string CompatibilityVersion { get; set; }

        /// <summary>
        /// Gets or sets a value specifing whether to check installed version.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Instructs to check the installed version of Eazfuscator.NET and return the result as exit code. This
        /// option cannot be combined with other options. (To get more help, please try to use it)
        /// </para>
        /// </remarks>
        public bool CheckVersion { get; set; }

        /// <summary>
        /// Gets or sets a value specifing the probing paths.
        /// </summary>
        public IEnumerable<DirectoryPath> ProbingPaths { get; set; }

        /// <summary>
        /// Gets or sets a value specifiying a list of warnings to treat as errors.
        /// </summary>
        /// <remarks>
        /// <para>
        /// A list of warnings to treat as errors separated by comma.
        /// To treat all warning as errors please put an argument 'all' to this option.
        /// </para>
        /// </remarks>
        public string WarningsAsErrors { get; set; }

        /// <summary>
        /// Gets or sets a value specifying a configuration file to use during obfuscation.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Configuration file in C# or VB.NET format with a list of assembly attributes for obfuscation. Please refer to
        /// documentation for configuration syntax.
        /// </para>
        /// </remarks>
        public FilePath ConfigurationFile { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not to produce obfuscation statistics report.
        /// </summary>
        public bool Statistics { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not to flush output messages with new line (CR/LF) symbols.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This feature is useful when integrating with third-party IDEs.
        /// </para>
        /// </remarks>
        public bool NewlineFlush { get; set; }
    }
}
