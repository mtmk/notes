// See https://aka.ms/new-console-template for more information

using library;

Console.WriteLine("Starting server..");

new Server().Start(true, 1234);

Console.ReadLine();
