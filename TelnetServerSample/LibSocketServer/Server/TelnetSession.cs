using System;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;

namespace LibSocketServer.Server
{
    public class TelnetSession : AppSession<TelnetSession>
    {
        protected override void OnSessionStarted()
        {
            Console.WriteLine($"New Session Connected: {RemoteEndPoint.Address} " +
                              $"@ {RemoteEndPoint.Port}.");
            Send("Welcome to SuperSocket Telnet Server.");
        }

        protected override void HandleUnknownRequest(StringRequestInfo requestInfo)
        {
            Console.WriteLine($"Unknown request {requestInfo.Key}.");
            Send("Unknown request.");
        }

        protected override void HandleException(Exception e)
        {
            Console.WriteLine($"Application error: {e.Message}.");
            Send($"Application error: {e.Message}.");
        }

        protected override void OnSessionClosed(CloseReason reason)
        {
            Console.WriteLine($"Session {RemoteEndPoint.Address} @ {RemoteEndPoint.Port} " +
                              $"Closed: {reason}.");
            base.OnSessionClosed(reason);
        }
    }
}
