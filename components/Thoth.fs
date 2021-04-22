namespace BikeHackers.Components.Thoth

open System
open BikeHackers.Components

#if FABLE_COMPILER
open Thoth.Json
#else
open Thoth.Json.Net
#endif

module Extras =

  module Decode =

    let set decoder : Decoder<Set<_>> =
      Decode.list decoder
      |> Decode.map Set.ofList

module Encode =

  open Extras

  let actuationRatio =
    (fun (x, y) -> Encode.string (sprintf "%i:%i" x y))

  let rearDerailleur : Encoder<RearDerailleur> =
    (fun x ->
      Encode.object
        [
          "manufacturer", Encode.string x.Manufacturer
          "productCode", Encode.string x.ProductCode
          "actuationRatio", actuationRatio x.ActuationRatio
          "speeds", Encode.int x.Speed
          "weight", Encode.int x.Weight
          "largestSprocketMaxTeeth", Encode.int x.LargestSprocketMaxTeeth
          "largestSprocketMinTeeth", (Encode.option Encode.int) x.LargestSprocketMinTeeth
          "smallestSprocketMaxTeeth", (Encode.option Encode.int) x.SmallestSprocketMaxTeeth
          "smallestSprocketMinTeeth", (Encode.option Encode.int) x.SmallestSprocketMinTeeth
          "capacity", (Encode.option Encode.int) x.Capacity
          "isClutched", Encode.bool x.Clutched
        ]
    )

  let chain : Encoder<Chain> =
    (fun x ->
      Encode.object
        [
          "manufacturerCode", Encode.string x.ManufacturerCode
          "manufacturerProductCode", Encode.string x.ManufacturerProductCode
          "speed", Encode.int x.Speed
          "weight", (Encode.option Encode.int) x.Weight
        ])

  let cassette : Encoder<Cassette> =
    (fun x ->
      Encode.object
        [
          "manufacturerCode", Encode.string x.ManufacturerCode
          "manufacturerProductCode", Encode.string x.ManufacturerProductCode
          "interface", Encode.string x.Interface
          "sprockets", Encode.list (x.Sprockets |> List.map Encode.int)
          "sprocketPitch", Encode.float x.SprocketPitch
        ])

  let handedness : Encoder<Handedness> =
    (fun x ->
      match x with
      | Ambi -> Encode.string "ambidextrous"
      | Specific Left -> Encode.string "left"
      | Specific Right -> Encode.string "right")

  let integratedShifter : Encoder<IntegratedShifter> =
    (fun x ->
      Encode.object
        [
          "manufacturerCode", Encode.string x.ManufacturerCode
          "manufacturerProductCode", (Encode.option Encode.string) x.ManufacturerProductCode
          "speed", Encode.int x.Speed
          "cablePull", Encode.float x.CablePull
          "hand", handedness x.Hand
          "weight", (Encode.option Encode.int) x.Weight
        ])

  let dropHandleBarSize : Encoder<DropHandleBarSize> =
    (fun x ->
      Encode.object
        [
          "manufacturerProductCode", (Encode.option Encode.string) x.ManufacturerProductCode
          "nominalSize", Encode.string x.NominalSize
          "clampDiameter", Encode.float x.ClampDiameter
          "clampAreaWidth", (Encode.option Encode.float) x.ClampAreaWidth
          "drop", (Encode.option Encode.float) x.Drop
          "reach", (Encode.option Encode.float) x.Reach
          "width", (Encode.option Encode.float) x.Width
          "dropFlare", (Encode.option Encode.float) x.DropFlare
          "dropFlareOut", (Encode.option Encode.float) x.DropFlareOut
          "rise", (Encode.option Encode.float) x.Rise
          "sweep", (Encode.option Encode.float) x.Sweep
          "outsideWidth", (Encode.option Encode.float) x.OutsideWidth
          "weight", (Encode.option Encode.int) x.Weight
        ])

  let dropHandleBar : Encoder<DropHandleBar> =
    (fun x ->
      Encode.object
        [
          "manufacturerCode", Encode.string x.ManufacturerCode
          "name", Encode.string x.Name
          "sizes", Encode.list (x.Sizes |> List.map dropHandleBarSize)
        ])

  let tyreType : Encoder<TyreType> =
    function
    | Clincher -> "clincher"
    | Tubeless -> "tubeless"
    | Tubular -> "tubular"
    >> Encode.string

  let tyreApplication : Encoder<TyreApplication> =
    function
    | Track -> "track"
    | Road -> "road"
    | RoughRoad -> "rough-road"
    | LightGravel -> "light-gravel"
    | RoughGravel -> "rough-gravel"
    | Singletrack -> "singletrack"
    >> Encode.string

  let tyreSize : Encoder<TyreSize> =
    (fun x ->
      Encode.object
        [
          "manufacturerProductCode", (Encode.option Encode.string) x.ManufacturerProductCode
          "bsd", Encode.int x.BeadSeatDiameter
          "type", tyreType x.Type
          "width", Encode.int x.Width
          "weight", (Encode.option Encode.int) x.Weight
          "treadColor", Encode.string x.TreadColor
          "sidewallColor", Encode.string x.SidewallColor
          "tpi", (Encode.option Encode.int) x.Tpi
        ])

  let tyre : Encoder<Tyre> =
    (fun x ->
      Encode.object
        [
          "id", Encode.guid x.ID
          "manufacturerCode", Encode.string x.ManufacturerCode
          "manufacturerProductCode", (Encode.option Encode.string) x.ManufacturerProductCode
          "name", Encode.string x.Name
          "sizes", Encode.list (x.Sizes |> List.map tyreSize)
          "application", Encode.option Encode.list (x.Application |> Option.map (Seq.toList >> List.map tyreApplication))
        ])

  let tyreClearance : Encoder<Map<int, int>> =
    (fun x ->
      x
      |> Map.toSeq
      |> Seq.map (fun (k, v) ->
        Encode.object
          [
            "bsd", Encode.int k
            "width", Encode.int v
          ]
      )
      |> Encode.seq)

  let frameMeasurements : Encoder<FrameMeasurements> =
    (fun x ->
      Encode.object
        [
          "stack", (Encode.option Encode.float) x.Stack
          "reach", (Encode.option Encode.float) x.Reach
          "topTubeActual", (Encode.option Encode.float) x.TopTubeActual
          "topTubeEffective", (Encode.option Encode.float) x.TopTubeEffective
          "seatTubeCenterToTop", (Encode.option Encode.float) x.SeatTubeCenterToTop
          "seatTubeCenterToCenter", (Encode.option Encode.float) x.SeatTubeCenterToCenter
          "headTubeLength", (Encode.option Encode.float) x.HeadTubeLength
          "headTubeAngle", (Encode.option Encode.float) x.HeadTubeAngle
          "seatTubeAngle", (Encode.option Encode.float) x.SeatTubeAngle
          "bottomBracketDrop", (Encode.option Encode.float) x.BottomBracketDrop
          "forkAxleToCrown", (Encode.option Encode.float) x.ForkAxleToCrown
          "forkRake", (Encode.option Encode.float) x.ForkRake
          "forkLength", (Encode.option Encode.float) x.ForkLength
          "wheelbase", (Encode.option Encode.float) x.Wheelbase
          "standoverHeight", (Encode.option Encode.float) x.StandoverHeight
          "seatPostDiameter", (Encode.option Encode.float) x.SeatPostDiameter
          "chainStayLength", (Encode.option Encode.float) x.ChainStayLength
          "frontTyreClearance", tyreClearance x.FrontTyreClearance
          "rearTyreClearance", tyreClearance x.RearTyreClearance
      ])

  let frameSize : Encoder<FrameSize> =
    (fun x ->
      Encode.object
        [
          "code", Encode.string x.Code
          "name", Encode.string x.Name
          "measurements", frameMeasurements x.Measurements
        ])

  let frame : Encoder<Frame> =
    (fun x ->
      Encode.object
        [
          "id", Encode.guid x.ID
          "code", Encode.string x.Code
          "name", Encode.string x.Name
          "manufacturerCode", Encode.string x.ManufacturerCode
          "manufacturerProductCode", (Encode.option Encode.string) x.ManufacturerProductCode
          "manufacturerRevision", (Encode.option Encode.string) x.ManufacturerRevision
          "hasDownTubeBottleCageMounts", Encode.bool x.HasDownTubeBottleCageMounts
          "hasFenderMounts", Encode.bool x.HasFenderMounts
          "hasForkCageMounts", Encode.bool x.HasForkCageMounts
          "hasFrontRackMounts", Encode.bool x.HasFrontRackMounts
          "hasRearRackMounts", Encode.bool x.HasRearRackMounts
          "hasSeatTubeBottleCageMounts", Encode.bool x.HasSeatTubeBottleCageMounts
          "hasTopTubeBagMounts", Encode.bool x.HasTopTubeBagMounts
          "hasUnderDownTubeBottleCageMounts", Encode.bool x.HasUnderDownTubeBottleCageMounts
          "sizes", Encode.list (List.map frameSize x.Sizes)
          "sources", Encode.list (List.map Encode.string x.Sources)
        ])

