module BikeHackers.Components.Tests.Thoth

open System
open FsUnit
open Xunit
open BikeHackers.Components
open BikeHackers.Components.Thoth
open BikeHackers.Components.Tests.Utils

#if FABLE_COMPILER
open Thoth.Json
#else
open Thoth.Json.Net
#endif

[<Fact>]
let ``Encode.rearDerailleur should work for a round-trip`` () =
  let x =
    {
      Manufacturer  = "shimano"
      ProductCode = "RD-RX812"
      ActuationRatio = 27, 10
      Capacity = Some 31
      LargestSprocketMaxTeeth = 42
      LargestSprocketMinTeeth = Some 40
      SmallestSprocketMaxTeeth = Some 11
      SmallestSprocketMinTeeth = Some 11
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

[<Fact>]
let ``Encode.dropHandleBar should work for a round-trip`` () =
  let x =
    {
      ManufacturerCode = "ritchey"
      Name = "WCS ErgoMax Handlebar"
      Sizes =
        [
          {
            ManufacturerProductCode = Some "30355008016"
            NominalSize = "42cm"
            ClampAreaWidth = Some 80.0
            ClampDiameter = 31.8
            Width = Some 420.0
            Drop = Some 128.0
            Reach = Some 73.0
            Rise = Some 10.0
            Sweep = Some 5.0
            DropFlare = Some 12.0
            DropFlareOut = Some 3.0
            OutsideWidth = Some 503.0
            Weight = None
          }
        ]
    }

  let encoded =
    Encode.dropHandleBar x
    |> Encode.toString 2

  let decoded =
    Decode.fromString Decode.dropHandleBar encoded
    |> Result.get

  decoded |> should equal x

[<Fact>]
let ``Encode.tyre should work for a round-trip`` () =
  let x =
    {
      ID = Guid.Parse "6993be65-008d-409f-8880-1311c6b9886e"
      ManufacturerCode = "pirelli"
      ManufacturerProductCode = None
      Name = "Cinturato Gravel M"
      Application = Some (Set.ofSeq [ RoughGravel ])
      Sizes = [
        {
          ManufacturerProductCode = None
          BeadSeatDiameter = 622
          Width = 45
          TreadColor = "black"
          SidewallColor = "brown"
          Weight = Some 570
          Type = Tubeless
          Tpi = Some 127
        }
      ]
    }

  let encoded =
    Encode.tyre x
    |> Encode.toString 2

  let decoded =
    Decode.fromString Decode.tyre encoded
    |> Result.get

  decoded |> should equal x

[<Fact>]
let ``Decode.code should work for simple cases`` () =
  let cases =
    [
      "abc", true
      "0123", false
      "1abc", false
      "a_b_c", false
      "a-b-c", true
      "abc-123", true
      "abc-def123", true
    ]

  for case in cases do
    let input, isValidCode = case

    let expected =
      if isValidCode
      then
        Some input
      else
        None

    let actual =
      Decode.fromString Decode.code (sprintf "\"%s\"" input)
      |> function
        | Ok x -> Some x
        | Error _ -> None

    actual |> should equal expected

[<Fact>]
let ``Encode.frameMeasurements should work for a round-trip`` () =
  let x =
    {
      Stack = Some 599.0
      Reach = Some 401.0
      TopTubeActual = Some  569.0
      TopTubeEffective = Some 583.0
      SeatTubeCenterToTop = Some 550.0
      SeatTubeCenterToCenter = None
      SeatTubeAngle = Some 73.0
      HeadTubeLength = Some 180.0
      HeadTubeAngle = Some 73.0
      BottomBracketDrop = Some 70.0
      ChainStayLength = Some 437.0
      Wheelbase = Some 1047.0
      ForkAxleToCrown = Some 383.0
      ForkRake = Some 47.0
      ForkLength = None
      SeatPostDiameter = Some 27.2
      StandoverHeight = Some 820.0
      FrontTyreClearance =
        Map.empty
        |> Map.add 622 40
      RearTyreClearance =
        Map.empty
        |> Map.add 622 40
    }

  let encoded =
    Encode.frameMeasurements x
    |> Encode.toString 2

  let decoded =
    Decode.fromString Decode.frameMeasurements encoded
    |> Result.get

  decoded |> should equal x

[<Fact>]
let ``Encode.frame should work for a round-trip`` () =
  let x =
    {
      ID = Guid.NewGuid ()
      Code = "abcdef"
      Name = "abcdef"
      ManufacturerCode = "xyz"
      ManufacturerProductCode = Some "xyz"
      ManufacturerRevision = Some "ijk"
      HasFenderMounts = false
      HasSeatTubeBottleCageMounts = false
      HasDownTubeBottleCageMounts = false
      HasUnderDownTubeBottleCageMounts = false
      HasForkCageMounts = false
      HasTopTubeBagMounts = false
      HasFrontRackMounts = false
      HasRearRackMounts = false
      Sizes =
        [
          {
            Name = "L"
            Code = "l"
            Measurements =
              {
                Stack = Some 599.0
                Reach = Some 401.0
                TopTubeActual = Some  569.0
                TopTubeEffective = Some 583.0
                SeatTubeCenterToTop = Some 550.0
                SeatTubeCenterToCenter = None
                SeatTubeAngle = Some 73.0
                HeadTubeLength = Some 180.0
                HeadTubeAngle = Some 73.0
                BottomBracketDrop = Some 70.0
                ChainStayLength = Some 437.0
                Wheelbase = Some 1047.0
                ForkAxleToCrown = Some 383.0
                ForkRake = Some 47.0
                ForkLength = None
                SeatPostDiameter = Some 27.2
                StandoverHeight = Some 820.0
                FrontTyreClearance =
                  Map.empty
                  |> Map.add 622 40
                RearTyreClearance =
                  Map.empty
                  |> Map.add 622 40
              }
          }
        ]
      Sources =
        [
          "https://example.com"
        ]
    }

  let encoded =
    Encode.frame x
    |> Encode.toString 2

  let decoded =
    Decode.fromString Decode.frame encoded
    |> Result.get

  decoded |> should equal x
