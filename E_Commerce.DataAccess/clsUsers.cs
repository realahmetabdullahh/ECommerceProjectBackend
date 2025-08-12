
using System.Data;
using Npgsql;
namespace E_Commerce.DataAccess
{
    public class clsUsers
    {
     

        public static bool GetUserInfoByUsernameAndPasswordHash(string username, string HashedPassword, ref int UserID, ref string Email, ref DateTime CreatedAt,ref string Role )
        {   
            bool IsFound = false;
            string query = "SELECT * FROM Users WHERE username = @username AND password_hash = @hashed_password;";
            using var Conn = new NpgsqlConnection(clsDataAccessSettings.ConnectionLink);
            using var cmd = new NpgsqlCommand(query,Conn);
             cmd.Parameters.AddWithValue("@username", username);
             cmd.Parameters.AddWithValue("@hashed_password", HashedPassword);
                try{  
                Conn.Open();
                        using var reader = cmd.ExecuteReader() ;
                        
                        if (reader.Read())
                        {
                            UserID = reader.GetInt32("id");
                            Email = reader.GetString("email");
                            CreatedAt = reader.GetDateTime("created_at");
                            Role = reader.GetString("Role");
                            IsFound = true;
                        }

                    }
                catch (Exception ex) { Console.WriteLine(ex.Message); IsFound = false;  }
               finally {Conn.Close(); }

                  
            return IsFound;

            }
        public static bool GetUserInfoByUsername(string username, ref int UserID, ref string Email, ref DateTime CreatedAt, ref string Role) { 
        bool IsFound = false;
            string query = "SELECT id, email, created_at, role FROM  Users WHERE username = @username ;";
            using var Conn = new NpgsqlConnection(clsDataAccessSettings.ConnectionLink);
            using var cmd = new NpgsqlCommand(query, Conn);
            cmd.Parameters.AddWithValue("@username", username);
            
            try
            {
                Conn.Open();
                using var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    UserID = reader.GetInt32("id");
              
                    Email = reader.GetString("email");
                    CreatedAt = reader.GetDateTime("created_at");
                    Role = reader.GetString("Role");
                    IsFound = true;
                }

            }
            catch (Exception ex) { Console.WriteLine(ex.Message); IsFound = false; }
            finally { Conn.Close(); }

