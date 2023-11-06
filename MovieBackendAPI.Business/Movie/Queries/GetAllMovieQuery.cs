using MediatR;
using MovieBackendAPI.Domain.BindingModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBackendAPI.Business.Movie.Queries
{
    public class GetAllMovieQuery:IRequest<GenericListSearchResult<IEnumerable<MovieResponse>>>
    {
        public int? Rating { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public decimal? TicketPrice { get; set; }
        public string? Search { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        [Required]
        public int PageSize { get; set; }
        [Required]
        public int PageNumber { get; set; }
    }
}
