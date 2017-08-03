using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HxLabsAdvanced.APIService.Entities
{
    public class Actor
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [ForeignKey("MovieId")]
        public Movie Movie { get; set; }

        public Guid MovieId { get; set; }
    }
}
