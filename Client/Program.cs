using System.Net;
using System.Net.Sockets;
using System.Text;

const string ip = "127.0.0.1";
const int port = 8080;

IPAddress address = IPAddress.Parse(ip);
IPEndPoint endPoint = new IPEndPoint(address, port);

Socket clientSocket = new Socket(
    addressFamily: AddressFamily.InterNetwork,
    socketType: SocketType.Stream,
    protocolType: ProtocolType.Tcp);

await clientSocket.ConnectAsync(endPoint);

try
{
    Console.WriteLine("Write message and press 'enter' to send...\n\n");
    while (true)
    {
        string message = Console.ReadLine() ?? string.Empty;
        var messageInBytes = Encoding.Unicode.GetBytes(message);

        await clientSocket.SendAsync(messageInBytes);
    }
}
catch (SocketException)
{
    Console.WriteLine("Disconnected from server!");
    Environment.Exit(0);
}
catch (Exception ex)
{
    Console.WriteLine("System error: " + ex);
}
finally
{
    clientSocket.Dispose();
}
