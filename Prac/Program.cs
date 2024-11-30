using System;
using System.Net;
using System.Text;

namespace Prac;

internal class Program
{
	static void Main(string[] args)
	{
		// 바이트 오더 : 바이트를 메모리에 저장하는
		int num2 = IPAddress.HostToNetworkOrder(12345678); // 빅앤디안으로 변경
		byte[] buffer2 = BitConverter.GetBytes(num2);
        Console.WriteLine(BitConverter.ToString(buffer2)); //00-BC-61-4E
		// 역직렬화
		int num3 = BitConverter.ToInt32(buffer2);
		// NetworkToHostOrder : BigEndian(Network) -> LittelEndian(내컴터)
		int num4 = IPAddress.NetworkToHostOrder(num3);
        Console.WriteLine("num4 : " + num4);
    }
}
