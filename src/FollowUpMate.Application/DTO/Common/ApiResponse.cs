using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FollowUpMate.Application.DTO.Common
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public object Errors { get; set; }
        public MetaData Meta { get; set; }
    }

    public class MetaData
    {
        public DateTime Timestamp { get; set; }
        public string RequestId { get; set; }
        public int StatusCode { get; set; }
    }
}
