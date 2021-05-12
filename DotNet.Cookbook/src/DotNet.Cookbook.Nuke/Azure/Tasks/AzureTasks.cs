using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using Nuke.Common.Tooling;
using JetBrains.Annotations;

namespace DotNet.Cookbook.Nuke.Azure.Tasks
{
    /// <summary>
    /// Tasks to access base Azure functionality.
    /// Intitial code is here https://github.com/arodus/nuke-azure. The latest release 0.9.0 is broken and crashes in runtime.
    /// </summary>
    public static partial class AzureTasks
    {
        /// <summary>
        ///   Path to the AzureAd executable.
        /// </summary>
        public static string AzurePath =>
            ToolPathResolver.TryGetEnvironmentExecutable("AZUREAD_EXE") ??
            ToolPathResolver.GetPathExecutable("az");

        public static Action<OutputType, string> AzureLogger { get; set; } = ProcessTasks.DefaultLogger;

        /// <summary>
        ///   <p>General Tasks.</p>
        ///   <p>For more details, visit the <a href="https://docs.microsoft.com/en-us/cli/azure/reference-index?view=azure-cli-latest">official website</a>.</p>
        /// </summary>
        public static IReadOnlyCollection<Output> AzureLogin(AzureLoginSettings toolSettings = null)
        {
            toolSettings = toolSettings ?? new AzureLoginSettings();
            var process = ProcessTasks.StartProcess(toolSettings);
            process.AssertZeroExitCode();
            return process.Output;
        }
    }

    /// <summary>
    ///   Used within <see cref="AzureTasks"/>.
    /// </summary>
    [PublicAPI]
    [ExcludeFromCodeCoverage]
    [Serializable]
    public partial class AzureLoginSettings : ToolSettings
    {
        /// <summary>
        ///   Path to the Azure executable.
        /// </summary>
        public override string ProcessToolPath => base.ProcessToolPath ?? AzureTasks.AzurePath;

        public override Action<OutputType, string> ProcessCustomLogger => AzureTasks.AzureLogger;
        /// <summary>
        ///   Support access tenants without subscriptions. It's uncommon but useful to run tenant level commands, such as 'az ad'.
        /// </summary>
        public virtual bool? AllowNoSubscriptions { get; internal set; }
        /// <summary>
        ///   Credentials like user password, or for a service principal, provide client secret or a pem file with key and public certificate. Will prompt if not given.
        /// </summary>
        public virtual string Password { get; internal set; }
        /// <summary>
        ///   The credential representing a service principal.
        /// </summary>
        public virtual bool? ServicePrincipal { get; internal set; }
        /// <summary>
        ///   The AAD tenant, must provide when using service principals.
        /// </summary>
        public virtual string Tenant { get; internal set; }
        /// <summary>
        ///   Used with a service principal configured with Subject Name and Issuer Authentication in order to support automatic certificate rolls.
        /// </summary>
        public virtual string UseCertSnIssuer { get; internal set; }
        /// <summary>
        ///   Use CLI's old authentication flow based on device code. CLI will also use this if it can't launch a browser in your behalf, e.g. in remote SSH or Cloud Shell.
        /// </summary>
        public virtual string UseDeviceCode { get; internal set; }
        /// <summary>
        ///   User name, service principal, or managed service identity ID.
        /// </summary>
        public virtual string Username { get; internal set; }
        /// <summary>
        ///   Log in using the Virtual Machine's identity.
        /// </summary>
        public virtual bool? Identity { get; internal set; }
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
              .Add("login")
              .Add("--allow-no-subscriptions", AllowNoSubscriptions)
              .Add("--password {value}", Password, secret: true)
              .Add("--service-principal", ServicePrincipal)
              .Add("--tenant {value}", Tenant)
              .Add("--use-cert-sn-issuer {value}", UseCertSnIssuer)
              .Add("--use-device-code {value}", UseDeviceCode)
              .Add("--username {value}", Username)
              .Add("--identity", Identity)
              .Add("--debug {value}", Debug)
              .Add("--help {value}", Help)
              .Add("--output {value}", Output)
              .Add("--query {value}", Query)
              .Add("--verbose {value}", Verbose);

            return base.ConfigureProcessArguments(arguments);
        }
    }

    /// <summary>
    ///   Used within <see cref="AzureTasks"/>.
    /// </summary>
    [PublicAPI]
    [Serializable]
    [ExcludeFromCodeCoverage]
    [TypeConverter(typeof(TypeConverter<AzureOutput>))]
    public partial class AzureOutput : Enumeration
    {
        public static AzureOutput json = new AzureOutput { Value = "json" };
        public static AzureOutput jsonc = new AzureOutput { Value = "jsonc" };
        public static AzureOutput table = new AzureOutput { Value = "table" };
        public static AzureOutput tsv = new AzureOutput { Value = "tsv" };
        public static AzureOutput none = new AzureOutput { Value = "none" };
    }
}