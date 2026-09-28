using Gaya_Server.BLL;
using Gaya_Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace Gaya_Server.Controllers
{
    [ApiController]
    [Route("api/GayaProject")]
    public class GayaController : ControllerBase
    {
        private readonly GayaService _gayaService;

        public GayaController(GayaService gayaService) => _gayaService = gayaService;


        [HttpGet]
        [Route("GetActiveOperators")]
        public IActionResult GetActiveOperators()
        {
            Result<string[]> res = _gayaService.GetActiveOperators();
            return GetActionResult(res);

        }

        private IActionResult GetActionResult<T>(Result<T> result) => StatusCode((int)result.StatusCode, result);
    }
}