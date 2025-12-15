/* 
 * IGameLobbyService.cs 
 * 
 * Interface that contains all the methods that the client can call to interact with the server.  
 * The interface is implemented by the GameLobbyService class. Used by the client to call the methods on the server.
 * 
 */

using Library;
using System.Collections.Generic;
using System.ServiceModel;

namespace Server
{
    [ServiceContract]
    public interface IGameLobbyService
    {
        [OperationContract]
        Dictionary<string, Player> GetActivePlayers();

        [OperationContract]
        Dictionary<string, List<Player>> GetActiveLobbyRooms();

        [OperationContract]
        bool Login(string username, string password);

        [OperationContract]
        void Logout(string username);

        [OperationContract]
        void JoinRoom(string roomName, string username);

        [OperationContract]
        bool CreateRoom(string roomName);

        [OperationContract]
        void LeaveRoom(string roomName, string username);

        [OperationContract]
        void SendGCMessages(string roomName, string username, string message);

        [OperationContract]
        void SendDMs(string roomName, string fromUsername, string toUsername, string message);

        [OperationContract]
        List<string> GetGroupMessages(string roomName);

        [OperationContract]
        Dictionary<string, string> getDMs(string receiver);

        [OperationContract]
        void ShareFile(string roomName, string username, byte[] fileData, string fileType, string filePath);

        [OperationContract]
        List<(string Username, byte[] FileData, string FileType, string FilePath)> GetSharedFiles(string roomName);

        [OperationContract]
        bool DeleteFile(string roomName, string username, string filePath);
    }
}