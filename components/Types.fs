namespace BikeHackers.Components

type Ratio = int * int

type FrontOrRear =
  | Front
  | Rear

type WheelBsd =
  | W26
  | W650B
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
    HeadTubeLength : float option
    HeadTubeAngle : float option
    SeatTubeAngle : float option
    BottomBracketDrop : float option
    ForkAxleToCrown : float option
    ChainStayLength : float option
    ForkLength : float option
    ForkRake : float option
    Wheelbase : float option
    StandoverHeight : float option
    SeatPostDiameter : float option
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
    ManufacturerProductCode : string
    Diameter : float
    Offset : int
    Length : int
    Weight : int option
  }

type SeatPost =
  {
    Sizes : SeatPostSize list
  }

type DropHandleBar =
  {
    ClampDiameter : float // mm
    ClampAreaWidth : float option
    WidthAtTops : float option // CTC mm
    Drop : float option // mm
    Reach : float option // mm
    DropFlare : float option // degrees
    Weight : int option
    DropFlareOut : float option
    OutsideWidth : float option // width of bars at the widest point
  }

type RearDerailleur =
  {
    Manufacturer : string
    ProductCode : string
    ActuationRatio : int * int
    Speed : int
    Weight : int
    LargestSprocketMaxTeeth : int
    LargestSprocketMinTeeth : int
    SmallestSprocketMaxTeeth : int
    SmallestSprocketMinTeeth : int
    Capacity : int
    Clutched : bool
  }

type Cassette =
  {
    Manufacturer : string
    ProductCode : string
    Sprockets : int list
    SprocketPitch : float
    Interface : string
  }

type Chain =
  {
    Manufacturer : string
    ProductCode : string
    Speed : int
    Weight : int
  }
