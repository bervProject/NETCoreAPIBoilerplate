using BervProject.WebApi.Boilerplate.Entities;
using BervProject.WebApi.Boilerplate.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BervProject.WebApi.Boilerplate.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class NoteController : ControllerBase
    {
        private readonly IAzureTableStorageService<Note> _noteTable;
        public NoteController(IAzureTableStorageService<Note> noteTable)
        {
            _noteTable = noteTable;
        }

        /// <summary>
        /// Create Azure Table Storage
        /// </summary>
        /// <returns></returns>
        [HttpPost("createTable")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]

        public async Task<IActionResult> CreateTable()
        {
            await _noteTable.CreateTableAsync();
            return Ok(true);
        }

        /// <summary>
        /// Upsert Note
        /// </summary>
        /// <param name="note"></param>
        /// <returns></returns>
        [HttpPost("upsert")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpsertNote([FromBody] Note note)
        {
            await _noteTable.UpsertAsync(note);
            return Ok(true);
        }

        /// <summary>
        /// Get Note
        /// </summary>
        /// <param name="partitionKey"></param>
        /// <param name="rowKey"></param>
        /// <returns></returns>
        [HttpGet("get")]
        [ProducesResponseType(typeof(Note), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromQuery] string partitionKey, [FromQuery] string rowKey)
        {
            var result = await _noteTable.GetAsync(partitionKey, rowKey);
            return Ok(result);
        }
    }
}
