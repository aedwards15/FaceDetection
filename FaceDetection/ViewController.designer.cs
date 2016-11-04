// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace FaceDetection
{
    [Register ("ViewController")]
    partial class ViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton FeatureMap { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton GetImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel imageLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView ImageViewer { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel messageLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton ProcessImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton ResetImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UINavigationItem Root { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        FaceDetection.MySwitch switchSwitch { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField textAperture { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField textThreshold1 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField textThreshold2 { get; set; }

        [Action ("FeatureMap_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void FeatureMap_TouchUpInside (UIKit.UIButton sender);

        [Action ("GetImage_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void GetImage_TouchUpInside (UIKit.UIButton sender);

        [Action ("ProcessImage_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void ProcessImage_TouchUpInside (UIKit.UIButton sender);

        [Action ("ResetImage_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void ResetImage_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (FeatureMap != null) {
                FeatureMap.Dispose ();
                FeatureMap = null;
            }

            if (GetImage != null) {
                GetImage.Dispose ();
                GetImage = null;
            }

            if (imageLabel != null) {
                imageLabel.Dispose ();
                imageLabel = null;
            }

            if (ImageViewer != null) {
                ImageViewer.Dispose ();
                ImageViewer = null;
            }

            if (messageLabel != null) {
                messageLabel.Dispose ();
                messageLabel = null;
            }

            if (ProcessImage != null) {
                ProcessImage.Dispose ();
                ProcessImage = null;
            }

            if (ResetImage != null) {
                ResetImage.Dispose ();
                ResetImage = null;
            }

            if (Root != null) {
                Root.Dispose ();
                Root = null;
            }

            if (switchSwitch != null) {
                switchSwitch.Dispose ();
                switchSwitch = null;
            }

            if (textAperture != null) {
                textAperture.Dispose ();
                textAperture = null;
            }

            if (textThreshold1 != null) {
                textThreshold1.Dispose ();
                textThreshold1 = null;
            }

            if (textThreshold2 != null) {
                textThreshold2.Dispose ();
                textThreshold2 = null;
            }
        }
    }
}