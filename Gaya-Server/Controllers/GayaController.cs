using Gaya_Server.BLL;
using Gaya_Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace Gaya_Server.Controllers
{
    /// <summary>
    /// HTTP API for showing or hiding operators and running calculations.
    /// </summary>
    [ApiController]
    [Route("api/GayaProject")]
    public class GayaController : ControllerBase
    {
        private readonly GayaService _gayaService;

        public GayaController(GayaService gayaService) => _gayaService = gayaService;


        /// <summary>
        /// Returns the names of operators that are currently shown.
        /// </summary>
        [HttpGet]
        [Route("GetActiveOperators")]
        public IActionResult GetActiveOperators()
        {
            Result<string[]> res = _gayaService.GetActiveOperators();
            return GetActionResult(res);

        }

        /// <summary>
        /// Returns every operator, including hidden ones, for the edit list.
        /// </summary>
        [HttpGet]
        [Route("GetAllOperators")]
        public IActionResult GetAllOperators()
        {
            Result<List<Operator>> res = _gayaService.GetAllOperators();
            return GetActionResult(res);
        }

        /// <summary>
        /// Hides an existing operator. The row stays in the database.
        /// </summary>
        [HttpDelete]
        [Route("DeleteOperator/{id}")]
        public IActionResult DeleteOperator(int id)
        {
            Result<string> res = _gayaService.UpdateOperator(id, false);

            return GetActionResult(res);
        }

        /// <summary>
        /// Shows an existing operator again. Does not insert a new row.
        /// </summary>
        [HttpPut]
        [Route("AddOperator/{id}")]
        public IActionResult AddOperator(int id)
        {
            Result<string> res = _gayaService.UpdateOperator(id, true);

            return GetActionResult(res);
        }

        /// <summary>
        /// Runs one calculation and returns its result, the three previous operations, and this month's count.
        /// </summary>
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