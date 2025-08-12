using E_Commerce.DataAccess;


namespace E_Commerce.Bussiness
{
    public class clsOrder_ItemsBussiness
    {
       public int OrderID { get; set; }

      public  int ProdcuctID { get; set; }

     public   int Quantity {  get; set; }

     public int Unit_price {  get; set; }

        public clsOrder_ItemsBussiness(int OrderID, int ProductID, int Quantity, int UnitPrice)
        {
            this.OrderID = OrderID; this.ProdcuctID = ProductID; this.Quantity = Quantity; this.Unit_price = UnitPrice;
        }

        public static bool AddOrder_Items(int OrderID,int ProductID,int Quantity,int UnitPrice)
        {
            int Result = clsOrder_Items.AddNewOrder_Item(OrderID, ProductID, Quantity, UnitPrice);
            return Result != -1;
        }

          
    }
}
