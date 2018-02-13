using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using System.Collections.Generic;
using Android.Content.PM;
using Android.Provider;
using Uri = Android.Net.Uri;
using Android;

namespace CameraApp2
{
    [Activity(Label = "CameraApp2", MainLauncher = true)]
    public class MainActivity : Activity

    {
        //used to track file we use
        public static Java.IO.File _file;

        //used to track the directory we use 
        public static Java.IO.File _dir;
        bool apic;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);


            if (IsThereAnAppToTakePictures() == true)
            {
                CreateDirectoryForPictures();
                FindViewById<Button>(Resource.Id.launchCamera).Click += TakePicture;

            }


        }



        private bool IsThereAnAppToTakePictures()
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            IList<ResolveInfo> availableActivities =
                PackageManager.QueryIntentActivities
                (intent, PackageInfoFlags.MatchDefaultOnly);
            return availableActivities != null && availableActivities.Count > 0;
        }



        private void CreateDirectoryForPictures()
        {
            _dir = new Java.IO.File(
                Android.OS.Environment.GetExternalStoragePublicDirectory(
                    Android.OS.Environment.DirectoryPictures), "CameraApp2");
            if (!_dir.Exists())
            {
                _dir.Mkdirs();
            }
        }

        private void TakePicture(object sender, System.EventArgs e)
        {
            apic = true;
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            _file = new Java.IO.File(_dir, string.Format("myPhoto_{0}.jpg", System.Guid.NewGuid()));
            //android.support.v4.content.FileProvider
            //getUriForFile(getContext(), "com.mydomain.fileprovider", newFile);
            //FileProvider.GetUriForFile

            //The line is a problem line for Android 7+ development
            intent.PutExtra(MediaStore.ExtraOutput, Android.Net.Uri.FromFile(_file));
            apic = true;
            StartActivityForResult(intent, 0);
        }



        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {

            Android.Graphics.Bitmap bitmap = null;
            Android.Graphics.Bitmap copyBitmap = null; ;
            ImageView imageView = null;
            base.OnActivityResult(requestCode, resultCode, data);


            Intent mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
            var contentUri = Android.Net.Uri.FromFile(_file);
            mediaScanIntent.SetData(contentUri);
            SendBroadcast(mediaScanIntent);


            // Display in ImageView. We will resize the bitmap to fit the display.
            // Loading the full sized image will consume too much memory
            // and cause the application to crash.      
            imageView = FindViewById<ImageView>(Resource.Id.takenPicture);
            int height = Resources.DisplayMetrics.HeightPixels;
            int width = height;
            // width = imageView.Height;
            bitmap = _file.Path.LoadAndResizeBitmap(width, height);

            if (bitmap != null)
            {
                copyBitmap = bitmap.Copy(Android.Graphics.Bitmap.Config.Argb8888, true);
                imageView.SetImageBitmap(copyBitmap);
                imageView.Visibility = Android.Views.ViewStates.Visible;
            }

            else
            {
                //SetContentView(Resource.Layout.Main);
                if (IsThereAnAppToTakePictures() == true)
                {
                    CreateDirectoryForPictures();
                    FindViewById<Button>(Resource.Id.launchCamera).Click += TakePicture;
                }
            }

            Button RemoveRed = FindViewById<Button>(Resource.Id.removeRed);
            Button RemoveGreen = FindViewById<Button>(Resource.Id.removeGreen);
            Button RemoveBlue = FindViewById<Button>(Resource.Id.removeBlue);
            Button NegateRed = FindViewById<Button>(Resource.Id.negateRed);
            Button NegateGreen = FindViewById<Button>(Resource.Id.NegateGreen);
            Button NegateBlue = FindViewById<Button>(Resource.Id.Negateblue);

            // check bitmap to see if it is null 
            if (copyBitmap != null)
            {
                SetContentView(Resource.Layout.layout1);
                RemoveRed.Click += delegate
                {
                    for (int i = 0; i < copyBitmap.Width; i++)
                    {
                        for (int j = 0; j < copyBitmap.Height; j++)
                        {
                            int p = bitmap.GetPixel(i, j);
                            Android.Graphics.Color c = new Android.Graphics.Color(p);
                            c.R = 0;
                            copyBitmap.SetPixel(i, j, c);
                        }
                    }
                    imageView.SetImageBitmap(copyBitmap);
                };

                RemoveGreen.Click += delegate
                {
                    for (int i = 0; i < copyBitmap.Width; i++)
                    {
                        for (int j = 0; j < copyBitmap.Height; j++)
                        {
                            int p = bitmap.GetPixel(i, j);
                            Android.Graphics.Color c = new Android.Graphics.Color(p);
                            c.G = 0;
                            copyBitmap.SetPixel(i, j, c);
                        }
                    }
                    imageView.SetImageBitmap(copyBitmap);
                };


                RemoveBlue.Click += delegate
                {
                    for (int i = 0; i < copyBitmap.Width; i++)
                    {
                        for (int j = 0; j < copyBitmap.Height; j++)
                        {
                            int p = bitmap.GetPixel(i, j);
                            Android.Graphics.Color c = new Android.Graphics.Color(p);
                            c.R = 0;
                            copyBitmap.SetPixel(i, j, c);
                        }
                    }
                    imageView.SetImageBitmap(copyBitmap);

                };



                NegateRed.Click += delegate
                {
                    for (int i = 0; i < copyBitmap.Width; i++)
                    {
                        for (int j = 0; j < copyBitmap.Height; j++)
                        {
                            int p = bitmap.GetPixel(i, j);
                            Android.Graphics.Color c = new Android.Graphics.Color(p);
                            c.R = (byte)(255-c.R);
                            copyBitmap.SetPixel(i, j, c);
                        }
                    }
                    imageView.SetImageBitmap(copyBitmap);
                };



                NegateGreen.Click += delegate
                {
                    for (int i = 0; i < copyBitmap.Width; i++)
                    {
                        for (int j = 0; j < copyBitmap.Height; j++)
                        {
                            int p = bitmap.GetPixel(i, j);
                            Android.Graphics.Color c = new Android.Graphics.Color(p);
                            c.G = (byte)(255 - c.G);
                            copyBitmap.SetPixel(i, j, c);
                        }
                    }
                    imageView.SetImageBitmap(copyBitmap);
                };


                NegateBlue.Click += delegate
                {
                    for (int i = 0; i < copyBitmap.Width; i++)
                    {
                        for (int j = 0; j < copyBitmap.Height; j++)
                        {
                            int p = bitmap.GetPixel(i, j);
                            Android.Graphics.Color c = new Android.Graphics.Color(p);
                            c.B = (byte)(255 - c.B);
                            copyBitmap.SetPixel(i, j, c);
                        }
                    }
                    imageView.SetImageBitmap(copyBitmap);
                };


            }



            
          
        }
    }
}
