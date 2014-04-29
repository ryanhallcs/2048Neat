using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentMigrator;

namespace Sharp2048.Migrations
{
    [TimestampMigration("4/28/2014 6:18:40 PM")]
    public class AddGenomesTable : Migration
    {
        /// <summary>
        /// Moves up to new version of DB
        /// </summary>
        public override void Up()
        {
            Execute.Sql(@"
CREATE TABLE dbo.Genomes(
	GenomeIdentifier UNIQUEIDENTIFIER NOT NULL,
	GenomeXml nvarchar(max) NOT NULL,
	 CONSTRAINT [PK_Genomes] PRIMARY KEY NONCLUSTERED 
(
	[GenomeIdentifier] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
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
