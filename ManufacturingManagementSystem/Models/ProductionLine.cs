using System.ComponentModel.DataAnnotations;

namespace ManufacturingManagementSystem.Models
{
    public enum ProductionLineStatus
    {
        Active,
        Inactive
    }
    public class ProductionLine
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ProductionLineStatus Status { get; set; }

        public int? CurrentOrderId { get; set; }
    }
}
