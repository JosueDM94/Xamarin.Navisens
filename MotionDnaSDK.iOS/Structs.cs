using System.Collections.Generic;
using System.Runtime.InteropServices;

//8 Errors -> Error CS0426: The type name 'attributes' does not exist in the type 'MotionDnaSDK'
namespace MotionDnaApi
{
    public enum MotionType : uint
    {
        Stationary,
        Fidgeting,
        Forward
    }

    public enum ErrorCode : uint
    {
        SensorTiming = 0,
        AuthenticationFailed = 1,
        SensorMissing = 2,
        SdkExpired = 3,
        WrongFloorInput = 4
    }

    public enum PowerConsumptionMode : uint
    {
        SuperLowConsumption,
        LowConsumption,
        MediumConsumption,
        Performance
    }

    public enum ExternalPositioningState : uint
    {
        Off,
        HighAccuracy,
        LowAccuracy,
        ExternalUndefined
    }

    public enum LocationStatus : uint
    {
        Uninitialized,
        NavisensInitializing,
        NavisensInitialized
    }

    public enum VerticalMotionStatus
    {
        LevelGround = 0,
        EscalatorUp = 1,
        EscalatorDown = -1,
        ElevatorUp = 2,
        ElevatorDown = -2,
        StairsUp = 3,
        StairsDown = -3
    }

    public enum CalibrationStatus : uint
    {
        Done,
        Calibrating,
        None
    }

    public enum EstimationMode : uint
    {
        Local,
        Global
    }

    public enum NetworkCode : uint
    {
        RawNetworkData,
        RoomCapacityStatus,
        ExceededRoomConnectionCapacity,
        ExceededServerRoomCapacity
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MotionStatistics
    {
        public double dwelling;

        public double walking;

        public double stationary;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Attitude
    {
        public double roll;

        public double pitch;

        public double yaw;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct XYZ
    {
        public double x;

        public double y;

        public double z;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct XY
    {
        public double x;

        public double y;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct GlobalLocation
    {
        public double latitude;

        public double longitude;

        public double altitude;

        public double accuracyInMeters;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct OrientationQuaternion
    {
        public double w;

        public double x;

        public double y;

        public double z;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Location
    {
        public LocationStatus locationStatus;

        public XYZ localLocation;

        public GlobalLocation globalLocation;

        public double heading;

        public double magneticHeading;

        public double localHeading;

        public XY uncertainty;

        public VerticalMotionStatus verticalMotionStatus;

        public int floor;

        public double absoluteAltitude;

        public double absoluteAltitudeUncertainty;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Motion
    {
        public double stepFrequency;

        public MotionType motionType;
    }

    public enum MapObjectType
    {
        street = 0,
        building = 1,
        voronoi = 2,
        door = 3,
        store = 4,
        elevator = 5,
        escalator = 6,
        obstacle = 7,
        building_side = 8,
        traversable = 9,
        natural = 10,
        parking = 11,
        ble = 12,
        invalid = -1,
        unknown = -2
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Obj
    {
        public List<Coord> coords;

        public MapObjectType type;

        public int ID;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Map
    {
        public bool hasIndoorMap;

        public bool hasOutdoorMap;

        public bool isUsingMapCorrections;

        public int floor;

        public bool isNew;

        public List<Obj> objects;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Coord
    {
        public Coord(double _lat, double _lng)
        {
            lat = _lat;
            lng = _lng;
        }

        public double lat;

        public double lng;
    }
}