using System;
using System.Runtime.Serialization;

namespace cFunding.Models
{
    public class answer
    {
        public int id { get; set; }
        public question fkquestion {get; set; }
        public user fkuser { get; set; }
        public DateTimeOffset answerdate { get; set; }
        public String answermessage { get; set; }
        public Boolean validatedAnswer { get; set; }
        
    }
}