using System;

using AppKit;
using Foundation;
using System.Timers;
using System.Text;

namespace Bantam
{
    public partial class ViewController : NSViewController
    {
        Timer MainTimer;
        int TimeLeft = 15 * 60;
        StringBuilder text = new StringBuilder();
        string lastChars = "";

        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            NSCursor.Hide();

            // Do any additional setup after loading the view.
            NSEvent.AddLocalMonitorForEventsMatchingMask(NSEventMask.KeyDown, (theEvent) =>
            {
                this.KeyDown(theEvent);
                return null;
            });

            MainTimer = new Timer(1000);
            MainTimer.Elapsed += (sender, e) =>
            {
                TimeLeft--;
                // Format the remaining time nicely for the label
                TimeSpan time = TimeSpan.FromSeconds(TimeLeft);
                string timeString = time.ToString(@"mm\:ss");
                InvokeOnMainThread(() =>
                {
                    //We want to interact with the UI from a different thread,
                    // so we must invoke this change on the main thread

                    timerLevel.DoubleValue = (1.0) * (15 * 60 - TimeLeft);
                });
            };
            MainTimer.Start();
        }

        private static string[] pboardTypes = new string[] { "NSStringPboardType" };

        public void SetText(string text)
        {
            NSPasteboard.GeneralPasteboard.DeclareTypes(pboardTypes, null);
            NSPasteboard.GeneralPasteboard.SetStringForType(text, pboardTypes[0]);
        }

        public override void KeyDown(NSEvent theEvent)
        {
            ushort kc = theEvent.KeyCode;
            if (kc == 53) // ESC
            {
                NSApplication.SharedApplication.Terminate(this);
            }
            //var str = kc.ToString()
            //if (kc > 122 && kc < 127)
            //{
            //    return;
            //}
            string s = theEvent.Characters;
            if (char.IsControl(s[0]))
            {
                if (s[0] != '\r')
                {
                    return;
                }
                s = " ";
            }
            if (s == lastChars)
            {
                charLabel.StringValue += s;
            }
            else
            {
                charLabel.StringValue = s;
            }
            lastChars = theEvent.Characters;

            text.Append(theEvent.Characters);
            this.SetText(text.ToString());

            if (s == " ")
            {
                this.goalLevel.DoubleValue += 1.0;
            }


        }

        public override NSObject RepresentedObject
        {
            get
            {
                return base.RepresentedObject;
            }
            set
            {
                base.RepresentedObject = value;
                // Update the view, if already loaded.
            }
        }
    }
}
