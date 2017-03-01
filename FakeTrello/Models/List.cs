using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FakeTrello.Models
{
    public class List
    {
        [Key]
        public int ListId { get; set; }
        public string Name { get; set; }
    }
}