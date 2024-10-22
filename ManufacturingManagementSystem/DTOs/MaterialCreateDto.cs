using System.ComponentModel.DataAnnotations;

namespace ManufacturingManagementSystem.DTOs
{
    public class MaterialCreateDto
    {
        [Required]
        public string Name { get; set; }

        [Range(0, int.MaxValue)]
        public int QuantityAvailable { get; set; }

        [Required]
        public string UnitOfMeasure { get; set; }
    }
}
