using System;
using CoreMotion;
using Foundation;
using ObjCRuntime;
using CoreLocation;

//8 Errors -> Error CS0426: The type name 'attributes' does not exist in the type 'MotionDnaSDK'
namespace MotionDnaApi
{
    //Error MT5211: Native linking failed, undefined Objective-C class: MotionDna. The symbol '_OBJC_CLASS_$_MotionDna'
    //could not be found in any of the libraries or frameworks linked with your application.
    // @interface MotionDna : NSObject <NSObject>
    [Protocol]
    [BaseType(typeof(NSObject))]
    interface MotionDna
    {
        // -(Location)getLocation;
        [Export("getLocation")]
        //[Verify(MethodToProperty)]
        Location Location { get; }

        // -(Location)getGPSLocation;
        [Export("getGPSLocation")]
        //[Verify(MethodToProperty)]
        Location GPSLocation { get; }

        // -(Attitude)getAttitude;
        [Export("getAttitude")]
        //[Verify(MethodToProperty)]
        Attitude Attitude { get; }

        // -(Motion)getMotion;
        [Export("getMotion")]
        //[Verify(MethodToProperty)]
        Motion Motion { get; }

        // -(CalibrationStatus)getCalibrationStatus;
        [Export("getCalibrationStatus")]
        //[Verify(MethodToProperty)]
        CalibrationStatus CalibrationStatus { get; }

        // -(NSString *)getID;
        [Export("getID")]
        //[Verify(MethodToProperty)]
        string ID { get; }

        // -(NSString *)getDeviceName;
        [Export("getDeviceName")]
        //[Verify(MethodToProperty)]
        string DeviceName { get; }

        // -(NSString *)getMotionName;
        [Export("getMotionName")]
        //[Verify(MethodToProperty)]
        string MotionName { get; }

        // -(MotionStatistics)getMotionStatistics;
        [Export("getMotionStatistics")]
        //[Verify(MethodToProperty)]
        MotionStatistics MotionStatistics { get; }

        // -(OrientationQuaternion)getQuaternion;
        [Export("getQuaternion")]
        //[Verify(MethodToProperty)]
        OrientationQuaternion Quaternion { get; }

        // -(XY)getDebugVector;
        [Export("getDebugVector")]
        //[Verify(MethodToProperty)]
        XY DebugVector { get; }

        // -(double)getTimestamp;
        [Export("getTimestamp")]
        //[Verify(MethodToProperty)]
        double Timestamp { get; }
    }

    // @interface  (MotionDna)
    //[Category]
    //[BaseType(typeof(MotionDna))]
    //interface MotionDna_
    //{
    //}

    //[Static]
    //[Verify(ConstantsInterfaceAssociation)]
    //partial interface Constants
    //{
    //    // extern double MotionDnaApplicationVersionNumber;
    //    [Field("MotionDnaApplicationVersionNumber", "__Internal")]
    //    double MotionDnaApplicationVersionNumber { get; }

    //    //Error BI1014: btouch: Unsupported type for Fields: global::System.Byte[] (BI1014) -> IntPtr
    //    // extern const unsigned char [] MotionDnaApplicationVersionString;
    //    [Field("MotionDnaApplicationVersionString", "__Internal")]
    //    IntPtr MotionDnaApplicationVersionString { get; }
    //}

    // Error CS0426: The type name 'MotionDnaLocationManagerDataSource' does not exist in the type 'MotionDnaSDK'
    // @protocol MotionDnaLocationManagerDelegate <NSObject>
    [Model]
    [BaseType(typeof(NSObject))]
    interface MotionDnaLocationManagerDelegate
    {
        // @required -(void)locationManager:(id<MotionDnaLocationManagerDataSource>)manager didUpdateLocations:(NSArray<CLLocation *> *)locations;
        [Abstract]
        [Export("locationManager:didUpdateLocations:")]
        void LocationManager(MotionDnaLocationManagerDataSource manager, CLLocation[] locations);

        // @required -(void)locationManager:(id<MotionDnaLocationManagerDataSource>)manager didFailWithError:(NSError *)error;
        [Abstract]
        [Export("locationManager:didFailWithError:")]
        void LocationManager(MotionDnaLocationManagerDataSource manager, NSError error);

