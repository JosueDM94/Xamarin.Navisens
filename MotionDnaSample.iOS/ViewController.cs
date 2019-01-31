using System;
using System.Text;
using System.Collections.Generic;

using UIKit;
using Foundation;
using MotionDnaApi;
using CoreFoundation;

namespace MotionDnaSample.iOS
{
    public partial class ViewController : UIViewController
    {
        private string devKey = "<ENTER YOUR DEV KEY HERE>";

        public MotionDnaManager manager = new MotionDnaManager();
        public Dictionary<string, MotionData> networkUsers = new Dictionary<string, MotionData>();
        public Dictionary<string, double> networkUsersTimestamps = new Dictionary<string, double>();

        protected ViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            StartMotionDna();
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        public void Receive(MotionDna motionDna)
        {
            var location = motionDna.Location;
            var localLocation = location.localLocation;
            var globalLocation = location.globalLocation;
            var motion = motionDna.Motion;

            var motionDnaLocalString = string.Format("Local XYZ Coordinates (meters) \n({0:F}, {1:F}, {2:F})", localLocation.x, localLocation.y, localLocation.z);
            var motionDnaHeadingString = string.Format("Current Heading: {0:F}", location.heading);
            var motionDnaGlobalString = string.Format("Global Position: \n(Lat: {0:F}, Lon: {1:F})", globalLocation.latitude, globalLocation.longitude);
            var motionDnaMotionTypeString = string.Format("Motion Type: {0}", motion.motionType.ToString());

            var motionDnaString = string.Format("MotionDna Location:\n{0}\n{1}\n{2}\n{3}", motionDnaLocalString,
                                         motionDnaHeadingString,
                                         motionDnaGlobalString,
                                         motionDnaMotionTypeString);
            DispatchQueue.MainQueue.DispatchAsync(() => { this.receiveMotionDnaTextField.Text = motionDnaString; });
        }

        public void ReceiveNetworkData(MotionDna motionDna)
        {
            var motionData = new MotionData(motionDna.ID, motionDna.DeviceName, motionDna.Location, motionDna.Motion.motionType);
            networkUsers[motionDna.ID] = motionData;
            var timeSinceBootSeconds = NSProcessInfo.ProcessInfo.SystemUptime;
            networkUsersTimestamps[motionDna.ID] = timeSinceBootSeconds;
            StringBuilder activeNetworkUsersString = new StringBuilder();
            List<string> toRemove = new List<string>();

            activeNetworkUsersString.Append("Network Shared Devices:\n");
            foreach(MotionData user in networkUsers.Values)
            {
                if (timeSinceBootSeconds - networkUsersTimestamps[user.id] > 2.0) {
                    toRemove.Add(user.id);
                } else {
                    activeNetworkUsersString = activeNetworkUsersString.Append(user.deviceName);
                    var location = user.location.localLocation;
                    activeNetworkUsersString.Append(string.Format(" ({0:2F}, {1:2F}, {2:2F})\n", location.x, location.y, location.z));
                }
            }

            foreach (string key in toRemove)
            {
                networkUsers.Remove(key);
                networkUsersTimestamps.Remove(key);
            }

            DispatchQueue.MainQueue.DispatchAsync (()=> {
                receiveNetworkDataTextField.Text = activeNetworkUsersString.ToString();
            });
        }

        public void ReceiveNetworkData(NetworkCode opcode, NSDictionary payload)
        {
        }

        private void StartMotionDna()
        {
            manager.receiver = this;

            if(devKey.Equals("<ENTER YOUR DEV KEY HERE>"))
            {
                var alertController = UIAlertController.Create("Key", "Enter your Navisens Dev Key.", UIAlertControllerStyle.Alert);
                alertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Cancel, delegate{return;}));
                PresentViewController(alertController, true, null);
            }
            //    This functions starts up the SDK. You must pass in a valid developer's key in order for
            //    the SDK to function. IF the key has expired or there are other errors, you may receive
            //    those errors through the reportError() callback route.
            
            manager.RunMotionDna(devKey);

            //    Use our internal algorithm to automatically compute your location and heading by fusing
            //    inertial estimation with global location information. This is designed for outdoor use and
            //    will not compute a position when indoors. Solving location requires the user to be walking
            //    outdoors. Depending on the quality of the global location, this may only require as little
            //    as 10 meters of walking outdoors.
            manager.SetLocationNavisens();

            //   Set accuracy for GPS positioning, states :HIGH/LOW_ACCURACY/OFF, OFF consumes
            //   the least battery.
            manager.SetExternalPositioningState(ExternalPositioningState.LowAccuracy);

            //    Manually sets the global latitude, longitude, and heading. This enables receiving a
            //    latitude and longitude instead of cartesian coordinates. Use this if you have other
            //    sources of information (for example, user-defined address), and need readings more
            //    accurate than GPS can provide.
            //        manager.setLocationLatitude(37.787582, longitude: -122.396627, andHeadingInDegrees: 0.0)
            //    Set the power consumption mode to trade off accuracy of predictions for power saving.
            manager.SetPowerMode(PowerConsumptionMode.Performance);

            //    Connect to your own server and specify a room. Any other device connected to the same room
            //    and also under the same developer will receive any udp packets this device sends.
            manager.StartUDP();

            //    Allow our SDK to record data and use it to enhance our estimation system.
            //    Send this file to support@navisens.com if you have any issues with the estimation
            //    that you would like to have us analyze.
            manager.SetBinaryFileLoggingEnabled(true);

            //    Tell our SDK how often to provide estimation results. Note that there is a limit on how
            //    fast our SDK can provide results, but usually setting a slower update rate improves results.
            //    Setting the rate to 0ms will output estimation results at our maximum rate.
            manager.SetCallbackUpdateRateInMs(500);

            //    When setLocationNavisens is enabled and setBackpropagationEnabled is called, once Navisens
            //    has initialized you will not only get the current position, but also a set of latitude
            //    longitude coordinates which lead back to the start position (where the SDK/App was started).
            //    This is useful to determine which building and even where inside a building the
            //    person started, or where the person exited a vehicle (e.g. the vehicle parking spot or the
            //    location of a drop-off).
            manager.SetBackpropagationEnabled(true);

            //    If the user wants to see everything that happened before Navisens found an initial
            //    position, he can adjust the amount of the trajectory to see before the initial
            //    position was set automatically.
            manager.SetBackpropagationBufferSize(2000);

            //    Enables AR mode. AR mode publishes orientation quaternion at a higher rate.
            //manager.setARModeEnabled(true)
        }

        public class MotionData
        {
            public string id;
            public string deviceName;
            public Location location;
            public MotionType motionType;

            public MotionData(string id, string deviceName, Location location, MotionType motionType)
            {
                this.id = id;
                this.deviceName = deviceName;
                this.location = location;
                this.motionType = motionType;
            }
        }
    }
}