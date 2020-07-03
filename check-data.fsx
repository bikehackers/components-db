#load @".paket/load/netcoreapp3.1/Newtonsoft.Json.fsx"
#load @".paket/load/netcoreapp3.1/main.group.fsx"

#load @"components/Types.fs"
#load @"components/Thoth.fs"

open System
open System.IO
open Thoth.Json.Net
open BikeHackers.Components
open BikeHackers.Components.Thoth
open DotNet.Globbing

module Result =

  let get =
    function
    | Ok o -> o
    | Error e -> failwith (sprintf "Expected Result.Ok but got %A" e)

let rec private filesUnderPath (basePath : string) =
  seq {
    if Directory.Exists basePath
    then
      for f in Directory.GetFiles (basePath) do
        yield f

      for d in Directory.GetDirectories (basePath) do
        yield! filesUnderPath d
  }

async {
  // Manufacturers
  let glob = Glob.Parse "./data/manufacturers/**/*.json"

  let xs =
    filesUnderPath "./data/manufacturers"
    |> Seq.filter glob.IsMatch
    |> Seq.toList

  for filePath in xs do
    printfn "%A" filePath

    let! content =
      File.ReadAllTextAsync filePath
      |> Async.AwaitTask

    let decoded =
      Decode.fromString Decode.manufacturer content
      |> Result.get

    printfn "%A" decoded

  // Frames
  let glob = Glob.Parse "./data/frames/**/*.json"

  let xs =
    filesUnderPath "./data/frames"
    |> Seq.filter glob.IsMatch
    |> Seq.toList

  for filePath in xs do
    printfn "%A" filePath

    let! content =
      File.ReadAllTextAsync filePath
      |> Async.AwaitTask

    let decoded =
      Decode.fromString Decode.frameMeasurements content
      |> Result.get

    printfn "%A" decoded

  // Rear Derailleurs
  let glob = Glob.Parse "./data/rear-derailleurs/**/*.json"

  let xs =
    filesUnderPath "./data/rear-derailleurs"
    |> Seq.filter glob.IsMatch
    |> Seq.toList

  for filePath in xs do
    printfn "%A" filePath

    let! content =
      File.ReadAllTextAsync filePath
      |> Async.AwaitTask

    let decoded =
      Decode.fromString Decode.rearDerailleur content
      |> Result.get

    printfn "%A" decoded

  // Seat Posts
  let glob = Glob.Parse "./data/seatposts/**/*.json"

  let xs =
    filesUnderPath "./data/seatposts"
    |> Seq.filter glob.IsMatch
    |> Seq.toList

  for filePath in xs do
    printfn "%A" filePath

    let! content =
      File.ReadAllTextAsync filePath
      |> Async.AwaitTask

    let decoded =
      Decode.fromString Decode.seatpost content
      |> Result.get

    printfn "%A" decoded

  // Handlebars
  let glob = Glob.Parse "./data/bars/**/*.json"

  let xs =
    filesUnderPath "./data/bars"
    |> Seq.filter glob.IsMatch
    |> Seq.toList

  for filePath in xs do
    printfn "%A" filePath

    let! content =
      File.ReadAllTextAsync filePath
      |> Async.AwaitTask

    let decoded =
      Decode.fromString Decode.dropHandleBar content
      |> Result.get

    printfn "%A" decoded
}
|> Async.RunSynchronously
