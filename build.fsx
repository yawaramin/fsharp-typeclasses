// include Fake libs
#r "./packages/FAKE/tools/FakeLib.dll"

open Fake
open Fake.Testing

// Directories
let buildDir  = "./build/"
let deployDir = "./deploy/"

// Filesets
let appReferences  =
    !! "/**/*.csproj"
    ++ "/**/*.fsproj"

// version info
let version = "0.1"  // or retrieve from CI server

// Targets
Target ? Clean <- fun _ -> CleanDirs [buildDir; deployDir]
Target ? Build <- fun _ ->
  MSBuildDebug buildDir "Build" appReferences |> Log "AppBuild-Output: "

Target ? BuildTest <- fun _ ->
  !! ("src/FSharpTypeclasses/**/*.fsproj")
    |> MSBuildDebug buildDir "Build"
    |> Log "TestBuild-Output: "

Target ? Test <- fun _ ->
  !! (buildDir + "FSharpTypeclassesTest.dll")
    |> NUnit3 (fun p ->
      { p with
          ToolPath =
            "packages/NUnit.ConsoleRunner/tools/nunit3-console.exe" })

Target ? Deploy <- fun _ ->
  !! (buildDir + "/**/*.*")
  -- "*.zip"
  |> Zip buildDir (deployDir + "ApplicationName." + version + ".zip")

// Build order
"Clean"
  ==> "Build"
  ==> "BuildTest"
  ==> "Test"
  ==> "Deploy"

// start build
RunTargetOrDefault ? Build
