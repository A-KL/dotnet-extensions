using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using Nuke.Common.Tooling;

namespace DotNet.Cookbook.Nuke.Azure.Tasks
{
    /// <summary>
    /// Tasks to access AD functionality.
    /// Intitial code is here https://github.com/arodus/nuke-azure. The latest release 0.9.0 is broken and crashes in runtime.
    /// </summary>
    public static partial class AzureAdTasks
    {
        /// <summary>
        ///   Path to the AzureAd executable.
        /// </summary>
        public static string AzureAdPath =>
            ToolPathResolver.TryGetEnvironmentExecutable("AZUREAD_EXE") ??
            ToolPathResolver.GetPathExecutable("az");

        public static Action<OutputType, string> AzureAdLogger { get; set; } = ProcessTasks.DefaultLogger;

        /// <summary>
        ///   <p>Manage Azure Active Directory Graph entities needed for Role Based Access Control.</p>
        ///   <p>For more details, visit the <a href="https://docs.microsoft.com/en-us/cli/azure/ad?view=azure-cli-latest">official website</a>.</p>
        /// </summary>
        public static IReadOnlyCollection<Output> AzureAdSignedInUserShow(AzureAdSignedInUserShowSettings toolSettings = null)
        {
            toolSettings = toolSettings ?? new AzureAdSignedInUserShowSettings();
            var process = ProcessTasks.StartProcess(toolSettings);
            process.AssertZeroExitCode();
            return process.Output;
        }

        /// <summary>
        ///   <p>Manage Azure Active Directory Graph entities needed for Role Based Access Control.</p>
        ///   <p>For more details, visit the <a href="https://docs.microsoft.com/en-us/cli/azure/ad?view=azure-cli-latest">official website</a>.</p>
        /// </summary>
        /// <remarks>
        ///   <p>This is a <a href="http://www.nuke.build/docs/authoring-builds/cli-tools.html#fluent-apis">CLI wrapper with fluent API</a> that allows to modify the following arguments:</p>
        ///   <ul>
        ///     <li><c>--debug</c> via <see cref="AzureAdSignedInUserShowSettings.Debug"/></li>
        ///     <li><c>--help</c> via <see cref="AzureAdSignedInUserShowSettings.Help"/></li>
        ///     <li><c>--output</c> via <see cref="AzureAdSignedInUserShowSettings.Output"/></li>
        ///     <li><c>--query</c> via <see cref="AzureAdSignedInUserShowSettings.Query"/></li>
        ///     <li><c>--verbose</c> via <see cref="AzureAdSignedInUserShowSettings.Verbose"/></li>
        ///   </ul>
        /// </remarks>
        public static IReadOnlyCollection<Output> AzureAdSignedInUserShow(Configure<AzureAdSignedInUserShowSettings> configurator)
        {
            return AzureAdSignedInUserShow(configurator(new AzureAdSignedInUserShowSettings()));
        }
    }

    [PublicAPI]
    [ExcludeFromCodeCoverage]
    [Serializable]
    public partial class AzureAdSignedInUserShowSettings : ToolSettings
    {
        /// <summary>
        ///   Path to the AzureAd executable.
        /// </summary>
        public override string ProcessToolPath => base.ProcessToolPath ?? AzureAdTasks.AzureAdPath;
        public override Action<OutputType, string> ProcessCustomLogger => AzureAdTasks.AzureAdLogger;
        /// <summary>
        ///   Increase logging verbosity to show all debug logs.
        /// </summary>
        public virtual string Debug { get; internal set; }
        /// <summary>
        ///   Show this help message and exit.
        /// </summary>
        public virtual string Help { get; internal set; }
        /// <summary>
        ///   Output format.
        /// </summary>
        public virtual AzureOutput Output { get; internal set; }
        /// <summary>
        ///   JMESPath query string. See <a href="http://jmespath.org/">http://jmespath.org/</a> for more information and examples.
        /// </summary>
        public virtual string Query { get; internal set; }
        /// <summary>
        ///   Increase logging verbosity. Use --debug for full debug logs.
        /// </summary>
        public virtual string Verbose { get; internal set; }
        protected override Arguments ConfigureProcessArguments(Arguments arguments)
        {
            arguments
              .Add("ad signed-in-user show")
              .Add("--debug {value}", Debug)
              .Add("--help {value}", Help)
              .Add("--output {value}", Output)
              .Add("--query {value}", Query)
              .Add("--verbose {value}", Verbose);

            return base.ConfigureProcessArguments(arguments);
        }
    }

