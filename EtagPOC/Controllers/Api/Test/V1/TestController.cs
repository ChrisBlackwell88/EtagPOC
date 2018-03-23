using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using CacheCow.Server.CacheControlPolicy;
using CacheCow.Server.CacheRefreshPolicy;

namespace EtagPOC.Controllers.Api.Test.V1
{
    public class TestController : ApiController
    {
        /// <summary>
        /// unique no cache
        /// </summary>
        /// <returns>A string</returns>
        [HttpGet]
        [ResponseType(typeof(string))]
        // This forces the server to not provide any caching by refreshing its cache table immediately (0 sec)
        [HttpCacheRefreshPolicy(0)]
        // This forces the client (browser) to not cache any data returned from the server (even if ETag is present) by setting the time-out to 0 and no-cache to true.
        [HttpCacheControlPolicy(true, 0, true)]
        ////[HttpCacheControlPolicy(true, 100)]
        public async Task<IHttpActionResult> NoCache()
        {
            try
            {     
                //// Should return a unique value that cant be cached (unique date time)
                var dateTime = DateTime.Now.ToString();
                var response = await Task.Run(() => "Should not cache - " + dateTime).ConfigureAwait(false);
                return this.Ok(response);
            }
            catch (Exception ex)
            {               
                return this.BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// A cacheable hard coded value should return 304 not modified
        /// </summary>
        /// <returns>A string</returns>
        [HttpGet]
        [ResponseType(typeof(string))]       
        public async Task<IHttpActionResult> HardCoded()
        {
            try
            {
                // this is a static value that will never change                
                var response = await Task.Run(() => "HardCoded").ConfigureAwait(false);
                return this.Ok(response);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// A cacheable but expirable in 2 secs
        /// </summary>
        /// <returns>A string</returns>
        [HttpGet]
        [ResponseType(typeof(string))]
        [HttpCacheRefreshPolicy(2)]
        public async Task<IHttpActionResult> ShouldRefreshCoded()
        {
            try
            {
                // this value is only cachable for a specific duration, then the etag should expire
                var dateTime = DateTime.Now.ToUniversalTime();
                var response = await Task.Run(() => "Should Refresh in 2 secs - " + dateTime).ConfigureAwait(false);
                return this.Ok(response);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
    }
}
