using Npgsql;

using System.Data;


namespace E_Commerce.DataAccess
{
    public class clsAddress
    {
        public static int AddNewAddress(int UserID, string Street,string City,string Country ,string? State = null,string? Postalcode = null )
        {
            int InsertedID = -1;
            string query ;
            bool IsStateNull = string.IsNullOrEmpty(State);
            bool IsPostalCodeNull = string.IsNullOrEmpty(Postalcode);

            if(IsStateNull && IsPostalCodeNull) query = "insert into addresses(user_id,street,city,state,postal_code,country) values(@user_id,@street,@city,null,null,@country) returning id;";
            else if (IsPostalCodeNull) query = "insert into addresses(user_id,street,city,state,postal_code,country) values(@user_id,@street,@city,@state,null,@country) returning id;";
            else if(IsStateNull) query = "insert into addresses(user_id,street,city,state,postal_code,country) values(@user_id,@street,@city,null,@postal_code,@country) returning id;";
            else query = "insert into addresses(user_id,street,city,state,postal_code,country) values(@user_id,@street,@city,@state,@postal_code,@country) returning id;";
            using var Conn = new NpgsqlConnection(clsDataAccessSettings.ConnectionLink);
            using var Comm =new NpgsqlCommand(query,Conn);
            Comm.Parameters.AddWithValue("@user_id", UserID);
            Comm.Parameters.AddWithValue("@street",Street);
            Comm.Parameters.AddWithValue("@city", City);
             if(State != null) Comm.Parameters.AddWithValue("@state",State);
           if(Postalcode != null) Comm.Parameters.AddWithValue("@postal_code",Postalcode);
            try
            {
                Conn.Open();
                var result = Comm.ExecuteScalar();
                if(result != null && int.TryParse(result.ToString(), out int NewID)) InsertedID = NewID;
    
            }   catch(Exception ex) { Console.WriteLine(ex.Message); }
                return InsertedID;
        }
        public static bool UpdateAddress(int UserID, string Street,string City,string Country, string? State = null, string? Postalcode = null)
        {
            bool IsUpdated = false;
            string query;
            bool IsStateNull = string.IsNullOrEmpty(State);
            bool IsPostalCodeNull = string.IsNullOrEmpty(Postalcode);

            if (IsStateNull && IsPostalCodeNull) query = "update addresses set street = @street, city = @city, state = null, postal_code = null, country = @country where user_id = @user_id;";

            else if (IsPostalCodeNull) query = "update addresses set street = @street, city = @city, state = @state, postal_code = null, country = @country where user_id = @user_id ;";

            else if (IsStateNull) query = "update addresses set street = @street, city = @city, state = null, postal_code = @postal_code, country = @country where user_id = @user_id;";

            else query = "update addresses set street = @street, city = @city, state = @state, postal_code = @postal_code, country = @country where user_id = @user_id;";
            using var Conn = new NpgsqlConnection(clsDataAccessSettings.ConnectionLink);
            using var Comm = new NpgsqlCommand(query, Conn);
            Comm.Parameters.AddWithValue("@user_id", UserID);
            Comm.Parameters.AddWithValue("@street", Street);
            Comm.Parameters.AddWithValue("@city", City);
           if(!string.IsNullOrEmpty(State))Comm.Parameters.AddWithValue("@state", State);
           if (!string.IsNullOrEmpty(Postalcode)) Comm.Parameters.AddWithValue("@postal_code", Postalcode);
           Comm.Parameters.AddWithValue("@country",Country);
            try
            {
                Conn.Open();
                var AffectedRows = Comm.ExecuteNonQuery();
                IsUpdated = Convert.ToInt32(AffectedRows) > 0;
            } catch (Exception ex) { Console.WriteLine(ex.Message); };

            return IsUpdated;
        }
        public static bool DeleteAddress(int UserID)
        {
            bool IsDeleted = false;
            string query = "Delete from addresses where user_id = @user_id";
            using var Conn = new NpgsqlConnection(clsDataAccessSettings.ConnectionLink);
            using var Comm = new NpgsqlCommand(query,Conn);
            Comm.Parameters.AddWithValue("@user_id", UserID);
            try
            {
                Conn.Open();
                var AffectedRows = Comm.ExecuteNonQuery();
                IsDeleted = Convert.ToInt32(AffectedRows) > 0;

            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            return IsDeleted;


        }
        public static bool GetAddressInfo(int UserID,ref string Street, ref string City,ref string State, ref string PostalCode,ref string Country,ref DateTime CreatedAt)
        {
            bool IsFound = false;
            string query = "select * from Addresses where user_id = @user_id";
            using var Conn = new NpgsqlConnection(clsDataAccessSettings.ConnectionLink);
            using var Comm = new NpgsqlCommand(query, Conn);
            Comm.Parameters.AddWithValue("@user_id",UserID);
            try
            {
                Conn.Open() ;
                
                using var Reader = Comm.ExecuteReader();
                if (Reader.Read())
                {
                    Street = Reader["street"]?.ToString() ?? "";
                    City = Reader["city"]?.ToString() ?? "";
                    State = Reader["state"]?.ToString() ?? "";
                    PostalCode = Reader["postal_code"]?.ToString() ?? "";
                    Country = Reader["country"]?.ToString() ?? "";
                    CreatedAt = Reader["created_at"] as DateTime? ?? DateTime.MinValue;
                    IsFound = true;
                }
            



            }
            catch (Exception ex) { Console.WriteLine(ex.Message) ; }
            return IsFound;
        }
        public static DataTable GetAllAddresses(string? City = null, string? Country = null) { 
            DataTable stAllAddresses = new DataTable();
        
            string query = "SELECT * FROM Addresses";
            var hasCity = !string.IsNullOrEmpty(City);
            var hasCountry = !string.IsNullOrEmpty(Country);
            var filters = new List<string>();

            if (hasCity) filters.Add("City = @city");
            if (hasCountry) filters.Add("Country = @country");

            if (filters.Count > 0) query += " WHERE " + string.Join(" AND ", filters);
            
            using var Conn = new NpgsqlConnection(clsDataAccessSettings.ConnectionLink);
            using var Comm = new NpgsqlCommand(query,Conn);

           if(!string.IsNullOrEmpty(City)) Comm.Parameters.AddWithValue("@city",City);
           if(!string.IsNullOrEmpty(Country)) Comm.Parameters.AddWithValue("@country",Country);
            try
            {
                Conn.Open();
                  using var reader = Comm.ExecuteReader();
               
                  stAllAddresses.Load(reader);
                

            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }



                return stAllAddresses;
        }
    }
}
