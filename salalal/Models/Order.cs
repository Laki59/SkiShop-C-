using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace salalal.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }
        public User User { get; set; } // Navigacioni property

        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.Now; // Stavljamo trenutno vreme

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Pending"; // Novi orderi idu odmah na pending po defaultu
    }
}
