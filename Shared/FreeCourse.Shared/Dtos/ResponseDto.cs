using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FreeCourse.Shared.Dtos
{
    public class ResponseDto<T>
    {
        [JsonIgnore]
        public int StatusCode{ get; set; }
        public IList<string> Error{ get; set; }
        public T Data { get; set; }
        [JsonIgnore]
        public bool IsSuccessful{ get; set; }
        public static ResponseDto<T> Success(T data,int statusCode)
        {
            return new ResponseDto<T>() {  StatusCode = statusCode, Data = data, IsSuccessful = true };
        }
        public static ResponseDto<T> Success(int statusCode)
        {
            return new ResponseDto<T>() { StatusCode = statusCode,IsSuccessful=true };
        }
        public static ResponseDto<T> Fail(string error,int statusCode)
        {
            return new ResponseDto<T>() { Error = {error }, StatusCode = statusCode,IsSuccessful=false };
        }

        public static ResponseDto<T> Fail(IList<string> errors, int statusCode)
        {
            return new ResponseDto<T>() { Error = errors, StatusCode = statusCode, IsSuccessful = false };
        }
    }
}
