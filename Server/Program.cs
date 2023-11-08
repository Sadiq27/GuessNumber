using System.Net;
using System.Net.Sockets;
using System.Text;

const string ip = "127.0.0.1";
const int port = 8080;

IPAddress address = IPAddress.Parse(ip);
IPEndPoint endPoint = new IPEndPoint(address, port);

Socket serverSocket = new Socket(
    addressFamily: AddressFamily.InterNetwork,
    socketType: SocketType.Stream,
    protocolType: ProtocolType.Tcp);

serverSocket.Bind(endPoint);
serverSocket.Listen(backlog: 5);

while (true)
{
    Socket clientSocket = await serverSocket.AcceptAsync();
    Console.WriteLine($"Client was connected {clientSocket.RemoteEndPoint}");

    int randomNum = new Random().Next(1, 101);
    int attempts = 5;

    byte[] buffer = new byte[1024];

    ThreadPool.QueueUserWorkItem(async (socket) =>
    {
        try
        {
            while (attempts > 0)
            {
                var size = await socket.ReceiveAsync(buffer);
                var requestMessage = Encoding.Unicode.GetString(buffer, 0, size);

                int cliendNum;
                if (int.TryParse(requestMessage, out cliendNum))
                {
                    if (cliendNum == randomNum)
                    {
                        string response = "Guessed right!";
                        await socket.SendAsync(Encoding.Unicode.GetBytes(response));
                        break;
                    }
                    else if (cliendNum < randomNum)
                    {
                        string response = "More";
                        await socket.SendAsync(Encoding.Unicode.GetBytes(response));
                    }
                    else
                    {
                        string response = "Less";
                        await socket.SendAsync(Encoding.Unicode.GetBytes(response));
                    }
                    attempts--;
                }
            }
            var gameOverMessageInBytes = Encoding.Unicode.GetBytes("Game over. The hidden number was: " + randomNum);
            await clientSocket.SendAsync(gameOverMessageInBytes);
        }
        catch (SocketException)
        {
            Console.WriteLine($"Client {socket.RemoteEndPoint} disconnected!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"System error in {socket.RemoteEndPoint} client.\n{ex}");
        }
    }, clientSocket, false);

    
}