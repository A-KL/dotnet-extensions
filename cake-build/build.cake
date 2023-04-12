#addin Cake.Json&version=7.0.1
#addin Cake.Terraform&version=0.11.0
#addin Cake.AzureStorage&version=1.0.0
#addin Newtonsoft.Json&version=11.0.2

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var unlockId = Argument("unlockId", "");
var env_arg = Argument("env", "dev");
var region_arg = Argument("region", "westus2");

var rootDirectory = Directory("./");
var artifactsDirectory = rootDirectory + Directory("artifacts");

var infraDirectory = rootDirectory + Directory("infra");
var terraformDirectory = infraDirectory + Directory("terraform");
var tempDirectory = Context.Environment.GetSpecialPath(SpecialPath.LocalTemp);
var terraformOutputDirectory = artifactsDirectory + Directory("terraform");

var terraformVersion = "1.1.7";
var terraformParametersDir =  rootDirectory + Directory("tfvars");
var terraformBackendDir =  rootDirectory + Directory("backend");

var terraformOsFamily = IsRunningOnMacOs() ? "darwin" : IsRunningOnWindows() ? "windows" : "linux";
var terraformPlatform = Context.Environment.Platform.Is64Bit ? "amd64" : "386";
var terraformExecutable = $"terraform{(IsRunningOnWindows() ? ".exe" : string.Empty)}";
const string terraformPlanName = "tfplan";

var region = EnvironmentVariable("REGION") ?? region_arg;
var env = EnvironmentVariable("ENV") ?? env_arg;

var terraformParametersFile = terraformParametersDir + File($"{env}-{region}.tfvars");
var terraformJsonParametersFile = terraformParametersDir + File($"{env}-{region}.json");

var terraformBackendFile = terraformBackendDir + File("backend.tf");
var terraformJsonBackendFile = terraformBackendDir + File($"{env}-{region}.json");

string BuildPlanArguments(bool toDestroy = false, string planName = terraformPlanName) {
    return
        "plan -no-color -input=false " +
        (toDestroy ? "-destroy " : string.Empty) +
        (string.IsNullOrEmpty(planName) ? string.Empty : $"-out={planName} ");
}

string BuildInitArguments() {
    return
        "init -no-color -input=false ";
}

string BuildTerraformParameters(string envVariableName = "TF_PARAMETERS") {
    var terraformParameters = EnvironmentVariable(envVariableName);

    if (!string.IsNullOrEmpty(terraformParameters)) {
        Information($"Using parameters from '{envVariableName}' env variable.");
        return terraformParameters;
    }
    if (FileExists(terraformParametersFile)) {
        Information("Using tfvars file.");
        return $"-var-file=\"{terraformParametersFile}\"";
    }
    if (FileExists(terraformJsonParametersFile)) {
        Information("Using json file.");
        var json = (IDictionary<string, JToken>)ParseJsonFromFile(terraformJsonParametersFile);
        var vars = json.Select(x=> $"-var {x.Key}=\"{x.Value}\"");
        return string.Join(' ', vars);
    }

    Warning("No parameters were found.");
    return string.Empty;
}

string BuildBackendParameters(string envVariableName = "TF_BACKEND_PARAMETERS"){
    var terraformBackendParameters = EnvironmentVariable(envVariableName);

    if (!string.IsNullOrEmpty(terraformBackendParameters)) {
        Information($"Using backend from '{envVariableName}' env variable.");
        return terraformBackendParameters;
    }

    if (FileExists(terraformJsonBackendFile)) {
        Information($"Using backend from {terraformJsonBackendFile} file.");
        var json = (IDictionary<string, JToken>)ParseJsonFromFile(terraformJsonBackendFile);
        var vars = json.Select(x=> $"-backend-config={x.Key}={x.Value}");
        return string.Join(' ', vars);
    }

    if (FileExists(terraformBackendFile)) {
        Information($"Using backend from {terraformBackendFile} file.");
    }

    return string.Empty;
}

void Run(string tool, string args) {
    using(var process = StartAndReturnProcess(tool, new ProcessSettings{ Arguments = args } )){
        process.WaitForExit();

        var exitCode = process.GetExitCode();
        if (exitCode != 0) {
            throw new Exception("Exit code:" + exitCode);
        }
    }
}

Task("TerraformClean")
    .Does(() =>
    {
        CleanDirectory(terraformOutputDirectory);
        CleanDirectory(terraformDirectory + Directory(".terraform"));
        CleanDirectory(terraformDirectory, x => System.IO.Path.GetExtension(x.Path.FullPath) == ".exe");
        CleanDirectory(terraformDirectory, x => System.IO.Path.GetExtension(x.Path.FullPath) == ".hcl");
        CleanDirectory(terraformDirectory, x => System.IO.Path.GetFileName(x.Path.FullPath) == terraformPlanName);
    });

Task("TerraformDownload")
    .WithCriteria(() => !string.IsNullOrEmpty(terraformVersion))
    .WithCriteria(() => !string.IsNullOrEmpty(terraformOsFamily))
    .WithCriteria(() => !string.IsNullOrEmpty(terraformPlatform))
    .IsDependentOn("TerraformClean")
    .Does(() =>
    {
        var url = $"https://releases.hashicorp.com/terraform/{terraformVersion}/terraform_{terraformVersion}_{terraformOsFamily}_{terraformPlatform}.zip";

        var zipFile = System.IO.Path.GetFileName(url);
        var zipFilePath = tempDirectory + "/" + File(zipFile);

        Information($"Downloading from {url}");

        DownloadFile(url, zipFilePath);

        Information($"Downloaded to {zipFilePath}");

        EnsureDirectoryExists(terraformDirectory);

        Unzip(zipFilePath, terraformDirectory);

        Information($"Extracted to {terraformDirectory}");

        if (IsRunningOnLinux())
        {
            StartProcess("chmod", $"+rx \"{terraformDirectory.Path + "/terraform" }\"");
        }

        Context.Environment.WorkingDirectory = MakeAbsolute(terraformDirectory);
        Run("terraform", "--version");
    });

Task("TerraformInit")
    .IsDependentOn("TerraformDownload")
    .Does(() =>
{
    Run("terraform", BuildInitArguments() + BuildBackendParameters());
});

Task("TerraformPlan")
    .IsDependentOn("TerraformInit")
    .Does(() =>
{
    Run("terraform", BuildPlanArguments(toDestroy: false) + BuildTerraformParameters());
});

Task("TerraformPlanDestroy")
    .IsDependentOn("TerraformInit")
    .Does(() =>
{
    Run("terraform", BuildPlanArguments(toDestroy: true));
});

Task("TerraformApply")
    .IsDependentOn("TerraformPlan")
    .Does(() =>
{
    Run("terraform", $"apply {terraformPlanName}");
});

Task("TerraformPublish")
    //.IsDependentOn("TerraformPlan") TerraformClean
    .Does(() =>
{
    var artifactZip = terraformOutputDirectory + File(terraformPlanName + ".zip");
    Zip(terraformDirectory, artifactZip, terraformDirectory + File(terraformPlanName));
    //UploadFileToBlob(terraformArtifactsStorageSettings, artifactZip);
});

Task("TerraformForceUnlock")
    .IsDependentOn("TerraformInit")
    .Does(() =>
{
    Run("terraform", $"force-unlock {unlockId}");
});

Task("TerraformDestroy")
    .IsDependentOn("TerraformPlanDestroy")
    .Does(() =>
{
    Run("terraform", $"apply -destroy {terraformPlanName}");
});

Task("Default")
    .IsDependentOn("TerraformPlan");

RunTarget(target);
