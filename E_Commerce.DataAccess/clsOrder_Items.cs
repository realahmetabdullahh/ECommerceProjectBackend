using Npgsql;


namespace E_Commerce.DataAccess
{
    public class clsOrder_Items
    {
        public static int AddNewOrder_Item(int OrderID, int ProductID, int Quantity,decimal unit_Price)
        {
            int insertedID = -1;
            string query = "insert into order_items(order_id,product_id,quantity,unit_price) values(@orderID,@productID,@quantity,@unit_price) returning id;";
            using var Con = new NpgsqlConnection(clsDataAccessSettings.ConnectionLink);
            using var Comm = new NpgsqlCommand(query, Con);
            Comm.Parameters.AddWithValue("@orderID", OrderID);
            Comm.Parameters.AddWithValue("@ProductID",ProductID);
            Comm.Parameters.AddWithValue("@quantity",Quantity);
            Comm.Parameters.AddWithValue("@unit_price", unit_Price);
            try
            {
                Con.Open();
                object? Result = Comm.ExecuteScalar();
                if (Result != null && int.TryParse(Result.ToString(),out int NewID)) insertedID = NewID;

            }
            catch(Exception ex) {Console.Write(ex.ToString());}



            return insertedID;
        }
        
    }
}
