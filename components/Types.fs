namespace BikeHackers.Components

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
