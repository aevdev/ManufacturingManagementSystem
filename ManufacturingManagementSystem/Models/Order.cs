using System.ComponentModel.DataAnnotations;

namespace ManufacturingManagementSystem.Models
{
    public enum OrderStatus
    {
        Pending,
        InProgress,
        Completed,
        Canceled
    }
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        public OrderStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }
        public ICollection<OrderMaterial> OrderMaterials { get; set; }
    }
}
