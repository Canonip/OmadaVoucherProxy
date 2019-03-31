using System;
using System.Collections.Generic;
using System.Text;

namespace OmadaAPI.Exceptions
{
    class OmadaResponseException : Exception
    {
        public OmadaResponseException(string message) : base(message)
        {
        }
        public OmadaResponseException(string responseMessage, int errorCode) :
            base($"Error {errorCode}: {responseMessage}") { }
    }
}