        // @required -(void)locationManager:(id<MotionDnaLocationManagerDataSource>)manager didUpdateHeading:(CLHeading *)newHeading;
        [Abstract]
        [Export("locationManager:didUpdateHeading:")]
        void LocationManager(MotionDnaLocationManagerDataSource manager, CLHeading newHeading);

        // @required -(BOOL)locationManagerShouldDisplayHeadingCalibration:(id<MotionDnaLocationManagerDataSource>)manager;
        [Abstract]
        [Export("locationManagerShouldDisplayHeadingCalibration:")]
        bool LocationManagerShouldDisplayHeadingCalibration(MotionDnaLocationManagerDataSource manager);
    }

    // @protocol MotionDnaLocationManagerDataSource <NSObject>
    [Protocol, Model]
    [BaseType(typeof(NSObject))]
    interface MotionDnaLocationManagerDataSource
    {
        [Wrap("WeakMotionDnaDelegate")]
        [NullAllowed]
        MotionDnaLocationManagerDelegate MotionDnaDelegate { get; set; }

        // @optional @property (readwrite, nonatomic, weak) id<MotionDnaLocationManagerDelegate> _Nullable motionDnaDelegate;
        [NullAllowed, Export("motionDnaDelegate", ArgumentSemantic.Weak)]
        NSObject WeakMotionDnaDelegate { get; set; }

        // @optional @property (readwrite, nonatomic) CLAuthorizationStatus authorizationStatus;
        [Export("authorizationStatus", ArgumentSemantic.Assign)]
        CLAuthorizationStatus AuthorizationStatus { get; set; }

        // @optional @property (readwrite, nonatomic) CLDeviceOrientation headingOrientation;
        [Export("headingOrientation", ArgumentSemantic.Assign)]
        CLDeviceOrientation HeadingOrientation { get; set; }

        // @optional -(void)requestAlwaysAuthorization;
        [Export("requestAlwaysAuthorization")]
        void RequestAlwaysAuthorization();

        // @optional -(void)requestWhenInUseAuthorization;
        [Export("requestWhenInUseAuthorization")]
        void RequestWhenInUseAuthorization();

        // @optional -(void)startUpdatingLocation;
        [Export("startUpdatingLocation")]
        void StartUpdatingLocation();

        // @optional -(void)stopUpdatingLocation;
        [Export("stopUpdatingLocation")]
        void StopUpdatingLocation();

        // @optional -(void)startUpdatingHeading;
        [Export("startUpdatingHeading")]
        void StartUpdatingHeading();

        // @optional -(void)stopUpdatingHeading;
        [Export("stopUpdatingHeading")]
        void StopUpdatingHeading();

        // @optional -(void)dismissHeadingCalibrationDisplay;
        [Export("dismissHeadingCalibrationDisplay")]
        void DismissHeadingCalibrationDisplay();
    }

    interface IMotionDnaLocationManagerDataSource { }

    // @interface MotionDnaSDK : NSObject <MotionDnaLocationManagerDataSource>
    [BaseType(typeof(NSObject))]
    interface MotionDnaSDK : IMotionDnaLocationManagerDataSource
    {
        [Wrap("WeakMotionDnaDelegate")]
        [NullAllowed]
        MotionDnaLocationManagerDelegate MotionDnaDelegate { get; set; }

        // @property (readwrite, nonatomic, weak) id<MotionDnaLocationManagerDelegate> _Nullable motionDnaDelegate;
        [NullAllowed, Export("motionDnaDelegate", ArgumentSemantic.Weak)]
        NSObject WeakMotionDnaDelegate { get; set; }

        // @property (getter = authorizationStatus, readwrite, nonatomic) CLAuthorizationStatus authorizationStatus;
        [Export("authorizationStatus", ArgumentSemantic.Assign)]
        CLAuthorizationStatus AuthorizationStatus { [Bind("authorizationStatus")] get; set; }

        // @property (getter = headingOrientation, readwrite, nonatomic) CLDeviceOrientation headingOrientation;
        [Export("headingOrientation", ArgumentSemantic.Assign)]
        CLDeviceOrientation HeadingOrientation { [Bind("headingOrientation")] get; set; }

        // -(void)requestAlwaysAuthorization;
        [Export("requestAlwaysAuthorization")]
        void RequestAlwaysAuthorization();

        // -(void)requestWhenInUseAuthorization;
        [Export("requestWhenInUseAuthorization")]
        void RequestWhenInUseAuthorization();

