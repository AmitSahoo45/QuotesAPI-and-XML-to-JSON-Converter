using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Services.Description;
using QuotesAPI.Models;

namespace QuotesAPI.Controllers
{
    public class QuotesController : ApiController
    {
        private readonly PracticeDBEntities db = new PracticeDBEntities();

        // GET: api/Quotes
        [HttpGet]
        public IHttpActionResult GetQuotes(int page = 1, int limit = 4, string search = "")
        {
            try
            {
                int index = (page - 1) * limit;

                IQueryable<Quote> quotesQuery = db.Quotes;

                if (!string.IsNullOrEmpty(search))
                {
                    quotesQuery = quotesQuery.Where(q =>
                        q.Title.Contains(search) ||
                        q.Author.Contains(search) ||
                        q.QuoteText.Contains(search) ||
                        q.Category.Contains(search));
                }

                int total = quotesQuery.Count();

                var quotes = quotesQuery.OrderBy(q => q.Id)
                                        .Skip(index)
                                        .Take(limit)
                                        .ToList();

                var viewModel = new QuoteViewModel
                {
                    Quotes = quotes,
                    CurrentPage = page,
                    NumberOfPages = (int)Math.Ceiling((double)total / limit)
                };

                return Ok(viewModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/Quotes/5
        [ResponseType(typeof(Quote))]
        public IHttpActionResult GetQuote(int id)
        {
            try
            {
                Quote quote = db.Quotes.Find(id);

                if (quote == null)
                    return Content(HttpStatusCode.NotFound, "Quote not found");

                return Ok(quote);
            }
            catch (Exception)
            {
                return BadRequest("Something went wrong");
            }
        }

        // PUT: api/Quotes/5
        [HttpPatch]
        [ResponseType(typeof(void))]
        public IHttpActionResult PatchQuote(int id, [FromBody] Quote quote)
        {
            if (quote is null)
                return BadRequest("Value cannot be null");

            if (string.IsNullOrWhiteSpace(quote.Title) ||
                string.IsNullOrWhiteSpace(quote.Author) ||
                string.IsNullOrWhiteSpace(quote.QuoteText))
                return BadRequest("Value cannot be empty");

            var quoteInDb = db.Quotes.Find(id);

            if (quoteInDb is null)
                return Content(HttpStatusCode.NotFound, "The related Quote wasn't found in the database");

            quoteInDb.Title = quote.Title;
            quoteInDb.Author = quote.Author;
            quoteInDb.QuoteText = quote.QuoteText;
            quoteInDb.Category = quote.Category;

            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok("Quote updated successfully");
        }

        // POST: api/Quotes
        [ResponseType(typeof(Quote))]
        public IHttpActionResult PostQuote([FromBody] Quote quote)
        {
            try
            {
                if (quote == null)
                    return BadRequest("Value cannot be null");

                if (string.IsNullOrWhiteSpace(quote.Title) || string.IsNullOrWhiteSpace(quote.Author) || string.IsNullOrWhiteSpace(quote.QuoteText))
                    return BadRequest("Value cannot be empty");

                if (!ModelState.IsValid)
                    return BadRequest("Value cannot be empty");

                db.Quotes.Add(quote);
                db.SaveChanges();

                return Ok("Quote added successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Quotes/5
        [ResponseType(typeof(Quote))]
        public IHttpActionResult DeleteQuote(int id)
        {
            Quote quote = db.Quotes.Find(id);
            if (quote == null)
                return NotFound();

            db.Quotes.Remove(quote);
            db.SaveChanges();

            return Ok("Quote deleted successfully");
        }

        [HttpGet]
        [Route("api/Quotes/Test/{id}")]
        public IHttpActionResult Test(int id)
        {
            return Ok("API is working " + id);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool QuoteExists(int id)
        {
            return db.Quotes.Count(e => e.Id == id) > 0;
        }
    }
}