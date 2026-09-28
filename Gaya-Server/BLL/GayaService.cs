using Gaya_Server.DAL;
using Gaya_Server.Models;
using System.Net;

namespace Gaya_Server.BLL
{
    /// <summary>
    /// Applies operator visibility rules and performs calculations.
    /// </summary>
    public class GayaService
    {
        private readonly GayaDAL _gayaDAL;
        private readonly ILogger<GayaService> _logger;

        public GayaService(GayaDAL gayaDAL, ILogger<GayaService> logger)
        {
            _gayaDAL = gayaDAL;
            _logger = logger;
        }

        /// <summary>
        /// Returns the names of operators that can be used in a calculation.
        /// </summary>
        public Result<string[]> GetActiveOperators()
        {
            Result<string[]> result = new();
            try
            {

                result.Value = _gayaDAL.GetActiveOperators();
                result.StatusCode = HttpStatusCode.OK;
                result.Message = "Operators retrieved successfully.";
            }
            catch (Exception ex)
            {
                result = CatchException<string[]>(ex);
            }
            return result;
        }


        /// <summary>
        /// Returns every operator so a hidden one can be shown again.
        /// </summary>
        public Result<List<Operator>> GetAllOperators()
        {
            Result<List<Operator>> result = new();
            try
            {
                result.Value = _gayaDAL.GetAllOperators();
                result.StatusCode = HttpStatusCode.OK;
                result.Message = "Operators retrieved successfully.";
            }
            catch (Exception ex)
            {
                result = CatchException<List<Operator>>(ex);
            }
            return result;
        }

        /// <summary>
        /// Sets whether an existing operator is shown. <paramref name="isActive"/> true shows it; false hides it.
        /// </summary>
        public Result<string> UpdateOperator(int id, bool isActive)
        {
            Result<string> result = new();
            try
            {
                _gayaDAL.UpdateOperator(id, isActive);
                result.StatusCode = HttpStatusCode.OK;
                result.Message = "Operators list updated successfully.";
            }
            catch (Exception ex)
            {
                result = CatchException<string>(ex);
            }
            return result;
        }

        /// <summary>
        /// Calculates the request, stores it, and returns the new result separately from the three operations saved before it.
        /// The monthly count includes the operation just stored.
        /// </summary>
        public Result<CalculationResponse> Calculate(CalculationRequest request)
        {
            Result<CalculationResponse> result = new();
            try
            {

                List<OperationHistory> lastOperations;
                int monthlyOperationCount;
                double fieldA = 0, fieldB = 0;
                string operationResult = string.Empty;

                if (!_gayaDAL.IsOperatorActive(request.Operator))
                {
                    result.StatusCode = HttpStatusCode.BadRequest;
                    result.Message = "Invalid operator.";
                    return result;
                }

                if (request.Operator != "Concat" &&
                    !TryParseNumbers(request, out fieldA, out fieldB))
                {
                    result.StatusCode = HttpStatusCode.BadRequest;
                    result.Message = "Invalid parameters.";
                    return result;
                }

                else if (request.Operator == "Divide" && fieldB == 0)
                {
                    result.StatusCode = HttpStatusCode.BadRequest;
                    result.Message = "Division by zero is not allowed.";
                    return result;
                }

                switch (request.Operator)
                {
                    case "Add":
                        operationResult = (fieldA + fieldB).ToString();
                        break;
                    case "Subtract":
                        operationResult = (fieldA - fieldB).ToString();
                        break;
                    case "Multiply":
                        operationResult = (fieldA * fieldB).ToString();
                        break;
                    case "Divide":
                        operationResult = (fieldA / fieldB).ToString();
                        break;
                    case "Concat":
                        operationResult = $"{request.FieldA}{request.FieldB}";
                        break;
                }

                var operationHistory = new OperationHistory
                {
                    Operator = request.Operator,
                    FieldA = request.FieldA,
                    FieldB = request.FieldB,
                    Result = operationResult,
                    CreatedAt = DateTime.Now
                };

                lastOperations = _gayaDAL.GetLastOperations(request.Operator);
                
                _gayaDAL.SaveOperation(operationHistory);

                monthlyOperationCount = _gayaDAL.GetMonthlyOperationCount(request.Operator);

                result.Value = new CalculationResponse
                {
                    Result = $"Result: {operationResult}",
                    LastOperations = lastOperations,
                    MonthlyOperationCount = monthlyOperationCount
                };

                result.StatusCode = HttpStatusCode.OK;
                result.Message = "Calculation completed successfully.";
            }
            catch (Exception ex)
            {
                result = CatchException<CalculationResponse>(ex);
            }
            return result;
        }

        private static bool TryParseNumbers(CalculationRequest request, out double fieldA, out double fieldB)
        {
            bool isFieldAValid = double.TryParse(request.FieldA, out fieldA);
            bool isFieldBValid = double.TryParse(request.FieldB, out fieldB);

            return isFieldAValid && isFieldBValid;
        }

        private  Result<T> CatchException<T>(Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing the request.");
            return new Result<T>
            {
                Value = default,
                StatusCode = HttpStatusCode.InternalServerError,
                Message = "An internal server error occurred",
            };
        }
    }
}
