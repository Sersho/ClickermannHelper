using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;

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

            string BotToken = INIManager.Default.GetPrivateString("Telegram", "BotToken");
            string ChatId = INIManager.Default.GetPrivateString("Telegram", "ChatId");
            string Proxy = INIManager.Default.GetPrivateString("Telegram", "Proxy");
            Proxy = ProxyRegex.IsMatch(Proxy) ? Proxy : null;
            
            string Message = GetMessage();

            #region Собираем сообщение

            string GetMessage()
            {
                string Concat = "";

                for (int i = 1; i < Data.Length; i++)
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
                HttpClientHandler Handle = new HttpClientHandler() { Proxy = ProxyObject };
                webClient = new HttpClient(Handle);
            }

            using (webClient)
            {
                var Status = webClient.GetAsync($"https://api.telegram.org/bot{BotToken}/sendMessage?chat_id={ChatId}&text={Message}");

                while (!Status.IsCompleted)
                {
                    Thread.Sleep(5);
                }
            }


            #endregion
        }

        #endregion

        #region Отправляем картинку в Telegram

        public static void SendPicture(string[] Data)
        {
            Regex ProxyRegex = new Regex("([0-9]{1,3})\\.([0-9]{1,3}).([0-9]{1,3})\\.([0-9]{1,3}):([0-9]{2,8})");

            bool UseINI = INIManager.Default.GetPrivateString("Telegram", "UseINI") == "1" ? true : false;

            string BotToken = INIManager.Default.GetPrivateString("Telegram", "BotToken");
            string ChatId = INIManager.Default.GetPrivateString("Telegram", "ChatId");
            string Proxy = INIManager.Default.GetPrivateString("Telegram", "Proxy");
            Proxy = ProxyRegex.IsMatch(Proxy) ? Proxy : null;
            string PicturePath = "";

            string PictureDescription = GetDescription();


            #region Собираем описание

            string GetDescription()
            {
                string Concat = "";

                for (int i = 2; i < Data.Length; i++)
                {
                    Concat += Data[i] + " ";
                }

                return Concat;
            }

            #endregion

            #region Отправляем картинку

            if (Proxy != null)
            {
                WebProxy ProxyObject = new WebProxy(Proxy);
                HttpClientHandler Handle = new HttpClientHandler() { Proxy = ProxyObject };
                webClient = new HttpClient(Handle);
            }

            //Подготавливаем MultiPart

            MultipartFormDataContent MultipartData = new MultipartFormDataContent();
            FileStream FS = new FileStream(PicturePath, FileMode.Open, FileAccess.Read);
            MultipartData.Add(new StringContent(ChatId), "chat_id");
            MultipartData.Add(new StreamContent(FS), "photo", "Photo");
            MultipartData.Add(new StringContent(PictureDescription), "caption");

            using (webClient)
            {
                var Status = webClient.PostAsync($"https://api.telegram.org/bot{BotToken}/sendPhoto", MultipartData);

                while (!Status.IsCompleted)
                {
                    Thread.Sleep(10);
                }
            }

            #endregion
        }

        #endregion
    }
}
