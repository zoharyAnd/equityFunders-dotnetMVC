using System.Web;
using System.Web.Mvc;

namespace cFunding.Models
{
    public class tinyMCE
    {
        // // This attributes allows your HTML Content to be sent up
        [AllowHtml]
        public string HtmlContent { get; set; } 

        public tinyMCE()
        {

        }
        
    }
}