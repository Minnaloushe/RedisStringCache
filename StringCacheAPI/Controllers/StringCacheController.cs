using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using StringCacheAPI.Services;

namespace StringCacheAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StringCacheController : ControllerBase
    {
        private readonly ILogger<StringCacheController> _logger;
        private readonly IConnectionMultiplexer _redis;
        private readonly IDatabase _database;
        private readonly IRedisCacheService _service;

        public StringCacheController(
            ILogger<StringCacheController> logger,
            IRedisCacheService service
            )
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType( typeof( string ), 200 )]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(string id)
        {
            var result = await _service.Get(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Post(string id, string data, int ttl)
        {
            await _service.Set(id, data, ttl);

            return Ok();
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(string id, string data)
        {
            if (!await _service.Update(id, data))
                return NotFound();

            return Ok();
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _service.Delete(id);
            if (!result)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}