using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace FloatingExample
{

    /// <summary>
    /// A service that can be started by an activity, it'll spawn a view on the screen above other apps.
    /// </summary>
    /// <remarks>
    /// In order to this to work properly you must enable the Draw Overlays permission in Android 6.0 and above.
    /// </remarks>
    [Service(Enabled = true)]
    internal class Floating : Service
    {
        private IWindowManager windowManager;
        private LinearLayout floatingView;

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            return StartCommandResult.Sticky;
        }

        public override void OnCreate()
        {
            base.OnCreate();
            WindowManagerTypes layoutType = WindowManagerTypes.Phone;

            if (Build.VERSION.SdkInt > BuildVersionCodes.NMr1) //Nougat 7.1
            {
                layoutType = WindowManagerTypes.ApplicationOverlay; //Android Oreo does not allow to add windows of WindowManagerTypes.Phone
            }

            windowManager = GetSystemService(WindowService).JavaCast<IWindowManager>();

            var layoutInflater = LayoutInflater.From(this);

            View view = null;
            //view =  (LinearLayout)layoutInflater.Inflate(Resource.Layout.Floating, null); //Replace with your custom axml view in resources.
            floatingView = view as LinearLayout;

            int width = 200;
            var floatingNotificationWidth = TypedValue.ApplyDimension(ComplexUnitType.Dip, width, Resources.DisplayMetrics); //To fit all screen sizes.

            WindowManagerLayoutParams layoutParams = new WindowManagerLayoutParams
            {
                Width = (int)floatingNotificationWidth,
                Height = ViewGroup.LayoutParams.WrapContent,
                Type = layoutType,
                Flags = WindowManagerFlags.NotFocusable, //If you are using it on immersive apps, this will avoid that app from losing the Immersive mode.
                Format = Format.Translucent,
                Gravity = GravityFlags.CenterHorizontal | GravityFlags.CenterVertical //Location of the view.
            };

            windowManager.AddView(floatingView, layoutParams);
        }

    }
}