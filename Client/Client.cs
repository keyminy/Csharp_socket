using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client;

internal class Client
{
	static void Main(string[] args)
	{
		using(Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
		{
			IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("192.168.45.249"), 20000);
			socket.Connect(endPoint); //서버와 Connect

			while(true)
			{
				string str = Console.ReadLine();
				if(str == "exit")
				{
					return;
				}
				//입력받은 String을 byte[]로 직렬화
				byte[] strBuffer = Encoding.UTF8.GetBytes(str);
				socket.Send(strBuffer); // 서버에 입력받은 메시지 전달

				// From server
				byte[] rcvBuffer = new byte[256];
				int bytesRead = socket.Receive(rcvBuffer);
				if(bytesRead < 1)
				{
                    Console.WriteLine("서버 연결 종료!");
					return;
                } else
				{
                    Console.WriteLine($"From Server : {Encoding.UTF8.GetString(rcvBuffer)}");
				}
			}
		}
	}
}