        // -(void)startUpdatingLocation;
        [Export("startUpdatingLocation")]
        void StartUpdatingLocation();

        // -(void)stopUpdatingLocation;
        [Export("stopUpdatingLocation")]
        void StopUpdatingLocation();

        // -(void)startUpdatingHeading;
        [Export("startUpdatingHeading")]
        void StartUpdatingHeading();

        // -(void)stopUpdatingHeading;
        [Export("stopUpdatingHeading")]
        void StopUpdatingHeading();

        // -(void)dismissHeadingCalibrationDisplay;
        [Export("dismissHeadingCalibrationDisplay")]
        void DismissHeadingCalibrationDisplay();

        // -(void)runMotionDna:(NSString *)ID receiver:(id)receiver;
        [Export("runMotionDna:receiver:")]
        void RunMotionDna(string ID, NSObject receiver);

        // -(void)runMotionDna:(NSString *)ID;
        [Export("runMotionDna:")]
        void RunMotionDna(string ID);

        // -(void)receiveMotionDna:(MotionDna *)motionDna;
        [Export("receiveMotionDna:")]
        void ReceiveMotionDna(MotionDna motionDna);

        // -(void)receiveNetworkData:(MotionDna *)motionDna;
        [Export("receiveNetworkData:")]
        void ReceiveNetworkData(MotionDna motionDna);

        // -(void)receiveNetworkData:(NetworkCode)opcode WithPayload:(NSDictionary *)payload;
        [Export("receiveNetworkData:WithPayload:")]
        void ReceiveNetworkData(NetworkCode opcode, NSDictionary payload);

        // -(void)runMotionDnaWithoutMotionManager:(NSString *)ID;
        [Export("runMotionDnaWithoutMotionManager:")]
        void RunMotionDnaWithoutMotionManager(string ID);

        // -(void)reportError:(ErrorCode)error WithMessage:(NSString *)message;
        [Export("reportError:WithMessage:")]
        void ReportError(ErrorCode error, string message);

        // -(void)receiveDeviceMotion:(CMDeviceMotion *)deviceMotion;
        [Export("receiveDeviceMotion:")]
        void ReceiveDeviceMotion(CMDeviceMotion deviceMotion);

        // -(void)setAverageFloorHeight:(double)floorHeight;
        [Export("setAverageFloorHeight:")]
        void SetAverageFloorHeight(double floorHeight);

        // -(void)setFloorNumber:(int)floor;
        [Export("setFloorNumber:")]
        void SetFloorNumber(int floor);

        // -(void)setLocationLatitude:(double)latitude Longitude:(double)longitude AndHeadingInDegrees:(double)heading;
        [Export("setLocationLatitude:Longitude:AndHeadingInDegrees:")]
        void SetLocationLatitude(double latitude, double longitude, double heading);

        // -(void)setGroundTruthPointWithIdentifier:(NSInteger)identifier andLabel:(NSString *)label;
        [Export("setGroundTruthPointWithIdentifier:andLabel:")]
        void SetGroundTruthPointWithIdentifier(nint identifier, string label);

        // -(void)setGroundTruthPointWithIdentifier:(NSInteger)identifier;
        [Export("setGroundTruthPointWithIdentifier:")]
        void SetGroundTruthPointWithIdentifier(nint identifier);

        // -(void)setLocationAndHeadingGPSMag;
        [Export("setLocationAndHeadingGPSMag")]
        void SetLocationAndHeadingGPSMag();

        // -(void)setLocationLatitude:(double)latitude Longitude:(double)longitude;
        [Export("setLocationLatitude:Longitude:")]
        void SetLocationLatitude(double latitude, double longitude);

        // -(void)setLocationGPSOnly;
        [Export("setLocationGPSOnly")]
        void SetLocationGPSOnly();

        // -(void)setHeadingMagInDegrees;
        [Export("setHeadingMagInDegrees")]
        void SetHeadingMagInDegrees();

        // -(void)setHeadingInDegrees:(double)heading;
        [Export("setHeadingInDegrees:")]
        void SetHeadingInDegrees(double heading);

        // -(void)setLocationBeacon;
        [Export("setLocationBeacon")]
        void SetLocationBeacon();

        // -(void)setLocationNavisens;
        [Export("setLocationNavisens")]
        void SetLocationNavisens();

        // -(void)pause;
        [Export("pause")]
        void Pause();

