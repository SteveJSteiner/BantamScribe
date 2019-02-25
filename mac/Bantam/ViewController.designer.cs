// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Bantam
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		AppKit.NSTextField charLabel { get; set; }

		[Outlet]
		AppKit.NSLevelIndicator goalLevel { get; set; }

		[Outlet]
		AppKit.NSLevelIndicator timerLevel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (charLabel != null) {
				charLabel.Dispose ();
				charLabel = null;
			}

			if (goalLevel != null) {
				goalLevel.Dispose ();
				goalLevel = null;
			}

			if (timerLevel != null) {
				timerLevel.Dispose ();
				timerLevel = null;
			}
		}
	}
}
