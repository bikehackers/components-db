module BikeHackers.Components.Tests.Thoth

open FsUnit
open Xunit
open Thoth.Json.Net
open BikeHackers.Components
open BikeHackers.Components.Thoth
open BikeHackers.Components.Tests.Utils

[<Fact>]
let ``Encode.rearDerailleur should work for a round-trip`` () =
  let rd =
    {
      Manufacturer  = "shimano"
      ProductCode = "RD-RX812"
      ActuationRatio = 27, 10
      Capacity = 31
      LargestSprocketMaxTeeth = 42
      LargestSprocketMinTeeth = 40
      SmallestSprocketMaxTeeth = 11
      SmallestSprocketMinTeeth = 11
      Clutched = true
      Weight = 267
      Speed = 11
    }

  let encoded =
    Encode.rearDerailleur rd
    |> Encode.toString 2

  let decoded =
    Decode.fromString Decode.rearDerailleur encoded
    |> Result.get

  decoded |> should equal rd

