using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBackendAPI.Domain.BindingModels
{
    public class GenreResponse
    {
        public long Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string Genre { get; set; }
        public long MoviesId { get; set; }
    }
}
