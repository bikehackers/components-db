namespace BikeHackers.Components.Thoth

open System
open BikeHackers.Components
open Thoth.Json.Net

module Decode =

  let private tryParseInt (x : string) =
    match Int32.TryParse x with
    | (true, x) -> Some x
    | _ -> None

  let manufacturer : Decoder<_> =
    Decode.object
      (fun get ->
        {
          Code = get.Required.Field "code" Decode.string
          Name = get.Required.Field "name" Decode.string
          Url = get.Optional.Field "url" Decode.string
        })

  let seatpostSize : Decoder<SeatPostSize> =
    Decode.object
      (fun get ->
        {
          ManufacturerProductCode = get.Optional.Field "manufacturerProduceCode" Decode.string
          Diameter = get.Required.Field "diameter" Decode.float
          Offset = get.Required.Field "offset" Decode.int
          Length = get.Required.Field "length" Decode.int
          Weight = get.Optional.Field "weight" Decode.int
        })

  let seatpost : Decoder<SeatPost> =
    Decode.object
      (fun get ->
        {
          ManufacturerCode = get.Required.Field "manufacturerCode" Decode.string
          Name = get.Required.Field "name" Decode.string
          Sizes = get.Required.Field "sizes" (Decode.list seatpostSize)
        })

  let dropHandleBarSize : Decoder<DropHandleBarSize> =
    Decode.object
      (fun get ->
        {
          ManufacturerProductCode = get.Optional.Field "manufacturerProductCode" Decode.string
          ClampDiameter = get.Required.Field "clampDiameter" Decode.float
          ClampAreaWidth = get.Optional.Field "clampAreaWidth" Decode.float
          Width = get.Optional.Field "width" Decode.float
          Drop = get.Optional.Field "drop" Decode.float
          Reach = get.Optional.Field "reach" Decode.float
          DropFlare = get.Optional.Field "dropFlare" Decode.float
          DropFlareOut = get.Optional.Field "dropFlareOut" Decode.float
          Rise = get.Optional.Field "rise" Decode.float
          OutsideWidth = get.Optional.Field "outsideWidth" Decode.float
          Weight = get.Optional.Field "weight" Decode.int
        })

  let dropHandleBar : Decoder<DropHandleBar> =
    Decode.object
      (fun get ->
        {
          ManufacturerCode = get.Required.Field "manufacturerCode" Decode.string
          Name = get.Required.Field "name" Decode.string
          Sizes = get.Required.Field "sizes" (Decode.list dropHandleBarSize)
        })

  let actuationRatio =
    Decode.string
    |> Decode.andThen (fun s ->
      let err = sprintf "%s must be of the format `int:int`" s

      match s.Split (":") with
      | [| l; r |] ->
        match tryParseInt l, tryParseInt r with
        | (Some l, Some r) -> Decode.succeed (l, r)
        | _ -> Decode.fail err
      | _ -> Decode.fail err
    )

  let rearDerailleur : Decoder<_> =
    Decode.object
      (fun get ->
        {
          Manufacturer = get.Required.Field "manufacturer" Decode.string
          ProductCode = get.Required.Field "productCode" Decode.string
          ActuationRatio = get.Required.Field "actuationRatio" actuationRatio
          Speed = get.Required.Field "speeds" Decode.int
          Weight = get.Required.Field "weight" Decode.int
          LargestSprocketMaxTeeth = get.Required.Field "largestSprocketMaxTeeth" Decode.int
          LargestSprocketMinTeeth = get.Required.Field "largestSprocketMinTeeth" Decode.int
          SmallestSprocketMaxTeeth = get.Required.Field "smallestSprocketMaxTeeth" Decode.int
          SmallestSprocketMinTeeth = get.Required.Field "smallestSprocketMinTeeth" Decode.int
          Capacity = get.Required.Field "capacity" Decode.int
          Clutched = get.Required.Field "isClutched" Decode.bool
        }
      )

  let frameMeasurements : Decoder<FrameMeasurements> =
    Decode.object
      (fun get ->
        {
          Stack = get.Optional.Field "stack" Decode.float
          Reach = get.Optional.Field "reach" Decode.float
          TopTubeActual = get.Optional.Field "topTubeActual" Decode.float
          TopTubeEffective = get.Optional.Field "topTubeEffective" Decode.float
          SeatTubeCenterToTop = get.Optional.Field "seatTubeCenterToTop" Decode.float
          HeadTubeLength = get.Optional.Field "headTubeLength" Decode.float
          HeadTubeAngle = get.Optional.Field "headTubeAngle" Decode.float
          SeatTubeAngle = get.Optional.Field "seatTubeAngle" Decode.float
          BottomBracketDrop = get.Optional.Field "bottomBracketDrop" Decode.float
          ForkLength = get.Optional.Field "forkLength" Decode.float
          StandoverHeight = get.Optional.Field "standoverHeight" Decode.float
          ForkRake = get.Optional.Field "forkRake" Decode.float
          ForkAxleToCrown = get.Optional.Field "forkAxleToCrown" Decode.float
          ChainStayLength = get.Optional.Field "chainStayLength" Decode.float
          Wheelbase = get.Optional.Field "bottomBracketDrop" Decode.float
          SeatPostDiameter = get.Optional.Field "seatPostDiameter" Decode.float
        }
      )

  let handedness : Decoder<Handedness> =
    Decode.string
    |> Decode.andThen (
      function
      | "right" -> Handedness.Specific Right |> Decode.succeed
      | "left" -> Handedness.Specific Left |> Decode.succeed
      | x ->
        sprintf "Expected \"left\", \"right\" or \"ambidextrous\" but got \"%s\"" x
        |> Decode.fail
    )

  let integratedShifter : Decoder<IntegratedShifter> =
    Decode.object
      (fun get ->
        {
          ManufacturerCode = get.Required.Field "manufacturerCode" Decode.string
          ManufacturerProductCode = get.Optional.Field "manufacturerProductCode" Decode.string
          Speed = get.Required.Field "speed" Decode.int
          CablePull = get.Required.Field "cablePull" Decode.float
          Hand = get.Required.Field "hand" handedness
          Weight = get.Optional.Field "weight" Decode.int
        })

  let chain : Decoder<Chain> =
    Decode.object
      (fun get ->
        {
          ManufacturerCode = get.Required.Field "manufacturerCode" Decode.string
          ManufacturerProductCode = get.Required.Field "manufacturerProductCode" Decode.string
          Speed = get.Required.Field "speed" Decode.int
          Weight = get.Optional.Field "weight" Decode.int
        })

  let cassette : Decoder<Cassette> =
    Decode.object
      (fun get ->
        {
          ManufacturerCode = get.Required.Field "manufacturerCode" Decode.string
          ManufacturerProductCode = get.Required.Field "manufacturerProductCode" Decode.string
          Sprockets = get.Required.Field "sprockets" (Decode.list Decode.int)
          SprocketPitch = get.Required.Field "sprocketPitch" Decode.float
          Interface = get.Required.Field "interface" Decode.string
        })

module CaliperRimBrake =

  let private decodePadCartridgeType : Decoder<_> =
    Decode.string
    |> Decode.andThen
      (function
      | "shimano" -> Decode.succeed Shimano
      | "campagnolo" -> Decode.succeed Campagnolo
      | "campagnolo-old" -> Decode.succeed CampagnoloOld
      | x -> Decode.fail (sprintf "Unknown pad cartridge type %s" x))

  let private decodeFixingType : Decoder<_> =
    Decode.string
    |> Decode.andThen
      (function
      | "recessed" -> Decode.succeed RecessedAllenBolt
      | "traditional" -> Decode.succeed TraditionalNut
      | x -> Decode.fail (sprintf "Unknown brake fixing type %s" x))

  let decoder : Decoder<CaliperRimBrake> =
    Decode.object
      (fun get ->
        {
          MinimumReach = get.Required.Field "minimumReach" Decode.int
          MaximumReach = get.Required.Field "maximumReach" Decode.int
          PadCartridgeType = get.Required.Field "padCartridgeType" decodePadCartridgeType
          FixingType = get.Required.Field "fixingType" decodeFixingType
        }
      )
