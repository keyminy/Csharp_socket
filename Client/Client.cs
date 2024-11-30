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
				// newBuffer : strBuffer길이 + header 2byte를 추가한다.
				byte[] newBuffer = new byte[2+strBuffer.Length];
				// 직렬화, header길이는 short 2byte, BigEndian표기로 바꾸고 직렬화
				byte[] dataSize = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)strBuffer.Length));
				// 값을 복사한다.
				Array.Copy(dataSize,0,newBuffer,0,dataSize.Length); // dataSize -> newBuffer에 먼저 2byte복사
				Array.Copy(strBuffer, 0, newBuffer, 2, strBuffer.Length); // strBuffer -> newBuffer[2~]부터 복사됨
				socket.Send(newBuffer); // 서버에 입력받은 메시지 전달

				// From server
/*				byte[] rcvBuffer = new byte[256];
				int bytesRead = socket.Receive(rcvBuffer);*/
				byte[] rcvBuffer = new byte[strBuffer.Length];
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
