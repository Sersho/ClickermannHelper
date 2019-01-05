using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickermannHelper
{
    class SysInfo
    {
        public static void GetOSVersion()
        {
            INIManager.Default.WritePrivateString("System", "OSVersion", Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\", "ProductName", "").ToString());
        }
    }
}
