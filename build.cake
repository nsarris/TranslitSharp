#module nuget:?package=Cake.DotNetTool.Module&version=0.3.1

#tool dotnet:?package=GitVersion.Tool&version=5.0.1

public static class Arguments {
    public const string Target = "target";
    public const string Configuration = "configuration";
    public const string Pack = "pack";
    public const string NoTests = "notests";
}

public static class Targets {
    public const string Default = "Default";
    public const string Clean = "Clean";
    public const string Restore = "Restore";
    public const string GetSemanticVersion = "SemVer";
    public const string Build = "Build";
    public const string Test = "Test";
    public const string Pack = "Pack";

    public static class AppVeyor {
        public const string SetVersion = "SetAppVeyorVersion";
        public const string PublishTestResults = "PublishAppVeyorTestResults";
        public const string PublishArtifacts = "PublishAppVeyorArtifacts";
    }
}

var target = Argument (Arguments.Target, "Default");
var configuration = Argument (Arguments.Configuration, "Release");

var assemblyVersion = "1.0.0";
var packageVersion = "1.0.0";

var solutionPath = "./";
var sourcePath = "./src";
var testsPath = "./tests";

var projectFiles = sourcePath + "/*/*.csproj";
var testProjectFiles = testsPath + "/*/*.csproj";

var artifactsDir = MakeAbsolute (Directory ("artifacts"));
var testsResultsDir = artifactsDir.Combine (Directory ("tests-results"));
var packagesDir = artifactsDir.Combine (Directory ("packages"));

Task (Targets.Clean)
    .Does (() => {
        CleanDirectory (artifactsDir);

        var settings = new DotNetCoreCleanSettings {
            Configuration = configuration
        };

        DotNetCoreClean (solutionPath, settings);
    });

Task (Targets.Restore)
    .IsDependentOn (Targets.Clean)
    .Does (() => {
        DotNetCoreRestore ();
    });

Task (Targets.GetSemanticVersion)
    .IsDependentOn (Targets.Restore)
    .Does (() => {
        var gitVersionSettings = new GitVersionSettings {
            NoFetch = true,
        };

        var gitVersion = GitVersion (gitVersionSettings);

        assemblyVersion = gitVersion.AssemblySemVer;
        packageVersion = gitVersion.NuGetVersion;

        Information ($"AssemblySemVer: {assemblyVersion}");
        Information ($"NuGetVersion: {packageVersion}");
    });

Task (Targets.AppVeyor.SetVersion)
    .IsDependentOn (Targets.GetSemanticVersion)
    .WithCriteria (() => AppVeyor.IsRunningOnAppVeyor)
    .Does (() => {
        AppVeyor.UpdateBuildVersion (packageVersion);
    });

Task (Targets.Build)
    .IsDependentOn (Targets.AppVeyor.SetVersion)
    .Does (() => {
        var settings = new DotNetCoreBuildSettings {
        Configuration = configuration,
        NoIncremental = true,
        NoRestore = true,
        MSBuildSettings = new DotNetCoreMSBuildSettings ()
            .SetVersion (assemblyVersion)
            .WithProperty ("FileVersion", packageVersion)
            .WithProperty ("InformationalVersion", packageVersion)
            .WithProperty ("nowarn", "7035")
        };

        if (IsRunningOnLinuxOrDarwin ()) {
            settings.Framework = "netstandard2.0";

            GetFiles (projectFiles)
                .ToList ()
                .ForEach (f => DotNetCoreBuild (f.FullPath, settings));

            settings.Framework = "netcoreapp2.2";

            GetFiles (testProjectFiles)
                .ToList ()
                .ForEach (f => DotNetCoreBuild (f.FullPath, settings));
        } else {
            DotNetCoreBuild (solutionPath, settings);
        }
    });

Task (Targets.Test)
    .IsDependentOn (Targets.Build)
    .WithCriteria (() => !HasArgument (Arguments.NoTests))
    .Does (() => {
        foreach (var projectFile in GetFiles (testProjectFiles)) {
            var testResultsFile = testsResultsDir.Combine ($"{projectFile.GetFilenameWithoutExtension()}.xml");
            var settings = new DotNetCoreTestSettings () {
                ResultsDirectory = testsResultsDir,
                    Logger = $"trx;logfilename={projectFile.GetFilenameWithoutExtension()}.xml",
                    Configuration = configuration,
                    NoBuild = true
            };

            if (IsRunningOnLinuxOrDarwin ())
            {
                settings.Framework = "netcoreapp2.2";
            }

            DotNetCoreTest (projectFile.FullPath, settings);
        }
    })
    .DeferOnError ();

Task (Targets.Pack)
    .IsDependentOn (Targets.Test)
    .WithCriteria (() => HasArgument (Arguments.Pack))
    .Does (() => {
        var settings = new DotNetCorePackSettings {
            Configuration = configuration,
            NoBuild = true,
            NoRestore = true,
            IncludeSymbols = true,
            OutputDirectory = packagesDir,
            MSBuildSettings = new DotNetCoreMSBuildSettings ()
				.WithProperty ("PackageVersion", packageVersion)
        };

        if (IsRunningOnLinuxOrDarwin())
        {
            settings.MSBuildSettings.WithProperty("TargetFrameworks", "netstandard2.0");
        }

        GetFiles (projectFiles)
            .ToList ()
            .ForEach (f => DotNetCorePack (f.FullPath, settings));
    });

Task (Targets.AppVeyor.PublishTestResults)
    .IsDependentOn (Targets.Test)
    .WithCriteria (() => !HasArgument (Arguments.NoTests) &&
        AppVeyor.IsRunningOnAppVeyor)
    .Does (() => {
        //CopyFiles($"{testsResultsDir}/*.xml", MakeAbsolute(Directory("./")), false);

        GetFiles ($"{testsResultsDir}/*.xml")
            .ToList ()
            .ForEach (f => AppVeyor.UploadTestResults (f, AppVeyorTestResultsType.XUnit));
    });

Task (Targets.AppVeyor.PublishArtifacts)
    .IsDependentOn (Targets.Pack)
    .WithCriteria (() => HasArgument (Arguments.Pack) && 
        AppVeyor.IsRunningOnAppVeyor &&
        EnvironmentVariable ("APPVEYOR_PULL_REQUEST_NUMBER") == null)
    .Does (() => {
        //CopyFiles($"{packagesDir}/*.nupkg", MakeAbsolute(Directory("./")), false);

        GetFiles ($"{packagesDir}/*.nupkg")
            .ToList ()
            .ForEach (f => AppVeyor.UploadArtifact (f, new AppVeyorUploadArtifactsSettings { DeploymentName = "packages" }));
    });

Task (Targets.Default)
    .IsDependentOn (Targets.Pack)
    .IsDependentOn (Targets.AppVeyor.PublishArtifacts)
    .IsDependentOn (Targets.AppVeyor.PublishTestResults);

RunTarget (target);


private bool IsRunningOnLinuxOrDarwin () {
    return Context.Environment.Platform.IsUnix ();
}