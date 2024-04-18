using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuotesAPI.Models
{
    public class QuoteViewModel
    {
        public IEnumerable<Quote> Quotes { get; set; }
        public int CurrentPage { get; set; }
        public int NumberOfPages { get; set; }
    }

}