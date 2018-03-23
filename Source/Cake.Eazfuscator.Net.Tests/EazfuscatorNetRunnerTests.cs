using System;
using Cake.Core;
using Cake.Core.IO;
using Cake.Testing;
using FluentAssertions;
using Xunit;

namespace Cake.Eazfuscator.Net.Tests
{
    public sealed class EazfuscatorNetRunnerTests
    {
        [Fact]
        public void Should_Throw_If_Settings_Are_Null()
        {
            // Given
            var fixture = new EazfuscatorNetRunnerFixture { Settings = null };

            // When
            Action result = () => fixture.Run();

            // Then
            result.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("settings");
        }

        [Fact]
        public void Should_Throw_If_EazfuscatorNet_Executable_Was_Not_Found()
        {
            // Given
            var fixture = new EazfuscatorNetRunnerFixture();
            fixture.GivenDefaultToolDoNotExist();

            // When
            Action result = () => fixture.Run();

            // Then
            result.Should().Throw<CakeException>().WithMessage("Eazfuscator.Net: Could not locate executable.");
        }

        [Theory]
        [InlineData("/bin/tools/EazfuscatorNet/Eazfuscator.Net.exe", "/bin/tools/EazfuscatorNet/Eazfuscator.Net.exe")]
        [InlineData("./tools/EazfuscatorNet/Eazfuscator.Net.exe", "/Working/tools/EazfuscatorNet/Eazfuscator.Net.exe")]
        public void Should_Use_EazfuscatorNet_Runner_From_Tool_Path_If_Provided(string toolPath, string expected)
        {
            // Given
            var fixture = new EazfuscatorNetRunnerFixture { Settings = { ToolPath = toolPath } };
            fixture.GivenSettingsToolPathExist();

            // When
            var result = fixture.Run();

            // Then
            result.Path.FullPath.Should().Be(expected);
        }

        [Theory]
        [InlineData("C:/EazfuscatorNet/Eazfuscator.Net.exe", "C:/EazfuscatorNet/Eazfuscator.Net.exe")]
        public void Should_Use_EazfuscatorNet_Runner_From_Tool_Path_If_Provided_On_Windows(string toolPath, string expected)
        {
            // Given
            var fixture = new EazfuscatorNetRunnerFixture { Settings = { ToolPath = toolPath } };
            fixture.GivenSettingsToolPathExist();

            // When
            var result = fixture.Run();

            // Then
            result.Path.FullPath.Should().Be(expected);
        }

        [Fact]
        public void Should_Find_EazfuscatorNet_Runner_If_Tool_Path_Not_Provided()
        {
            // Given
            var fixture = new EazfuscatorNetRunnerFixture();

            // When
            var result = fixture.Run();

            // Then
            result.Path.FullPath.Should().Be("/Working/tools/Eazfuscator.Net.exe");
        }

        [Fact]
        public void Should_Set_Working_Directory()
        {
            // Given
            var fixture = new EazfuscatorNetRunnerFixture();

            // When
            var result = fixture.Run();

            // Then
            result.Process.WorkingDirectory.FullPath.Should().Be("/Working");
        }

        [Fact]
        public void Should_Throw_If_Process_Was_Not_Started()
        {
            // Given
            var fixture = new EazfuscatorNetRunnerFixture();
            fixture.GivenProcessCannotStart();

            // When
            Action result = () => fixture.Run();

            // Then
            result.Should().Throw<CakeException>().WithMessage("Eazfuscator.Net: Process was not started.");
        }

        [Fact]
        public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
        {
            // Given
            var fixture = new EazfuscatorNetRunnerFixture();
            fixture.GivenProcessExitsWithCode(1);

            // When
            Action result = () => fixture.Run();

            // Then
            result.Should().Throw<CakeException>().WithMessage("Eazfuscator.Net: Process returned an error (exit code 1).");
        }

