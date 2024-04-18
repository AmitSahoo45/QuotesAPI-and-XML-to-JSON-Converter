using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QuotesAPI.Models
{
    public class QuotesExtend
    {
        [DisplayName("Quote Title")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Quote Title is required")]
        [MinLength(3, ErrorMessage = "Quote Title must be at least 3 characters long")]
        public string Title { get; set; }

        [StringLength(50)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Author is required")]
        [MinLength(3, ErrorMessage = "Author must be at least 3 characters long")]
        public string Author { get; set; }

        [MinLength(10, ErrorMessage = "Quote Text must be at least 10 characters long")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Quote Text is required")]
        public string QuoteText { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Category is required")]
        public string Category { get; set; }
    }

    [MetadataType(typeof(QuotesExtend))]
    public partial class Quote
    {
    }
}