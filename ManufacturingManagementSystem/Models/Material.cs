using System.ComponentModel.DataAnnotations;

namespace ManufacturingManagementSystem.Models
{
    public class Material
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Range(0, int.MaxValue)]
        public int QuantityAvailable { get; set; }

        [Required]
        public string UnitOfMeasure { get; set; }
        public ICollection<OrderMaterial> OrderMaterials { get; set; }
    }
}
