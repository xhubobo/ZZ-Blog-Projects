using System;
using LibSocketServer.Server;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;

namespace LibSocketServer.Command
{
    public class EchoCommand : CommandBase<TelnetSession, StringRequestInfo>
    {
        public override string Name => "ECHO";
        public override void ExecuteCommand(TelnetSession session, StringRequestInfo requestInfo)
        {
            Console.WriteLine($"{Name} command: {requestInfo.Body}.");
            session.Send(requestInfo.Body);
        }
    }
}