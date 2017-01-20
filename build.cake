///////////////////////////////////////////////////////////////////////////////
// Tools and Addins
///////////////////////////////////////////////////////////////////////////////

///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var buildFolder = MakeAbsolute(Directory(Argument("buildFolder", "./build")));

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

Setup(ctx =>
{
	// Executed BEFORE the first task.
	Information("Swashbuckle ASP.NET API Versioning Example");
});

Teardown(ctx =>
{
	// Executed AFTER the last task.
	Information("Finished running tasks.");
});

///////////////////////////////////////////////////////////////////////////////
// USER TASKS
// PUT ALL YOUR BUILD GOODNESS IN HERE
///////////////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
	{
		CleanDirectory(buildFolder);
	});
	
Task("Restore")
	.IsDependentOn("Clean").Does(() =>
	{
		DotNetCoreRestore("./src/SwashbuckleAspNetApiVersioningExample.sln");
	});

Task("Build")
	.IsDependentOn("Restore")
	.Does(() =>
	{
		Information("Running Build...");
		DotNetCoreBuild("./src/SwashbuckleAspNetApiVersioningExample.sln", new DotNetCoreBuildSettings {
			Configuration = "Release",
			OutputDirectory = buildFolder.ToString()
		});
	});
	
Task("Default")
    .IsDependentOn("Build");

RunTarget(target);