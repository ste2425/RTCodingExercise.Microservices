using System.ComponentModel.DataAnnotations;

namespace Catalog.API.Models
{
    public class PlateModel
    {
        [Required]
        public string? Registration { get; set; }

        [Required]
        public decimal PurchasePrice { get; set; }

        [Required]
        public decimal SalePrice { get; set; }

        [Required]
        public bool Reserved { get; set; }

        [Required]
        public bool ForSale { get; set; }
    }
}