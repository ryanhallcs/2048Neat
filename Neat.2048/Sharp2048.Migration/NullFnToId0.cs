using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator;

namespace Sharp2048.Migrations
{
    [TimestampMigration("5/5/2014 10:58:08 PM")]
    public class NullFnToId0 : Migration
    {
        /// <summary>
        /// Moves up to new version of DB
        /// </summary>
        public override void Up()
        {
            Execute.Sql(@"

UPDATE dbo.ActivationFunctions
SET ActivationFunctionId = 0
WHERE ActivationFunctionId = 1
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
