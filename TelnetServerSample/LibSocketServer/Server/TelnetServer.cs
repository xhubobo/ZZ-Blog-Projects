using System;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;

namespace LibSocketServer.Server
{
    public class TelnetServer : AppServer<TelnetSession>
    {
        protected override bool Setup(IRootConfig rootConfig, IServerConfig config)
        {
            Console.WriteLine("TelnetServer Setup");
            return base.Setup(rootConfig, config);
        }

        protected override void OnStarted()
        {
            Console.WriteLine("TelnetServer OnStarted");
            base.OnStarted();
        }

        protected override void OnStopped()
        {
            Console.WriteLine();
            Console.WriteLine("TelnetServer OnStopped");
            base.OnStopped();
        }
    }
}
