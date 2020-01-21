using System;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using System.Web.Mvc;

namespace cFunding.Models
{
    public class project
    {
        [ForeignKey("project")]
        public int id { get; set; }
        public string projectname { get; set; }
        public string projectdescription { get; set; }
        public double projectamounttoraise { get; set; }
        public double projectamountraised { get; set; }
        public DateTimeOffset projectcreationdate{ get; set; }
        public DateTimeOffset projectclosingdate { get; set; }
        public string projectimage1{ get; set; }
        public string projectimage2 { get; set; }
        public string projectimage3 { get; set; }
        public string projectimage4 { get; set; }
        public user fkuser { get; set; }
        public category fkcategory { get; set; }
        public int nbshareordinary { get; set; }
        public int nbsharepreferencial { get; set; }
        public int nbsharenonvoting { get; set; }
        public int nbshareredeemable { get; set; }
        public Boolean validatedProject { get; set; }
        
    }
}