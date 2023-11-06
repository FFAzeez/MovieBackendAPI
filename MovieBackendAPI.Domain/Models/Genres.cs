using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBackendAPI.Domain.Models
{
    [Table("Genres")]
    public class Genres:BaseModel
    {
        [Required]
        public string Genre { get; set; }
        [ForeignKey("Movies")]
        public long MoviesId { get; set; }
    }
}
