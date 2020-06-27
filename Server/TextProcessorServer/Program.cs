using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Internal;
using TextProcessorServer.DataService;

namespace TextProcessorServer
{
    sealed class Program
    {
        private static string connectionString;

        /// <param name="args">arg 1 - connection string (in quotes), arg 2 - port</param>
        static void Main(string[] args)
        {
            connectionString = args[0];
            using (var context = new Context(connectionString))
            {
                var tcpListener = new TcpListener(IPAddress.Any, int.Parse(args[1]));
                tcpListener.Start();
                Listen(tcpListener);

                var dictionaryService = new DictionaryService(context);
                while (true)
                {
                    var command = Console.ReadLine();
                    if (command == "создание словаря")
                    {
                        Console.WriteLine("Введите путь к файлу для создания словаря");
                        dictionaryService.Create(Console.ReadLine());
                    }
                    else if (command == "обновление словаря")
                    {
                        Console.WriteLine("Введите путь к файлу для обновления словаря");
                        dictionaryService.Update(Console.ReadLine());
                    }
                    else if (command == "очистить словарь")
                    {
                        dictionaryService.Delete();
                    }
                }
            }
        }

        private static async void Listen(TcpListener listener)
        {
            while (true)
            {
                var client = await listener.AcceptTcpClientAsync();
                Task.Factory.StartNew(() => Process(client));
            }
        }

        private static void Process(TcpClient client)
        {
            using (var context = new Context(connectionString))
            {
                var dictionaryService = new DictionaryService(context);
                using (var stream = client.GetStream())
                {
                    byte[] data = new byte[64];

                    while (true)
                    {
                        StringBuilder commandBuilder = new StringBuilder();
                        do
                        {
                            var bytes = stream.Read(data, 0, data.Length);
                            commandBuilder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                        }
                        while (stream.DataAvailable);

                        var command = commandBuilder.ToString();
                        
                        if (command.StartsWith("get "))
                        {
                            var prefix = command.Replace("get ", "");

                            var words = dictionaryService.Read(prefix);
                            data = Encoding.Unicode.GetBytes(words.Join("\n") + "\n");
                            stream.Write(data, 0, data.Length);
                        }
                    }
                }
            }
        }
    }
}
