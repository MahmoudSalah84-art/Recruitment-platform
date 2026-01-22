using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Infrastructure.ReadModels
{
	/// <summary>
	/// Separate DbContext optimized for read queries (CQRS)
	/// </summary>
	public class ReadDbContext : DbContext
	{
		public ReadDbContext(DbContextOptions<ReadDbContext> options)
			: base(options) 
		{ }

		public DbSet<JobSearchView> JobSearch => Set<JobSearchView>();
		public DbSet<ApplicationOverviewView> Applications => Set<ApplicationOverviewView>();

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<JobSearchView>().HasNoKey().ToView("vw_JobSearch");
			modelBuilder.Entity<ApplicationOverviewView>().HasNoKey().ToView("vw_ApplicationOverview");
		}
	}
}
