using DiffEngine;
using System.Runtime.CompilerServices;

namespace MovieRating.Test
{
    internal class GlobalSetups
    {
        internal static readonly string DefaultSnapshotsFielsPath = @"Snapshots";

        [ModuleInitializer]
        internal static void ModuleInitializer()
        {
            Environment.SetEnvironmentVariable("Verify_DisableClipboard", "true");
            DiffTools.UseOrder(DiffTool.VisualStudio, DiffTool.Rider);
        }

        internal static VerifySettings GetMovieVerifySettings()
        {
            var settings = new VerifySettings();
            settings.UseDirectory(Path.Combine(DefaultSnapshotsFielsPath, "movie"));
            return settings;
        }

        internal static VerifySettings GetActorVerifySettings()
        {
            var settings = new VerifySettings();
            settings.UseDirectory(Path.Combine(DefaultSnapshotsFielsPath, "actor"));
            return settings;
        }
    }
}
