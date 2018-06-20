using System.Collections.Generic;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Eazfuscator.Net
{
    /// <summary>
    /// <para>
    /// Eazfuscator.NET is the obfuscator for .NET platform.
    /// Sure, you love your code. We all do! Chances are that you want to shelter your precious intellectual property. Eazfuscator.NET helps to protect .NET code and your valuable assets.
    /// <see href="https://www.gapotchenko.com/eazfuscator.net">More information</see>.
    /// </para>
    /// <para>
    /// In order to use the commands for this addin, you will need to have Eazfuscator installed on the machine it is
    /// running on, also you will need the following in your cake file:
    /// <code>
    /// #addin nuget:?package=Cake.Eazfuscator.Net
    /// </code>
    /// </para>
    /// </summary>
    [CakeAliasCategory("Eazfuscator.Net")]
    public static class EazfuscatorNetAliases
    {
        /// <summary>
        /// Executes <see href="https://www.gapotchenko.com/eazfuscator.net">Eazfuscator.Net.exe</see> on single file.
        /// </summary>
        /// <param name="context">The context.</param>
        [CakeMethodAlias]
        public static void EazfuscatorNet(this ICakeContext context, FilePath inputFile)
        {
            EazfuscatorNet(context, new[] { inputFile }, new EazfuscatorNetSettings());
        }

        /// <summary>
        /// Executes <see href="https://www.gapotchenko.com/eazfuscator.net">Eazfuscator.Net.exe</see> on selection of files.
        /// </summary>
        /// <param name="context">The context.</param>
        [CakeMethodAlias]
        public static void EazfuscatorNet(this ICakeContext context, IEnumerable<FilePath> inputFiles)
        {
            EazfuscatorNet(context, inputFiles, new EazfuscatorNetSettings());
        }

        /// <summary>
        /// Executes <see href="https://www.gapotchenko.com/eazfuscator.net">Eazfuscator.Net.exe</see> on single file using the specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="inputFile">The file to be obfuscated</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        public static void EazfuscatorNet(this ICakeContext context, FilePath inputFile, EazfuscatorNetSettings settings)
        {
            EazfuscatorNet(context, new[] { inputFile }, settings);
        }

        /// <summary>
        /// Executes <see href="https://www.gapotchenko.com/eazfuscator.net">Eazfuscator.Net.exe</see> on selection of files using the specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="inputFiles">The files to be obfuscated</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        public static void EazfuscatorNet(this ICakeContext context, IEnumerable<FilePath> inputFiles, EazfuscatorNetSettings settings)
        {
            var runner = new EazfuscatorNetRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            runner.Run(inputFiles, settings);
        }
    }
}
