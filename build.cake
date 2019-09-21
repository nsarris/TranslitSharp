#module nuget:?package=Cake.DotNetTool.Module&version=0.3.1

#tool dotnet:?package=GitVersion.Tool&version=5.0.1

//#r Newtonsoft.Json

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

var assemblyVersion = "1.0.0";
var packageVersion = "1.0.0";

var artifactsDir = MakeAbsolute(Directory("artifacts"));
var testsResultsDir = artifactsDir.Combine(Directory("tests-results"));
var packagesDir = artifactsDir.Combine(Directory("packages"));

var solutionPath = "./TranslitSharp.sln";

Task("Clean")
    .Does(() =>
    {
        CleanDirectory(artifactsDir);

        var settings = new DotNetCoreCleanSettings
        {
            Configuration = configuration
        };

        DotNetCoreClean(solutionPath, settings);
    });

Task("Restore")
    .IsDependentOn("Clean")
    .Does(() =>
    {
        DotNetCoreRestore();
    });

Task("SemVer")
    .IsDependentOn("Restore")
    .Does(() =>
    {
        var gitVersionSettings = new GitVersionSettings
        {
            NoFetch = true,
        };

        var gitVersion = GitVersion(gitVersionSettings);

        assemblyVersion = gitVersion.AssemblySemVer;
        packageVersion = gitVersion.NuGetVersion;

        Information($"AssemblySemVer: {assemblyVersion}");
        Information($"NuGetVersion: {packageVersion}");
    });

Task("SetAppVeyorVersion")
    .IsDependentOn("Semver")
    .WithCriteria(() => AppVeyor.IsRunningOnAppVeyor)
    .Does(() =>
    {
        AppVeyor.UpdateBuildVersion(packageVersion);
    });

Task("Build")
    .IsDependentOn("SetAppVeyorVersion")
    .Does(() =>
    {
        var settings = new DotNetCoreBuildSettings
        {
            Configuration = configuration,
            NoIncremental = true,
            NoRestore = true,
            MSBuildSettings = new DotNetCoreMSBuildSettings()
                .SetVersion(assemblyVersion)
                .WithProperty("FileVersion", packageVersion)
                .WithProperty("InformationalVersion", packageVersion)
                .WithProperty("nowarn", "7035")
        };

        if (IsRunningOnLinuxOrDarwin())
        {
            settings.Framework = "netstandard2.0";

            GetFiles("./src/*/*.csproj")
                .ToList()
                .ForEach(f => DotNetCoreBuild(f.FullPath, settings));

            settings.Framework = "netcoreapp2.2";

            GetFiles("./tests/*/*Tests.csproj")
                .ToList()
                .ForEach(f => DotNetCoreBuild(f.FullPath, settings));
        }
        else
        {
            DotNetCoreBuild(solutionPath, settings);
        }
    });

Task("Test")
    .IsDependentOn("Build")
    .WithCriteria(() => !HasArgument("notests"))
    .Does(() =>
    {
        // var settings = new DotNetCoreToolSettings();

        // var argumentsBuilder = new ProcessArgumentBuilder()
        //     .Append("-configuration")
        //     .Append(configuration)
        //     .Append("-nobuild");

        // if (IsRunningOnLinuxOrDarwin())
        // {
        //     argumentsBuilder
        //         .Append("-framework")
        //         .Append("netcoreapp2.2");
        // }

        var projectFiles = GetFiles("./tests/*/*Tests.csproj");

        foreach (var projectFile in projectFiles)
        {
            var testResultsFile = testsResultsDir.Combine($"{projectFile.GetFilenameWithoutExtension()}.xml");
            //var arguments = $"{argumentsBuilder.Render()} -xml \"{testResultsFile}\"";

            //DotNetCoreTool(projectFile, "xunit", arguments, settings);

            DotNetCoreTest(
                projectFile.FullPath,
                new DotNetCoreTestSettings()
                {
                    ResultsDirectory = testsResultsDir,
                    Logger = $"trx;logfilename={projectFile.GetFilenameWithoutExtension()}.xml",
                    //WorkingDirectory = testsResultsDir,
                    Configuration = configuration,
                    NoBuild = true
                });
        }
    })
    .DeferOnError();

Task("Pack")
    .IsDependentOn("Test")
    .WithCriteria(() => HasArgument("pack"))
    .Does(() =>
    {
        var settings = new DotNetCorePackSettings
        {
            Configuration = configuration,
            NoBuild = true,
            NoRestore = true,
            IncludeSymbols = true,
            OutputDirectory = packagesDir,
            MSBuildSettings = new DotNetCoreMSBuildSettings()
                .WithProperty("PackageVersion", packageVersion)
                .WithProperty("Copyright", $"Copyright Nikos Sarris {DateTime.Now.Year}")
        };

        // if (IsRunningOnLinuxOrDarwin())
        // {
        //     settings.MSBuildSettings.WithProperty("TargetFrameworks", "netstandard2.0");
        // }

        GetFiles("./src/*/*.csproj")
            .ToList()
            .ForEach(f => DotNetCorePack(f.FullPath, settings));
    });

// Task("PublishAppVeyorArtifacts")
//     .IsDependentOn("Pack")
//     .WithCriteria(() => HasArgument("pack") && AppVeyor.IsRunningOnAppVeyor)
//     .Does(() =>
//     {
//         CopyFiles($"{packagesDir}/*.nupkg", MakeAbsolute(Directory("./")), false);

//         GetFiles($"./*.nupkg")
//             .ToList()
//             .ForEach(f => AppVeyor.UploadArtifact(f, new AppVeyorUploadArtifactsSettings { DeploymentName = "packages" }));
//     });

Task("Default")
    .IsDependentOn("Pack");

RunTarget(target);

/// <summary>
/// - No .NET Framework installed, only .NET Core
/// </summary>
private bool IsRunningOnLinuxOrDarwin()
{
    return Context.Environment.Platform.IsUnix();
}