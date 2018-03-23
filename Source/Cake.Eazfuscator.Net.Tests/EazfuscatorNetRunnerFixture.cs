using System.Collections.Generic;
using Cake.Core.IO;
using Cake.Testing.Fixtures;

namespace Cake.Eazfuscator.Net.Tests
{
    internal sealed class EazfuscatorNetRunnerFixture : ToolFixture<EazfuscatorNetSettings>
    {
        public EazfuscatorNetRunnerFixture()
            : base("Eazfuscator.Net.exe")
        {
            InputFiles = new[] { new FilePath("input/test.dll") };
        }

        public IEnumerable<FilePath> InputFiles { get; set; }

        protected override void RunTool()
        {
            var tool = new EazfuscatorNetRunner(FileSystem, Environment, ProcessRunner, Tools);
            tool.Run(InputFiles, Settings);
        }
    }
}
