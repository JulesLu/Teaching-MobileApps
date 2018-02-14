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

namespace CameraApp2
{
    [Activity(Label = "CameraApp2", MainLauncher = true)]
    public class MainActivity : Activity

    {
        //used to track file we use
        public static Java.IO.File _file;

        //used to track the directory we use 
        public static Java.IO.File _dir;
        //bool apic;


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
            //apic = true;
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            _file = new Java.IO.File(_dir, string.Format("myPhoto_{0}.jpg", System.Guid.NewGuid()));
            //android.support.v4.content.FileProvider
            //getUriForFile(getContext(), "com.mydomain.fileprovider", newFile);
            //FileProvider.GetUriForFile

            //The line is a problem line for Android 7+ development
            //intent.PutExtra(MediaStore.ExtraOutput, Android.Net.Uri.FromFile(_file));
            //apic = true;
            StartActivityForResult(intent, 0);
        }



        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {

            Android.Graphics.Bitmap bitmap = bitmap = (Android.Graphics.Bitmap)data.Extras.Get("data");
            Android.Graphics.Bitmap copyBitmap = null;
            bool original_pic = true;
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
            //bitmap = _file.Path.LoadAndResizeBitmap(width, height);

            if (bitmap != null)
            {
                SetContentView(Resource.Layout.layout1);
                imageView = FindViewById<ImageView>(Resource.Id.takenPicture);
                copyBitmap = bitmap.Copy(Android.Graphics.Bitmap.Config.Argb8888, true);
                imageView.SetImageBitmap(bitmap);
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
            Button NegateRed = FindViewById<Button>(Resource.Id.NegateRed);
            Button NegateGreen = FindViewById<Button>(Resource.Id.NegateGreen);
            Button NegateBlue = FindViewById<Button>(Resource.Id.NegateBlue);
            Button GreyScale = FindViewById<Button>(Resource.Id.GreyScale);
            Button HighContrast = FindViewById<Button>(Resource.Id.HighContrast);
            Button WoodGrain = FindViewById<Button>(Resource.Id.Woodgrain);
            Button NewPicture = FindViewById<Button>(Resource.Id.NewPicture);
            Button Clear = FindViewById<Button>(Resource.Id.Clear);

            // check bitmap to see if it is null 
            if (copyBitmap != null)
            {
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
                    original_pic = false;
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
                    original_pic = false;
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
                    original_pic = false;

                };



                NegateRed.Click += delegate
                {
                    for (int i = 0; i < copyBitmap.Width; i++)
                    {
                        for (int j = 0; j < copyBitmap.Height; j++)
                        {
                            int p = bitmap.GetPixel(i, j);
                            Android.Graphics.Color c = new Android.Graphics.Color(p);
                            c.R = (byte)(255 - c.R);
                            copyBitmap.SetPixel(i, j, c);
                        }
                    }
                    imageView.SetImageBitmap(copyBitmap);
                    original_pic = false;
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
                    original_pic = false;
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
                    original_pic = false;
                };

                GreyScale.Click += delegate
                {
                    for (int i = 0; i < copyBitmap.Width; i++)
                    {
                        for (int j = 0; j < copyBitmap.Height; j++)
                        {
                            int average = 0;
                            int p = copyBitmap.GetPixel(i, j);
                            Android.Graphics.Color c = new Android.Graphics.Color(p);
                            int r_temp = c.R;
                            int g_temp = c.G;
                            int b_temp = c.B;

                            average = (r_temp + g_temp + b_temp) / 3;

                            c.R = Convert.ToByte(average);
                            c.G = Convert.ToByte(average);
                            c.B = Convert.ToByte(average);
                            copyBitmap.SetPixel(i, j, c);
                        }
                    }
                    imageView.SetImageBitmap(copyBitmap);
                    original_pic = false;

                };


                HighContrast.Click += delegate
                {
                    for (int i = 0; i < copyBitmap.Width; i++)
                    {
                        for (int j = 0; j < copyBitmap.Height; j++)
                        {
                            int check_num = 255 / 2;
                            int p = copyBitmap.GetPixel(i, j);
                            Android.Graphics.Color c = new Android.Graphics.Color(p);
                            int r_temp = c.R;
                            int g_temp = c.G;
                            int b_temp = c.B;

                            if (r_temp > check_num)
                            {
                                r_temp = 255;
                            }

                            else
                            {
                                r_temp = 0;
                            }

                            if (g_temp > check_num)
                            {
                                g_temp = 255;
                            }

                            else
                            {
                                g_temp = 0;
                            }

                            if (b_temp > check_num)
                            {
                                b_temp = 255;
                            }

                            else
                            {
                                b_temp = 0;
                            }

                            c.R = Convert.ToByte(r_temp);
                            c.G = Convert.ToByte(g_temp);
                            c.B = Convert.ToByte(b_temp);
                            copyBitmap.SetPixel(i, j, c);
                        }
                    }
                    imageView.SetImageBitmap(copyBitmap);
                    original_pic = false;

                };

                WoodGrain.Click += delegate
                {
                    for (int i = 0; i < copyBitmap.Width; i++)
                    {
                        for (int j = 0; j < copyBitmap.Height; j++)
                        {
                            int p = copyBitmap.GetPixel(i, j);
                            Android.Graphics.Color c = new Android.Graphics.Color(p);
                            Random r = new Random();
                            int randnum = r.Next(-20, 21);
                            int r_temp = c.R;
                            int g_temp = c.G;
                            int b_temp = c.B;

                            r_temp = r_temp + randnum;
                            g_temp = g_temp + randnum;
                            b_temp = b_temp + randnum;

                            if (r_temp > 255)
                            {
                                c.R = 255;
                            }

                            else if (r_temp < 0)
                            {
                                c.R = 0;
                            }

                            else
                            {
                                c.R = Convert.ToByte(r_temp);
                            }

                            if (g_temp > 255)
                            {
                                c.G = 255;
                            }

                            else if (g_temp < 0)
                            {
                                c.G = 0;
                            }

                            else
                            {
                                c.G = Convert.ToByte(g_temp);
                            }

                            if (b_temp > 255)
                            {
                                c.B = 255;
                            }

                            else if (b_temp < 0)
                            {
                                c.B = 0;
                            }

                            else
                            {
                                c.B = Convert.ToByte(b_temp);
                            }

                            copyBitmap.SetPixel(i, j, c);
                        }
                    }
                    imageView.SetImageBitmap(copyBitmap);
                    original_pic = false;
                };

                


            NewPicture.Click += delegate
            {
                SetContentView(Resource.Layout.Main);
                if (IsThereAnAppToTakePictures() == true)
                {
                    CreateDirectoryForPictures();
                    FindViewById<Button>(Resource.Id.launchCamera).Click += TakePicture;
                }
            };

            Clear.Click += delegate
            {
                if (original_pic == false)
                {
                    copyBitmap = bitmap.Copy(Android.Graphics.Bitmap.Config.Argb8888, true);
                    imageView.SetImageBitmap(copyBitmap);
                    original_pic = true;
                }
                else if (original_pic)
                {
                    Toast.MakeText(this, "No Effects Used", ToastLength.Short).Show();
                }

            };


        }





        }
    }
}
