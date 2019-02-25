using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BantamScribe
{


    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        StringBuilder text = new StringBuilder();
        int wordCount = 0;
        bool inWord = false;
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        char lastChar = ' ';

        public MainWindow()
        {
            InitializeComponent();
            Mouse.OverrideCursor = Cursors.None;

            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            TimeBar.Value++;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.Key == Key.Escape)
            {
                var t = text.ToString();
                Clipboard.SetText(t);
                this.Close();
            }

            char? ckey = GetCharFromKey(e.Key);
            if (!ckey.HasValue)
            {
                return;
            }
            char c = ckey.Value;
            if (Char.IsControl(c) && c != '\r')
            {
                return;
            }
            if (inWord && char.IsWhiteSpace(c))
            {
                wordCount++;
                inWord = false;
            }
            if (!inWord && char.IsLetterOrDigit(c))
            {
                inWord = true;
            }
            if (c == lastChar)
            {
                this.CharWindow.Text += c.ToString();
            }
            else
            {
                this.CharWindow.Text = c.ToString();
            }
            lastChar = c;
            text.Append(c);
            ProgressBar.Value = wordCount;
        }

        public enum MapType : uint
        {
            MAPVK_VK_TO_VSC = 0x0,
            MAPVK_VSC_TO_VK = 0x1,
            MAPVK_VK_TO_CHAR = 0x2,
            MAPVK_VSC_TO_VK_EX = 0x3,
        }

        [DllImport("user32.dll")]
        public static extern int ToUnicode(
            uint wVirtKey,
            uint wScanCode,
            byte[] lpKeyState,
            [Out, MarshalAs(UnmanagedType.LPWStr, SizeParamIndex = 4)]
            StringBuilder pwszBuff,
            int cchBuff,
            uint wFlags);

        [DllImport("user32.dll")]
        public static extern bool GetKeyboardState(byte[] lpKeyState);

        [DllImport("user32.dll")]
        public static extern uint MapVirtualKey(uint uCode, MapType uMapType);

        public static char? GetCharFromKey(Key key)
        {
            char? ch = null;

            int virtualKey = KeyInterop.VirtualKeyFromKey(key);
            byte[] keyboardState = new byte[256];
            GetKeyboardState(keyboardState);

            uint scanCode = MapVirtualKey((uint)virtualKey, MapType.MAPVK_VK_TO_VSC);
            StringBuilder stringBuilder = new StringBuilder(2);

            int result = ToUnicode((uint)virtualKey, scanCode, keyboardState, stringBuilder, stringBuilder.Capacity, 0);
            switch (result)
            {
                case -1:
                    break;
                case 0:
                    break;
                case 1:
                    {
                        ch = stringBuilder[0];
                        break;
                    }
                default:
                    {
                        ch = stringBuilder[0];
                        break;
                    }
            }
            return ch;
        }

        private void Window_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            this.TopGrid.Background = Brushes.Firebrick;
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void Window_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            this.TopGrid.Background = Brushes.Black;
            Mouse.OverrideCursor = Cursors.None;
        }
    }
}
