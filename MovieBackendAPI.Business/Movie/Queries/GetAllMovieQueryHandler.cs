using AutoMapper;
using MediatR;
using MovieBackendAPI.Domain.BindingModels;
using MovieBackendAPI.Domain.Const;
using MovieBackendAPI.Domain.Models;
using MovieBackendAPI.Domain.Utility;
using MovieBackendAPI.Infrastructure.Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MovieBackendAPI.Business.Movie.Queries
{
    public class GetAllMovieQueryHandler : IRequestHandler<GetAllMovieQuery, GenericListSearchResult<IEnumerable<MovieResponse>>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public GetAllMovieQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<GenericListSearchResult<IEnumerable<MovieResponse>>> Handle(GetAllMovieQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Movies, bool>> predicate = x => true;
            var response = new GenericListSearchResult<IEnumerable<MovieResponse>>();
            if (!string.IsNullOrEmpty(request.Search))
            {
                predicate = predicate.And(_=>_.Name.Contains(request.Search) || _.Description.Contains(request.Search) 
                || _.Photo.Contains(request.Search) || _.Country.Contains(request.Search));
            }
            if (request.Rating.HasValue)
            {
                predicate = predicate.And(_ => _.Rating == request.Rating);
            }
            if (request.ReleaseDate.HasValue)
            {
                predicate = predicate.And(_ => _.ReleaseDate == request.ReleaseDate);
            }
            if (request.TicketPrice.HasValue)
            {
                predicate = predicate.And(_ => _.TicketPrice == request.TicketPrice);
            }

            var movie = _uow.GetRepository<Movies>().Get(predicate, includeProperties: "Genres").ToList();
            if (movie.Any())
            {
                var map = _mapper.Map<IEnumerable<MovieResponse>>(movie);
                var responseMap = Pagination<MovieResponse>.ToPagedList(map, request.PageNumber, request.PageSize);
                response.Result = responseMap;
                response.StatusCode = ResponseCode.SUCCESSFUL;
                response.StatusMessage = "Retrieved Successfully";
                response.CurrentPage = responseMap.CurrentPage;
                response.PageSize = responseMap.PageSize;
                response.TotalPages = responseMap.TotalPages;
                response.TotalRows = responseMap.TotalCount;
            }
            response.StatusMessage = "Not Retrieved.";
            response.StatusCode = ResponseCode.NOTFOUND;
            return response;
        }
    }
}