        // -(void)resume;
        [Export("resume")]
        void Resume();

        // -(void)startCalibration;
        [Export("startCalibration")]
        void StartCalibration();

        // -(void)stop;
        [Export("stop")]
        void Stop();

        // +(NSString *)checkSDKVersion;
        [Static]
        [Export("checkSDKVersion")]
        //[Verify(MethodToProperty)]
        string CheckSDKVersion { get; }

        // -(NSString *)getDeviceID;
        [Export("getDeviceID")]
        //[Verify(MethodToProperty)]
        string DeviceID { get; }

        // -(NSString *)getDeviceName;
        [Export("getDeviceName")]
        //[Verify(MethodToProperty)]
        string DeviceName { get; }

        // -(void)setMapCorrectionEnabled:(BOOL)state;
        [Export("setMapCorrectionEnabled:")]
        void SetMapCorrectionEnabled(bool state);

        // -(void)setCallbackUpdateRateInMs:(double)rate;
        [Export("setCallbackUpdateRateInMs:")]
        void SetCallbackUpdateRateInMs(double rate);

        // -(void)setNetworkUpdateRateInMs:(double)rate;
        [Export("setNetworkUpdateRateInMs:")]
        void SetNetworkUpdateRateInMs(double rate);

        // -(void)setBinaryFileLoggingEnabled:(BOOL)state;
        [Export("setBinaryFileLoggingEnabled:")]
        void SetBinaryFileLoggingEnabled(bool state);

        // -(void)setBackpropagationEnabled:(BOOL)state;
        [Export("setBackpropagationEnabled:")]
        void SetBackpropagationEnabled(bool state);

        // -(void)setBackpropagationBufferSize:(size_t)bufferSize;
        [Export("setBackpropagationBufferSize:")]
        void SetBackpropagationBufferSize(nuint bufferSize);

        // -(void)setCorrectionBackpropagationEnabled:(BOOL)state;
        [Export("setCorrectionBackpropagationEnabled:")]
        void SetCorrectionBackpropagationEnabled(bool state);

        // -(void)setExternalPositioningState:(ExternalPositioningState)state;
        [Export("setExternalPositioningState:")]
        void SetExternalPositioningState(ExternalPositioningState state);

        // -(void)startUDPRoom:(NSString *)room AtHost:(NSString *)host AndPort:(NSString *)port;
        [Export("startUDPRoom:AtHost:AndPort:")]
        void StartUDPRoom(string room, string host, string port);

        // -(void)startUDPHost:(NSString *)host AndPort:(NSString *)port;
        [Export("startUDPHost:AndPort:")]
        void StartUDPHost(string host, string port);

        // -(void)startUDPRoom:(NSString *)room;
        [Export("startUDPRoom:")]
        void StartUDPRoom(string room);

        // -(void)startUDP;
        [Export("startUDP")]
        void StartUDP();

        // -(void)stopUDP;
        [Export("stopUDP")]
        void StopUDP();

        // -(void)setUDPRoom:(NSString *)room;
        [Export("setUDPRoom:")]
        void SetUDPRoom(string room);

        // -(void)sendUDPPacket:(NSString *)msg;
        [Export("sendUDPPacket:")]
        void SendUDPPacket(string msg);

        // -(void)sendUDPQueryRooms:(NSMutableArray *)rooms;
        [Export("sendUDPQueryRooms:")]
        void SendUDPQueryRooms(NSMutableArray rooms);

        // -(void)setBackgroundModeEnabled:(BOOL)state;
        [Export("setBackgroundModeEnabled:")]
        void SetBackgroundModeEnabled(bool state);

        // -(void)logBeaconWithName:(NSString *)name ID:(NSString *)id RSSI:(int)RSSI andTimestamp:(double)timestamp;
        [Export("logBeaconWithName:ID:RSSI:andTimestamp:")]
        void LogBeaconWithName(string name, string id, int RSSI, double timestamp);

        // -(void)setBeaconRangingEnabled:(BOOL)state;
        [Export("setBeaconRangingEnabled:")]
        void SetBeaconRangingEnabled(bool state);

        // -(void)setBeaconCorrectionsEnabled:(BOOL)state;
        [Export("setBeaconCorrectionsEnabled:")]
        void SetBeaconCorrectionsEnabled(bool state);

