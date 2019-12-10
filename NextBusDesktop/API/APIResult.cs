using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace NextBusDesktop.API
{
    public class APIResult<T>
    {
        public HttpStatusCode Status { get; private set; }
        public T Data { get; private set; }

        public APIResult(T data, HttpStatusCode status)
        {
            Data = data;
            Status = status;
        }

        private APIResult(IRestResponse<T> response)
        {
            Data = response.Data;
            Status = response.StatusCode;
        }

        public static APIResult<T> Parse(IRestResponse<T> response) => new APIResult<T>(response);

        public override string ToString() => Status.ToString();
    }
}
