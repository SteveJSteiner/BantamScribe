using AppKit;
using Foundation;

namespace Bantam
{
    [Register("AppDelegate")]
    public class AppDelegate : NSApplicationDelegate
    {
        public AppDelegate()
        {
        }

        public override void DidFinishLaunching(NSNotification notification)
        {
            // Insert code here to initialize your application
            var win = NSApplication.SharedApplication.MainWindow;
            var opts = new NSMutableDictionary();
            var view = win.ContentView;
            view.EnterFullscreenModeWithOptions(NSScreen.MainScreen, opts);
        }

        public override void WillTerminate(NSNotification notification)
        {
            // Insert code here to tear down your application
        }
    }
}
