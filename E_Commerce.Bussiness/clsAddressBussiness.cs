using E_Commerce.DataAccess;
using System;
using System.Data;


namespace E_Commerce.Bussiness
{
    public class clsAddressBussiness
    {
        public  int UserID {  get; set; }
        public string? Street {  get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }

        public DateTime? CreatedDate { get; set; } 

        public clsAddressBussiness(int userID, string? street, string? city, string? state, string? postalCode, string? country, DateTime? CreatedAt)
        {
            UserID = userID;
            Street = street;
            City = city;
            State = state;
            PostalCode = postalCode;
            Country = country;
            CreatedDate = CreatedAt;
        }

        public bool AddNewAddress(int UserID, string street, string city, string state, string postalCode, string country) { 
          int IsAdded =clsAddress.AddNewAddress(UserID, street, city, country, state, postalCode);
            return IsAdded != -1;  }
        public bool UpdateAddress(int UserID, string street, string city, string country, string? state = null, string? postalCode = null) 
            => clsAddress.UpdateAddress(UserID, street, city, country, state, postalCode); 
       
        public bool DeleteAddress (int UserID) => clsAddress.DeleteAddress(UserID);

        public DataTable GetAllAdsresses(string? City = null, string? Country = null) =>  clsAddress.GetAllAddresses(City, Country);
           
        public clsAddressBussiness? GetAddressInfo(int UserID)
        {
            string city = "", street = "", state = "", postal_code = "", country = ""; DateTime  created_at = DateTime.MinValue;

            bool IsFound = clsAddress.GetAddressInfo(UserID,ref street,ref city,ref state,ref postal_code,ref country,ref created_at);
            if (IsFound) return new clsAddressBussiness(UserID, street, city, state, postal_code, country, created_at);
            else return null;

        }
           
        
        
    }
}
