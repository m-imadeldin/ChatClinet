using System;
using System.Threading.Tasks;

namespace ChatClientApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.Title = "VG Chatklient";
            Console.WriteLine("=== VG Chatklient med Socket.IO ===");

            string username = "";
            while (string.IsNullOrWhiteSpace(username))
            {
                Console.Write("Ange användarnamn: ");
                username = Console.ReadLine()?.Trim();
            }

            var user = new User(username);
            var history = new MessageHistory();
            history.Load();

            var client = new ChatClient(user, history);
            var commands = new CommandHandler(client, history);

            await client.ConnectAsync();

            Console.WriteLine("Skriv /help för kommandon.\n");

            while (true)
            {
                string? input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                    continue;

                if (input.StartsWith("/"))
                {
                    await commands.HandleAsync(input);
                }
                else
                {
                    await client.SendMessageAsync(input);
                }
            }
        }
    }
}