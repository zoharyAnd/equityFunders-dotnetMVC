using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace cFunding.Models
{
    public class user
    {
        [ForeignKey("user")]
        public int id { get; set; }
        public string userfname { get; set; }
        public string userlname { get; set; }
        public string username { get; set; }
        public string useremail { get; set; }
        public string userpassword { get; set; }
        public string userphoto { get; set; }
        public DateTimeOffset userdob { get; set; }
        public string useraddress { get; set; }
        public string usercountry { get; set; }
        public string companyname { get; set; }
        public string companylogo { get; set; }
        public string companydescription { get; set; }
        public int nbshareordinary { get; set; }
        public double sharevalueordinary { get; set; }
        public string descordinary { get; set; }
        public string additionalordinary { get;set; }
        public int nbsharepreferencial { get; set; }
        public double sharevaluepreferencial { get; set; }
        public string descpreferencial { get; set; }
        public string additionalpreferencial { get;set; }
        public int nbsharenonvoting { get; set; }
        public double sharevaluenonvoting { get; set; }
        public string descnonvoting { get; set; }
        public string additionalnonvoting { get;set; }
        public int nbshareredeemable { get; set; }
        public double sharevalueredeemable { get; set; }
        public string descredeemable { get; set; }
        public string additionalredeemable { get;set; }

        public double userassets { get; set; }
        public Boolean isadmin{get; set;}
        public Boolean validatedUser {get; set;}

        
    }
}