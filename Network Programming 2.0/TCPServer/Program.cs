using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TCPServer
{
    class Program
    {
        static void Main(string[] args)
        {
            server();
        }

        public static void server()
        {
            int recv;
            int port = 9050;
            byte[] data = new byte[1024];
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any,
                                   9050);

            Socket newsock = new
                Socket(AddressFamily.InterNetwork,
                            SocketType.Stream, ProtocolType.Tcp);

            newsock.Bind(ipep);
            newsock.Listen(10);
            Console.WriteLine("Waiting for a client on port " + port  + " ...");
            Socket client = newsock.Accept();
            IPEndPoint clientep =
                         (IPEndPoint)client.RemoteEndPoint;
            Console.WriteLine("Connected with {0} at port {1}",
                            clientep.Address, clientep.Port);


            string welcome = "Welcome to my test server\n";
            data = Encoding.ASCII.GetBytes(welcome);
            client.Send(data, data.Length,
                              SocketFlags.None);
            while (true)
            {
                data = new byte[1024];
                recv = client.Receive(data);
                if (recv == 0)
                    break;

                Console.WriteLine(
                         Encoding.ASCII.GetString(data, 0, recv));
                client.Send(data, recv, SocketFlags.None);
            }
            Console.WriteLine("Disconnected from {0}",
                              clientep.Address);
            client.Close();
            newsock.Close();
            server();
        }
    }
    
}
