using System.Net.Sockets;

namespace Jam.Runtime.Net_
{

    public class SocketEx
    {
        public static Socket CreateSocket()
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            SetSocketOpt(socket);
            return socket;
        }

        public static void SetSocketOpt(Socket socket)
        {
            // 1.端口关闭后马上重新启用
            bool isReuseaddr = true;
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, isReuseaddr);

            // 2.发送、接收timeout
            int netTimeout = 3000;
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, netTimeout);
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, netTimeout);

            // 3.非阻塞
            socket.Blocking = false;

            // 4.禁用Nagle算法
            socket.NoDelay = true;
        }
    }

}