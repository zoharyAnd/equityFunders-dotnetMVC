using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.CodeAnalysis;

namespace cFunding.Models
{
    public class transaction
    {
        public int id { get; set;}

        public project fkproject { get; set; }

        public user fkuser { get; set; }

        public double amount { get; set; }

        public DateTimeOffset transactiondate {get; set; }

        public String accountemail { get; set; }

    }
}