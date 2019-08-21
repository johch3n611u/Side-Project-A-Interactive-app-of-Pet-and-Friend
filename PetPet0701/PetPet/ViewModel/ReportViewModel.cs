using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PetPet.Models;

namespace PetPet.ViewModel
{
    public class ReportViewModel
    {
        public List<Post> RPost { get; set; }
        public List<Member> RMember { get; set; }
        public List<Post_img> RPost_img { get; set; }
        public List<Violation_type> RViolation_type { get; set; }
        public List<ReportView> RReportView { get; set; }

    }
}