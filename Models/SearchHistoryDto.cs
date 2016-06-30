using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FrontwiseRecruitingApp.Models
{
    public class SearchHistoryDto
    {
        public int ID { get; set; }
        public string SearchID { get; set; }
        public string UserID { get; set; }
        public string SearchLabel { get; set; }
        public Nullable<DateTime> SubmitDate { get; set; }
        public string GroupID { get; set; }
    }
}