using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Horseplay.Static
{
    public static class CustomHelper
    {
        public static IHtmlString CreatePrompter(string text, string color) {
            string str = "<div class=\"prompter my-prompter\"><ul><li><button type = \"button\" class=\"btn bg-"+color+"\">?</button><ul><li><table class=\"info-table table-"+ color+"\"><tr><th><div class=\"info-header\"><div class=\"small-circle circle-"+color+"\">?</div><p class=\""+color+"\">Wskazówka</p></div></th></tr><tr><td><p>"+text+"</p></td></tr></table></li></ul></li></ul></div>";
            return new HtmlString(str);
        }

        public static IHtmlString CreateGreeter(string text, string color)
        {
            string str = "<table class=\"info-table table-" + color + "\"><tr><th><div class=\"info-header\"><div class=\"small-circle circle-" + color + "\">?</div><p class=\"" + color + "\">Wskazówka</p></div></th></tr><tr><td><p>" + text + "</p></td></tr></table>";
            return new HtmlString(str);
        }

    }
}