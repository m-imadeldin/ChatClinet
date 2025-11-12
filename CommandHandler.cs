using System;

namespace ChatClientApp
{
    public class CommandHandler
    {
        private ChatClient _client;
        private MessageHistory _history;

        // Konstruktor: ta emot ChatClient och MessageHistory
        public CommandHandler(ChatClient client, MessageHistory history)
        {
            _client = client;
            _history = history;
        }

        // Hantera kommandon som börjar med "/"
        public void HandleCommand(string input)
        {
            if (input.StartsWith("/"))
            {
                string[] parts = input.Split(' ');
                string command = parts[0].ToLower();

                if (command == "/help")
                {
                    ShowHelp();
                }
                else if (command == "/quit")
                {
                    Console.WriteLine("Exiting program...");
                    _client.DisconnectAsync().Wait(); // enkel blockering
                    Environment.Exit(0);
                }
                else if (command == "/history")
                {
                    int count = 20;
                    if (parts.Length > 1)
                    {
                        try
                        {
                            count = int.Parse(parts[1]);
                        }
                        catch
                        {
                            Console.WriteLine("Invalid number, showing 20 messages.");
                        }
                    }
                    _history.ShowLast(count);
                }
                else if (command == "/dm")
                {
                    if (parts.Length < 3)
                    {
                        Console.WriteLine("Usage: /dm <user> <text>");
                        return;
                    }

                    string recipient = parts[1];
                    string text = parts[2];

                    _client.SendPrivateMessageAsync(recipient, text).Wait();
                    Console.WriteLine("(DM sent to " + recipient + ") " + text);
                }
                else
                {
                    Console.WriteLine("Unknown command. Type /help for list.");
                }
            }
        }

        // Visa hjälptext
        private void ShowHelp()
        {
            Console.WriteLine("--- Commands ---");
            Console.WriteLine("/help        - show this help");
            Console.WriteLine("/quit        - exit the program");
            Console.WriteLine("/history [n] - show last n messages");
            Console.WriteLine("/dm <user> <text> - send direct message");
            Console.WriteLine("----------------");
        }
    }
}
