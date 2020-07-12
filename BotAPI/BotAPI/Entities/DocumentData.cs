using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BotAPI.Entities
{
    public class DocumentData
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string DocumentPath { get; set; }
        public string Summary { get; set; }
    }
}