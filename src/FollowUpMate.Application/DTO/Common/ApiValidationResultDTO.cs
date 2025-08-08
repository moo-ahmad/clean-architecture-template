using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FollowUpMate.Application.DTO.Common
{
    public class ApiValidationResultDTO
    {
        public bool IsValid { get; set; }
        public string MessageCode { get; set; }
        public object Data { get; set; }
        public string Message { get; set; }

    }
}
