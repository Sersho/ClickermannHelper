namespace ClickermannHelper
{
    class Program
    {
        static void Main(string[] args)
        {
            switch (args[0])
            {
                case "TMessage":
                {
                    Telegram.SendMessage(args);
                    break;
                }

                case "TPicture":
                {
                    Telegram.SendPicture(args);
                    break;
                }

            }
        }
    }
}