module Decode =

  open System.Text.RegularExpressions
  open Extras

  let code : Decoder<string> =
    Decode.string
    |> Decode.andThen (fun x ->
      let pattern = Regex "^[a-z][a-z0-9]*(-[a-z0-9]+)*$"

      if pattern.IsMatch x
      then
        Decode.succeed x
      else
        Decode.fail (sprintf "\"%s\" does not match the pattern /%A/" x pattern)
    )

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
          NominalSize = get.Required.Field "nominalSize" Decode.string
          ClampDiameter = get.Required.Field "clampDiameter" Decode.float
          ClampAreaWidth = get.Optional.Field "clampAreaWidth" Decode.float
          Width = get.Optional.Field "width" Decode.float
          Drop = get.Optional.Field "drop" Decode.float
          Reach = get.Optional.Field "reach" Decode.float
          DropFlare = get.Optional.Field "dropFlare" Decode.float
          DropFlareOut = get.Optional.Field "dropFlareOut" Decode.float
          Rise = get.Optional.Field "rise" Decode.float
          Sweep = get.Optional.Field "sweep" Decode.float
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
          LargestSprocketMinTeeth = get.Optional.Field "largestSprocketMinTeeth" Decode.int
          SmallestSprocketMaxTeeth = get.Optional.Field "smallestSprocketMaxTeeth" Decode.int
          SmallestSprocketMinTeeth = get.Optional.Field "smallestSprocketMinTeeth" Decode.int
          Capacity = get.Optional.Field "capacity" Decode.int
          Clutched = get.Required.Field "isClutched" Decode.bool
        }
      )

  let handedness : Decoder<Handedness> =
    Decode.string
    |> Decode.andThen (
      function
      | "ambidextrous" -> Handedness.Ambi |> Decode.succeed
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

  let private padCartridgeType : Decoder<_> =
    Decode.string
    |> Decode.andThen
      (function
      | "shimano" -> Decode.succeed Shimano
      | "campagnolo" -> Decode.succeed Campagnolo
      | "campagnolo-old" -> Decode.succeed CampagnoloOld
      | x -> Decode.fail (sprintf "Unknown pad cartridge type %s" x))

  let private brakeFixingType : Decoder<_> =
    Decode.string
    |> Decode.andThen
      (function
      | "recessed" -> Decode.succeed RecessedAllenBolt
      | "traditional" -> Decode.succeed TraditionalNut
      | x -> Decode.fail (sprintf "Unknown brake fixing type %s" x))

  let caliperRimBrake : Decoder<CaliperRimBrake> =
    Decode.object
      (fun get ->
        {
          MinimumReach = get.Required.Field "minimumReach" Decode.int
          MaximumReach = get.Required.Field "maximumReach" Decode.int
          PadCartridgeType = get.Required.Field "padCartridgeType" padCartridgeType
          FixingType = get.Required.Field "fixingType" brakeFixingType
        }
      )

  let tyreType : Decoder<TyreType> =
    Decode.string
    |> Decode.andThen (fun x ->
      match x with
      | "clincher" -> Decode.succeed TyreType.Clincher
      | "tubeless" -> Decode.succeed TyreType.Tubeless
      | "tubular" -> Decode.succeed TyreType.Tubular
      | _ -> Decode.fail (sprintf "%s is not a valid tyre type" x)
    )

  let tyreApplication : Decoder<TyreApplication> =
    Decode.string
    |> Decode.andThen (fun x ->
      match x with
      | "track" -> Decode.succeed TyreApplication.Track
      | "road" -> Decode.succeed TyreApplication.Road
      | "rough-road" -> Decode.succeed TyreApplication.RoughRoad
      | "light-gravel" -> Decode.succeed TyreApplication.LightGravel
      | "rough-gravel" -> Decode.succeed TyreApplication.RoughGravel
      | "singletrack" -> Decode.succeed TyreApplication.Singletrack
      | _ -> Decode.fail (sprintf "%s is not a valid tyre application" x)
    )

  let tyreSize : Decoder<TyreSize> =
    Decode.object
      (fun get ->
        {
          BeadSeatDiameter = get.Required.Field "bsd" Decode.int
          ManufacturerProductCode = get.Optional.Field "manufacturerProductCode" Decode.string
          Type = get.Required.Field "type" tyreType
          Weight = get.Optional.Field "weight" Decode.int
          Width = get.Required.Field "width" Decode.int
          TreadColor = get.Required.Field "treadColor" Decode.string
          SidewallColor = get.Required.Field "sidewallColor" Decode.string
          Tpi = get.Optional.Field "tpi" Decode.int
        }
      )

  let tyre : Decoder<Tyre> =
    Decode.object
      (fun get ->
        {
          ID = get.Required.Field "id" Decode.guid
          ManufacturerCode = get.Required.Field "manufacturerCode" Decode.string
          ManufacturerProductCode = get.Optional.Field "manufacturerProductCode" Decode.string
          Name = get.Required.Field "name" Decode.string
          Application = get.Optional.Field "application" (Decode.set tyreApplication)
          Sizes = get.Required.Field "sizes" (Decode.list tyreSize)
        }
      )

  let private tyreClearance : Decoder<_> =
    Decode.object (fun get ->
      let bsd = get.Required.Field "bsd" Decode.int
      let width = get.Required.Field "width" Decode.int

      bsd, width
    )
    |> Decode.list
    |> Decode.map Map.ofSeq

  let frameMeasurements : Decoder<FrameMeasurements> =
    Decode.object
      (fun get ->
        {
          Stack = get.Optional.Field "stack" Decode.float
          Reach = get.Optional.Field "reach" Decode.float
          TopTubeActual = get.Optional.Field "topTubeActual" Decode.float
          TopTubeEffective = get.Optional.Field "topTubeEffective" Decode.float
          SeatTubeCenterToTop = get.Optional.Field "seatTubeCenterToTop" Decode.float
          SeatTubeCenterToCenter = get.Optional.Field "seatTubeCenterToCenter" Decode.float
          HeadTubeLength = get.Optional.Field "headTubeLength" Decode.float
          HeadTubeAngle = get.Optional.Field "headTubeAngle" Decode.float
          SeatTubeAngle = get.Optional.Field "seatTubeAngle" Decode.float
          BottomBracketDrop = get.Optional.Field "bottomBracketDrop" Decode.float
          ForkAxleToCrown = get.Optional.Field "forkAxleToCrown" Decode.float
          ForkRake = get.Optional.Field "forkRake" Decode.float
          ForkLength = get.Optional.Field "forkLength" Decode.float
          Wheelbase = get.Optional.Field "wheelbase" Decode.float
          StandoverHeight = get.Optional.Field "standoverHeight" Decode.float
          SeatPostDiameter = get.Optional.Field "seatPostDiameter" Decode.float
          ChainStayLength = get.Optional.Field "chainStayLength" Decode.float
          FrontTyreClearance = get.Required.Field "frontTyreClearance" tyreClearance
          RearTyreClearance = get.Required.Field "rearTyreClearance" tyreClearance
        })

  let frameSize : Decoder<FrameSize> =
    Decode.object
      (fun get ->
        {
          Code = get.Required.Field "code" code
          Name = get.Required.Field "name" Decode.string
          Measurements = get.Required.Field "measurements" frameMeasurements
        }
      )

  let frame : Decoder<Frame> =
    Decode.object
      (fun get ->
        {
          ID = get.Required.Field "id" Decode.guid
          Code = get.Required.Field "code" code
          ManufacturerCode = get.Required.Field "manufacturerCode" code
          ManufacturerProductCode = get.Optional.Field "manufacturerProductCode" Decode.string
          ManufacturerRevision = get.Optional.Field "manufacturerRevision" Decode.string
          Name = get.Required.Field "name" Decode.string
          Sizes = get.Required.Field "sizes" (Decode.list frameSize)
          HasFenderMounts = get.Required.Field "hasFenderMounts" Decode.bool
          HasFrontRackMounts = get.Required.Field "hasFrontRackMounts" Decode.bool
          HasRearRackMounts = get.Required.Field "hasRearRackMounts" Decode.bool
          HasDownTubeBottleCageMounts = get.Required.Field "hasDownTubeBottleCageMounts" Decode.bool
          HasSeatTubeBottleCageMounts = get.Required.Field "hasSeatTubeBottleCageMounts" Decode.bool
          HasUnderDownTubeBottleCageMounts = get.Required.Field "hasUnderDownTubeBottleCageMounts" Decode.bool
          HasTopTubeBagMounts = get.Required.Field "hasTopTubeBagMounts" Decode.bool
          HasForkCageMounts = get.Required.Field "hasForkCageMounts" Decode.bool
          Sources = get.Required.Field "sources" (Decode.list Decode.string)
        }
      )
