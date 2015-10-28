// include Fake lib
#r @"packages\FAKE\tools\FakeLib.dll"
open System
open Fake

// Properties
let buildDir = "./build/"

// Targets
Target "Clean" (fun _ ->
  //trace "Cleaning..."
  CleanDir buildDir
)


Target "BuildApp" (fun _ ->
  //!! "**/*.csproj"
  //  |> MSBuildDebug buildDir "Build"
  //  |> Log "AppBuild-Output: "
  //!! "FSharpKoans.sln"
    //|> MSBuildDebug "" "Build"
    MSBuildDebug "" "Build" ["FSharpKoans.sln"]
    |> Log "AppBuild-Output: "
)

Target "RebuildApp" (fun _ ->
  Run "Clean"
  Run "BuildApp"
)

Target "Watch" (fun _ ->
    use watcher = !! "FSharpKoans/*.fs" |> WatchChanges (fun changes ->
        tracefn "changes (%d): %A" (Seq.length <| changes) changes
        //if Seq.exists (fun i -> i.Status <> FileStatus.Deleted) changes then
        Run "BuildApp"
        //ExecProcessRedirected (fun psi -> ()) (TimeSpan.FromSeconds(5.0)) |> ignore
        Shell.Exec "FSharpKoans/bin/Debug/FSharpKoans.exe" |> ignore
    )

    System.Console.ReadLine() |> ignore //Needed to keep FAKE from exiting

    watcher.Dispose() // Use to stop the watch from elsewhere, ie another task.
)

// Default target
Target "Default" (fun _ ->
    trace "Hello World from FAKE"
)

// Dependencies

//"Clean"
"BuildApp"
  ==> "Default"


// start build
RunTargetOrDefault "Default"
