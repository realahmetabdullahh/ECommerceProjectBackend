using E_Commerce.DataAccess;
using System.Data;


namespace E_Commerce.Bussiness
{
    public class clsOrdersBussiness
    {
        public int OrderID {  get; set; }
        public int UserID {  get; set; }
        public int AddressID {  get; set; }

        public DateTime OrderDate { get; set; }
        public string Status {  get; set; }
        public decimal Total_Amount {  get; set; }




        public clsOrdersBussiness(int orderID,int userID,int addressID,DateTime orderDate, string status,decimal total_amount) { 
            
            this.OrderID = orderID;
            this.UserID = userID;
            this.AddressID = addressID;
            this.OrderDate = orderDate;
            this.Status = status;
            this.Total_Amount = total_amount;
        }

        public static bool AddNewOrder(int userID, int addressID, DateTime orderDate, string status, decimal total_amount) {
            int Result = clsOrders.AddNewOrder( userID, addressID, status, total_amount);
            return Result != -1;
        }
        public static bool DeleteOrder(int orderID) => clsOrders.DeleteOrder(orderID);

        public static clsOrdersBussiness? GetOrderInfo(int OrderID)
        {
            string status =""; int UserID = 0; int AddressID = 0; DateTime OrderDate = DateTime.MinValue; decimal total_amount = 0;
            bool IsFound = clsOrders.GetOrderInfo(OrderID,ref UserID,ref AddressID,ref OrderDate,ref status,ref total_amount);
            if (IsFound) return new clsOrdersBussiness(OrderID,UserID,AddressID,OrderDate,status,total_amount);
            return null;
           
        }

        public static DataTable GetOrderList(string? status = null,int? overThanThisAmount = 0, int? userID = null) => clsOrders.GetAllOrders(status,overThanThisAmount,userID);

    }
}   