            return IsFound;
        } 
        public static bool GetUserInfoByUserID(int UserID, ref string Username,ref string Email,ref DateTime CreatedAt, ref string Role){
            bool IsFound = false;
            string query = "SELECT username, email, created_at, role FROM  Users WHERE id = @id ;";
            using var Conn = new NpgsqlConnection(clsDataAccessSettings.ConnectionLink);
            using var cmd = new NpgsqlCommand(query, Conn);
            cmd.Parameters.AddWithValue("@id", UserID);

            try
            {
                Conn.Open();
                using var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    Username = reader.GetString("username");
                    Email = reader.GetString("email");
                    CreatedAt = reader.GetDateTime("created_at");
                    Role = reader.GetString("Role");
                    IsFound = true;
                }

            }
            catch (Exception ex) { Console.WriteLine(ex.Message); IsFound = false; }
            finally { Conn.Close(); }
            return IsFound;
        }
        public static int AddNewUser( string Username, string Email, string PasswordHash, string Role)
        {
            int insertedID = -1;
            string query = "insert Into Users (username,email,password_hash,role) values (@username,@email,@hashedpassword,@role) returning id;";
            using var conn = new NpgsqlConnection(clsDataAccessSettings.ConnectionLink);
            using var cmd = new NpgsqlCommand(query,conn);
            
            cmd.Parameters.AddWithValue("@username",Username);
            cmd.Parameters.AddWithValue("email", Email);
            cmd.Parameters.AddWithValue("@hashedpassword", PasswordHash);
            cmd.Parameters.AddWithValue("@role", Role);
            try
            {
                conn.Open();
                var Result = cmd.ExecuteScalar();
                if (Result != null && int.TryParse(Result.ToString(), out int NewID)) { 
                
                    insertedID = NewID;
                }


            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            finally {  conn.Close(); }


            return insertedID;

        }

        public static bool UpdateUser(int UserID, string Username, string Email, string? Role = null) { 
        bool isUpdated = false;
            string query = string.IsNullOrEmpty(Role) ? "update users Set username = @username, email = @email where id = @id" :
                "update users Set username = @username, email = @email, role = @role where id = @id" ;
          
            using var conn = new NpgsqlConnection(clsDataAccessSettings.ConnectionLink);
            using var cmd = new NpgsqlCommand(query, conn);  
            if (!string.IsNullOrEmpty(Role)) cmd.Parameters.AddWithValue("@role", Role);
            
                
               
            cmd.Parameters.AddWithValue("@id", UserID);
            cmd.Parameters.AddWithValue("@username", Username);
            cmd.Parameters.AddWithValue("@email", Email);
          

            try
            {
                conn.Open();
               int RowsEffected =  cmd.ExecuteNonQuery();
                isUpdated = RowsEffected> 0;
            }catch(Exception ex) { Console.WriteLine(ex.Message); }
            finally { conn.Close(); }
            return isUpdated;
        }
        public static bool DeleteUser(int UserID) {
        bool isDeleted = false;
            string query = "Delete  from users where id = @id;";
            using var conn = new NpgsqlConnection(clsDataAccessSettings.ConnectionLink);
            using var cmd = new NpgsqlCommand(query,conn);
            cmd.Parameters.AddWithValue("@id",UserID);
            try
            {
                conn.Open();
                int Result = cmd.ExecuteNonQuery();
                isDeleted = Result > 0;

            }catch(Exception ex) { Console.WriteLine(ex.Message) ; }
            finally { conn.Close(); }
            return isDeleted;
        }
        public static bool DeleteUser(string Username)
        {
            bool isDeleted = false;
            string query = "Delete  from users where username = @username;";
            using var conn = new NpgsqlConnection(clsDataAccessSettings.ConnectionLink);
            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@username", Username);
            try
            {
                conn.Open();
                int Result = cmd.ExecuteNonQuery();
                isDeleted = Result > 0;

            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            finally { conn.Close(); }
            return isDeleted;
        }
        public static bool IsUserExists(int UserID) {
            bool IsExists = false;
            string query = "select 1 from users where id = @id";
            using var conn = new NpgsqlConnection(clsDataAccessSettings.ConnectionLink);
            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", UserID);
            try
            {
                conn.Open();
                var Result = cmd.ExecuteScalar();
                IsExists = Result != null && Convert.ToInt32(Result) == 1;

            } catch (Exception ex) { Console.WriteLine(ex.Message); }
            finally { conn.Close(); } 
        
                
            return IsExists;
        }
        public static bool IsUserExists(string Username)
        {
            bool IsExists = false;
            string query = "select 1 from users where username = @username";
            using var conn = new NpgsqlConnection(clsDataAccessSettings.ConnectionLink);
            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@username", Username);
            try
            {
                conn.Open();
                var Result = cmd.ExecuteScalar();
                IsExists =Result != null && Convert.ToInt32(Result) == 1;

            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            finally { conn.Close(); }
            return IsExists;
        }
        public static bool ChangePassword(int UserID, string HashedPassword)
        {
            bool isChanged = false;
            string query = "update users Set password_hash = @hashed_password where id = @id";
            using var conn = new NpgsqlConnection(clsDataAccessSettings.ConnectionLink);
            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", UserID);
            cmd.Parameters.AddWithValue("@hashed_password",HashedPassword);
            try
            {
                conn.Open();
                int RowsEffected = cmd.ExecuteNonQuery();
                isChanged = RowsEffected > 0;
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            finally { conn.Close(); } 
            return isChanged;
        }
        public static bool ChangePassword(string Username, string HashedPassword)
        {
            bool isChanged = false;
            string query = "update users Set password_hash = @hashed_password where username = @username";
            using var conn = new NpgsqlConnection(clsDataAccessSettings.ConnectionLink);
            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@username", Username);
            cmd.Parameters.AddWithValue("@hashed_password", HashedPassword);
            try
            {
                conn.Open();
                int RowsEffected = cmd.ExecuteNonQuery();
                isChanged = RowsEffected > 0;
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            finally { conn.Close(); }
            return isChanged;
        }

        public static DataTable GetAllUsers(string? Role = null){ 
            DataTable dtAllUsers = new DataTable();

            string query = string.IsNullOrEmpty(Role)
            ? "SELECT id, username, email, created_at, role FROM users"
            : "SELECT id, username, email, created_at, role FROM users WHERE role = @role";

            using var Conn = new NpgsqlConnection(clsDataAccessSettings.ConnectionLink);
            using var Comm = new NpgsqlCommand(query, Conn);

            if (!string.IsNullOrEmpty(Role)) Comm.Parameters.AddWithValue("@role", Role);

                try
                {
                    Conn.Open();
                    using var reader = Comm.ExecuteReader();
                      dtAllUsers.Load(reader);
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
                

            return dtAllUsers;
        }

        public static string GetUserHashedPass(int UserID)
        {
            string HashedPassword ="";
            string query = "select password_hash from users where id = @id";
            using var Conn = new NpgsqlConnection(clsDataAccessSettings.ConnectionLink);
            using var Comm = new NpgsqlCommand(query,Conn);
            Comm.Parameters.AddWithValue("@id",UserID);
            try
            {
                Conn.Open();
                using var Reader = Comm.ExecuteReader();
                if(Reader.Read() && !Reader.IsDBNull(0)) HashedPassword = (string)Reader["password_hash"];
             
            }catch(Exception ex) { Console.WriteLine(   ex.Message); };
            return HashedPassword;
        }
        public static string GetUserHashedPass(string Username)
        {
            string HashedPassword = "";
            string query = "select password_hash from users where username = @username";
            using var Conn = new NpgsqlConnection(clsDataAccessSettings.ConnectionLink);
            using var Comm = new NpgsqlCommand(query, Conn);
            Comm.Parameters.AddWithValue("@username", Username);
            try
            {
                Conn.Open();
                using var Reader = Comm.ExecuteReader();
                if (Reader.Read() && !Reader.IsDBNull(0)) HashedPassword = (string)Reader["password_hash"];

            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            ;
            return HashedPassword;
        }


    }
}
