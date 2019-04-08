using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using static Simple_API_Database.Models.EF_Models;
using  Simple_API_Database.Models;

namespace Simple_API_Database.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Company> Companies { get; set; }
        public DbSet<KeyStat> KeyStats { get; set; }
        //public DbSet<Dividends> Dividends { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<HistoricalData> HistoricalDatas { get; set; }

        public DbSet<CompanyInfo> CompanyInfo { get; set; }
    }
}
