using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using Emgu.CV.Util;
using UIKit;

namespace FaceDetection
{
    public static class EdgeDetection
    {
        public static VectorOfVectorOfPoint DetectEdges(UIImage myImage, double th1, double th2, int aperture, bool value)
        {
            //Load the image from file and resize it for display
            Image<Bgr, Byte> img = 
                new Image<Bgr, byte>(myImage.CGImage);
                //.Resize(400, 400, Emgu.CV.CvEnum.Inter.Linear, true);

            //Convert the image to grayscale and filter out the noise
            UMat uimage = new UMat();
            CvInvoke.CvtColor(img, uimage, ColorConversion.Bgr2Gray);

            //use image pyr to remove noise
            UMat pyrDown = new UMat();
            CvInvoke.PyrDown(uimage, pyrDown);
            CvInvoke.PyrUp(pyrDown, uimage);

            //Image<Gray, Byte> gray = img.Convert<Gray, Byte>().PyrDown().PyrUp();

            #region circle detection
            double cannyThreshold = th1;
            //double circleAccumulatorThreshold = 120;
            //CircleF[] circles = CvInvoke.HoughCircles(uimage, HoughType.Gradient, 2.0, 20.0, cannyThreshold, circleAccumulatorThreshold, 5);

            #endregion

            #region Canny and edge detection
            double cannyThresholdLinking = th2;
            UMat cannyEdges = new UMat();
            CvInvoke.Canny(uimage, cannyEdges, cannyThreshold, cannyThresholdLinking, aperture, true);

            VectorOfVectorOfPoint contourEdges = new VectorOfVectorOfPoint();
            UMat hierarchy = new UMat();
            CvInvoke.FindContours(cannyEdges, contourEdges, hierarchy, 0, ChainApproxMethod.ChainApproxNone);

            VectorOfVectorOfPoint newContourEdges = new VectorOfVectorOfPoint ();
            for (int i = 0; i < contourEdges.Size; i++)
            {
                if (contourEdges [i].Size > 3000) 
                {
                    newContourEdges.Push (contourEdges [i]);
                }
            }

            contourEdges.Dispose ();

            VectorOfPoint test1 = new VectorOfPoint();
            VectorOfVectorOfPoint temp = new VectorOfVectorOfPoint();
            temp.Push(newContourEdges [0]);
            for (int i = 0; i < newContourEdges.Size; i++) {
                Point[] testing = newContourEdges [i].ToArray ();
                temp[0].Push(newContourEdges [i].ToArray());
            }

            VectorOfVectorOfPoint hull = new VectorOfVectorOfPoint(1);
            CvInvoke.ConvexHull(temp[0], hull[0], true);

            /*LineSegment2D[] lines = CvInvoke.HoughLinesP(
                cannyEdges, 
                1, //Distance resolution in pixel-related units
                Math.PI/45.0, //Angle resolution measured in radians.
                20, //threshold
                30, //min Line width
                5); //gap between lines

            //VectorOfPoint test1 = new VectorOfPoint();
            //VectorOfVectorOfPoint temp = new VectorOfVectorOfPoint();
            //temp.Push(contourEdges[0]);
            for (int i = 0; i < contourEdges.Size; i++) {
                //temp[0].Push(contourEdges[i].ToArray());

                CvInvoke.DrawContours(img, contourEdges, i, new MCvScalar(255,255,0), 4);
            }*/

            //VectorOfVectorOfPoint hull = new VectorOfVectorOfPoint(1);
            //CvInvoke.ConvexHull(temp[0], hull[0], true);

            //VectorOfVectorOfPoint result = new VectorOfVectorOfPoint();


            #endregion

            #region Find triangles and rectangles
            //List<Triangle2DF> triangleList = new List<Triangle2DF>();
            //List<RotatedRect> boxList = new List<RotatedRect>(); //a box is a rotated rectangle

            /*using (VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint())
            {
                CvInvoke.FindContours(cannyEdges, contours, null, RetrType.List, ChainApproxMethod.ChainApproxSimple );
                int count = contours.Size;
                for (int i = 0; i < count; i++)
                {
                    using (VectorOfPoint contour = contours[i])
                    using (VectorOfPoint approxContour = new VectorOfPoint())
                    {
                        CvInvoke.ApproxPolyDP(contour, approxContour, CvInvoke.ArcLength(contour, true) * 0.05, true);
                        if (CvInvoke.ContourArea(approxContour, false) > 250) //only consider contours with area greater than 250
                        {
                            if (approxContour.Size == 3) //The contour has 3 vertices, it is a triangle
                            {
                                Point[] pts = approxContour.ToArray();
                                triangleList.Add(new Triangle2DF(
                                    pts[0],
                                    pts[1],
                                    pts[2]
                                ));
                            } else if (approxContour.Size == 4) //The contour has 4 vertices.
                            {
                                #region determine if all the angles in the contour are within [80, 100] degree
                                bool isRectangle = true;
                                Point[] pts = approxContour.ToArray();
                                LineSegment2D[] edges = PointCollection.PolyLine(pts, true);

                                for (int j = 0; j < edges.Length; j++)
                                {
                                    double angle = Math.Abs(
                                        edges[(j + 1) % edges.Length].GetExteriorAngleDegree(edges[j]));
                                    if (angle < 80 || angle > 100)
                                    {
                                        isRectangle = false;
                                        break;
                                    }
                                }
                                #endregion

                                if (isRectangle) boxList.Add(CvInvoke.MinAreaRect(approxContour));
                            }
                        }
                    }
                }
            }*/
            #endregion

            //imageView.Image = img;

            #region draw triangles and rectangles
            //Image<Bgr, Byte> triangleRectangleImage = img;
            //foreach (Triangle2DF triangle in triangleList)
            //    triangleRectangleImage.Draw(triangle, new Bgr(Color.DarkBlue), 2);
            //foreach (RotatedRect box in boxList)
            //    triangleRectangleImage.Draw(box, new Bgr(Color.DarkOrange), 2);
            //imageView.Image = triangleRectangleImage;
            #endregion

            #region draw circles
            //Image<Bgr, Byte> circleImage = img.CopyBlank();
            //foreach (CircleF circle in circles)
            //    triangleRectangleImage.Draw(circle, new Bgr(Color.Brown), 2);
            //imageView.Image = circleImage;
            #endregion

            #region draw lines
            //Image<Bgr, Byte> lineImage = img;
            //foreach (LineSegment2D line in lines)
            //    img.Draw(line, new Bgr(Color.Yellow), 2);
            //imageView.Image = lineImage;
            #endregion

            return value ? hull : newContourEdges; //lineImage.ToUIImage();
        }
    }
}

