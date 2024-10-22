using System.ComponentModel.DataAnnotations;

namespace ManufacturingManagementSystem.DTOs
{
    public class OrderMaterialDto
    {
        public int MaterialId { get; set; }

        [Range(1, int.MaxValue)]
        public int QuantityUsed { get; set; }
    }
}
