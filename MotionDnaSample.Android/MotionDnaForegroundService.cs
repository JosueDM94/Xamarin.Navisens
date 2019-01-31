using System;

using Android.OS;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Support.V4.App;

namespace MotionDnaSample.Android
{
    [Service(Label = "MotionDnaForegroundService")]
    [IntentFilter(new string[] { "com.jdiaz.MotionDnaForegroundService" })]
    public class MotionDnaForegroundService : Service
    {
        private IBinder binder;
        private static readonly DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static readonly int NOTIFICATION_ID = 1;

        public override void OnCreate()
        {
            base.OnCreate();
            int myTid = Process.MyTid();
            Process.SetThreadPriority(myTid, ThreadPriority.UrgentAudio);

            Intent mainIntent = new Intent(this, typeof(MainActivity));
            mainIntent.AddFlags(ActivityFlags.ClearTop | ActivityFlags.SingleTop);
            PendingIntent contentIntent = PendingIntent.GetActivity(ApplicationContext, NOTIFICATION_ID, mainIntent, 0);
            Notification notification = new NotificationCompat.Builder(this, CreateChannel())
                .SetWhen((long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds)
                .SetSmallIcon(Resource.Drawable.navisens_logo_big)
                .SetContentText("MotionDna is running")
                .SetContentIntent(contentIntent)
                .SetContentTitle("Navisens")
                .SetTicker("Navisens")
                .SetOngoing(true)
                .Build();
            StartForeground(NOTIFICATION_ID, notification);
        }

        private string CreateChannel()
        {
            NotificationManager notificationManager = (NotificationManager)this.GetSystemService(Context.NotificationService);

            string name = "Navisens Location";
            var importance = NotificationImportance.Low;

            NotificationChannel mChannel = new NotificationChannel("Navisens Channel", name, importance);

            mChannel.EnableLights(true);
            mChannel.LightColor = Color.Blue;
            if (notificationManager != null)
            {
                notificationManager.CreateNotificationChannel(mChannel);
            }
            else
            {
                StopSelf();
            }
            return "Navisens Channel";
        }

        public override void OnDestroy()
        {
            StopForeground(true);
            StopSelf();
            base.OnDestroy();
        }

        public override IBinder OnBind(Intent intent)
        {
            binder = new MotionDnaForegroundServiceBinder(this);
            return binder;
        }
    }

    public class MotionDnaForegroundServiceBinder : Binder
    {
        readonly MotionDnaForegroundService service;

        public MotionDnaForegroundServiceBinder(MotionDnaForegroundService service)
        {
            this.service = service;
        }

        public MotionDnaForegroundService GetMotionDnaForegroundService()
        {
            return service;
        }
    }
}