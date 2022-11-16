using AdvertApi.Models;
using AdvertApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace AdvertApi.Controllers
{
    [ApiController]
    [Route("api/v1/adverts")]
    public class AdvertsController : ControllerBase
    {
        private readonly IAdvertStorageService _advertStorageService;

        public AdvertsController(IAdvertStorageService advertStorageService)
        {
            _advertStorageService = advertStorageService;
        }

        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(404)]
        [ProducesResponseType(201, Type = typeof(CreateAdvertResponse))]
        public async Task<IActionResult> Create(CreateAdvertRequest model)
        {
            string recordId;
            try
            {
                recordId = await _advertStorageService.Add(model);
            }
            // TODO: I know exceptions should be handled in a centralized way using middleware for example
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return Accepted(new CreateAdvertResponse() { Id = recordId });
        }

        [HttpPut]
        [Route("Confirm")]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Confirm(ConfirmAdvertRequest model)
        {
            try
            {
                await _advertStorageService.Confirm(model);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}