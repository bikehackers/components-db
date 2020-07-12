module BikeHackers.Components.Tests.Thoth

open FsUnit
open Xunit
open Thoth.Json.Net
open BikeHackers.Components
open BikeHackers.Components.Thoth
open BikeHackers.Components.Tests.Utils

[<Fact>]
let ``Encode.rearDerailleur should work for a round-trip`` () =
  let x =
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
    Encode.rearDerailleur x
    |> Encode.toString 2

  let decoded =
    Decode.fromString Decode.rearDerailleur encoded
    |> Result.get

  decoded |> should equal x

[<Fact>]
let ``Encode.chain should work for a round-trip`` () =
  let x =
    {
      ManufacturerCode  = "kmc"
      ManufacturerProductCode = "BX10ELT14"
      Weight = Some 262
      Speed = 10
    }

  let encoded =
    Encode.chain x
    |> Encode.toString 2

  let decoded =
    Decode.fromString Decode.chain encoded
    |> Result.get

  decoded |> should equal x

[<Fact>]
let ``Encode.cassette should work for a round-trip`` () =
  let x =
    {
      ManufacturerCode  = "shimano"
      ManufacturerProductCode = "CS-HG50-10"
      Interface = "hyperglide"
      Sprockets = [ 11; 13; 15; 17; 19; 21; 24; 28; 32; 36 ]
      SprocketPitch = 3.95
    }

  let encoded =
    Encode.cassette x
    |> Encode.toString 2

  let decoded =
    Decode.fromString Decode.cassette encoded
    |> Result.get

  decoded |> should equal x

[<Fact>]
let ``Encode.integratedShifter should work for a round-trip`` () =
  let x =
    {
      ManufacturerCode = "shimano"
      ManufacturerProductCode = Some "ST-5700-R"
      Speed = 10
      Hand = Specific Right
      CablePull = 2.3
      Weight = Some 243
    }

  let encoded =
    Encode.integratedShifter x
    |> Encode.toString 2

  let decoded =
    Decode.fromString Decode.integratedShifter encoded
    |> Result.get

  decoded |> should equal x
