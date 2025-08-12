using Npgsql;
using System;
using System.Data;


namespace E_Commerce.DataAccess
{
    public class clsProducts
    {
        public static int AddNewProduct( string Name, string? Description,decimal Price, int CategoryID, int CreatedByUserId, string? ImageURL)
        {
            int InsertedID = -1;
            string query = "insert into products (name,description,price,category_id,user_id,image_url) values (@name,@description,@price,@category_id,@user_id,@image_url) returning id;";
            using var Conn = new NpgsqlConnection(clsDataAccessSettings.ConnectionLink);
            using var Comm = new NpgsqlCommand(query,Conn);
            Comm.Parameters.AddWithValue("@name", Name);
            Comm.Parameters.AddWithValue("@description", Description ?? "");
            Comm.Parameters.AddWithValue("@price",Price);
            Comm.Parameters.AddWithValue("@category_id", CategoryID);
            Comm.Parameters.AddWithValue("@user_id", CreatedByUserId);
            Comm.Parameters.AddWithValue("@image_url", ImageURL ?? "");

            try
            {
                Conn.Open();
                var Result = Comm.ExecuteScalar();
                if(Result != null && int.TryParse(Result.ToString(),out int NewID)) InsertedID = NewID;
            }
            catch(Exception ex) { Console.WriteLine(ex.Message); }


            return InsertedID;
        }
        public static bool UpdateProduct(int ProductID, string Name, string Description, decimal Price, int CategoryID, int CreatedByUserID, string ImageURL)
        {
            bool isUpdated = false;
            string query = "update products set name = @name, description = @description, price = @price, category_id = @category_id, user_id = @user_id, image_url = @image_url where id = @id;";

            using var Conn = new NpgsqlConnection(clsDataAccessSettings.ConnectionLink);
            using var Comm = new NpgsqlCommand(query, Conn);

            Comm.Parameters.AddWithValue("@id", ProductID);
            Comm.Parameters.AddWithValue("@name", Name);
            Comm.Parameters.AddWithValue("@description", Description ?? "");
            Comm.Parameters.AddWithValue("@price", Price);
            Comm.Parameters.AddWithValue("@category_id", CategoryID);
            Comm.Parameters.AddWithValue("@user_id", CreatedByUserID);
            Comm.Parameters.AddWithValue("@image_url", ImageURL ?? "");

            try
            {
                Conn.Open();
                int rowsAffected = Comm.ExecuteNonQuery();
                isUpdated = rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return isUpdated;
        }

        public static bool DeleteProduct(int ProductID) { 
            bool IsDeleted = false;
            string query = "delete from products where id = @id";
            using var Conn = new NpgsqlConnection(clsDataAccessSettings.ConnectionLink);
            using var Comm = new NpgsqlCommand(query,Conn);
            Comm.Parameters.AddWithValue("@id",ProductID);
            try
            {
                Conn.Open();
                int RowsAffected = Comm.ExecuteNonQuery();
                IsDeleted = RowsAffected > 0;


            }catch(Exception ex) {  Console.WriteLine(ex.Message);}
            return IsDeleted;
        
        }
        public static bool GetProductInfo(int ProductID,ref string Name,ref string Description, ref decimal Price, ref int CategoryID,ref int CreatedByUserID,ref string ImageURL, ref DateTime CreatedAt) { 
            bool isFound = false;
          string query = "SELECT id, name, description, price, category_id, user_id, image_url, created_at FROM products";

            using var Conn = new NpgsqlConnection(clsDataAccessSettings.ConnectionLink);
            using var Comm = new NpgsqlCommand(query, Conn);
            Comm.Parameters.AddWithValue("@id", ProductID);
            try
            {
                Conn.Open();
                using var reader = Comm.ExecuteReader();
                if (reader.Read())
                {
                    Name = (string)reader["name"];
                    Description = (string)reader["description"];
                    Price = (decimal)reader["price"];
                    CategoryID = (int)reader["category_id"];
                    CreatedByUserID = (int)reader["user_id"];
                    ImageURL = (string)reader["image_url"];
                    CreatedAt = (DateTime)reader["created_at"];
                    isFound = true;
                }

            }catch(Exception ex) { Console.WriteLine(ex.Message ); }
            return isFound;
        
        }
        public static DataTable GetAllProducts(int? CategoryID = null, string? OrderAscOrDesc = null) { 
            DataTable dtAllProducts = new DataTable();
            string query = "select * from products";
            if (CategoryID != null)  query += " where category_id = @category_id";

            if (!string.IsNullOrEmpty(OrderAscOrDesc))
            {
                var order = OrderAscOrDesc.ToUpper();
                if (order == "ASC" || order == "DESC")
                    query += $" ORDER BY price {order}";
            }
            

                using var Conn = new NpgsqlConnection(clsDataAccessSettings.ConnectionLink);
            using var Comm = new NpgsqlCommand(query, Conn);
            if (CategoryID != null) Comm.Parameters.AddWithValue("@category_id", CategoryID);
            try
            {
                Conn.Open();
                using var Reader = Comm.ExecuteReader();
                dtAllProducts.Load(Reader);
            }catch(Exception ex) { Console.WriteLine( ex.Message ); }
               
         
            return dtAllProducts;
        }
       

    }
}
