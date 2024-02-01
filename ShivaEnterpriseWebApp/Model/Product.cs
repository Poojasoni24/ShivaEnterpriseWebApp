using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ShivaEnterpriseWebApp.Model
{
    public class Product
    {
        public Guid ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string? ProductDescription { get; set; }
        public bool ProductStatus { get; set; }
        public string ProductImage { get; set; }
        public string ProductCategoryId { get; set; }
        public string ProductGroupId { get; set; }
        public string ProductTypeId { get; set; }
        public List<SelectListItem> ProductsCategoryId { get; set;}
        public List<SelectListItem> ProductsGroupId { get; set;}
        public List<SelectListItem> ProductsTypeId { get; set;}
    }
}
