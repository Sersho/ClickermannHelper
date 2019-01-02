using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ClickermannHelper
{
    class Telegram
    {
        #region HTTP Client

        private static HttpClient webClient = new HttpClient();

        #endregion

        #region Отправить сообщение в Telegram

        public static void SendMessage(string[] Data)
        {
            Regex ProxyRegex = new Regex("([0-9]{1,3})\\.([0-9]{1,3}).([0-9]{1,3})\\.([0-9]{1,3}):([0-9]{2,8})");

            string BotToken = Data[1];
            string ChatId = Data[2];
            string Proxy = ProxyRegex.IsMatch(Data[3]) ? Data[3] : null;
            string Message = GetMessage();

            #region Собираем сообщение

            string GetMessage()
            {
                string Concat = "";

                for (int i = Proxy == null ? 3 : 4; i < Data.Length; i++)
                {
                    Concat += Data[i] + " ";
                }

                return Concat;
            }

            #endregion

            #region Отправляем сообщение

            if (Proxy != null)
            {
                WebProxy ProxyObject = new WebProxy(Proxy);
                HttpClientHandler Handle = new HttpClientHandler(){Proxy = ProxyObject};
                webClient = new HttpClient(Handle);
            }

            webClient.GetAsync($"https://api.telegram.org/bot{BotToken}/sendMessage?chat_id={ChatId}&text={Message}");
            webClient.Dispose();

            #endregion
        }

        #endregion


    }
}
