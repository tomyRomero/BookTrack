using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class Genre
    {
        [Key]
        public int GenreId { get; set; } 

        [Required]
        public required string Name { get; set; }

        // Navigation property for books (optional)
        public ICollection<Book>? Books { get; set; } //Many-to-one
    }
}