    /// <summary>
    ///   Used within <see cref="AzureAdTasks"/>.
    /// </summary>
    [PublicAPI]
    [ExcludeFromCodeCoverage]
    public static partial class AzureAdSignedInUserShowSettingsExtensions
    {
        #region Debug
        /// <summary>
        ///   <p><em>Sets <see cref="AzureAdSignedInUserShowSettings.Debug"/></em></p>
        ///   <p>Increase logging verbosity to show all debug logs.</p>
        /// </summary>
        [Pure]
        public static AzureAdSignedInUserShowSettings SetDebug(this AzureAdSignedInUserShowSettings toolSettings, string debug)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.Debug = debug;
            return toolSettings;
        }
        /// <summary>
        ///   <p><em>Resets <see cref="AzureAdSignedInUserShowSettings.Debug"/></em></p>
        ///   <p>Increase logging verbosity to show all debug logs.</p>
        /// </summary>
        [Pure]
        public static AzureAdSignedInUserShowSettings ResetDebug(this AzureAdSignedInUserShowSettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.Debug = null;
            return toolSettings;
        }
        #endregion
        #region Help
        /// <summary>
        ///   <p><em>Sets <see cref="AzureAdSignedInUserShowSettings.Help"/></em></p>
        ///   <p>Show this help message and exit.</p>
        /// </summary>
        [Pure]
        public static AzureAdSignedInUserShowSettings SetHelp(this AzureAdSignedInUserShowSettings toolSettings, string help)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.Help = help;
            return toolSettings;
        }
        /// <summary>
        ///   <p><em>Resets <see cref="AzureAdSignedInUserShowSettings.Help"/></em></p>
        ///   <p>Show this help message and exit.</p>
        /// </summary>
        [Pure]
        public static AzureAdSignedInUserShowSettings ResetHelp(this AzureAdSignedInUserShowSettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.Help = null;
            return toolSettings;
        }
        #endregion
        #region Output
        /// <summary>
        ///   <p><em>Sets <see cref="AzureAdSignedInUserShowSettings.Output"/></em></p>
        ///   <p>Output format.</p>
        /// </summary>
        [Pure]
        public static AzureAdSignedInUserShowSettings SetOutput(this AzureAdSignedInUserShowSettings toolSettings, AzureOutput output)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.Output = output;
            return toolSettings;
        }
        /// <summary>
        ///   <p><em>Resets <see cref="AzureAdSignedInUserShowSettings.Output"/></em></p>
        ///   <p>Output format.</p>
        /// </summary>
        [Pure]
        public static AzureAdSignedInUserShowSettings ResetOutput(this AzureAdSignedInUserShowSettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.Output = null;
            return toolSettings;
        }
        #endregion
        #region Query
        /// <summary>
        ///   <p><em>Sets <see cref="AzureAdSignedInUserShowSettings.Query"/></em></p>
        ///   <p>JMESPath query string. See <a href="http://jmespath.org/">http://jmespath.org/</a> for more information and examples.</p>
        /// </summary>
        [Pure]
        public static AzureAdSignedInUserShowSettings SetQuery(this AzureAdSignedInUserShowSettings toolSettings, string query)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.Query = query;
            return toolSettings;
        }
        /// <summary>
        ///   <p><em>Resets <see cref="AzureAdSignedInUserShowSettings.Query"/></em></p>
        ///   <p>JMESPath query string. See <a href="http://jmespath.org/">http://jmespath.org/</a> for more information and examples.</p>
        /// </summary>
        [Pure]
        public static AzureAdSignedInUserShowSettings ResetQuery(this AzureAdSignedInUserShowSettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.Query = null;
            return toolSettings;
        }
        #endregion
        #region Verbose
        /// <summary>
        ///   <p><em>Sets <see cref="AzureAdSignedInUserShowSettings.Verbose"/></em></p>
        ///   <p>Increase logging verbosity. Use --debug for full debug logs.</p>
        /// </summary>
        [Pure]
        public static AzureAdSignedInUserShowSettings SetVerbose(this AzureAdSignedInUserShowSettings toolSettings, string verbose)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.Verbose = verbose;
            return toolSettings;
        }
        /// <summary>
        ///   <p><em>Resets <see cref="AzureAdSignedInUserShowSettings.Verbose"/></em></p>
        ///   <p>Increase logging verbosity. Use --debug for full debug logs.</p>
        /// </summary>
        [Pure]
        public static AzureAdSignedInUserShowSettings ResetVerbose(this AzureAdSignedInUserShowSettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.Verbose = null;
            return toolSettings;
        }
        #endregion
    }
}
