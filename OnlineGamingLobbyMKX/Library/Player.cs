/*
 * Player.cs
 * 
 * This class is used to store the player's username, password, and message.
 * 
 */

namespace Library
{
    public class Player
    {
        private string username;
        private string password;

        // Setters and Getters for username
        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        // Setters and Getters for password
        public string Password
        {
            get { return password; }
            set { password = value; }
        }
    }
}