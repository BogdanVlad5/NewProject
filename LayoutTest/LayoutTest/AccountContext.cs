using LayoutTest.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace LayoutTest
{
    public class AccountContext : DbContext
    {
        public AccountContext(string connection): base(connection)
        {
        }

        public IDbSet<Account> Accounts{ get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}