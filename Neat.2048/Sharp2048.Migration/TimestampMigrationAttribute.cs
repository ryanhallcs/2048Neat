using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator;

namespace Sharp2048.Migrations
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class TimestampMigrationAttribute : MigrationAttribute
    {
        public TimestampMigrationAttribute(string dateTime)
            : base(DateTime.Parse(dateTime).Ticks)
        {
            
        }

        public TimestampMigrationAttribute(long version) : base(version)
        {
        }

        public TimestampMigrationAttribute(long version, string description) : base(version, description)
        {
        }

        public TimestampMigrationAttribute(long version, TransactionBehavior transactionBehavior) : base(version, transactionBehavior)
        {
        }

        public TimestampMigrationAttribute(long version, TransactionBehavior transactionBehavior, string description) : base(version, transactionBehavior, description)
        {
        }
    }
}
