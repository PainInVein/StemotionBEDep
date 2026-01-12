using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Application.DTO.ResponseDTOs
{
    public class ResponseDTO<T>
    {
        public string Message { get; set; } = string.Empty;
        public bool IsSuccess { get; set; }
        public T? Result { get; set; }
        // public T? Errors { get; set; }

        public ResponseDTO()
        {
        }
        public ResponseDTO(string message, bool isSuccess, T? result)
        {
            Message = message;
            IsSuccess = isSuccess;
            Result = result;
        }
    }
}
