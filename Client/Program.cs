using System.Net;
using System.Net.Sockets;

const string ip = "127.0.0.1";
const int port = 8080;

IPAddress address = IPAddress.Parse(ip);
IPEndPoint endPoint = new IPEndPoint(address, port);

Socket clientSocket = new Socket(
    addressFamily: AddressFamily.InterNetwork,
    socketType: SocketType.Stream,
    protocolType: ProtocolType.Tcp);

await clientSocket.ConnectAsync(endPoint);
