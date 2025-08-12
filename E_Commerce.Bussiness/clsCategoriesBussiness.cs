using E_Commerce.DataAccess;
using System.Data;

namespace E_Commerce.Business
{
    public class clsCategoriesBusiness
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string? CategoryDescription { get; set; }

        public clsCategoriesBusiness(int categoryID, string categoryName, string? categoryDescription = null)
        {
            CategoryID = categoryID;
            CategoryName = categoryName;
            CategoryDescription = categoryDescription;
        }

        public static bool AddNewCategory(string categoryName, string? description)
        {
            int categoryID = clsCategories.AddNewCategory( categoryName, description);
            return categoryID != -1;
        }
        public static bool DeleteCategory(string categoryName) => clsCategories.DeleteCategory(categoryName);

        public static clsCategoriesBusiness? ReadCategoryInfo(string categoryName)
        {
            int categoryID = 0;
            string description = "";

            bool isFound = clsCategories.GetCategoryInfo(categoryName, ref categoryID, ref description);
            return isFound ? new clsCategoriesBusiness(categoryID, categoryName, description) : null;
        }

       

        public static DataTable GetAllCategories() => clsCategories.GetAllCategories();
    }
}
