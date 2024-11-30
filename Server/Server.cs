using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server;

internal class Server
{
	static void Main(string[] args)
	{
		using(Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
		{
			IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("192.168.45.249"), 20000);
			serverSocket.Bind(endPoint);
			serverSocket.Listen(20);

			using (Socket clientSocket = serverSocket.Accept())
			{ 
				// RemoteEndPoint : IP주소와 Port번호 알 수 있음
				Console.WriteLine("연결됨!!" + clientSocket.RemoteEndPoint);
				while (true)
				{
					byte[] rcvBuffer = new byte[256];
					int totalBytes = clientSocket.Receive(rcvBuffer);
					if (totalBytes < 1)
					{
						Console.WriteLine("클라이언트의 연결 종료");
						return;
					}
					// 받은 데이터 역직렬화
					Console.WriteLine($"Server : {Encoding.UTF8.GetString(rcvBuffer)}" );
					// Client에게 받은 메시지 전달
					clientSocket.Send(rcvBuffer);
				}
            }
		}
    }
}
