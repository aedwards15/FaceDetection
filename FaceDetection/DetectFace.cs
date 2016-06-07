//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;

namespace FaceDetection
{
   public static class DetectFace
   {
      public static void Detect(
		Mat image, Dictionary<String, String> facialFeatures, 
            Dictionary<string, List<Rectangle>> recObjects,
        out long detectionTime)
		{
			Stopwatch watch;
         
			//Many opencl functions require opencl compatible gpu devices. 
			//As of opencv 3.0-alpha, opencv will crash if opencl is enable and only opencv compatible cpu device is presented
			//So we need to call CvInvoke.HaveOpenCLCompatibleGpuDevice instead of CvInvoke.HaveOpenCL (which also returns true on a system that only have cpu opencl devices).
			//CvInvoke.UseOpenCL = tryUseOpenCL && CvInvoke.HaveOpenCLCompatibleGpuDevice;

			//Read the HaarCascade objects
			using (CascadeClassifier face = new CascadeClassifier(facialFeatures["face"]))
			using (CascadeClassifier eyeR = new CascadeClassifier(facialFeatures["reye"]))
			using (CascadeClassifier eyeL = new CascadeClassifier(facialFeatures["leye"]))
            using (CascadeClassifier mouth = new CascadeClassifier(facialFeatures["mouth"]))
            using (CascadeClassifier nose = new CascadeClassifier(facialFeatures["nose"]))
			using (CascadeClassifier bodyU = new CascadeClassifier(facialFeatures["ubody"]))
			using (CascadeClassifier bodyL = new CascadeClassifier(facialFeatures["lbody"]))
            using (CascadeClassifier body = new CascadeClassifier(facialFeatures["body"]))
			{
				watch = Stopwatch.StartNew();

                /*List<Rectangle> regions = new List<Rectangle>();
                using (HOGDescriptor des = new HOGDescriptor())
                {
                    des.SetSVMDetector(HOGDescriptor.GetDefaultPeopleDetector());

                    watch = Stopwatch.StartNew();
                    Emgu.CV.Structure.MCvObjectDetection[] test = des.DetectMultiScale(image);
                    
                    foreach (MCvObjectDetection obj in test)
                    {
                        regions.Add(obj.Rect);
                    }
                    recObjects["people"].AddRange(regions);
                }*/

				using (UMat ugray = new UMat())
				{
					CvInvoke.CvtColor(image, ugray, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);

					//normalizes brightness and increases contrast of the image
					CvInvoke.EqualizeHist(ugray, ugray);

                    Rectangle[] bodiesDetectedU = bodyU.DetectMultiScale(
                        ugray,
                        1.01,
                        25,
                        new Size(600, 600));
                    recObjects["upperBodies"].AddRange(bodiesDetectedU);

                    Rectangle[] bodiesDetectedL = bodyL.DetectMultiScale(
                        ugray,
                        1.01,
                        25,
                        new Size(600, 600)); 

                    recObjects["lowerBodies"].AddRange(bodiesDetectedL);

                    Rectangle[] bodiesDetected = body.DetectMultiScale(
                        ugray,
                        1.01,
                        25,
                        new Size(600, 600));
                    recObjects["bodies"].AddRange(bodiesDetected);

					//Detect the faces  from the gray scale image and store the locations as rectangle
					//The first dimensional is the channel
					//The second dimension is the index of the rectangle in the specific channel
                    Rectangle[] facesDetected = {};
                    float reduceBy = 1.35f;

                    while (facesDetected.Length == 0 && reduceBy >= 1f)
                    {
                        facesDetected = face.DetectMultiScale(
                            ugray,
                            reduceBy,
                            10,
                            new Size(50, 50));

                        reduceBy -= .1f;
                    }

                    reduceBy = 1.35f;
                     
					recObjects["faces"].AddRange(facesDetected);

					foreach (Rectangle f in facesDetected)
					{
                        Rectangle rightHalf = new Rectangle(f.X, f.Y, f.Width / 2, f.Height);
                        Rectangle leftHalf = new Rectangle((f.X + (f.Width / 2)), f.Y, (f.Width / 2), f.Height);

						//Get the region of interest on the faces
                        using (UMat faceRegion = new UMat(ugray, rightHalf))
                        {
                            {
                                Rectangle[] eyesDetectedR = eyeR.DetectMultiScale(
                                                                faceRegion,
                                                                1.05,
                                                                10,
                                                                new Size(20, 20));
                                
                                Rectangle eyeRect = new Rectangle();

                                foreach (Rectangle e in eyesDetectedR)
                                {
                                    if (e.Height >= eyeRect.Height && e.Width >= eyeRect.Width)
                                    {
                                        eyeRect = e;
                                        eyeRect.Offset(rightHalf.X, rightHalf.Y);
                                    }
                                }

                                /*if (eyeRect.Width > eyeRect.Height)
                                {
                                    bool flip = true;
                                    while (eyeRect.Width > eyeRect.Height)
                                    {
                                        if (flip)
                                        {
                                            eyeRect.Height++;
                                            flip = false;
                                        }
                                        else
                                        {
                                            eyeRect.Y--;
                                            flip = true;
                                        }
                                    }
                                }
                                else if (eyeRect.Width < eyeRect.Height)
                                {
                                    bool flip = true;
                                    while (eyeRect.Width < eyeRect.Height)
                                    {
                                        if (flip)
                                        {
                                            eyeRect.Width++;
                                            flip = false;
                                        }
                                        else
                                        {
                                            eyeRect.X--;
                                            flip = true;
                                        }
                                    }
                                }*/

                                recObjects["rightEyes"].Add(eyeRect);
                            }
                        }

                        using (UMat faceRegion = new UMat(ugray, leftHalf))
                        {
                            {
                                Rectangle[] eyesDetectedL = eyeL.DetectMultiScale(
                                                                faceRegion,
                                                                1.01,
                                                                5,
                                                                new Size(10, 10));
                                
                                Rectangle eyeRect = new Rectangle();

                                foreach (Rectangle e in eyesDetectedL)
                                {
                                    if (e.Height >= eyeRect.Height && e.Width >= eyeRect.Width)
                                    {
                                        eyeRect = e;
                                        eyeRect.Offset(leftHalf.X, leftHalf.Y);
                                    }
                                }

                                recObjects["leftEyes"].Add(eyeRect);
                            }
                        }

                        using (UMat faceRegion = new UMat(ugray, f))
                        {
                            {
                                Rectangle[] mouthDetected = mouth.DetectMultiScale(
                                                                faceRegion,
                                                                1.05,
                                                                30,
                                                                new Size(20,20));
                                
                                Rectangle mouthRect = new Rectangle();

                                foreach (Rectangle e in mouthDetected)
                                {
                                    e.Offset(f.X, f.Y);
                                    bool intersectsRightEye = e.IntersectsWith(recObjects["rightEyes"][0]);
                                    bool intersectsLeftEye = e.IntersectsWith(recObjects["leftEyes"][0]);
                                    if (e.Height >= mouthRect.Height && e.Width >= mouthRect.Width 
                                        && !(intersectsRightEye || intersectsLeftEye))
                                    {
                                        mouthRect = e;
                                    }
                                }

                                /*if (mouthRect.Width > mouthRect.Height)
                                {
                                    bool flip = true;
                                    while (mouthRect.Width > mouthRect.Height)
                                    {
                                        if (flip)
                                        {
                                            mouthRect.Height++;
                                            flip = false;
                                        }
                                        else
                                        {
                                            mouthRect.Y--;
                                            flip = true;
                                        }
                                    }
                                }
                                else if (mouthRect.Width < mouthRect.Height)
                                {
                                    bool flip = true;
                                    while (mouthRect.Width < mouthRect.Height)
                                    {
                                        if (flip)
                                        {
                                            mouthRect.Width++;
                                            flip = false;
                                        }
                                        else
                                        {
                                            mouthRect.X--;
                                            flip = true;
                                        }
                                    }
                                }*/

                                recObjects["mouths"].Add(mouthRect);
                            }

                            {
                                Rectangle[] noseDetected = nose.DetectMultiScale(
                                                               faceRegion,
                                                               1.05,
                                                               10,
                                                               new Size(50, 50));
                                
                                Rectangle noseRect = new Rectangle();

                                foreach (Rectangle e in noseDetected)
                                {
                                    if (e.Height >= noseRect.Height && e.Width >= noseRect.Width)
                                    {
                                        noseRect = e;
                                        noseRect.Offset(f.X, f.Y);
                                    }
                                }

                                /*if (noseRect.Width > noseRect.Height)
                                {
                                    bool flip = true;
                                    while (noseRect.Width > noseRect.Height)
                                    {
                                        if (flip)
                                        {
                                            noseRect.Height++;
                                            flip = false;
                                        }
                                        else
                                        {
                                            noseRect.Y--;
                                            flip = true;
                                        }
                                    }
                                }
                                else if (noseRect.Width < noseRect.Height)
                                {
                                    bool flip = true;
                                    while (noseRect.Width < noseRect.Height)
                                    {
                                        if (flip)
                                        {
                                            noseRect.Width++;
                                            flip = false;
                                        }
                                        else
                                        {
                                            noseRect.X--;
                                            flip = true;
                                        }
                                    }
                                }*/

                                recObjects["noses"].Add(noseRect);
                            }
						}  
					}
				}
				watch.Stop();
			}
			detectionTime = watch.ElapsedMilliseconds;
		}
   }
}
