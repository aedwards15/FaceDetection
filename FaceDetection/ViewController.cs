using System;
using CoreGraphics;
using AssetsLibrary;
using UIKit;
using Foundation;
using Emgu.CV;
using Emgu.CV.Structure;
using MonoTouch.Dialog;
using FaceDetection;
using System.Collections.Generic;
using System.Drawing;
using CoreImage;
using Emgu.CV.Util;
using System.IO;

namespace FaceDetection
{
	public partial class ViewController : UIViewController
	{
		public ViewController (IntPtr handle) : base (handle)
		{
		}

        UIImagePickerController imagePicker;
        UIImageView imageView;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            Title = "Choose Photo";
            View.BackgroundColor = UIColor.White;

            imageView = ImageViewer;
        }

        List<Rectangle> faces = new List<Rectangle>();
        List<Rectangle> leftEyes = new List<Rectangle>();
        List<Rectangle> rightEyes = new List<Rectangle>();
        List<Rectangle> upperBodies = new List<Rectangle>();
        List<Rectangle> lowerBodies = new List<Rectangle>();
        List<Rectangle> mouths = new List<Rectangle>();
        List<Rectangle> noses = new List<Rectangle>();
        List<Rectangle> people = new List<Rectangle>();
        List<Rectangle> bodies = new List<Rectangle>();

        CGPoint FlipForBottomOrigin (CGPoint point, int height)
        {
            return new CGPoint(point.X, height - point.Y);
        }

        void Process()
        {
            /*var context = CIContext.FromOptions(null);
            var detector = CIDetector.CreateFaceDetector (context, true);
            var ciImage = CIImage.FromCGImage(imageView.Image.CGImage);
            CIFeature[] features = detector.FeaturesInImage(ciImage);

            using (Image<Bgr, Byte> image = new Image<Bgr, Byte>(imageView.Image.CGImage))
            {
                if (features != null)
                {
                    foreach (CIFaceFeature face in features)
                    {
                        CGPoint pTest = FlipForBottomOrigin(face.LeftEyePosition, 960);
                        image.Draw(new Rectangle((int)pTest.X, (int)pTest.Y, 200, 200), new Bgr(Color.Blue), 3);
                    }
                }
            */
            faces = new List<Rectangle>();
            leftEyes = new List<Rectangle>();
            rightEyes = new List<Rectangle>();
            upperBodies = new List<Rectangle>();
            lowerBodies = new List<Rectangle>();
            mouths = new List<Rectangle>();
            noses = new List<Rectangle>();
            people = new List<Rectangle>();
            bodies = new List<Rectangle>();

            CGImage myImage = imageView.Image.CGImage;

            long processingTime;
            using (Image<Bgr, Byte> image = new Image<Bgr, Byte>(myImage))
            {
                CGRect p = imageView.Frame;

                Dictionary<String, List<Rectangle>> recObjects = new Dictionary<string, List<Rectangle>>();
                recObjects.Add("faces", faces);
                recObjects.Add("leftEyes", leftEyes);
                recObjects.Add("rightEyes", rightEyes);
                recObjects.Add("upperBodies", upperBodies);
                recObjects.Add("lowerBodies", lowerBodies);
                recObjects.Add("mouths", mouths);
                recObjects.Add("noses", noses);
                recObjects.Add("people", people);
                recObjects.Add("bodies", bodies);

                Dictionary<String, String> facialFeatures = new Dictionary<string, string>();
                facialFeatures.Add("face", "haarcascade_frontalface_alt.xml");
                facialFeatures.Add("reye", "haarcascade_righteye_2splits.xml");
                facialFeatures.Add("leye", "haarcascade_lefteye_2splits.xml");
                facialFeatures.Add("mouth", "Mouth.xml");
                facialFeatures.Add("nose", "Nariz.xml");
                facialFeatures.Add("ubody", "haarcascade_mcs_upperbody.xml");
                facialFeatures.Add("lbody", "haarcascade_lowerbody.xml");
                facialFeatures.Add("body", "haarcascade_fullbody.xml");
                DetectFace.Detect(
                    image.Mat,
                    facialFeatures,
                    recObjects,
                    out processingTime
                );

                /*foreach (Rectangle body in upperBodies)
                    image.Draw(body, new Bgr(Color.LimeGreen), 3);
                foreach (Rectangle body in lowerBodies)
                    image.Draw(body, new Bgr(Color.LimeGreen), 3);
                foreach (Rectangle body in bodies)
                    image.Draw(body, new Bgr(Color.Yellow), 3);
                //foreach (Rectangle person in people)
                //    image.Draw(person, new Bgr(Color.Blue), 5);
                foreach (Rectangle face in faces)
                    image.Draw(face, new Bgr(Color.Red), 3);
                foreach (Rectangle eye in leftEyes)
                    image.Draw(eye, new Bgr(Color.LightBlue), 3);
                foreach (Rectangle eye in rightEyes)
                    image.Draw(eye, new Bgr(Color.DarkBlue), 3);
                foreach (Rectangle mouth in mouths)
                    image.Draw(mouth, new Bgr(Color.Yellow), 3);
                foreach (Rectangle nose in noses)
                    image.Draw(nose, new Bgr(Color.Violet), 3);*/

                //using (Image<Bgr, Byte> resized = image.Resize((int)Window.Frame.Width, (int)Window.Frame.Height, Emgu.CV.CvEnum.Inter.Linear, true))
                //{
                //imageView.Frame = new RectangleF(PointF.Empty, resized.Size);
                imageView.Image = image.ToUIImage();

                if (faces.Count > 0)
                {
                    using (CGImage cr = imageView.Image.CGImage.WithImageInRect(faces[0]))
                    {
                        UIImage cropped = UIImage.FromImage(cr);
                        //imageView.Image = cropped;
                    }
                }
                //}

                //messageLabel.Text = String.Format("Face: (x: {0}, y: {1}, h: {2}, w: {3}", faces[0].X, faces[0].Y, faces[0].Height, faces[0].Width);
            
            //messageLabel.Text = String.Format("Processing Time: {0} milliseconds.", processingTime);
            //messageElement.GetImmediateRootElement().Reload(messageElement, UITableViewRowAnimation.Automatic);

            imageView.SetNeedsDisplay();
            }
        }

        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            base.TouchesEnded(touches, evt);
            UITouch touch = touches.AnyObject as UITouch;

