using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SimpleToDo.Domain.Entities;
using SimpleToDo.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Infrastructure.Data
{
    public class SimpleToDoDbContext:IdentityDbContext<ApplicationUser>
    {
        public SimpleToDoDbContext(DbContextOptions<SimpleToDoDbContext> options) : base(options) { }
        public DbSet<ToDoItem> ToDoItems { get; set; }
    }
}
