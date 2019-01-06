using System;
using System.Diagnostics;
using Microsoft.Win32;
using System.Windows.Forms;
using Microsoft.VisualBasic.Devices;
using System.Threading;

namespace ClickermannHelper
{
    class SysInfo
    {
        static Computer Computer = new Computer();
        static ComputerInfo RAM = new ComputerInfo();
        static PerformanceCounter CPU = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        static PerformanceCounter Disk = new PerformanceCounter("PhysicalDisk", "% Disk Time", "_Total");

        public static void GetInfo()
        {
            GetOSVersion();          //Версия Windows_
            GetCurrentLanguage();    //Теущая раскладка клавиатуры
            GetRAMLoad();            //Сколько оперативной памяти занято
            GetCPULoad();            //Насколько загружен процессор
            GetDisk();               //Получить информацию о диске
        }

        #region Версия Windows

        private static void GetOSVersion()
        {
            INIManager.Default.WritePrivateString("System", "OSVersion", "0");
            INIManager.Default.WritePrivateString("System", "OSVersion", Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\", "ProductName", "").ToString());
        }

        #endregion

        #region Язык раскладки

        private static void GetCurrentLanguage()
        {
            INIManager.Default.WritePrivateString("System", "CurrentLanguage", "0");
            INIManager.Default.WritePrivateString("System", "CurrentLanguage", InputLanguage.CurrentInputLanguage.Culture.EnglishName);
        }

        #endregion

        #region Загрузка процессора

        private static void GetCPULoad()
        {
            CPU.NextValue();
            Thread.Sleep(100);
            INIManager.Default.WritePrivateString("System", "CPULoad", Math.Round(CPU.NextValue()).ToString());
        }

        #endregion

        #region Оперативная память

        private static void GetRAMLoad()
        {
            //Оперативная память
            var RAMTotal = RAM.TotalPhysicalMemory / 1024 / 1024;
            var RAMAvailable = RAM.AvailablePhysicalMemory / 1024 / 1024;
            var RAMBusy = RAMTotal - RAMAvailable;
            var RAMFree = RAMTotal - RAMBusy;
            var RAMBusyProc = (RAMBusy * 100) / RAMTotal;
            var RAMFreeProc = 100 - RAMBusyProc;
                
            //Занято в мегабайтах
            INIManager.Default.WritePrivateString("System", "RAMLoad", RAMBusy.ToString());
            //Свободно в мегабайтах
            INIManager.Default.WritePrivateString("System", "RAMFree", RAMFree.ToString());
            //Занято в процентах
            INIManager.Default.WritePrivateString("System", "RAMLoadProc", RAMBusyProc.ToString());
            //Свободно в процентах
            INIManager.Default.WritePrivateString("System", "RAMFreeProc", RAMFreeProc.ToString());
        }

        #endregion

        #region Загрузка диска

        private static void GetDisk()
        {
            Disk.NextValue();
            Thread.Sleep(100);
            INIManager.Default.WritePrivateString("System", "HDDLoad", Math.Round(Disk.NextValue()).ToString());
        }

        #endregion
    }
}