        [Fact]
        public void Should_Use_All_InputFiles()
        {
            // Given
            var fixture = new EazfuscatorNetRunnerFixture { InputFiles = new[] { new FilePath("input/test1.dll"), new FilePath("input/test2.dll")  }};

            // When
            var result = fixture.Run();

            // Then
            result.Args.Should().Be(@"""/Working/input/test1.dll"" ""/Working/input/test2.dll""");
        }

        [Fact]
        public void Should_Set_NoLogo()
        {
            // Given
            var fixture = new EazfuscatorNetRunnerFixture { Settings = { NoLogo = true } };

            // When
            var result = fixture.Run();

            // Then
            result.Args.Should().Be(@"""/Working/input/test.dll"" --nologo");
        }

        [Fact]
        public void Should_Set_OutputFile()
        {
            // Given
            var fixture = new EazfuscatorNetRunnerFixture { Settings = { OutputFile = new FilePath("output/test.dll") } };

            // When
            var result = fixture.Run();

            // Then
            result.Args.Should().Be(@"""/Working/input/test.dll"" --output ""/Working/output/test.dll""");
        }

        [Fact]
        public void Should_Set_KeyFile()
        {
            // Given
            var fixture = new EazfuscatorNetRunnerFixture { Settings = { KeyFile = new FilePath("input/key.file") } };

            // When
            var result = fixture.Run();

            // Then
            result.Args.Should().Be(@"""/Working/input/test.dll"" --key-file ""/Working/input/key.file""");
        }

        [Fact]
        public void Should_Set_KeyContainer()
        {
            // Given
            var fixture = new EazfuscatorNetRunnerFixture { Settings = { KeyContainer = "containerA" } };

            // When
            var result = fixture.Run();

            // Then
            result.Args.Should().Be(@"""/Working/input/test.dll"" --key-container ""containerA""");
        }

        [Fact]
        public void Should_Set_Quiet()
        {
            // Given
            var fixture = new EazfuscatorNetRunnerFixture { Settings = { Quiet = true } };

            // When
            var result = fixture.Run();

            // Then
            result.Args.Should().Be(@"""/Working/input/test.dll"" --quiet");
        }

        [Fact]
        public void Should_Set_DecodeStackTraceWithPassword()
        {
            // Given
            var fixture = new EazfuscatorNetRunnerFixture { Settings = { DecodeStackTraceWithPassword = "password" } };

            // When
            var result = fixture.Run();

            // Then
            result.Args.Should().Be(@"""/Working/input/test.dll"" --decode-stack-trace-with-password ""password""");
        }

        [Fact]
        public void Should_Set_ErrorSandbox()
        {
            // Given
            var fixture = new EazfuscatorNetRunnerFixture { Settings = { ErrorSandbox = "sandboxA" } };

            // When
            var result = fixture.Run();

            // Then
            result.Args.Should().Be(@"""/Working/input/test.dll"" --error-sandbox ""sandboxA""");
        }

        [Fact]
        public void Should_Set_EnsureObfuscated()
        {
            // Given
            var fixture = new EazfuscatorNetRunnerFixture { Settings = { EnsureObfuscated = true } };

            // When
            var result = fixture.Run();

            // Then
            result.Args.Should().Be(@"""/Working/input/test.dll"" --ensure-obfuscated");
        }

        [Fact]
        public void Should_Set_MSBuildProjectPath()
        {
            // Given
            var fixture = new EazfuscatorNetRunnerFixture { Settings = { MSBuildProjectPath = new FilePath("input/msbuild.proj") } };

            // When
            var result = fixture.Run();

            // Then
            result.Args.Should().Be(@"""/Working/input/test.dll"" --msbuild-project-path ""/Working/input/msbuild.proj""");
        }

        [Fact]
        public void Should_Set_MSBuildProjectConfiguration()
        {
            // Given
            var fixture = new EazfuscatorNetRunnerFixture { Settings = { MSBuildProjectConfiguration = "configurationTest" } };

            // When
            var result = fixture.Run();

            // Then
            result.Args.Should().Be(@"""/Working/input/test.dll"" --msbuild-project-configuration ""configurationTest""");
        }

        [Fact]
        public void Should_Set_MSBuildProjectPlatform()
        {
            // Given
            var fixture = new EazfuscatorNetRunnerFixture { Settings = { MSBuildProjectPlatform = "platformA" } };

            // When
            var result = fixture.Run();

            // Then
            result.Args.Should().Be(@"""/Working/input/test.dll"" --msbuild-project-platform ""platformA""");
        }

        [Fact]
        public void Should_Set_MSBuildSolutionPath()
        {
            // Given
            var fixture = new EazfuscatorNetRunnerFixture { Settings = { MSBuildSolutionPath = new FilePath("input/test.sln") } };

            // When
            var result = fixture.Run();

            // Then
            result.Args.Should().Be(@"""/Working/input/test.dll"" --msbuild-solution-path ""/Working/input/test.sln""");
        }

        [Fact]
        public void Should_Set_ProtectProject()
        {
            // Given
            var fixture = new EazfuscatorNetRunnerFixture { Settings = { ProtectProject = true } };

            // When
            var result = fixture.Run();

            // Then
            result.Args.Should().Be(@"""/Working/input/test.dll"" --protect-project");
        }

        [Fact]
        public void Should_Set_UnprotectProject()
        {
            // Given
            var fixture = new EazfuscatorNetRunnerFixture { Settings = { UnprotectProject = true } };

            // When
            var result = fixture.Run();

            // Then
            result.Args.Should().Be(@"""/Working/input/test.dll"" --unprotect-project");
        }

        [Fact]
        public void Should_Set_CompatibilityVersion()
        {
            // Given
            var fixture = new EazfuscatorNetRunnerFixture { Settings = { CompatibilityVersion = "1.1.1" } };

            // When
            var result = fixture.Run();

            // Then
            result.Args.Should().Be(@"""/Working/input/test.dll"" --compatibility-version ""1.1.1""");
        }

        [Fact]
        public void Should_Set_CheckVersion()
        {
            // Given
            var fixture = new EazfuscatorNetRunnerFixture { Settings = { CheckVersion = true } };

            // When
            var result = fixture.Run();

            // Then
            result.Args.Should().Be(@"""/Working/input/test.dll"" --check-version");
        }

        [Fact]
        public void Should_Set_ProbingPaths()
        {
            // Given
            var fixture = new EazfuscatorNetRunnerFixture { Settings = { ProbingPaths = new []{ new DirectoryPath(@".\input\path1"), new DirectoryPath(@".\input\path2") } } };

            // When
            var result = fixture.Run();

            // Then
            result.Args.Should().Be(@"""/Working/input/test.dll"" --probing-paths ""/Working/input/path1;/Working/input/path2""");
        }

        [Fact]
        public void Should_Set_WarningsAsErrors()
        {
            // Given
            var fixture = new EazfuscatorNetRunnerFixture { Settings = { WarningsAsErrors = "EF-4001,EF-4002" } };

            // When
            var result = fixture.Run();

            // Then
            result.Args.Should().Be(@"""/Working/input/test.dll"" --warnings-as-errors ""EF-4001,EF-4002""");
        }

        [Fact]
        public void Should_Set_ConfigurationFile()
        {
            // Given
            var fixture = new EazfuscatorNetRunnerFixture { Settings = { ConfigurationFile = new FilePath("input/test.config") } };

            // When
            var result = fixture.Run();

            // Then
            result.Args.Should().Be(@"""/Working/input/test.dll"" --configuration-file ""/Working/input/test.config""");
        }

        [Fact]
        public void Should_Set_Statistics()
        {
            // Given
            var fixture = new EazfuscatorNetRunnerFixture { Settings = { Statistics = true } };

            // When
            var result = fixture.Run();

            // Then
            result.Args.Should().Be(@"""/Working/input/test.dll"" --statistics");
        }

        [Fact]
        public void Should_Set_NewlineFlush()
        {
            // Given
            var fixture = new EazfuscatorNetRunnerFixture { Settings = { NewlineFlush = true } };

            // When
            var result = fixture.Run();

            // Then
            result.Args.Should().Be(@"""/Working/input/test.dll"" --newline-flush");
        }
    }
}
