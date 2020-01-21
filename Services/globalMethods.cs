using System;
using cFunding.Models;
using System.Collections.Generic;

namespace cfunding.Services
{
    public static class globalMethods
    {
        public static String formattedDate(DateTimeOffset mydate){
            var weekDay = mydate.DateTime.DayOfWeek;
            var d = mydate.DateTime.Day;
            var year = mydate.DateTime.Year;
            int m = mydate.DateTime.Month;
            string month = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(m);
            string strDate = weekDay+" "+d + " " + month + " " + year;
            return strDate;
        }
        
        public static String trimTitle(project myproject){
            var title = myproject.projectname;

            if (title.Length > 30)
            {
                title = title.Substring(0, 30) + " ...";
            }
            return title;
        }

        public static String trimDescription(project myproject){
            var description = myproject.projectdescription;

            if (description.Length > 80)
            {
                description = description.Substring(0, 80) + " ...";
            }
            return description;
        }

        public static String trimStrDescription(string description)
        {
            if (description.Length > 80)
            {
                description = description.Substring(0, 80) + " ...";
            }
            return description;
        }
        public static String percentRaised(project myproject){
            //amount
            var percentRaised = (myproject.projectamountraised * 100) / myproject.projectamounttoraise;
            return Math.Round(percentRaised, 2).ToString();
        }

        public static String percentRange(project myproject){
            Double pRaised = Double.Parse(percentRaised(myproject));
            var percentRange = (int)Math.Ceiling(pRaised);
            return percentRange.ToString()+"%";
        }

    }
}