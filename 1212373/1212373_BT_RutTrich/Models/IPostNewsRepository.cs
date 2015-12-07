using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _1212373_BT_RutTrich.Models
{
    public interface IPostNewsRepository
    {
        IQueryable<PostNews> GetAll();
        void Add(PostNews postnews);
    }
}