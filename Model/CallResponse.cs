using Microsoft.AspNetCore.Http;

namespace smarthome.Model
{
    public class CallResponse<T>
    {
        public string Error {get;set;}
        public int statusCode {get;set;}
        public T data {get;set;}
    }
}