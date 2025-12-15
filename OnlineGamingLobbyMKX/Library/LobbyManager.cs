/* 
 * LobbyManager.cs
 * 
 * This class is responsible for managing the active players and lobby rooms.
 * It contains methods to add and remove players, add and remove players from lobby rooms,
 * and set the message of a player.
 * 
 */

using System;
using System.Collections.Generic;

namespace Library
{
    public class LobbyManager
    {
        // Stores the active players, key is the username
        private Dictionary<string, Player> activePlayers;

        // Stores the active lobby rooms, key is the room name
        private Dictionary<string, List<Player>> activeLobbyRooms;

        // Constructor for initializing the dictionaries
        public LobbyManager()
        {
            activePlayers = new Dictionary<String, Player>();
            activeLobbyRooms = new Dictionary<string, List<Player>>();
        }

        // Getters for the active players and lobby rooms
        public Dictionary<string, Player> GetActivePlayers()
        {
            return activePlayers;
        }

        // Getters for the active players and lobby rooms
        public Dictionary<string, List<Player>> GetActiveLobbyRooms()
        {
            return activeLobbyRooms;
        }

        // Add a player to the active players dictionary
        public void AddPlayer(string username, string password)
        {
            activePlayers.Add(username, new Player
            {
                Username = username,
                Password = password
            });
        }

        // Remove a player from the active players dictionary
        public void RemovePlayer(string username)
        {
            activePlayers.Remove(username);
        }

        // Add a lobby room to the active lobby rooms dictionary
        public void AddLobbyRoom(string roomName)
        {
            activeLobbyRooms.Add(roomName, new List<Player>());
        }

        // Add a player to a lobby room
        public void AddPlayerToLobbyRoom(string roomName, string username)
        {
            Player player = activePlayers[username];
            activeLobbyRooms[roomName].Add(player);
        }

        // Remove a player from a lobby room
        public void RemovePlayerFromLobbyRoom(string roomName, string username)
        {
            Player player = activePlayers[username];
            activeLobbyRooms[roomName].Remove(player);
        }
    }
}