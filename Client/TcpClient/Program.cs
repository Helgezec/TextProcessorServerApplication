using System;
using System.Text;

namespace TcpClient
{
    class Program
    {
        static void Main(string[] args)
        {
            args = new[] {"127.0.0.1", "8080"};
            var client = new System.Net.Sockets.TcpClient(args[0], int.Parse(args[1]));
            var stream = client.GetStream();

            Console.WriteLine("Введите команду вида: get <prefix>");
            while (true)
            {
                var data = Encoding.Unicode.GetBytes(Console.ReadLine());
                stream.Write(data, 0, data.Length);
                data = new byte[64]; 
                StringBuilder builder = new StringBuilder();
                int bytes = 0;
                do
                {
                    bytes = stream.Read(data, 0, data.Length);
                    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                }
                while (stream.DataAvailable);

                Console.Write(builder.ToString());
            }
        }
    }
}
