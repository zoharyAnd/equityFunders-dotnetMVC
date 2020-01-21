using System;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations.Schema;

namespace cFunding.Models
{
    public class question
    {
        [ForeignKey("question")]
        public int id { get; set; }
        public user fkuser { get; set; }
        public DateTimeOffset questiondate{ get; set; }
        public String questionmessage { get; set; }

        public Boolean validatedQuestion { get; set; }
        
    }
}