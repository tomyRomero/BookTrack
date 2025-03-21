using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class Rating
    {
        [Key]
        public int RatingId { get; set; } 

        [Required]
        public required int UserId { get; set; } 

        [Required] 
        [Range(1, 5)]
        public required int Score { get; set; } 

        [Required]
        public required int BookId { get; set; }

        [ForeignKey(nameof(UserId))] 
        public User? User { get; set; } 

        [ForeignKey(nameof(BookId))] 
        public Book? Book { get; set; }
    }
}