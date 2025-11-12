using System;

namespace ChatClientApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // === Start the chat client ===
            Console.WriteLine("=== Chat Client with Socket.IO ===");
            
            // Ask for username
            Console.Write("Ange användarnamn: ");
            string username = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(username))
            {
                username = "Anonymous";
            }

            // Create user
            var user = new User(username);

            // Print username
            user.PrintUsername();

            // Create message history
            var history = new MessageHistory();

            // Create chat client
            var client = new ChatClient(user, history);

            // Create command handler
            var commandHandler = new CommandHandler(client, history);

            // Connect to server
            client.ConnectAsync().Wait();

            // Main input loop
            while (true)
            {
                string input = Console.ReadLine() ?? "";

                if (input.StartsWith("/"))
                {
                    // Handle commands like /help, /quit
                    commandHandler.HandleCommand(input);
                }
                else
                {
                    // Send normal message
                    client.SendMessageAsync(input).Wait();
                }
            }
        }
    }
}
