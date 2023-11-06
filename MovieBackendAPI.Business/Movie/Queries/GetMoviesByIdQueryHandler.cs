using AutoMapper;
using MediatR;
using MovieBackendAPI.Domain.BindingModels;
using MovieBackendAPI.Domain.Const;
using MovieBackendAPI.Domain.Models;
using MovieBackendAPI.Infrastructure.Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MovieBackendAPI.Business.Movie.Queries
{
    public class GetMoviesByIdQueryHandler : IRequestHandler<GetMoviesByIdQuery, ServiceResponse<MovieResponse>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public GetMoviesByIdQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }
        public async Task<ServiceResponse<MovieResponse>> Handle(GetMoviesByIdQuery request, CancellationToken cancellationToken)
        {
            var response = new ServiceResponse<MovieResponse>();
            var movie = _uow.GetRepository<Movies>().Get(_ => _.Id == request.Id, includeProperties: "Genres").FirstOrDefault();
            if(movie == null)
            {
                response.StatusMessage = "Invalid Id "+request.Id ;
                response.StatusCode = ResponseCode.NOTFOUND;
                return response;
            }
            var map = _mapper.Map<MovieResponse>(movie);
            response.StatusMessage = "Successfully Fetched.";
            response.StatusCode = ResponseCode.SUCCESSFUL;
            response.ResponseObject = map;
            return response;
        }
    }
}
