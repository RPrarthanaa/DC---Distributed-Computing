/* 
 * Program.cs
 * 
 * Server program that will host the GameLobbyService
 *
 */

using System;
using System.ServiceModel;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            //Start the server
            Console.WriteLine("Welcome!");

            //Bind the interface
            var tcp = new NetTcpBinding();

            //Create the host
            var host = new ServiceHost(typeof(GameLobbyServiceImpl));
            host.AddServiceEndpoint(typeof(IGameLobbyService), tcp, "net.tcp://0.0.0.0:8100/MortalXGamingService");
            host.Open();

            //Hold the server open until someone does something
            Console.WriteLine("Game Lobby Server Online\n");
            Console.ReadLine();

            //Close the host
            host.Close();
        }
    }
}