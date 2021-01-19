using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyApi.Models.Dtos;
using ParkyApi.Models.Instances;
using ParkyApi.Models.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyApi.Controllers
{
    //[Route("api/v{version_apiVersion}/nationalparks")]
    //[ApiExplorerSettings(GroupName = "NationalParks")]
    [ApiVersion("1.0")]
    [Route("api/nationalparks")]
    [ApiController]
    public class NationalParksController : ControllerBase
    {
        private INationalParkRepository _npRepo;
        private readonly IMapper _mapper;

        public NationalParksController(INationalParkRepository npRepo, IMapper mapper)
        {
            this._npRepo = npRepo;
            this._mapper = mapper;
        }

        #region Get
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<NationalParkDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public IActionResult GetNationalParks()
        {
            IEnumerable<NationalPark> nationalParks = _npRepo.GetAll();

            IEnumerable<NationalParkDto> parks = this._mapper.Map<IEnumerable<NationalParkDto>>(nationalParks);

            return Ok(parks);
        }


        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<NationalParkDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        [HttpGet("{id:int}")]
        public IActionResult GetNationalPark(int id)
        {
            try
            {
                var nationalParks = _npRepo.Get(id);
                var nationnalParkdto = _mapper.Map<NationalParkDto>(nationalParks);

                if (nationnalParkdto is null)
                    return NoContent();
                else
                    return Ok(nationnalParkdto);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        #endregion

        #region Post
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<NationalParkDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public IActionResult CreateNationalPark(NationalParkDto nationalParkDto)
        {
            try
            {
                if (nationalParkDto is null)
                    return BadRequest(ModelState);

                if (_npRepo.NationalParkExists(nationalParkDto.Name))
                {
                    ModelState.AddModelError("", "National Park already exists");
                    return StatusCode(400, ModelState);
                }


                var mapped = this._mapper.Map<NationalPark>(nationalParkDto);

                bool added = _npRepo.Add(mapped);

                if (!added)
                {
                    ModelState.AddModelError("", "Failed to create records");
                    return StatusCode(500, ModelState);
                }

                return Ok();

                //return CreatedAtRoute("GetNationalPark", new { NationalParkId = nParkId }, nationalParkDto);
            }
            catch (Exception e)
            {
                return ExceptionError(e);
            }

        }
        #endregion

        #region Patch

        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(IEnumerable<NationalParkDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateNationalPark(NationalParkDto nationalParkDto)
        {
            try
            {
                if (nationalParkDto is null)
                    return BadRequest(ModelState);

                var park = this._mapper.Map<NationalPark>(nationalParkDto);

                if (!_npRepo.Update(park))
                {
                    ModelState.AddModelError("", $"Something went wrong when updating the record {nationalParkDto.Name}");
                    return StatusCode(500, ModelState);
                }

                return NoContent();
            }
            catch (Exception e)
            {
                return ExceptionError(e);
            }

        }

        #endregion

        #region Delete
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<NationalParkDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{id:int}")]
        public IActionResult DeleteNationalPark(int id)
        {
            try
            {
                var nationnalPark = _npRepo.Get(id);

                var nationnalParkdto = _mapper.Map<NationalParkDto>(nationnalPark);

                if (nationnalParkdto is null)
                    return NotFound();

                var park = this._mapper.Map<NationalPark>(nationnalPark);

                if (!_npRepo.Remove(park))
                {
                    ModelState.AddModelError("", $"Something went wrong when deletinng the record {nationnalParkdto.Name}");
                    return StatusCode(500, ModelState);
                }

                return NoContent();
            }
            catch (Exception e)
            {
                return ExceptionError(e);
            }

        }
        #endregion

        #region PrivateError
        private IActionResult ExceptionError(Exception e)
        {
            ModelState.AddModelError("", e.Message);
            return StatusCode(500, ModelState);
        }
        #endregion



    }
}
