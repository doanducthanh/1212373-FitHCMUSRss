using _1212373_BT_RutTrich.Models;
using AngleSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
namespace _1212373_BT_RutTrich.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class FitExtractionController : ApiController
    {
        private static readonly IPostNewsRepository _repo = new PostNewsRepository();

        [Route("api/fithcmus/home_news")]
        public async Task<IEnumerable<PostNews>> GetAllPostNews()
        {
            Request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/rss+xml"));
            // Setup the configuration to support document loading
            var config = new Configuration().WithDefaultLoader();
            var address = "http://www.fit.hcmus.edu.vn/vn/Default.aspx?tabid=36";
            var document = await BrowsingContext.New(config).OpenAsync(address);
            // Asynchronously get the document in a new context using the configuration
            //var document = await BrowsingContext.New(config).OpenAsync(address);
            // This CSS selector gets the desired content
            
            var aPostTitle = "td.post_title > a";
            var tdDayMonth = "td.day_month";
            var tdYear = "td.post_year";
            // Perform the query to get all cells with the content

            var cells_aPostTitle = document.QuerySelectorAll(aPostTitle);
            var cells_spanDayMonth = document.QuerySelectorAll(tdDayMonth);
            var cells_spanYear = document.QuerySelectorAll(tdYear);

            // We are only interested in the text - select it with LINQ

            List<String> listPostTitle = cells_aPostTitle.Select(m => m.TextContent).ToList();
            List<String> listLinks = cells_aPostTitle.Select(m => m.OuterHtml).ToList();
            List<String> listDayMonth = cells_spanDayMonth.Select(m => m.TextContent).ToList();
            List<String> listYear = cells_spanYear.Select(m => m.TextContent).ToList();

            int countPostNews = listPostTitle.Count();
            int index_DayMonth = -2;
            for (int i = 0; i < countPostNews; i++)
            {
                index_DayMonth += 2;

                string _title = listPostTitle[i].Replace("\n", "").Replace("\t", "");
                string [] splitLink = listLinks[i].Split('"');
                string day = listDayMonth[index_DayMonth].Replace("\n", "").Replace("\t", "");
                string month = listDayMonth[index_DayMonth +1].Replace("\n", "").Replace("\t", ""); ;
                string year = listYear[i].Replace("\n", "").Replace("\t", "");

                string s = month + "/" + day + "/" + year;
                DateTime date_month_year = DateTime.Parse(s);
                //string s = day + "/" + month + "/" + year;
                //DateTime date_month_year = DateTime.ParseExact(s, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                PostNews temp = new PostNews()
                {
                    title = _title,
                    link = "http://www.fit.hcmus.edu.vn/vn/" + splitLink[1],
                    postedDate = date_month_year
                   
                };

                _repo.Add(temp);
            }

            return _repo.GetAll();
        }
    }
}