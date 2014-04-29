using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentMigrator;

namespace Sharp2048.Migrations
{
    [TimestampMigration("4/28/2014 6:58:08 PM")]
    public class AddGenomeDescription : Migration
    {
        /// <summary>
        /// Moves up to new version of DB
        /// </summary>
        public override void Up()
        {
            Execute.Sql(@"
ALTER TABLE dbo.Genomes
ADD Description nvarchar(max)
");
        }

        /// <summary>
        /// Moves down to old version of DB
        /// </summary>
        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}
