using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Transportation.Data;
using Transportation.Entities.Dto;
using Transportation.Entities.Entity;
using Transportation.Logic;

namespace Transportation.Endpoint.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VehicleController: ControllerBase
    {
        VehicleLogic logic;
        public VehicleController(VehicleLogic logic)
        {
            this.logic = logic;
        }
        [HttpGet]
        public IEnumerable<VehicleViewDto> Get()
        {
            return logic.Read();
        }
        [HttpGet("{id}")]
        public VehicleViewDto Get(string id)
        {
            return logic.Read(id);
        }
        [HttpPost]
        public async Task Post(VehicleCreateUpdateDto dto)
        {
            await logic.Create(dto);
        }
        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            await logic.Delete(id);
        }
        [HttpPut("{id}")]
        public async Task Update([FromBody] VehicleCreateUpdateDto dto, string id)
        {
            await logic.Update(dto, id);
        }
        [HttpPost("trip-suggestions")]
        public IEnumerable<TripSuggestionDto> GetTripSuggestions([FromBody] TripRequestDto dto)
        {
            return logic.GetTripSuggestion(dto.Passengers, dto.DistanceKm);
        }
    }
}
