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
    public class GetMoviesByIdQuery:IRequest<ServiceResponse<MovieResponse>>
    {
        [Required]
        public long Id { get; set; }
    }
}
