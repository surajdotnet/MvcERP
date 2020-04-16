using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Entity.CustomHelper
{
  public static  class CustomHelpers
    {
        public static string ImageUrlFor(this HtmlHelper helper, string contentUrl)
        {
            // Put some caching logic here if you want it to perform better
            UrlHelper urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
            if (!File.Exists(helper.ViewContext.HttpContext.Server.MapPath(contentUrl)))
            {
                return urlHelper.Content("~/content/images/none.png");
            }
            else
            {
                return urlHelper.Content(contentUrl);
            }
        }
    }
}
