﻿using System.Web;
using System.Web.Mvc;

namespace MusicAPI.Mashup
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}