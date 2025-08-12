using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.DataAccess
{
    public class clsOrders
    {
        public static int AddNewOrder(int UserID,int AddressID,string Status, decimal TotalAmount)
        {
            int insertedID = -1;
            string query = "insert into orders (user_id,address_id,status,total_amount) values(@user_id,@address_id,@status,@total_amount) RETURNING id";
            using var conn = new NpgsqlConnection(clsDataAccessSettings.ConnectionLink);
            using var Com = new NpgsqlCommand(query, conn);
            Com.Parameters.AddWithValue("@user_id", UserID);
            Com.Parameters.AddWithValue("@address_id",AddressID);
            Com.Parameters.AddWithValue("@status", Status);
            Com.Parameters.AddWithValue("@total_amount",TotalAmount);
            try
            {
                conn.Open();
                var Result = Com.ExecuteScalar();
                if(Result != null && int.TryParse(Result.ToString(),out int NewID)) insertedID = NewID;


            }catch(Exception ex) { Console.WriteLine(ex.ToString()); }
            return insertedID;
        }

        public static DataTable GetAllOrders(string? status = null, int? OverThanThisTotalAmount = 0, int? UserID = null)
        {
            DataTable dtAllOrders = new DataTable();
            using var Conn = new NpgsqlConnection(clsDataAccessSettings.ConnectionLink);

            string query = "SELECT * FROM Orders";

            List<string> filters = new();

            if (!string.IsNullOrEmpty(status)) filters.Add("status = @status");
            if (UserID.HasValue) filters.Add("user_id = @user_id");
            if (OverThanThisTotalAmount.HasValue) filters.Add("total_amount > @OverThanThisTotalAmount");

            if (filters.Count > 0)
                query += " WHERE " + string.Join(" AND ", filters);

            using var Com = new NpgsqlCommand(query, Conn);

            if (!string.IsNullOrEmpty(status))
                Com.Parameters.AddWithValue("@status", status);

            if (UserID.HasValue)
                Com.Parameters.AddWithValue("@user_id", UserID.Value);

            if (OverThanThisTotalAmount.HasValue)
                Com.Parameters.AddWithValue("@OverThanThisTotalAmount", OverThanThisTotalAmount.Value);

            try
            {
                Conn.Open();
                using var reader = Com.ExecuteReader();
                dtAllOrders.Load(reader);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return dtAllOrders;
        }

        public static bool DeleteOrder(int OrderID)
        {
            bool IsDeleted = false;
            string query = "Delete from Orders where id = @id";
            using var Conn = new NpgsqlConnection(clsDataAccessSettings.ConnectionLink);
            using var Com = new NpgsqlCommand(query,Conn);
            Com.Parameters.AddWithValue("@id",OrderID);
            try
            {
                Conn.Open();
                int AffectedRows = Com.ExecuteNonQuery();
                return AffectedRows > 0;
            }
            catch (Exception ex){ Console.WriteLine(ex.Message); }
            return IsDeleted;
        }
        public static bool GetOrderInfo(int OrderID,ref int UserID,ref int AddressID,ref DateTime OrderDate, ref string Status,ref decimal TotalAmount) { 
        bool IsFound = false;
            string query = "Select * from Orders where id = @order_id";
            using var Conn = new NpgsqlConnection(clsDataAccessSettings.ConnectionLink);
            using var Com = new NpgsqlCommand(query, Conn);
            Com.Parameters.AddWithValue("@order_id", OrderID);
            try
            {

                Conn.Open();
                using var Reader = Com.ExecuteReader();
                if (Reader.Read())
                {
                    UserID = (int)Reader["user_id"];
                    AddressID = (int)Reader["address_id"];
                    OrderDate = (DateTime)Reader["order_date"];
                    Status = (string)Reader["status"];
                    TotalAmount = (decimal)Reader["total_amount"];

                    IsFound = true;
                }

            }catch(Exception ex) {  Console.WriteLine(ex.Message); }
            return IsFound;
        }
    }
}
