using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace webblog.Models
{
    public class blogpost
    {
        public blogpost()
        {
            this.Comments = new HashSet<comment>();
        }
        public int Id { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Updated { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        [AllowHtml]
        public string Body { get; set; }
        public string MediaURL { get; set; }
        public bool Published { get; set; }
        public string Category { get; set; }

        public virtual ICollection<comment> Comments { get; set; }
    }
}