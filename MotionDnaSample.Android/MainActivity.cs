using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V7.App;
using Android.Widget;
using Java.Lang;
using Navisens.MotionDnaApi;
using AlertDialog = Android.Support.V7.App.AlertDialog;

namespace MotionDnaSample.Android
{
    [Activity(MainLauncher = true)]
    public class MainActivity : AppCompatActivity, IMotionDnaInterface
    {
        private static readonly int REQUEST_MDNA_PERMISSIONS = 1;

        //    The two required methods shown below bind
        //    the interface to your application's activity,
        //    so MotionDna is able to retrieve the necessary
        //    permissions and capabilities
        public Context AppContext => ApplicationContext;
        public PackageManager PkgManager => PackageManager;
        private string devKey = "<ENTER YOUR DEV KEY HERE>";

        public Intent motionDnaServiceIntent;
        public TextView textView, networkTextView;
        public MotionDnaApplication motionDnaApplication;
        public Dictionary<string, MotionDna> networkUsers = new Dictionary<string, MotionDna>();
        public Dictionary<string, double> networkUsersTimestamps = new Dictionary<string, double>();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);
            textView = FindViewById<TextView>(Resource.Id.HELLO);
            networkTextView = FindViewById<TextView>(Resource.Id.network);

            // Requests app
            ActivityCompat.RequestPermissions(this, MotionDnaApplication.NeedsRequestingPermissions(), REQUEST_MDNA_PERMISSIONS);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            if (MotionDnaApplication.CheckMotionDnaPermissions(this)) // permissions already requested
            {

                // Starts a foreground service to ensure that the
                // App continues to sample the sensors in background
                motionDnaServiceIntent = new Intent(this, typeof(MotionDnaForegroundService));
                StartService(motionDnaServiceIntent);

                // Start the MotionDna Core
                StartMotionDna();
            }
        }

        protected override void OnDestroy()
        {
            // Shuts downs the MotionDna Core
            motionDnaApplication.Stop();

            // Handle destruction of the foreground service if
            // it is enabled
            if (motionDnaServiceIntent != null)
            {
                AppContext.StopService(motionDnaServiceIntent);
            }
            base.OnDestroy();
        }

        public void StartMotionDna()
        {
            if (devKey.Equals("<ENTER YOUR DEV KEY HERE>"))
            {
                var alert = new AlertDialog.Builder(this);
                alert.SetMessage("Enter your Navisens Dev Key.").SetCancelable(false).SetNeutralButton("Ok", delegate
                {
                    alert.Dispose();
                    return;
                }).Show();
            }

            motionDnaApplication = new MotionDnaApplication(this);

            //    This functions starts up the SDK. You must pass in a valid developer's key in order for
            //    the SDK to function. IF the key has expired or there are other errors, you may receive
            //    those errors through the reportError() callback route.

            motionDnaApplication.RunMotionDna(devKey);

            //    Use our internal algorithm to automatically compute your location and heading by fusing
            //    inertial estimation with global location information. This is designed for outdoor use and
            //    will not compute a position when indoors. Solving location requires the user to be walking
            //    outdoors. Depending on the quality of the global location, this may only require as little
            //    as 10 meters of walking outdoors.

            motionDnaApplication.SetLocationNavisens();

            //   Set accuracy for GPS positioning, states :HIGH/LOW_ACCURACY/OFF, OFF consumes
            //   the least battery.

            motionDnaApplication.SetExternalPositioningState(MotionDna.ExternalPositioningState.LowAccuracy);

            //    Manually sets the global latitude, longitude, and heading. This enables receiving a
            //    latitude and longitude instead of cartesian coordinates. Use this if you have other
            //    sources of information (for example, user-defined address), and need readings more
            //    accurate than GPS can provide.
            //        motionDnaApplication.setLocationLatitudeLongitudeAndHeadingInDegrees(37.787582, -122.396627, 0);

            //    Set the power consumption mode to trade off accuracy of predictions for power saving.

            motionDnaApplication.SetPowerMode(MotionDna.PowerConsumptionMode.Performance);

            //    Connect to your own server and specify a room. Any other device connected to the same room
            //    and also under the same developer will receive any udp packets this device sends.

            motionDnaApplication.StartUDP();

            //    Allow our SDK to record data and use it to enhance our estimation system.
            //    Send this file to support@navisens.com if you have any issues with the estimation
            //    that you would like to have us analyze.

            motionDnaApplication.SetBinaryFileLoggingEnabled(true);

            //    Tell our SDK how often to provide estimation results. Note that there is a limit on how
            //    fast our SDK can provide results, but usually setting a slower update rate improves results.
            //    Setting the rate to 0ms will output estimation results at our maximum rate.

            motionDnaApplication.SetCallbackUpdateRateInMs(500);

            //    When setLocationNavisens is enabled and setBackpropagationEnabled is called, once Navisens
            //    has initialized you will not only get the current position, but also a set of latitude
            //    longitude coordinates which lead back to the start position (where the SDK/App was started).
            //    This is useful to determine which building and even where inside a building the
            //    person started, or where the person exited a vehicle (e.g. the vehicle parking spot or the
            //    location of a drop-off).
            motionDnaApplication.SetBackpropagationEnabled(true);

            //    If the user wants to see everything that happened before Navisens found an initial
            //    position, he can adjust the amount of the trajectory to see before the initial
            //    position was set automatically.
            motionDnaApplication.SetBackpropagationBufferSize(2000);

            //    Enables AR mode. AR mode publishes orientation quaternion at a higher rate.

            //        motionDnaApplication.setARModeEnabled(true);
        }


        //    This event receives the estimation results using a MotionDna object.
        //    Check out the Getters section to learn how to read data out of this object.
        public void ReceiveMotionDna(MotionDna motionDna)
        {
            string str = "Navisens MotionDna Location Data:\n";
            str += "Lat: " + motionDna.GetLocation().GlobalLocation.Latitude + " Lon: " + motionDna.GetLocation().GlobalLocation.Longitude + "\n";
            MotionDna.XYZ location = motionDna.GetLocation().LocalLocation;
            str += string.Format(" ({0:F}, {1:F}, {2:F})\n", location.X, location.Y, location.Z);
            str += "Hdg: " + motionDna.GetLocation().Heading + " \n";
            str += "motionType: " + motionDna.GetMotion().MotionType + "\n";
            textView.SetTextColor(Color.Black);
            RunOnUiThread(() => { textView.Text = str; });
        }

        //    This event receives estimation results from other devices in the server room. In order
        //    to receive anything, make sure you call startUDP to connect to a room. Again, it provides
        //    access to a MotionDna object, which can be unpacked the same way as above.
        //
        //
        //    If you aren't receiving anything, then the room may be full, or there may be an error in
        //    your connection. See the reportError event below for more information.
        public void ReceiveNetworkData(MotionDna motionDna)
        {
            networkUsers[motionDna.ID] = motionDna;
            double timeSinceBootSeconds = SystemClock.ElapsedRealtime() / 1000.0;
            networkUsersTimestamps[motionDna.ID]= timeSinceBootSeconds;
            StringBuilder activeNetworkUsersStringBuilder = new StringBuilder();
            List<string> toRemove = new List<string>();

            activeNetworkUsersStringBuilder.Append("Network Shared Devices:\n");
            foreach (MotionDna user in networkUsers.Values)
            {
                if (timeSinceBootSeconds - networkUsersTimestamps[user.ID] > 2.0)
                {
                    toRemove.Add(user.ID);
                }
                else
                {
                    activeNetworkUsersStringBuilder.Append(user.DeviceName);
                    MotionDna.XYZ location = user.GetLocation().LocalLocation;
                    activeNetworkUsersStringBuilder.Append(string.Format(" ({0:2F}, {1:2F}, {2:2F})", location.X, location.Y, location.Z));
                    activeNetworkUsersStringBuilder.Append("\n");
                }

            }
            foreach (string key in toRemove)
            {
                networkUsers.Remove(key);
                networkUsersTimestamps.Remove(key);
            }

            networkTextView.Text = activeNetworkUsersStringBuilder.ToString();
        }

        //    This event receives arbitrary data from the server room. You must have
        //    called startUDP already to connect to the room.
        public void ReceiveNetworkData(MotionDna.NetworkCode motionDna, IDictionary<string, Java.Lang.Object> p1)
        {

        }

        //    Report any errors of the estimation or internal SDK
        public void ReportError(MotionDna.ErrorCode errorCode, string s)
        {

            if (MotionDna.ErrorCode.ErrorAuthenticationFailed == errorCode)
                Console.WriteLine("Error: authentication failed " + s);
            else if (MotionDna.ErrorCode.ErrorSdkExpired == errorCode)
                Console.WriteLine("Error: SDK expired " + s);
            else if (MotionDna.ErrorCode.ErrorPermissions == errorCode)
                Console.WriteLine("Error: permissions not granted " + s);
            else if (MotionDna.ErrorCode.ErrorSensorMissing == errorCode)
                Console.WriteLine("Error: sensor missing " + s);
            else if (MotionDna.ErrorCode.ErrorSensorTiming == errorCode)
                Console.WriteLine("Error: sensor timing " + s);
        }
    }
}