// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace MotionDnaSample.iOS
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		UIKit.UITextView receiveMotionDnaTextField { get; set; }

		[Outlet]
		UIKit.UITextView receiveNetworkDataTextField { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (receiveMotionDnaTextField != null) {
				receiveMotionDnaTextField.Dispose ();
				receiveMotionDnaTextField = null;
			}

			if (receiveNetworkDataTextField != null) {
				receiveNetworkDataTextField.Dispose ();
				receiveNetworkDataTextField = null;
			}
		}
	}
}
