using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using System.Collections.Generic;
using Android.Content.PM;
using Android.Provider;
using Uri = Android.Net.Uri;
using Android;
using System;

namespace GoogleApiExample
{
    [Activity(Label = "GoogleApiExample", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {

        //Android.Graphics.Bitmap copymap;


        public static Java.IO.File _file;
        public static Java.IO.File _dir;

        string thing; 
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            if (IsThereAnAppToTakePictures() == true)
            {
                CreateDirectoryForPictures();
                FindViewById<Button>(Resource.Id.launchCameraButton).Click += TakePicture;
            }
        }

        /// <summary>
        /// Apparently, some android devices do not have a camera.  To guard against this,
        /// we need to make sure that we can take pictures before we actually try to take a picture.
        /// </summary>
        /// <returns></returns>
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
                    Android.OS.Environment.DirectoryPictures), "GoogleApiExample");
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

            intent.PutExtra(MediaStore.ExtraOutput, Android.Net.Uri.FromFile(_file));
            StartActivityForResult(intent, 0);
        }


        // <summary>
        // Called automatically whenever an activity finishes
        // </summary>
        // <param name = "requestCode" ></ param >
        // < param name="resultCode"></param>
        /// <param name="data"></param>
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {

          
            Android.Graphics.Bitmap bitmap = null;
            ImageView imageView = null;

            base.OnActivityResult(requestCode, resultCode, data);
            //SetContentView(Resource.Layout.TakenPic);

            // Display in ImageView. We will resize the bitmap to fit the display.
            // Loading the full sized image will consume too much memory
            // and cause the application to crash.
            imageView = FindViewById<ImageView>(Resource.Id.takenPicture);
            int height = Resources.DisplayMetrics.HeightPixels;
            int width = 1024;
            bitmap = _file.Path.LoadAndResizeBitmap(width, height);

            
                Intent mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
                var contentUri = Android.Net.Uri.FromFile(_file);
                mediaScanIntent.SetData(contentUri);
                SendBroadcast(mediaScanIntent);

               //mageView = FindViewById<ImageView>(Resource.Id.takenPicture);
                
            

            if (bitmap != null)
            {
                SetContentView(Resource.Layout.TakenPic);
                imageView = FindViewById<ImageView>(Resource.Id.takenPicture);
                imageView.Visibility = Android.Views.ViewStates.Visible;
                imageView.SetImageBitmap(bitmap);
                SetContentView(Resource.Layout.TakenPic);
            }

            else
            {
                SetContentView(Resource.Layout.Main);
                if (IsThereAnAppToTakePictures() == true)
                    {
                        CreateDirectoryForPictures();
                        FindViewById<Button>(Resource.Id.launchCameraButton).Click += TakePicture;
                    }
            }

            //AC: workaround for not passing actual files
            // Android.Graphics.Bitmap bitmap = (Android.Graphics.Bitmap)data.Extras.Get("data");
            if (bitmap != null)
            {
                //convert bitmap into stream to be sent to Google API
                string bitmapString = "";
                using (var stream = new System.IO.MemoryStream())
                {
                    bitmap.Compress(Android.Graphics.Bitmap.CompressFormat.Jpeg, 0, stream);

                    var bytes = stream.ToArray();
                    bitmapString = System.Convert.ToBase64String(bytes);
                }

                //credential is stored in "assets" folder
                string credPath = "google_api.json";
                Google.Apis.Auth.OAuth2.GoogleCredential cred;

                //Load credentials into object form
                using (var stream = Assets.Open(credPath))
                {
                    cred = Google.Apis.Auth.OAuth2.GoogleCredential.FromStream(stream);
                }
                cred = cred.CreateScoped(Google.Apis.Vision.v1.VisionService.Scope.CloudPlatform);

                // By default, the library client will authenticate 
                // using the service account file (created in the Google Developers 
                // Console) specified by the GOOGLE_APPLICATION_CREDENTIALS 
                // environment variable. We are specifying our own credentials via json file.
                var client = new Google.Apis.Vision.v1.VisionService(new Google.Apis.Services.BaseClientService.Initializer()
                {
                    ApplicationName = "Project3",
                    HttpClientInitializer = cred
                });

                //set up request
                var request = new Google.Apis.Vision.v1.Data.AnnotateImageRequest();
                request.Image = new Google.Apis.Vision.v1.Data.Image();
                request.Image.Content = bitmapString;

                //tell google that we want to perform label detection
                request.Features = new List<Google.Apis.Vision.v1.Data.Feature>();
                request.Features.Add(new Google.Apis.Vision.v1.Data.Feature() { Type = "LABEL_DETECTION" });
                var batch = new Google.Apis.Vision.v1.Data.BatchAnnotateImagesRequest();
                batch.Requests = new List<Google.Apis.Vision.v1.Data.AnnotateImageRequest>();
                batch.Requests.Add(request);

                //send request.  Note that I'm calling execute() here, but you might want to use
                //ExecuteAsync instead
                var apiResult = client.Images.Annotate(batch).Execute();
                thing = apiResult.Responses[0].LabelAnnotations[0].Description;
                FindViewById<TextView>(Resource.Id.yourpic).Text += thing;
                SetContentView(Resource.Layout.TakenPic);

            }
                // Dispose of the Java side bitmap.
                System.GC.Collect();
        }
    }
}