        // -(void)removeBeaconRegionWithUUID:(NSUUID *)proximityUUID andIdentifier:(NSString *)identifierForVendor;
        [Export("removeBeaconRegionWithUUID:andIdentifier:")]
        void RemoveBeaconRegionWithUUID(NSUuid proximityUUID, string identifierForVendor);

        // -(void)registerBeaconRegionWithUUID:(NSUUID *)proximityUUID andIdentifier:(NSString *)identifier;
        [Export("registerBeaconRegionWithUUID:andIdentifier:")]
        void RegisterBeaconRegionWithUUID(NSUuid proximityUUID, string identifier);

        // -(BOOL)isRangingBLE;
        [Export("isRangingBLE")]
        //[Verify(MethodToProperty)]
        bool IsRangingBLE { get; }

        // -(void)setPowerMode:(PowerConsumptionMode)mode;
        [Export("setPowerMode:")]
        void SetPowerMode(PowerConsumptionMode mode);

        // -(void)setLocalHeadingOffsetInDegrees:(double)hdg;
        [Export("setLocalHeadingOffsetInDegrees:")]
        void SetLocalHeadingOffsetInDegrees(double hdg);

        // -(void)setCartesianOffsetInMetersX:(double)x Y:(double)y;
        [Export("setCartesianOffsetInMetersX:Y:")]
        void SetCartesianOffsetInMetersX(double x, double y);

        // -(void)setCartesianPositionX:(double)x Y:(double)y;
        [Export("setCartesianPositionX:Y:")]
        void SetCartesianPositionX(double x, double y);

        // -(void)setLocalHeading:(double)hdg;
        [Export("setLocalHeading:")]
        void SetLocalHeading(double hdg);

        // -(void)resetLocalHeading;
        [Export("resetLocalHeading")]
        void ResetLocalHeading();

        // -(void)setARModeEnabled:(BOOL)state;
        [Export("setARModeEnabled:")]
        void SetARModeEnabled(bool state);

        // -(void)setEstimationMode:(EstimationMode)mode;
        [Export("setEstimationMode:")]
        void SetEstimationMode(EstimationMode mode);

        // -(void)resetLocalEstimation;
        [Export("resetLocalEstimation")]
        void ResetLocalEstimation();

        // -(void)enableBackgroundSensors;
        [Export("enableBackgroundSensors")]
        void EnableBackgroundSensors();

        // -(void)addFloorNumber:(int)floor AndHeight:(double)height;
        [Export("addFloorNumber:AndHeight:")]
        void AddFloorNumber(int floor, double height);

        // -(void)inputGlobalCoordinatesLat:(double)lat Lon:(double)lon Timestamp:(double)timestamp AndAngleThreshold:(double)angle;
        [Export("inputGlobalCoordinatesLat:Lon:Timestamp:AndAngleThreshold:")]
        void InputGlobalCoordinatesLat(double lat, double lon, double timestamp, double angle);

        // -(void)inputGlobalCoordinatesLat:(double)lat Lon:(double)lon AndTimestamp:(double)timestamp;
        [Export("inputGlobalCoordinatesLat:Lon:AndTimestamp:")]
        void InputGlobalCoordinatesLat(double lat, double lon, double timestamp);

        // -(void)inputMotionWithTimestamp:(double)timestamp roll:(double)roll pitch:(double)pitch yaw:(double)yaw accX:(double)accX accY:(double)accY accZ:(double)accZ magX:(double)magX magY:(double)magY magZ:(double)magZ;
        [Export("inputMotionWithTimestamp:roll:pitch:yaw:accX:accY:accZ:magX:magY:magZ:")]
        void InputMotionWithTimestamp(double timestamp, double roll, double pitch, double yaw, double accX, double accY, double accZ, double magX, double magY, double magZ);

        // -(void)inputMotionWithTimestamp:(double)timestamp roll:(double)roll pitch:(double)pitch yaw:(double)yaw accX:(double)accX accY:(double)accY accZ:(double)accZ;
        [Export("inputMotionWithTimestamp:roll:pitch:yaw:accX:accY:accZ:")]
        void InputMotionWithTimestamp(double timestamp, double roll, double pitch, double yaw, double accX, double accY, double accZ);
    }

    // @interface RELEASEA (MotionDnaSDK)
    //[Category]
    //[BaseType(typeof(MotionDnaSDK))]
    //interface MotionDnaSDK_RELEASEA
    //{
    //}
}