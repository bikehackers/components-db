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

module Async =

  let sequence xs =
    async {
      let mutable results = []

      for x in xs do
        let! result = x
        results <- result :: results

      return results
    }

let rec private filesUnderPath (basePath : string) =
  seq {
    if Directory.Exists basePath
    then
      for f in Directory.GetFiles basePath do
        yield f

      for d in Directory.GetDirectories basePath do
        yield! filesUnderPath d
  }

async {
  // Manufacturers
  let glob = Glob.Parse "./data/manufacturers/**/*.json"

  let xs =
    filesUnderPath "./data/manufacturers"
    |> Seq.filter glob.IsMatch
    |> Seq.toList

  let! manufacturers =
    xs
    |> Seq.map (fun filePath -> async {
      printfn "%A" filePath

      let! content =
        File.ReadAllTextAsync filePath
        |> Async.AwaitTask

      let decoded =
        Decode.fromString Decode.manufacturer content
        |> Result.get

      printfn "%A" decoded

      return decoded
    })
    |> Async.sequence

  let manufacturers =
    manufacturers
    |> Seq.map (fun x -> x.Code, x)
    |> Map.ofSeq

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

  // Integrated shifters
  let glob = Glob.Parse "./data/integrated-shifters/**/*.json"

  let xs =
    filesUnderPath "./data/integrated-shifters"
    |> Seq.filter glob.IsMatch
    |> Seq.toList

  for filePath in xs do
    printfn "%A" filePath

    let! content =
      File.ReadAllTextAsync filePath
      |> Async.AwaitTask

    let decoded =
      Decode.fromString Decode.integratedShifter content
      |> Result.get

    printfn "%A" decoded

  // Chains
  let glob = Glob.Parse "./data/chains/**/*.json"

  let xs =
    filesUnderPath "./data/chains"
    |> Seq.filter glob.IsMatch
    |> Seq.toList

  for filePath in xs do
    printfn "%A" filePath

    let! content =
      File.ReadAllTextAsync filePath
      |> Async.AwaitTask

    let decoded =
      Decode.fromString Decode.chain content
      |> Result.get

    printfn "%A" decoded

  // Cassettes
  let glob = Glob.Parse "./data/cassettes/**/*.json"

  let xs =
    filesUnderPath "./data/cassettes"
    |> Seq.filter glob.IsMatch
    |> Seq.toList

  for filePath in xs do
    printfn "%A" filePath

    let! content =
      File.ReadAllTextAsync filePath
      |> Async.AwaitTask

    let decoded =
      Decode.fromString Decode.cassette content
      |> Result.get

    printfn "%A" decoded

    // Tyres
    let glob = Glob.Parse "./data/tyres/**/*.json"

    let xs =
      filesUnderPath "./data/tyres"
      |> Seq.filter glob.IsMatch
      |> Seq.toList

    for filePath in xs do
      printfn "%A" filePath

      let! content =
        File.ReadAllTextAsync filePath
        |> Async.AwaitTask

      let decoded =
        Decode.fromString Decode.tyre content
        |> Result.get

      printfn "%A" decoded
      printfn "%A" (Map.find decoded.ManufacturerCode manufacturers)

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
        Decode.fromString Decode.frame content
        |> Result.get

      printfn "%A" decoded
      printfn "%A" (Map.find decoded.ManufacturerCode manufacturers)
}
|> Async.RunSynchronously
