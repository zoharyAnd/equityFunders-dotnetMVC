using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace cFunding.Models
{
    public class category
    {
        [ForeignKey("category")]
        public int id{ get; set; }
        public string categoryname { get; set; }
    }
}