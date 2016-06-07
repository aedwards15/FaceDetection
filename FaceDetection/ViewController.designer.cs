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
		UIButton GetImage { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel imageLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIImageView ImageViewer { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel messageLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton ProcessImage { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton ResetImage { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UINavigationItem Root { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		MySwitch switchSwitch { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField textAperture { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField textThreshold1 { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField textThreshold2 { get; set; }

		[Action ("GetImage_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void GetImage_TouchUpInside (UIButton sender);

		[Action ("ProcessImage_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void ProcessImage_TouchUpInside (UIButton sender);

		[Action ("ResetImage_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void ResetImage_TouchUpInside (UIButton sender);

		void ReleaseDesignerOutlets ()
		{
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
