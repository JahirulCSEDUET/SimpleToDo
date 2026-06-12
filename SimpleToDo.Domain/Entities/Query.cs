using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Domain.Entities
{
    public class Query
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public string? FileName { get; set; }
        public string? FilePath { get; set; }
        public int UserId { get; set;  }
        public User User { get; set; }
        public int TodoId {  get; set; }
        public Todo Todo { get; set; }
    }
}
