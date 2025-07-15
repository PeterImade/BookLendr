using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Results
{
    public class Result<T> where T : class
    {
        public bool isSuccess { get; }
        public bool isFailed => !isSuccess;
        public string Error { get; }
        public T? Value { get; }
        public Result(T? value, bool is_success, string error) 
        {
            Value = value;
            Error = error;
            isSuccess = is_success;
        }
        public Result() { }
        public static Result<T> Success(T value) => new Result<T>(value, true, string.Empty);
        public static Result<T> Failed(string error) => new Result<T>(default, false, error);
    }

    public class Result
    {
        public bool isSuccess { get; }
        public bool isFailed => !isSuccess;
        public string? Error { get; }

        public Result(bool is_success, string? error)
        {
            isSuccess = is_success;
            Error = error;
        }

        public static Result Success() => new(true, string.Empty);
        public static Result Failed(string? error) => new(false, error);
    }
}
