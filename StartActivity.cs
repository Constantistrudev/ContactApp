using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Media;
using AndroidX.Core.App;
using Android.Support.V4.App;
using AndroidX.Core.Content;
using System;

namespace ContactApp
{

    [Activity(Label = "Контакты",
        MainLauncher = true, Theme = "@style/SplashTheme")]
    public class Activity1 : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            var player = MediaPlayer.Create(this, Resource.Drawable.sound);
            player.Start();
            System.Threading.Tasks.Task.Delay(2000).ContinueWith(t =>
            {
                
                StartActivity(new Intent(this, typeof(MainActivity)));
            });
        }
    }
}