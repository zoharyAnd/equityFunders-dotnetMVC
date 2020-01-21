using System;
using System.Runtime.Serialization;

namespace cFunding.Models
{
    public class favorite
    {
        public int id { get; set; }
        public user fkuser { get; set; }
        public project fkproject { get; set; }
    }
}