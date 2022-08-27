using Android.App;
using Android.Content;
using Android.Graphics;
using AndroidX.Core.App;
using Firebase.Messaging;
using Java.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Color = Android.Graphics.Color;

namespace MAUI_Notification.Platforms.Android
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    public class myFirebaseMessaging : FirebaseMessagingService
    {

        public override void OnNewToken(string token)
        {
            //Handle your token here
        }

        public override void OnMessageReceived(RemoteMessage message)
        {
            SendNotification(message.Data);
        }

        private void SendNotification(IDictionary<string, string> data)
        {
            var intent = new Intent(this, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop);

            foreach (var key in data.Keys)
            {
                intent.PutExtra(key, data[key]);
            }
            string title = "Welcome";
            string messageBody = "Welcome to Xamarin Notify";
            if (data.ContainsKey("title"))
            {
                title = data["title"].ToString();
            }

            if (data.ContainsKey("body"))
            {
                messageBody = data["body"].ToString();
            }


            var res = BaseContext.Resources;
            var largeImageBitmap = BitmapFactory.DecodeResource(res, Resource.Drawable.abc_ab_share_pack_mtrl_alpha);

            NotificationCompat.Builder notificationBuilder;
            var pendingIntent = PendingIntent.GetActivity(this, 100, intent, PendingIntentFlags.OneShot);
            notificationBuilder = new NotificationCompat.Builder(this, "my_notification_channel")
                                      .SetSmallIcon(Resource.Drawable.abc_ab_share_pack_mtrl_alpha)
                                      .SetContentTitle(title)
                                      .SetColor(Color.Argb(0, 230, 25, 64))
                                      .SetContentText(messageBody)
                                      .SetAutoCancel(true)
                                      .SetLargeIcon(largeImageBitmap)
                                      .SetContentIntent(pendingIntent);
            try
            {
                if (data.ContainsKey("ImageUrl"))
                {
                    var urlString = data["ImageUrl"].ToString();
                    var url = new URL(urlString);
                    var connection = (HttpURLConnection)url.OpenConnection();
                    connection.DoInput = true;
                    connection.Connect();
                    var input = connection.InputStream;
                    var bitmap = BitmapFactory.DecodeStream(input);
                    var style = new NotificationCompat.BigPictureStyle()
                            .BigPicture(bitmap)
                            .SetSummaryText(messageBody);
                    connection.Dispose();
                    notificationBuilder.SetStyle(style);
                }
            }
            catch
            {

            }


            var notificationManager = NotificationManagerCompat.From(this);
            notificationManager.Notify(100, notificationBuilder.Build());
        }

        public override void HandleIntent(Intent p0)
        {
            try
            {
                if (p0.Extras != null)
                {
                    var builder = new RemoteMessage.Builder("FirebaseMessagingService");

                    foreach (string key in p0.Extras.KeySet())
                    {
                        builder.AddData(key, p0.Extras.Get(key).ToString());
                    }

                    OnMessageReceived(builder.Build());
                }
                else
                {
                    base.HandleIntent(p0);
                }
            }
            catch (Exception)
            {
                base.HandleIntent(p0);
            }
        }
    }
}
