using System;
using System.Reflection;
using SuperSocket.SocketBase;
using SuperSocket.SocketEngine;

namespace LibSocketServer
{
    public class SocketServerManager
    {
        private readonly IBootstrap _bootstrap;

        public bool Startup(int port)
        {
            if (!_bootstrap.Initialize())
            {
                Console.WriteLine("SuperSocket Failed to initialize!");
                return false;
            }

            var ret = _bootstrap.Start();
            Console.WriteLine($"SuperSocket Start result: {ret}.");

            return ret == StartResult.Success;
        }

        public void Shutdown()
        {
            _bootstrap.Stop();
        }

        #region Singleton

        private static SocketServerManager _instance;
        private static readonly object LockHelper = new object();

        private SocketServerManager()
        {
            var location = Assembly.GetExecutingAssembly().Location;
            var configFile = $"{location}.config";
            _bootstrap = BootstrapFactory.CreateBootstrapFromConfigFile(configFile);
        }

        public static SocketServerManager Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }

                lock (LockHelper)
                {
                    _instance = _instance ?? new SocketServerManager();
                }

                return _instance;
            }
        }

        #endregion
    }
}
