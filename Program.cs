using System;
using SocketIOClient;
using System.Threading;

//TODO: Fix this template project name
namespace csharpdemo
{
    class Program
    {

        public static SocketIO client = null;

        public static void Main(string[] args)
        {
            //TODO: Write a proper argument parser so the server address and other
            //Useful params can be configured without recompile.
            startClient(args);
            do {
                Thread.Sleep(5000);
            } while (true);
        }

        async static void startClient(string[] hooks)
        {
            client = new SocketIO("http://localhost:3000/", new SocketIOOptions
            {
                EIO = 4
            });
            client.On("ch-post", response =>
            {
                Console.WriteLine(response.GetValue<string>() + "\r\n");
            });
            client.On("ch-get", response =>
            {
                Console.WriteLine(response.GetValue<string>());
            });
            client.OnConnected += async (sender, e) =>
            {
                Console.Error.WriteLine("Connected");
                foreach (string hook in hooks)
                {
                    await client.EmitAsync("ch-subscribe", hook);
                }
            };
            client.OnDisconnected += (sender, e) =>
            {
                Console.Error.WriteLine("Disconnected");
            };
            try
            {
                await client.ConnectAsync();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
            }
        }
    }
}
