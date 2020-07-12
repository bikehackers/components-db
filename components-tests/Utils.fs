module BikeHackers.Components.Tests.Utils

module Result =

  let get =
    function
    | Ok o -> o
    | Error e -> failwith (sprintf "Expected Result.Ok but got %A" e)
