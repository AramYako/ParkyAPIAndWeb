using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyApi.Models.Dtos;
using ParkyApi.Models.Dtos.Trail;
using ParkyApi.Models.Instances;
using ParkyApi.Models.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyApi.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [ApiController]
    //[Route("api/v{version_apiVersion}/trails")]
    //[ApiExplorerSettings(GroupName = "ParkyTrail")]
    public class TrailsController : ControllerBase
    {
        private ITrailRepository _trailRepo;
        private readonly IMapper _mapper;

        public TrailsController(ITrailRepository trailRepo, IMapper mapper)
        {
            this._trailRepo = trailRepo;
            this._mapper = mapper;
        }

        #region Get
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TrailDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public IActionResult GetTrails()
        {
            IEnumerable<TrailDto> Trails = _trailRepo.GetTrails();

            return Ok(Trails);
        }


        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TrailDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "Admin")]
        [HttpGet("{id:int}")]
        public IActionResult GetTrails(int id)
        {
            try
            {
                TrailDto Trails = _trailRepo.GetTrail(id);


                if (Trails is null)
                    return NoContent();
                else
                    return Ok(Trails);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        #endregion

        #region Post
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TrailDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public IActionResult CreateTrail(TrailCreateDto TrailDto)
        {
            try
            {
                if (TrailDto is null)
                    return BadRequest(ModelState);

                if (_trailRepo.TrailExists(TrailDto.Name))
                {
                    ModelState.AddModelError("", "National Park already exists");
                    return StatusCode(400, ModelState);
                }

                var trail = this._mapper.Map<Trail>(TrailDto);
                bool added = _trailRepo.Add(trail);

                if (!added)
                {
                    ModelState.AddModelError("", "Failed to create records");
                    return StatusCode(500, ModelState);
                }

                return Ok();

                //return CreatedAtRoute("GetTrail", new { TrailId = nParkId }, TrailDto);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return StatusCode(500, ModelState);
            }

        }
        #endregion

        #region Patch

        [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(IEnumerable<TrailDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPatch]
        public async Task<IActionResult> UpdateTrail(TrailUpdateDto TrailDto)
        {
            try
            {
                if (TrailDto is null)
                    return BadRequest(ModelState);

                bool exists = await _trailRepo.Exists(TrailDto.TrailId);

                if (exists)
                {
                    ModelState.AddModelError("", "National Park does not exist");
                    return StatusCode(400, ModelState);
                }

                var trail = this._mapper.Map<Trail>(TrailDto);
                if (!_trailRepo.Update(trail))
                {
                    ModelState.AddModelError("", $"Something went wrong when updating the record {TrailDto.Name}");
                    return StatusCode(500, ModelState);
                }

                return NoContent();
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return StatusCode(500, ModelState);
            }

        }

        #endregion

        #region Delete
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TrailDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteTrail(int id)
        {
            try
            {
                TrailDto trailDto = this._mapper.Map<TrailDto>(_trailRepo.Get(id));

                if (trailDto is null)
                    return NotFound();

                var trail = this._mapper.Map<Trail>(trailDto);
                if (!_trailRepo.Remove(trail))
                {
                    ModelState.AddModelError("", $"Something went wrong when deletinng the record {trailDto.Name}");
                    return StatusCode(500, ModelState);
                }

                return NoContent();
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return StatusCode(500, ModelState);
            }

        }
        #endregion

    }
}
