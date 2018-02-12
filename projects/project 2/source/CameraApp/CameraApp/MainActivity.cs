using Android.App;
using Android.Widget;
using Android.OS;
using Android.Graphics;
using Android.Content;
using System.Collections.Generic;
using Android.Content.PM;
using Android.Provider;
using Uri = Android.Net.Uri;
namespace CameraApp
{
    [Activity(Label = "CameraApp", MainLauncher = true)]
    public class MainActivity : Activity

    {
        public static Java.IO.File _file;
        public static Java.IO.File _dir;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);


            if (IsThereAnAppToTakePictures() == true)
            {
                CreateDirectoryForPictures();
                FindViewById<Button>(Resource.Id.launchCamera).Click += TakePicture;
                //Button 
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
                        Android.OS.Environment.DirectoryPictures), "CameraApp");
                if (!_dir.Exists())
                {
                    _dir.Mkdirs();
                }
            }

        private void TakePicture(object sender, System.EventArgs e)
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            _file = new Java.IO.File(_dir, string.Format("myPhoto_{0}.jpg", System.Guid.NewGuid()));
            //android.support.v4.content.FileProvider
            //getUriForFile(getContext(), "com.mydomain.fileprovider", newFile);
            //FileProvider.GetUriForFile

            //The line is a problem line for Android 7+ development
            intent.PutExtra(MediaStore.ExtraOutput, Android.Net.Uri.FromFile(_file));
            StartActivityForResult(intent, 0);
        }


        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            //Make image available in the gallery
            /*
            Intent mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
            var contentUri = Android.Net.Uri.FromFile(_file);
            mediaScanIntent.SetData(contentUri);
            SendBroadcast(mediaScanIntent);
            */

            if ((resultCode == Result.Ok) && (data != null))
            {
                //Make image available in the gallery
                //Test to see if we came from the camera or gallery
                //If we came from galley no need to make pic available
                if (requestCode == 0)
                {
                    Intent mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
                    var contentUri = Android.Net.Uri.FromFile(_file);
                    mediaScanIntent.SetData(contentUri);
                    SendBroadcast(mediaScanIntent);
                }

                else
                {
                    Uri uri = data.Data;
                    //_file.SetImageURI(uri);

                }
            }

            // Display in ImageView. We will resize the bitmap to fit the display.
            // Loading the full sized image will consume too much memory
            // and cause the application to crash.
            ImageView imageView = FindViewById<ImageView>(Resource.Id.takenPicture);
            int height = Resources.DisplayMetrics.HeightPixels;
            int width = imageView.Height;
            Android.Graphics.Bitmap bitmap = _file.Path.
            Android.Graphics.Bitmap copyBitmap = bitmap.Copy(Android.Graphics.Bitmap.Config.Argb8888, true);
            imageView.SetImageBitmap(copyBitmap);
            ////this code removes all red from a picture
            //for (int i = 0; i < bitmap.Width; i++)
            //{
            //    for (int j = 0; j < bitmap.Height; j++)
            //    {
            //        int p = bitmap.GetPixel(i, j);
            //        Android.Graphics.Color c = new Android.Graphics.Color(p);
            //        c.R = 0;
            //        copyBitmap.SetPixel(i, j, c);
            //    }
            //}
            //if (copyBitmap != null)
            //{
            //    imageView.SetImageBitmap(copyBitmap);
            //    imageView.Visibility = Android.Views.ViewStates.Visible;
            //    bitmap = null;
            //    copyBitmap = null;
            //}

            //// Dispose of the Java side bitmap.
            //System.GC.Collect();
        }
    }
}











