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
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MovieBackendAPI.Business.Movie.Commands
{
    public class AddMoviesCommandHandler : IRequestHandler<AddMoviesCommand, ServiceResponse>
    {
        private readonly IUnitOfWork _uow;
        private readonly IFileUpload _file;
        private readonly IMapper _mapper;
        public AddMoviesCommandHandler(IUnitOfWork uow, IFileUpload file, IMapper mapper)
        {
            _uow = uow;
            _file = file;
            _mapper = mapper;
        }

        public async Task<ServiceResponse> Handle(AddMoviesCommand request, CancellationToken cancellationToken)
        {
            var response = new ServiceResponse();
            var fileResponse = await _file.SaveImageAsync(request.PhotoFile);
            if (string.IsNullOrEmpty(fileResponse))
            {
                response.StatusMessage = "Unable to create file, please try again later.";
                response.StatusCode = ResponseCode.BadRequest;
                return response;
            }
            using (var transaction = _uow.BeginTransaction())
            {
                var movies = _mapper.Map<Movies>(request);
                movies.Photo = fileResponse;
                _uow.GetRepository<Movies>().Insert(movies);
                _uow.Save();
                transaction.Commit();
            }
            response.StatusMessage = "Successfully Created";
            response.StatusCode = ResponseCode.SUCCESSFUL;
            return response;
        }
    }
}
