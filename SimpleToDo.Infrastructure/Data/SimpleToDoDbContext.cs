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
        public DbSet<Todo> Todos { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectMember> ProjectMembers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProjectMember>()
                .HasOne(pm => pm.Project)
                .WithMany(p => p.ProjectMembers)
                .HasForeignKey(p => p.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<ProjectMember>()
                .HasOne(pm => pm.Member)
                .WithMany(m => m.ProjectMembers)
                .HasForeignKey(pm => pm.MemberId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
