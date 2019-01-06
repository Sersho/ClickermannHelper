using System;
using System.Threading;

namespace ClickermannHelper
{
    class Program
    {
        static void Main(string[] args)
        {
            
            switch (args[0].ToUpper())
            {
                case "TMESSAGE":
                    {
                        Telegram.SendMessage(args);
                        break;
                    }

                case "TPICTURE":
                    {
                        Telegram.SendPicture(args);
                        break;
                    }
                case "SYSTEMINFO":
                {
                    SysInfo.GetInfo();
                    break;
                }
            }
        }
    }
}
