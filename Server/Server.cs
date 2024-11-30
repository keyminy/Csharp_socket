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
					//byte[] rcvBuffer = new byte[256];
					//header 2byte
					byte[] headerBuffer = new byte[2];
					//int totalBytes1 = clientSocket.Receive(rcvBuffer);
					int totalBytes1 = clientSocket.Receive(headerBuffer);
					if (totalBytes1 < 1)
					{
						Console.WriteLine("클라이언트의 연결 종료");
						return;
					}else if(totalBytes1 == 1)
					{
						// headerBuffer의 0번째 인덱스에는 데이터가 존재하므로 1번째 인덱스부터 1byte만큼 복사
						clientSocket.Receive(headerBuffer, 1, 1, SocketFlags.None);
					}
					// header버퍼를 통해, dataSize지정
					short dataSize = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(headerBuffer)); // 2byte
					byte[] dataBuffer = new byte[dataSize];
					//Console.WriteLine($"dataSize : {dataSize}");
					int totalRecv = 0;

					while(totalRecv < dataSize)
					{
						//실제로 받은 데이터 수가, 받은 데이터 보다 작다면?
						// dataSize : 30byte받아야함
						// 첫루프에서, totalBytes2 = 10이라면 다음 루프에서는 10 offset부터 받고, 30-10=20byte만큼 복사
						int totalBytes2 = clientSocket.Receive(dataBuffer,totalRecv,dataSize - totalRecv, SocketFlags.None);
						totalRecv += totalBytes2;
					}

					// 받은 데이터 역직렬화
					Console.WriteLine($"Server : {Encoding.UTF8.GetString(dataBuffer)}" );
					// Client에게 받은 메시지 전달
					clientSocket.Send(dataBuffer);
				}
            }
		}
    }
}
