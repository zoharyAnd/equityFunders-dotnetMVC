using System;
using System.Runtime.Serialization;

namespace cFunding.Models
{
    public class contactus
    {
        public int id { get; set; }

        public user fkuser { get; set; }

        public String sendername { get; set; }

        public String senderemail { get; set; }

        public String mailsubject { get; set; }

        public String mailmessage { get; set; }
    }
}