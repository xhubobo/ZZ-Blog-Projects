using System;
using System.Linq;
using LibSocketServer.Server;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;

namespace LibSocketServer.Command
{
    public class AddCommand : CommandBase<TelnetSession, StringRequestInfo>
    {
        public override string Name => "ADD";
        public override void ExecuteCommand(TelnetSession session, StringRequestInfo requestInfo)
        {
            Console.WriteLine($"{Name} command: {requestInfo.Body}.");
            session.Send(requestInfo.Parameters.Select(p => Convert.ToInt32(p)).Sum().ToString());
        }
    }
}
