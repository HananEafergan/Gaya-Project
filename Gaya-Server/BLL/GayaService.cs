using Gaya_Server.DAL;
using Gaya_Server.Models;
using System.Net;

namespace Gaya_Server.BLL
{
    public class GayaService
    {
        private readonly GayaDAL _gayaDAL;

        public GayaService(GayaDAL gayaDAL)
        {
            _gayaDAL = gayaDAL;
        }

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

        private static Result<T> CatchException<T>(Exception ex)
        {
            return new Result<T>
            {
                Value = default,
                StatusCode = HttpStatusCode.InternalServerError,
                Message = "An internal server error occurred",
            };
        }
    }
}
