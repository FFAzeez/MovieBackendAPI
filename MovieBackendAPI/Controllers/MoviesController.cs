using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MovieBackendAPI.Business.Movie.Commands;
using MovieBackendAPI.Business.Movie.Queries;
using MovieBackendAPI.Domain.BindingModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MovieBackendAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class MoviesController : BaseController
    {
        private readonly ILogger<MoviesController> _logger;
        private readonly string _env;
        private readonly IMediator _mediator;
        public MoviesController(ILogger<MoviesController> logger, IWebHostEnvironment environment, IMediator mediator)
        {
            _logger = logger;
            _env = environment.EnvironmentName;
            _mediator = mediator;
        }


        [ProducesResponseType(typeof(ServiceResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ServiceResponse), (int)HttpStatusCode.OK)]
        [HttpPost]
        public async Task<IActionResult> AddMoviesCommand([FromForm] AddMoviesCommand model)
        {
            _logger.LogInformation($"AddMoviesCommand - Details", JsonConvert.SerializeObject(model));
            try
            {
                if (model == null || !ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var response = await _mediator.Send(model);
                _logger.LogInformation($"AddMovies successful, response ", JsonConvert.SerializeObject(response));
                return Ok(response);
            }
            catch (Exception ex)
            {
                return HandleException(ex, _logger, _env);
            }
        }

        [ProducesResponseType(typeof(ServiceResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ServiceResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ServiceResponse), (int)HttpStatusCode.OK)]
        [HttpPut]
        public async Task<IActionResult> UpdateMoviesCommand([FromForm] UpdateMoviesCommand model)
        {
            _logger.LogInformation($"UpdateMoviesCommand - Details", JsonConvert.SerializeObject(model));
            try
            {
                if (model == null || !ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var response = await _mediator.Send(model);
                _logger.LogInformation($"UpdateMovies successful, response ", JsonConvert.SerializeObject(response));
                return Ok(response);
            }
            catch (Exception ex)
            {
                return HandleException(ex, _logger, _env);
            }
        }

        [ProducesResponseType(typeof(ServiceResponse<MovieResponse>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ServiceResponse), (int)HttpStatusCode.OK)]
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetMoviesByIdQuery([FromRoute] long Id)
        {
            _logger.LogInformation($"AddMoviesCommand - Details");
            try
            {
                var response = await _mediator.Send(new GetMoviesByIdQuery() { Id = Id });
                _logger.LogInformation($"AddMovies successful, response ", JsonConvert.SerializeObject(response));
                return Ok(response);
            }
            catch (Exception ex)
            {
                return HandleException(ex, _logger, _env);
            }
        }

        [ProducesResponseType(typeof(ServiceResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ServiceResponse), (int)HttpStatusCode.OK)]
        [HttpGet]
        public async Task<IActionResult> GetAllMovieQuery([FromQuery] GetAllMovieQuery model)
        {
            _logger.LogInformation($"GetAllMovieQuery - Details", JsonConvert.SerializeObject(model));
            try
            {
                if (model == null || !ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var response = await _mediator.Send(model);
                _logger.LogInformation($"GetAllMovie successful, response ", JsonConvert.SerializeObject(response));
                return Ok(response);
            }
            catch (Exception ex)
            {
                return HandleException(ex, _logger, _env);
            }
        }

    }
}
