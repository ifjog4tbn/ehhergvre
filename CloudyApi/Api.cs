namespace CloudyApi
{
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;

    public static class Api
    {
        private static Timer time12 = new Timer();
        private static bool isua = false;
        private static bool _autoInject;

        static Api()
        {
            AutoSetup();
        }

        public static void AutoInject(bool enable)
        {
            Initialize();
            _autoInject = enable;
            if (enable)
            {
                inject();
            }
        }

        private static void AutoSetup()
        {
            string[] strArray = new string[] { "Cloudy.dll", "libcrypto-3-x64.dll", "libssl-3-x64.dll", "xxhash.dll", "zstd.dll" };
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string[] strArray2 = strArray;
            int index = 0;
            while (true)
            {
                while (true)
                {
                    if (index >= strArray2.Length)
                    {
                        return;
                    }
                    string str2 = strArray2[index];
                    string str3 = Path.Combine(path, str2);
                    if (!File.Exists(str3))
                    {
                        try
                        {
                            string address = "https://github.com/cloudyExecutor/webb/releases/download/dlls/" + str2;
                            using (WebClient client = new WebClient())
                            {
                                client.DownloadFile(address, str3);
                            }
                        }
                        catch (Exception exception)
                        {
                            MessageBox.Show("Failed to Download " + str2 + ": " + exception.Message, "CloudyApi");
                        }
                    }
                    break;
                }
                index++;
            }
        }

        public static void execute(string scriptSource)
        {
            Func<ClientInfo, string> selector = <>c.<>9__10_0;
            if (<>c.<>9__10_0 == null)
            {
                Func<ClientInfo, string> local1 = <>c.<>9__10_0;
                selector = <>c.<>9__10_0 = c => c.name;
            }
            string[] clientUsers = GetClientsList().Select<ClientInfo, string>(selector).ToArray<string>();
            Execute(Encoding.UTF8.GetBytes(scriptSource), clientUsers, clientUsers.Length);
        }

        [DllImport(@"bin\Cloudy.dll", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.Ansi)]
        private static extern void Execute(byte[] scriptSource, string[] clientUsers, int numUsers);
        [DllImport(@"bin\Cloudy.dll", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr GetClients();
        public static List<ClientInfo> GetClientsList()
        {
            List<ClientInfo> list = new List<ClientInfo>();
            IntPtr clients = GetClients();
            while (true)
            {
                ClientInfo item = Marshal.PtrToStructure<ClientInfo>(clients);
                if (item.name == null)
                {
                    return list;
                }
                list.Add(item);
                clients += Marshal.SizeOf<ClientInfo>();
            }
        }

        public static string GetUsername()
        {
            string str3;
            string userName = Environment.UserName;
            string path = @"C:\\Users\\" + userName + @"\\AppData\\Local\\Roblox\\LocalStorage\\appStorage.json";
            if (File.Exists(path))
            {
                try
                {
                    JObject obj2 = JObject.Parse(File.ReadAllText(path));
                    if (obj2.ContainsKey("Username"))
                    {
                        string text1;
                        JToken token1 = obj2.get_Item("Username");
                        if (token1 != null)
                        {
                            text1 = token1.ToString();
                        }
                        else
                        {
                            JToken local1 = token1;
                            text1 = null;
                        }
                        return text1;
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Error getting username " + exception.Message, "CloudyApi");
                }
                str3 = null;
            }
            else
            {
                str3 = null;
            }
            return str3;
        }

        [DllImport(@"bin\Cloudy.dll", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Initialize();
        public static void inject()
        {
            Initialize();
            Thread.Sleep(0x3e8);
            string s = "\tgame:GetService(\"StarterGui\"):SetCore(\"SendNotification\", {\r\n\t\tTitle = \"[Cloudy API]\",\r\n\t\tText = \"Injected!\"\r\n\t})";
            Func<ClientInfo, string> selector = <>c.<>9__9_0;
            if (<>c.<>9__9_0 == null)
            {
                Func<ClientInfo, string> local1 = <>c.<>9__9_0;
                selector = <>c.<>9__9_0 = c => c.name;
            }
            string[] clientUsers = GetClientsList().Select<ClientInfo, string>(selector).ToArray<string>();
            Execute(Encoding.UTF8.GetBytes(s), clientUsers, clientUsers.Length);
        }

        public static bool IsAutoInjectEnabled() => 
            _autoInject;

        public static bool IsInjected()
        {
            try
            {
                return (GetClientsList().Count > 0);
            }
            catch
            {
                return false;
            }
        }

        public static void killRoblox()
        {
            foreach (Process process in Process.GetProcessesByName("RobloxPlayerBeta"))
            {
                try
                {
                    process.Kill();
                    process.WaitForExit();
                }
                catch (Exception exception)
                {
                    Console.WriteLine("Failed to kill process: " + exception.Message);
                }
            }
        }

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly Api.<>c <>9 = new Api.<>c();
            public static Func<Api.ClientInfo, string> <>9__9_0;
            public static Func<Api.ClientInfo, string> <>9__10_0;

            internal string <execute>b__10_0(Api.ClientInfo c) => 
                c.name;

            internal string <inject>b__9_0(Api.ClientInfo c) => 
                c.name;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ClientInfo
        {
            public string version;
            public string name;
            public int id;
        }
    }
}

