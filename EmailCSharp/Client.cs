using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace EmailCSharp
{
    public class Client
    {
        public void StartClient(ClientMessage clientMessage)
        {
            byte[] buffer = new byte[0x8000];

            try
            {
                IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 0x2af8);

                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                try
                {
                    socket.Connect(remoteEP);

                    Console.WriteLine("Socket connected to {0}", socket.RemoteEndPoint.ToString());

                    byte[] bytes = Encoding.UTF8.GetBytes("{\"Name\":\"Martin\",\"messages\":[]}");

                    socket.Send(bytes);
                    socket.Shutdown(SocketShutdown.Send);

                    int count = socket.Receive(buffer);

                    Console.WriteLine("Server response = {0}", Encoding.UTF8.GetString(buffer, 0, count));
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException: {0}", ane.ToString());
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException: {0}", se.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unhandled exception: " + e.ToString());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Unhandled exception: " + e.ToString());
            }
        }
    }
}
