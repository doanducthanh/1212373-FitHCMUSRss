using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _1212373_BT_RutTrich.Models
{
    public class PostNewsRepository:IPostNewsRepository
    {
        private List<PostNews> listPostNews = new List<PostNews>();
        public IQueryable<PostNews> GetAll()
        {
            return listPostNews.AsQueryable();
        }

        public void Add(PostNews postnews)
        {
            listPostNews.Add(postnews);
        }
    }
}