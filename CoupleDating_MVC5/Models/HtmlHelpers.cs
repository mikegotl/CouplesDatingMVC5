using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CoupleDating_MVC5.Models
{
    public static class HtmlHelpers
    {
        public static MvcHtmlString Image(this HtmlHelper helper, string src, string altText, string height)
        {
            var builder = new TagBuilder("img");
            builder.MergeAttribute("src", src);
            builder.MergeAttribute("alt", altText);
            builder.MergeAttribute("height", height);
            return MvcHtmlString.Create(builder.ToString(TagRenderMode.SelfClosing));
        }

        public static HtmlHelperAdapter<TModel> Adapt<TModel>(this HtmlHelper<TModel> helper)
        {
            return new HtmlHelperAdapter<TModel>(helper);
        }

        public class HtmlHelperAdapter<TModel>
        {
            private readonly HtmlHelper<TModel> _helper;

            public HtmlHelperAdapter(HtmlHelper<TModel> helper)
            {
                _helper = helper;
            }

            public HtmlHelper<TNewModel> For<TNewModel>()
            {
                return new HtmlHelper<TNewModel>(_helper.ViewContext, new ViewPage(), _helper.RouteCollection);
            }
        }

    }
}