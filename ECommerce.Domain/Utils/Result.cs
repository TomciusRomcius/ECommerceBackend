using System.ComponentModel.DataAnnotations;

namespace ECommerce.Domain.Utils
{
    public class Result<T>
    {
        public T? ReturnResult { init; get; }
        public IEnumerable<ResultError> Errors { init; get; }

        public Result(T? returnResult, IEnumerable<ResultError> errors)
        {
            ReturnResult = returnResult;
            Errors = errors;
        }

        public Result(T? returnResult)
        {
            ReturnResult = returnResult;
            Errors = [];
        }
    }
}