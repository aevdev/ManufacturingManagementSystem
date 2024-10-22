using System.ComponentModel.DataAnnotations;

namespace ManufacturingManagementSystem.DTOs
{
    public class OrderCreateDto
    {
        [Required]
        public string ProductName { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        public List<OrderMaterialDto> Materials { get; set; }
    }
}
