using MediatR;
using Microsoft.AspNetCore.Http;
using MovieBackendAPI.Domain.BindingModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBackendAPI.Business.Movie.Commands
{
    public class AddMoviesCommand:IRequest<ServiceResponse>
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime ReleaseDate { get; set; }
        [Required, Range(1, 5)]
        public int Rating { get; set; }
        [Required]
        public decimal TicketPrice { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public IFormFile PhotoFile { get; set; }
        public IEnumerable<AddGenreCommand> Genres { get; set; }
    }

    public class AddGenreCommand
    {
        [Required]
        public string Genre { get; set; }
    }
}
