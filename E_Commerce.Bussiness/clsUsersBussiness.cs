using E_Commerce.DataAccess;
using System.Data;
using BCrypt.Net;

namespace E_Commerce.Bussiness
{
    public class clsUsersBussiness
    {

        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Role { get; set; }

        public clsUsersBussiness(int userID, string Username,  string Email,DateTime CreatedAt,string Role)
        {
            this.UserID = userID;
            this.UserName = Username;  this.Email = Email; this.CreatedAt = CreatedAt; this.Role = Role;
        }
        public static clsUsersBussiness? ReadUserInfoByUsernameAndPassword(string username, string password)
        {
            int UserID = 0; string Role = "", email = ""; DateTime CreatedAt = DateTime.Now;
            string HashedPassword = GetUserHashedPassword(username);

             bool isMatch = BCrypt.Net.BCrypt.Verify(password, HashedPassword);
            if (!isMatch) return null;
           
              bool IsFound = DataAccess.clsUsers.GetUserInfoByUsername(username, ref UserID, ref email, ref CreatedAt, ref Role); 
 
            if (IsFound)

                return new clsUsersBussiness(UserID, username, email, CreatedAt, Role);
            else
                return null;

        }
        public static clsUsersBussiness? ReadUserInfoByUserIDAndPassword(int UserID, string password)
        {
            string Username = ""; string Role = "", email = ""; DateTime CreatedAt = DateTime.Now;
            string HashedPassword = GetUserHashedPassword(UserID);

            bool isMatch = BCrypt.Net.BCrypt.Verify(password, HashedPassword);
            if (!isMatch) return null;

            bool IsFound = DataAccess.clsUsers.GetUserInfoByUserID(UserID, ref Username, ref email, ref CreatedAt, ref Role);

            if (IsFound)

                return new clsUsersBussiness(UserID, Username, email, CreatedAt, Role);
            else
                return null;

        }
        public static clsUsersBussiness? ReadUserInfoByUsername(string username) {

            int UserID = 0; string Role = "", email = ""; DateTime CreatedAt = DateTime.Now;

            bool IsFound = DataAccess.clsUsers.GetUserInfoByUsername(username,ref UserID,ref email,ref CreatedAt,ref Role);

            if (IsFound) 
                return new clsUsersBussiness(UserID, username, email, CreatedAt, Role);

            else 
                return null;         

        }
        public static clsUsersBussiness? ReadUserInfoByUserID(int ID) {

            string Role = "", email = "", Username = ""; DateTime CreatedAt = DateTime.Now;


            bool IsFound = DataAccess.clsUsers.GetUserInfoByUserID(ID, ref Username, ref email, ref CreatedAt, ref Role);

            if (IsFound)
                return new clsUsersBussiness(ID, Username, email, CreatedAt, Role);
            else 
                return null;
        
        
        
        }

        public static bool AddNewUser(string username,string Email,string Password,string Role)
        {
            string HashedPassword = BCrypt.Net.BCrypt.HashPassword(Password);
            int InsertedID = clsUsers.AddNewUser( username, Email,HashedPassword, Role);
            return InsertedID != -1;
        }
        public static bool UpdateUser(int UserID,string Username,string email,string? Role = null)
        {
         
             return clsUsers.UpdateUser(UserID, Username, email, Role);
            
        }

         static string GetUserHashedPassword(int UserID) => clsUsers.GetUserHashedPass(UserID);
        static string GetUserHashedPassword(string Username) => clsUsers.GetUserHashedPass(Username);

        public static bool DeleteUser(string Username)
        {
            return clsUsers.DeleteUser(Username);
        }
        public static bool DeleteUser(int ID)
        {
            return clsUsers.DeleteUser(ID);
        }
      
        public static bool IsUserExists(string username)
        {
            return clsUsers.IsUserExists(username);
        }
        public static bool IsUserExists(int ID)
        {
            return clsUsers.IsUserExists(ID);
        }

        public static bool ChangePassword(int UserID,string OldPassword,string NewPassword)
        {   
            string HashedPassword = GetUserHashedPassword(UserID);
              bool isMatch = BCrypt.Net.BCrypt.Verify(OldPassword, HashedPassword);
            string NewHashedPassword = BCrypt.Net.BCrypt.HashPassword(NewPassword);
            if (isMatch)  return clsUsers.ChangePassword(UserID, NewHashedPassword);
            return false;
        }
        public static bool ChangePassword(string Username, string OldPassword, string NewPassword)
        {
            string HashedPassword = GetUserHashedPassword(Username);
            bool isMatch = BCrypt.Net.BCrypt.Verify(OldPassword, HashedPassword);
            string NewHashedPassword = BCrypt.Net.BCrypt.HashPassword(NewPassword);
            if (isMatch) return clsUsers.ChangePassword(Username, NewHashedPassword);
            return false;
        }
        public static DataTable GetAllUsers(string? Role = null)
        {
            return DataAccess.clsUsers.GetAllUsers(Role);
        }

    }
}
