using System;
using Cake.Common.Diagnostics;
using Cake.Common.IO;
using Cake.Eazfuscator.Net;
using Cake.Frosting;

namespace Build.Tasks
{
    [TaskName("Alias-ResolvesToToolError")]
    public sealed class AliasResolvesToToolErrorTask : FrostingTask<BuildContext>
    {
        public override void Run(BuildContext context)
        {
            // Calling the alias with a fake input file should reach the tool
            // resolution step and fail there with "Eazfuscator.Net.exe could
            // not be found" — that confirms the alias is wired correctly even
            // though the licensed tool isn't installed in CI.
            var threw = false;
            try
            {
                context.EazfuscatorNet(context.File("./fake-input.dll"));
            }
            catch (Exception ex) when (ex.Message.IndexOf("Eazfuscator", StringComparison.OrdinalIgnoreCase) >= 0
                                       || ex.Message.IndexOf("not be found", StringComparison.OrdinalIgnoreCase) >= 0
                                       || ex.Message.IndexOf("could not locate", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                threw = true;
                context.Information("Alias resolved correctly; tool-not-found exception was: {0}", ex.Message);
            }

            if (!threw)
            {
                throw new Exception("Expected EazfuscatorNet alias to throw a tool-not-found exception (Eazfuscator.Net.exe is licensed and not installed in CI)");
            }
        }
    }
}
