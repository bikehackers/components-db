namespace BikeHackers.Components

open System

type Ratio = int * int

type Side =
  | Left
  | Right

type FrontOrRear =
  | Front
  | Rear

type WheelBsd =
  | W26
  | W650B
  | W650C
  | W700C

type Manufacturer =
  {
    Code : string
    Name : string
    Url : string option
  }

type ProductMeta =
  {
    ManufacturerCode : string
    ManufacturerProductCode : string option
    SeriesCode : string option
    RangeCode : string option
    Name : string
    VariantCode : string option
  }

type FrameMeasurements =
  {
    Stack : float option
    Reach : float option
    TopTubeActual : float option
    TopTubeEffective : float option
    SeatTubeCenterToTop : float option
    SeatTubeCenterToCenter : float option
    HeadTubeLength : float option
    HeadTubeAngle : float option
    SeatTubeAngle : float option
    BottomBracketDrop : float option
    ChainStayLength : float option
    ForkLength : float option
    ForkRake : float option
    ForkAxleToCrown : float option
    Wheelbase : float option
    StandoverHeight : float option
    SeatPostDiameter : float option
    FrontTyreClearance : Map<int, int>
    RearTyreClearance : Map<int, int>
  }

type FrameSize =
  {
    Name : string
    Code : string
    Measurements : FrameMeasurements
  }

type RimBrakePadCartridgeType =
  | Shimano
  | Campagnolo
  | CampagnoloOld

type BrakeFixingType =
  | RecessedAllenBolt
  | TraditionalNut

type CaliperRimBrake =
  {
    MinimumReach : int
    MaximumReach : int
    PadCartridgeType : RimBrakePadCartridgeType
    FixingType : BrakeFixingType
  }

type SeatPostSize =
  {
    ManufacturerProductCode : string option
    Diameter : float
    Offset : int
    Length : int
    Weight : int option
  }

type SeatPost =
  {
    ManufacturerCode : string
    Name : string
    Sizes : SeatPostSize list
  }

type DropHandleBarSize =
  {
    ManufacturerProductCode : string option
    NominalSize : string
    ClampDiameter : float // mm
    ClampAreaWidth : float option
    Width : float option // CTC at the hoods mm
    Drop : float option // mm
    Reach : float option // mm
    DropFlare : float option // degrees
    Weight : int option
    Sweep : float option
    Rise : float option // mm
    DropFlareOut : float option
    OutsideWidth : float option // width of bars at the widest point
  }

type DropHandleBar =
  {
    ManufacturerCode : string
    Name : string
    Sizes : DropHandleBarSize list
  }

type RearDerailleur =
  {
    Manufacturer : string
    ProductCode : string
    ActuationRatio : int * int
    Speed : int
    Weight : int
    LargestSprocketMaxTeeth : int
    LargestSprocketMinTeeth : int option
    SmallestSprocketMaxTeeth : int option
    SmallestSprocketMinTeeth : int option
    Capacity : int option
    Clutched : bool
  }

type Cassette =
  {
    ManufacturerCode : string
    ManufacturerProductCode : string
    Sprockets : int list
    SprocketPitch : float
    Interface : string
  }

type Chain =
  {
    ManufacturerCode : string
    ManufacturerProductCode : string
    Speed : int
    Weight : int option
  }

type Handedness =
  | Specific of Side
  | Ambi

type IntegratedShifter =
  {
    ManufacturerCode : string
    ManufacturerProductCode : string option
    Speed : int
    CablePull : float
    Hand : Handedness
    Weight : int option
  }

type TyreType =
  | Clincher
  | Tubeless
  | Tubular

type TyreApplication =
  | Track
  | Road
  | RoughRoad
  | LightGravel
  | RoughGravel
  | Singletrack

type TyreSize =
  {
    ManufacturerProductCode : string option
    BeadSeatDiameter : int
    Width : int
    Weight : int option
    Type : TyreType
    TreadColor : string
    SidewallColor : string
    Tpi : int option
  }

type Tyre =
  {
    ID : Guid
    ManufacturerCode : string
    ManufacturerProductCode : string option
    Name : string
    Sizes : TyreSize list
    Application : (Set<TyreApplication>) option
  }

type Frame =
  {
    ID : Guid
    Code : string
    ManufacturerCode : string
    ManufacturerProductCode : string option
    ManufacturerRevision : string option
    Name : string
    Sizes : FrameSize list
    HasFenderMounts : bool
    HasRearRackMounts : bool
    HasFrontRackMounts : bool
    HasTopTubeBagMounts : bool
    HasSeatTubeBottleCageMounts : bool
    HasDownTubeBottleCageMounts : bool
    HasUnderDownTubeBottleCageMounts : bool
    HasForkCageMounts : bool
    Sources : string list
  }

module WheelBsd =

  let toMillimeters (bsd : WheelBsd) =
    match bsd with
    | W26 -> 559
    | W650C -> 571
    | W650B -> 584
    | W700C -> 622

  let fromMillimeters (x : int) =
    match x with
    | 622 -> Some W700C
    | 584 -> Some W650B
    | 571 -> Some W650C
    | 559 -> Some W26
    | _ -> None
