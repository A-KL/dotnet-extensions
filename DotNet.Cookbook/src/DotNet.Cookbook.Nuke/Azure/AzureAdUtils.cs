using Nuke.Common;
using Nuke.Common.Tooling;

using DotNet.Cookbook.Nuke.Azure.Tasks;

using static Nuke.Common.IO.FileSystemTasks;

using static DotNet.Cookbook.Nuke.Azure.Tasks.AzureTasks;
using static DotNet.Cookbook.Nuke.Azure.Tasks.AzureAdTasks;
using static DotNet.Cookbook.Nuke.Azure.Tasks.AzureOutput;

namespace DotNet.Cookbook.Nuke.Azure
{
    public static class AzureAdUtils
    {
        public static void EnsureAccess()
        {
            try
            {
                AzureAdSignedInUserShow(c => c.SetOutput(none));
            }
            catch (ProcessException error) when (error.Message.Contains("Please run 'az login' to setup account"))
            {
                Logger.Info($"AzureAD account is not setup, running az login");

                AzureLogin();
            }

            Logger.Info($"AzureAD account is setup. Ready to continue.");
        }
    }
}