            if (touch != null && imageView.Image != null)
            {
                CGPoint imageViewPoint = touch.LocationInView(this.imageView);

                nfloat percentX = imageViewPoint.X / imageView.Frame.Size.Width;
                nfloat percentY = imageViewPoint.Y / imageView.Frame.Size.Height;

                CGPoint imagePoint = new CGPoint(imageView.Image.Size.Width * percentX, imageView.Image.Size.Height * percentY);
            
                Rectangle rect = new Rectangle(new Point((int)imagePoint.X, (int)imagePoint.Y), new Size(1,1));

                foreach (Rectangle face in faces)
                {
                    if (rect.IntersectsWith(face))
                    {
                        bool isFace = true;
                        foreach (Rectangle eye in leftEyes)
                        {
                            if (rect.IntersectsWith(eye))
                            {
                                isFace = false;
                                messageLabel.Text = String.Format("Left Eye: (x: {0}, y: {1}, w: {2}, h: {3})", eye.X, (imageView.Image.Size.Height - (eye.Y + eye.Height)), eye.Width, eye.Height);
                            }
                        }

                        foreach (Rectangle eye in rightEyes)
                        {
                            if (rect.IntersectsWith(eye))
                            {
                                isFace = false;
                                messageLabel.Text = String.Format("Right Eye: (x: {0}, y: {1}, w: {2}, h: {3})", eye.X, (imageView.Image.Size.Height - (eye.Y + eye.Height)), eye.Width, eye.Height);
                            }
                        }

                        foreach (Rectangle mouth in mouths)
                        {
                            if (rect.IntersectsWith(mouth))
                            {
                                isFace = false;
                                messageLabel.Text = String.Format("Mouth (x: {0}, y: {1}, w: {2}, h: {3})", mouth.X, (imageView.Image.Size.Height - (mouth.Y + mouth.Height)), mouth.Width, mouth.Height);
                            }
                        }

                        foreach (Rectangle nose in noses)
                        {
                            if (rect.IntersectsWith(nose))
                            {
                                isFace = false;
                                messageLabel.Text = String.Format("Nose: (x: {0}, y: {1}, w: {2}, h: {3})", nose.X, (imageView.Image.Size.Height - (nose.Y + nose.Height)), nose.Width, nose.Height);
                            }
                        }

                        if (isFace)
                        {
                            messageLabel.Text = String.Format("Face: (x: {0}, y: {1}, w: {2}, h: {3})", face.X, (imageView.Image.Size.Height - (face.Y + face.Height)), face.Width, face.Height);
                        }
                    }
                }
            }
        }


        // Do something when the
        void Handle_Canceled(object sender, EventArgs e)
        {
            Console.WriteLine("picker cancelled");
            imagePicker.DismissModalViewController(true);
        }

        private String imageURL = String.Empty;
        // This is a sample method that handles the FinishedPickingMediaEvent
        protected void Handle_FinishedPickingMedia(object sender, UIImagePickerMediaPickedEventArgs e)
        {
            // determine what was selected, video or image
            bool isImage = false;
            switch(e.Info[UIImagePickerController.MediaType].ToString())
            {
                case "public.image":
                    Console.WriteLine("Image selected");
                    isImage = true;
                    break;

                case "public.video":
                    Console.WriteLine("Video selected");
                    break;
            }

            Console.Write("Reference URL: [" + UIImagePickerController.ReferenceUrl + "]");

            // get common info (shared between images and video)
            NSUrl referenceURL = e.Info[new NSString("UIImagePickerControllerReferenceURL")] as NSUrl;
            imageURL = referenceURL.AbsoluteString;
            if(referenceURL != null)
                Console.WriteLine(referenceURL.ToString());

            // if it was an image, get the other image info
            if(isImage)
            {
                // get the original image
                originalImage = e.Info[UIImagePickerController.OriginalImage] as UIImage;
                if(originalImage != null)
                {
                    // do something with the image
                    Console.WriteLine("got the original image");
                    imageView.Image = originalImage;
                    imageLabel.Text = "Width: " + imageView.Image.Size.Width + ", Height: " + imageView.Image.Size.Height;
                }

                // get the edited image
                UIImage editedImage = e.Info[UIImagePickerController.EditedImage] as UIImage;
                if(editedImage != null)
                {
                    // do something with the image
                    Console.WriteLine("got the edited image");
                    imageView.Image = editedImage;
                }

                //- get the image metadata
                NSDictionary imageMetadata = e.Info[UIImagePickerController.MediaMetadata] as NSDictionary;
                if(imageMetadata != null)
                {
                    // do something with the metadata
                    Console.WriteLine("got image metadata");
                }

            }
            // if it's a video
            else
            {
                // get video url
                NSUrl mediaURL = e.Info[UIImagePickerController.MediaURL] as NSUrl;
                if(mediaURL != null)
                {
                    //
                    Console.WriteLine(mediaURL.ToString());
                }
            }

            // dismiss the picker
            imagePicker.DismissModalViewController(true);
        }

        partial void GetImage_TouchUpInside(UIButton sender)
        {
            // create a new picker controller
            imagePicker = new UIImagePickerController();

            // set our source to the photo library
            imagePicker.SourceType = UIImagePickerControllerSourceType.PhotoLibrary;

            // set what media types
            imagePicker.MediaTypes = UIImagePickerController.AvailableMediaTypes(UIImagePickerControllerSourceType.PhotoLibrary);

            imagePicker.FinishedPickingMedia += Handle_FinishedPickingMedia;
            imagePicker.Canceled += Handle_Canceled;

            // show the picker
            NavigationController.PresentModalViewController(imagePicker, true);
            //UIPopoverController picc = new UIPopoverController(imagePicker);
        }

        partial void ProcessImage_TouchUpInside(UIButton sender)
        {
            Process();
            if (faces.Count > 0)
            {
                using (CGImage cr = imageView.Image.CGImage.WithImageInRect (faces[0])) {
                    //UIImage cropped = UIImage.FromImage (cr);
                    VectorOfVectorOfPoint contourEdges = EdgeDetection.DetectEdges(imageView.Image, Convert.ToDouble(textThreshold1.Text), Convert.ToDouble(textThreshold2.Text), Convert.ToInt32(textAperture.Text), switchSwitch.On);

                    /*VectorOfVectorOfPoint temp = new VectorOfVectorOfPoint();

                    for (int j = 0; j < contourEdges.Size; j++)
                    {
                        List<Point> rowTemp = new List<Point>();
                        for (int i = 0; i < contourEdges[j].Size; i++)
                        {
                            Point p = new Point(contourEdges[j][i].X + faces[0].X, contourEdges[j][i].Y + faces[0].Y);
                            rowTemp.Add(p);
                        }
                        VectorOfPoint point = new VectorOfPoint(rowTemp.ToArray());
                        temp.Push(point);
                    }*/


                    Image<Bgr, Byte> fullImage = new Image<Bgr, byte>(imageView.Image.CGImage);
                    //fullImage.Draw(new Rectangle(rowTemp[0], new Size(50,50)), new Bgr(Color.LightSkyBlue), 2);
                    for (int i = 0; i < contourEdges.Size; i++) {
                        CvInvoke.DrawContours(fullImage, contourEdges, i, new MCvScalar(255,255,0), 4);
                        //fullImage.Draw(new Rectangle(rowTemp[i], new Size(10,10)), new Bgr(Color.LightSkyBlue), 1);
                    }

                    foreach (Rectangle body in upperBodies)
                        fullImage.Draw(body, new Bgr(Color.LimeGreen), 3);
                    foreach (Rectangle body in lowerBodies)
                        fullImage.Draw(body, new Bgr(Color.LimeGreen), 3);
                    foreach (Rectangle body in bodies)
                        fullImage.Draw(body, new Bgr(Color.Yellow), 3);
                    //foreach (Rectangle person in people)
                    //    image.Draw(person, new Bgr(Color.Blue), 5);
                    foreach (Rectangle face in faces)
                        fullImage.Draw(face, new Bgr(Color.Red), 3);
                    foreach (Rectangle eye in leftEyes)
                        fullImage.Draw(eye, new Bgr(Color.LightBlue), 3);
                    foreach (Rectangle eye in rightEyes)
                        fullImage.Draw(eye, new Bgr(Color.DarkBlue), 3);
                    foreach (Rectangle mouth in mouths)
                        fullImage.Draw(mouth, new Bgr(Color.Yellow), 3);
                    foreach (Rectangle nose in noses)
                        fullImage.Draw(nose, new Bgr(Color.Violet), 3);
                    
                    imageView.Image = fullImage.ToUIImage();
                }

                var FileManager = new NSFileManager ();
                var appGroupContainer = FileManager.GetContainerUrl ("group.com.AngryElfStudios.PhotoData");
                var appGroupContainerPath = appGroupContainer.Path;


                var filename = Path.Combine (appGroupContainerPath, "Data.txt");
                Console.WriteLine ("Group Path: " + filename);

                String dataToWrite = imageURL + "\n";
                dataToWrite += faces [0].X + "," + (imageView.Image.Size.Height - (faces [0].Y + faces [0].Height)) + "," + faces [0].Width + "," + faces [0].Height + "\n";
                dataToWrite += leftEyes [0].X + "," + (imageView.Image.Size.Height - (leftEyes [0].Y + leftEyes [0].Height)) + "," + leftEyes [0].Width + "," + leftEyes [0].Height + "\n";
                dataToWrite += rightEyes [0].X + "," + (imageView.Image.Size.Height - (rightEyes [0].Y + rightEyes [0].Height)) + "," + rightEyes [0].Width + "," + rightEyes [0].Height + "\n";
                dataToWrite += mouths [0].X + "," + (imageView.Image.Size.Height - (mouths [0].Y + mouths [0].Height)) + "," + mouths [0].Width + "," + mouths [0].Height + "\n";
                dataToWrite += noses [0].X + "," + (imageView.Image.Size.Height - (noses [0].Y + noses [0].Height)) + "," + noses [0].Width + "," + noses [0].Height + "\n";

                if (File.Exists (filename)) {
                    File.Delete (filename);
                }


                File.WriteAllText (filename, dataToWrite);

                var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                filename = Path.Combine (documents, "Data.txt");
                File.WriteAllText (filename, dataToWrite);

            }
        }

        UIImage originalImage;
        partial void ResetImage_TouchUpInside(UIButton sender)
        {
            if (originalImage != null)
            {
                imageView.Image = originalImage;
            }
        }

        partial void FeatureMap_TouchUpInside (UIButton sender)
        {
            if (!UIApplication.SharedApplication.OpenUrl (NSUrl.FromString ("iOSDevTips://com.AngryElfStudios.FeatureMapping"))) {
                //Use the code below to go to itunes if application not found.
                UIApplication.SharedApplication.OpenUrl (NSUrl.FromString ("itms://itunes.apple.com/in/app/appname/appid"));
            }
        }
	}
}

