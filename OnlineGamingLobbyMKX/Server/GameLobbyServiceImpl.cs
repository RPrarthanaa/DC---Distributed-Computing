/* 
 * GameLobbyServiceImpl.cs
 * 
 * This class implements the IGameLobbyService interface and provides the implementation for the game lobby service.
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Library;

namespace Server
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class GameLobbyServiceImpl : IGameLobbyService
    {
        // Initialize new LobbyManager
        public LobbyManager lobbyManager = new LobbyManager();

        //Dictionary to store existing lobby rooms
        private Dictionary<string, List<Player>> activeLobbyRooms;

        //Dictionary to store active players
        private Dictionary<string, Player> activePlayers;

        // Dictionary to store files shared in each room
        private Dictionary<string, List<(string Username, byte[] FileData, string FileType, string FilePath)>> roomFiles = new Dictionary<string, List<(string, byte[], string, string)>>();

        // Dictornary for room messages
        private Dictionary<string, List<string>> roomMessages = new Dictionary<string, List<string>>();
        private Dictionary<string, Dictionary<string, string>> privateDMs = new Dictionary<string, Dictionary<string, string>>();

        // Returns the active lobby rooms
        public Dictionary<string, List<Player>> GetActiveLobbyRooms()
        {
            return lobbyManager.GetActiveLobbyRooms();
        }

        // Returns the active players
        public Dictionary<string, Player> GetActivePlayers()
        {
            return lobbyManager.GetActivePlayers();
        }

        // Logs in the player
        public bool Login(string username, string password)
        {
            activeLobbyRooms = GetActiveLobbyRooms();
            activePlayers = GetActivePlayers();

            // Check if username entered is already in use
            if (activePlayers.ContainsKey(username))
            {
                return false;
            }
            else
            {
                lobbyManager.AddPlayer(username, password);
                Console.WriteLine($"{username} logged in");
                return true;
            }
        }

        // Logs out the player
        public void Logout(string username)
        {
            // Check if the player is active
            if (activePlayers.ContainsKey(username))
            {
                lobbyManager.RemovePlayer(username);
                Console.WriteLine($"{username} logged out");
            }
        }

        // Joins player to room
        public void JoinRoom(string roomName, string username)
        {
            if (activeLobbyRooms.ContainsKey(roomName))
            {
                InitializeDMList(username);
                lobbyManager.AddPlayerToLobbyRoom(roomName, username);
                Console.WriteLine($"{username} joined room {roomName}");
            }
        }

        // Creates a room
        public bool CreateRoom(string roomName)
        {
            // Check if the room already exists
            if (activeLobbyRooms.ContainsKey(roomName))
            {
                return false;
            }
            else
            {
                lobbyManager.AddLobbyRoom(roomName);
                roomMessages.Add(roomName, new List<string>());
                Console.WriteLine($"{roomName} room is created");
                return true;
            }
        }

        // Leave the room
        public void LeaveRoom(string roomName, string username)
        {
            if (activeLobbyRooms.ContainsKey(roomName))
            {
                lobbyManager.RemovePlayerFromLobbyRoom(roomName, username);
                privateDMs.Clear();

                Console.WriteLine($"{username} left room {roomName}");
            }
        }

        // Send group chat messages
        public void SendGCMessages(string roomName, string username, string message)
        {
            if (activeLobbyRooms.ContainsKey(roomName))
            {
                if (!roomMessages.ContainsKey(roomName))
                {
                    roomMessages[roomName] = new List<string>();
                }
                string formattedMessage = $"{username}: {message}";
                roomMessages[roomName].Add(formattedMessage);

                Console.WriteLine($"{username} sent a group text: {message}");
            }
        }

        // Send direct messages
        public void SendDMs(string roomName, string fromUsername, string toUsername, string message)
        {
            // If receiver is not in the DM list, add them
            if (!privateDMs.ContainsKey(toUsername))
                privateDMs[toUsername] = new Dictionary<string, string>();

            // If sender is not in the DM list, add them
            if (!privateDMs.ContainsKey(fromUsername))
                privateDMs[fromUsername] = new Dictionary<string, string>();

            string formattedMessage = $"{fromUsername}: {message}";

            // Add the message to the sender and receiver's DM list
            if (!privateDMs[fromUsername].ContainsKey(toUsername))
            {
                privateDMs[fromUsername][toUsername] = formattedMessage + "\n";
            }
            else
            {
                privateDMs[fromUsername][toUsername] += formattedMessage + "\n";
            }
            if (!privateDMs[toUsername].ContainsKey(fromUsername))
            {
                privateDMs[toUsername][fromUsername] = formattedMessage + "\n";
            }
            else
            {
                privateDMs[toUsername][fromUsername] += formattedMessage + "\n";
            }

            Console.WriteLine($"{toUsername} received a private message: {message}");
        }

        // Initializes the direct message list
        public void InitializeDMList(string username)
        {
            if (!privateDMs.ContainsKey(username))
            {
                privateDMs.Add(username, new Dictionary<string, string>());
            }
        }

        // Get group chat messages
        public List<string> GetGroupMessages(string roomName)
        {
            if (roomMessages.ContainsKey(roomName))
            {
                return roomMessages[roomName];
            }
            else
            {
                return new List<string>();
            }
        }

        // Get direct messages
        public Dictionary<string, string> getDMs(string receiver)
        {
            // Check if the receiver is in the DM list
            if (privateDMs.ContainsKey(receiver))
            {
                return privateDMs[receiver];
            }
            else
            {
                return new Dictionary<string, string>();
            }
        }

        // Share files
        public void ShareFile(string roomName, string username, byte[] fileData, string fileType, string filePath)
        {
            // Check if the room exists
            if (activeLobbyRooms.ContainsKey(roomName))
            {
                // Log file share
                Console.WriteLine($"{username} shared a {fileType} file in room {roomName}");

                foreach (var player in activeLobbyRooms[roomName])
                {
                    Console.WriteLine($"Notify {player.Username} about the {fileType} file from {username}");
                }

                if (!roomFiles.ContainsKey(roomName))
                {
                    roomFiles[roomName] = new List<(string Username, byte[] FileData, string FileType, string FilePath)>();
                }

                roomFiles[roomName].Add((username, fileData, fileType, filePath));

                Console.WriteLine($"File from {username} saved in room {roomName}");
            }
            else
            {
                Console.WriteLine($"Room {roomName} does not exist.");
            }
        }

        // Get shared files
        public List<(string Username, byte[] FileData, string FileType, string FilePath)> GetSharedFiles(string roomName)
        {
            if (roomFiles.ContainsKey(roomName))
            {
                return roomFiles[roomName].Select(file => (file.Username, file.FileData, file.FileType, file.FilePath)).ToList();
            }
            else
            {
                return new List<(string, byte[], string, string)>();
            }
        }

        // Delete a shared file
        public bool DeleteFile(string roomName, string username, string filePath)
        {
            // Check if the room exists
            if (roomFiles.ContainsKey(roomName))
            {
                // Find the file to delete in the room's shared files
                var fileToDelete = roomFiles[roomName].FirstOrDefault(file => file.FilePath == filePath && file.Username == username);

                if (fileToDelete != default)
                {
                    // Remove the file from the list
                    roomFiles[roomName].Remove(fileToDelete);
                    Console.WriteLine($"{username} deleted the file {filePath} from room {roomName}");
                    return true; // Indicate successful deletion
                }
                else
                {
                    Console.WriteLine($"File {filePath} not found in room {roomName} or {username} is not authorized to delete it.");
                    return false; // Indicate failure to find or delete the file
                }
            }
            else
            {
                Console.WriteLine($"Room {roomName} does not exist.");
                return false; // Indicate that the room does not exist
            }
        }
    }
}