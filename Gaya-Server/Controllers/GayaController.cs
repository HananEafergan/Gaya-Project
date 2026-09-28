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

        [HttpGet]
        [Route("GetAllOperators")]
        public IActionResult GetAllOperators()
        {
            Result<List<Operator>> res = _gayaService.GetAllOperators();
            return GetActionResult(res);
        }

        [HttpDelete]
        [Route("DeleteOperator/{id}")]
        public IActionResult DeleteOperator(int id)
        {
            Result<string> res = _gayaService.UpdateOperator(id, false);

            return GetActionResult(res);
        }

        [HttpPut]
        [Route("AddOperator/{id}")]
        public IActionResult AddOperator(int id)
        {
            Result<string> res = _gayaService.UpdateOperator(id, true);

            return GetActionResult(res);
        }

        [HttpPost]
        [Route("Calculate")]
        public IActionResult Calculate([FromBody] CalculationRequest request)
        {
            Result<CalculationResponse> res = _gayaService.Calculate(request);
            return GetActionResult(res);
        }

        private IActionResult GetActionResult<T>(Result<T> result) => StatusCode((int)result.StatusCode, result);
    }
}