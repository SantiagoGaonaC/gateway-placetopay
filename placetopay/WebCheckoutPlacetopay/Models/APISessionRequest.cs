using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCheckoutPlacetopay.Models
{
    public class Payment
    {
        public string? reference { get; set; }
        public string? description { get; set; }
        public Amount? amount { get; set; }
    }

    public class Amount
    {
        public string? currency { get; set; }
        public decimal? total { get; set; }
    }

    public class APISessionRequest
    {
        public string? locale { get; set; }
        public APIAuth? auth { get; set; }
        public Payment? payment { get; set; }
        public DateTime? expiration { get; set; }
        public string? returnUrl { get; set; }
        public string? ipAddress { get; set; }
        public string? userAgent { get; set; }
    }
}
