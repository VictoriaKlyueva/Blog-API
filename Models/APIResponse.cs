using System.Net;

namespace BackendLaboratory.Models
{
    public class APIResponse
    {
        public HttpStatusCode StatusCode { get; set; } 
        public bool IsSucesses { get; set; }
        public List<string> ErrorMessages { get; set; }
        public object Result { get; set; }

        public APIResponse() 
        {
            ErrorMessages = new List<string>();
        }
    }
}
