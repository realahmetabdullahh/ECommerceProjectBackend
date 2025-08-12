using E_Commerce.DataAccess;


namespace E_Commerce.Bussiness
{
    public  class clsProductsBussiness
    {
        public int ID { get; set; }
        public string Name { get; set; }    
        public string? Description { get; set; }
        public decimal Price {  get; set; }
        public int CategoryID { get; set; }
        public int UserID {  get; set; }
        public string? ImageURL {  get; set; }

        public DateTime CreatedAt { get; set; }

        public  clsProductsBussiness(int iD, string name, string? description, decimal price, int categoryID, int userID, string? imageURL, DateTime createdAt)
        {
            ID = iD;
            Name = name;
            Description = description;
            Price = price;
            CategoryID = categoryID;
            UserID = userID;
            ImageURL = imageURL;
            CreatedAt = createdAt;
        }

        public static bool AddNewProduct(string Name,string? Description,decimal Price,int CategoryID,int CreatedByUserID,string? ImageURL)
        {
            int InsertedID = clsProducts.AddNewProduct(Name,Description,Price,CategoryID,CreatedByUserID,ImageURL);
            return InsertedID != -1;
        }

        public static bool DeleteProduct(int ProductID) => clsProducts.DeleteProduct(ProductID);
        public static bool UpdateProduct(int ProductID, string Name, string Description, decimal price, int CategoryID, int CreatedByUserID, string ImageURL)
        {
            return clsProducts.UpdateProduct(ProductID, Name, Description, price, CategoryID,CreatedByUserID, ImageURL);
        }
        public static  clsProductsBussiness? GetProductInfo(int iD, ref string name, ref string description,ref decimal price,ref int categoryID, ref int userID,ref string imageURL,ref DateTime createdAt)
        {
            bool IsFound = clsProducts.GetProductInfo(iD, ref name, ref description, ref price, ref categoryID, ref userID, ref imageURL , ref createdAt);
            if(IsFound) return new clsProductsBussiness(iD, name, description, price, categoryID,userID,imageURL, createdAt);
            return null;
        }
          
        
    }
}
