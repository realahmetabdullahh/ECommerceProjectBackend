using Npgsql;
using System.Data;



namespace E_Commerce.DataAccess
{
    public class clsCategories
    {
        public static int AddNewCategory( string Name, string? desc)
        {
            int InsertedID = -1;
            string query = string.IsNullOrWhiteSpace(desc) ? "insert into categories (name) values (@name) returning id;" : "insert into categories (name,description) values (@name,@description) returning id;";
            using var Conn = new NpgsqlConnection(clsDataAccessSettings.ConnectionLink);
            using var Comm = new NpgsqlCommand(query,Conn);
            Comm.Parameters.AddWithValue("@name", Name);
            if(!string.IsNullOrWhiteSpace(desc))   Comm.Parameters.AddWithValue("@description", desc);
            try
            {
                Conn.Open();

                object? Result =Comm.ExecuteScalar();
                if(Result != null && int.TryParse(Result.ToString(), out int newID)) InsertedID = newID;
                    
            } 
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            
 
            return InsertedID;
        }
        public static bool DeleteCategory(string CategoryName)
        {
            bool IsDeleted = false;
            string query = "delete from categories where name = @name";
            using var Conn = new NpgsqlConnection(clsDataAccessSettings.ConnectionLink);
            using var Comm = new NpgsqlCommand(query, Conn);
            Comm.Parameters.AddWithValue("@name", CategoryName);
            try
            {
                Conn.Open();
                int RowsAffected = Comm.ExecuteNonQuery();
                IsDeleted = RowsAffected > 0;

            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
           


            return IsDeleted;
        }

        public static bool GetCategoryInfo(string CategoryName, ref int CategoryID, ref string Description)
        {
            bool IsFound = false;
            string query = "select id, name, description from categories where name = @name";
            using var Conn = new NpgsqlConnection(clsDataAccessSettings.ConnectionLink);
            using var Comm = new NpgsqlCommand(query, Conn);
            Comm.Parameters.AddWithValue("@name", CategoryName);
           
            try
            {
                Conn.Open();
                using var Reader = Comm.ExecuteReader();
                if (Reader.Read())
                {
                    CategoryID = Convert.ToInt32( Reader["id"]);
                    Description = Reader["description"].ToString() ?? "";
                    IsFound = true;
                }

            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            

            return IsFound;
        }
        public static DataTable GetAllCategories()
        {
            DataTable dtAllCategories = new DataTable();
            string query = "select * from categories;";
            using var conn = new NpgsqlConnection(clsDataAccessSettings.ConnectionLink);
            using var Comm = new NpgsqlCommand(query, conn);
           
            try
            {
                conn.Open(); 
                using var reader = Comm.ExecuteReader();
               
                  dtAllCategories.Load(reader);

                

            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }


            return dtAllCategories;
        }
    }
}
