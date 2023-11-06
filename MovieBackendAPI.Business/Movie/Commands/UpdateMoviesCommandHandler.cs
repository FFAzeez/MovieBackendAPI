using AutoMapper;
using MediatR;
using MovieBackendAPI.Domain.BindingModels;
using MovieBackendAPI.Domain.Const;
using MovieBackendAPI.Domain.Models;
using MovieBackendAPI.Domain.Utility;
using MovieBackendAPI.Infrastructure.Persistence.UnitOfWork;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MovieBackendAPI.Business.Movie.Commands
{
    public class UpdateMoviesCommandHandler : IRequestHandler<UpdateMoviesCommand, ServiceResponse>
    {
        private readonly IUnitOfWork _uow;
        private readonly IFileUpload _file;
        private readonly IMapper _mapper;
        public UpdateMoviesCommandHandler(IUnitOfWork uow, IFileUpload file, IMapper mapper)
        {
            _uow = uow;
            _file = file;
            _mapper = mapper;
        }

        public async Task<ServiceResponse> Handle(UpdateMoviesCommand request, CancellationToken cancellationToken)
        {
            var movie = _uow.GetRepository<Movies>().Get(_ => _.Id == request.Id, includeProperties: "Genres").FirstOrDefault();
            var response = new ServiceResponse();
            var fileResponse = await _file.SaveImageAsync(request.PhotoFile);
            if (string.IsNullOrEmpty(fileResponse))
            {
                response.StatusMessage = "Unable to create file, please try again later.";
                response.StatusCode = ResponseCode.BadRequest;
                return response;
            }
            using(var transaction = _uow.BeginTransaction())
            {
                movie.TicketPrice = request.TicketPrice;
                movie.Photo = fileResponse;
                movie.Country = request.Country;
                movie.Description = request.Description;
                movie.Name = request.Name;
                movie.Rating = request.Rating;
                movie.ReleaseDate = request.ReleaseDate;
                foreach(var item in request.Genres)
                {
                    if (item.Id.HasValue)
                    {
                        var genre = _uow.GetRepository<Genres>().GetByID(item.Id);
                        if(genre != null)
                        {
                            genre.Genre = item.Genre;
                            _uow.GetRepository<Genres>().Update(genre);
                        }
                    }
                    else
                    {
                        var genre = new Genres
                        {
                            Genre = item.Genre,
                            MoviesId = movie.Id
                        };
                        _uow.GetRepository<Genres>().Insert(genre);
                    }
                }
                _uow.GetRepository<Movies>().Update(movie);
                _uow.Save();
                transaction.Commit();
            }
            response.StatusMessage = "Successfully Updated";
            response.StatusCode = ResponseCode.SUCCESSFUL;
            return response;
        }
    }
}
