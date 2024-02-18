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
        public bool IsActive { get; set; }
        public string ProductImage { get; set; }

        [Display(Name = "Add a productImage")]
        [DataType(DataType.Upload)]
        [FileExtensions(Extensions = "jpg,png,gif,jpeg,bmp,svg")]
        public IFormFile ImageFile { get; set; }
        public string ProductCategoryId { get; set; }
        public string ProductGroupId { get; set; }
        public string ProductTypeId { get; set; }
        public List<SelectListItem> ProductsCategoryId { get; set;}
        public ProductCategory ProductCategory { get; set; }
        public List<SelectListItem> ProductsGroupId { get; set;}
        public ProductGroup ProductGroup { get; set; }
        public List<SelectListItem> ProductsTypeId { get; set;}
        public ProductType ProductType { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
    }
}
