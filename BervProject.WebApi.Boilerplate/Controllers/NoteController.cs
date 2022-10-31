using BervProject.WebApi.Boilerplate.Entities;
using BervProject.WebApi.Boilerplate.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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

        [HttpPost("createTable")]
        public async Task<IActionResult> CreateTable()
        {
            await _noteTable.CreateTableAsync();
            return Ok(true);
        }

        [HttpPost("upsert")]
        public async Task<IActionResult> UpsertNote([FromBody] Note note)
        {
            await _noteTable.UpsertAsync(note);
            return Ok(true);
        }

        [HttpGet("get")]
        public async Task<IActionResult> Get([FromQuery] string partitionKey, [FromQuery] string rowKey)
        {
            var result = await _noteTable.GetAsync(partitionKey, rowKey);
            return Ok(result);
        }
    }
}
