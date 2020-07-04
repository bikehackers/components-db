open System
open System.IO
open Thoth.Json.Net
open DotNet.Globbing
open Fable.Core
open Fable.React
open Fable.React.Props
open Fable.React.Isomorphic
open BikeHackers.Components
open BikeHackers.Components.Thoth

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

let renderRows (x : DropHandleBar) =
  seq {
    for s in x.Sizes do
      tr
        []
        [
          td [] [ str (Option.defaultValue "-" x.ManufacturerCode) ]
        ]
  }

[<EntryPoint>]
let main argv =
  async {
    // Handlebars
    let glob = Glob.Parse "./data/bars/**/*.json"

    let xs =
      filesUnderPath "./data/bars"
      |> Seq.filter glob.IsMatch
      |> Seq.toList

    let! bars =
      xs
      |> Seq.map (fun path ->
        async {
          let! content =
            File.ReadAllTextAsync path
            |> Async.AwaitTask

          let decoded =
            Decode.fromString Decode.dropHandleBar content
            |> Result.get

          return decoded
        }
      )
      |> Async.Parallel

    let page =
      html
        []
        [
          head
            []
            [
              meta
                [
                  CharSet "UTF-8"
                ]
              title
                []
                [
                  str "Drop Handlebar Database"
                ]
              link
                [
                  Rel "stylesheet"
                  Href "https://cdn.jsdelivr.net/npm/@exampledev/new.css@1.1.2/new.min.css"
                ]
            ]

          h1 [] [ str "Drop Handlebar Database" ]
          table
            []
            (seq {
              yield
                tr
                  []
                  [
                    th [] [ str "Manufacturer Code" ]
                  ]

              yield!
                bars
                |> Seq.collect renderRows
                |> Seq.toList
            } |> Seq.toList)
        ]

    let contents = Fable.ReactServer.renderToString page

    do!
      File.WriteAllTextAsync ("./index.html", contents)
      |> Async.AwaitTask

    ()
  }
  |> Async.RunSynchronously

  0
