using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCheckoutPlacetopay.Models
{
    namespace WebCheckoutPlacetopay.Models
    {
        public class APIResponse
        {
            public APIResponseStatus status { get; set; }
            public int requestId { get; set; }
            public string processUrl { get; set; }
        }

        public class APIResponseStatus
        {
            public string status { get; set; }
            public string reason { get; set; }
            public string message { get; set; }
            public string date { get; set; }
            public string requestId { get; set; }
        }
    }
}