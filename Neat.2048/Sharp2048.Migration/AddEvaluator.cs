using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator;

namespace Sharp2048.Migrations
{
    [TimestampMigration("5/12/2014 9:06:17 PM")]
    public class AddActivationFunction : Migration
    {
        /// <summary>
        /// Moves up to new version of DB
        /// </summary>
        public override void Up()
        {
            Execute.Sql(@"ALTER TABLE [dbo].[Genomes]
ADD [EvaluatorType] nvarchar(200) NULL
GO

UPDATE [dbo].[Genomes]
SET [EvaluatorType] = 'DirectMover2048Ai'

ALTER TABLE [dbo].[Genomes]
ALTER COLUMN [EvaluatorType] nvarchar(200) NOT NULL");
        }


        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}