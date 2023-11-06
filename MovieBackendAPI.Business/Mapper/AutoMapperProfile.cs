using AutoMapper;
using MovieBackendAPI.Business.Movie.Commands;
using MovieBackendAPI.Domain.BindingModels;
using MovieBackendAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBackendAPI.Business.Mapper
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AddMoviesCommand, Movies>();
            CreateMap<AddGenreCommand, Genres>();
            CreateMap<MovieResponse, Movies>().ReverseMap();
            CreateMap<GenreResponse, Genres>().ReverseMap();
        }
    }
}
