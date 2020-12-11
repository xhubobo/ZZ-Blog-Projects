using System;
using LibSocketServer;

namespace TelnetServerSample
{
    class Program
    {
        static void Main()
        {
            try
            {
                //启动SuperSocket
                if (!SocketServerManager.Instance.Startup(2021))
                {
                    Console.WriteLine("Failed to start TelnetServer!");
                    Console.ReadKey();
                    return;
                }

                Console.WriteLine("TelnetServer is listening on port 2021.");
                Console.WriteLine();
                Console.WriteLine("Press key 'q' to stop it!");
                Console.WriteLine();
                while (Console.ReadKey().KeyChar.ToString().ToUpper() != "Q")
                {
                    Console.WriteLine();
                }

                //关闭SuperSocket
                SocketServerManager.Instance.Shutdown();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
            }

            Console.WriteLine();
            Console.WriteLine("TelnetServer was stopped!");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
