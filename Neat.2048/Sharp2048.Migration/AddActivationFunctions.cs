using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentMigrator;

namespace Sharp2048.Migrations
{
    [TimestampMigration("5/5/2014 6:06:17 PM")]
    public class AddActivationFunctions : Migration
    {
        /// <summary>
        /// Moves up to new version of DB
        /// </summary>
        public override void Up()
        {
            Execute.Sql(@"

CREATE TABLE [dbo].[ActivationFunctions](
	[ActivationFunctionId] [int] NOT NULL,
	[Lookup] [nvarchar](30) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[FunctionString] [nvarchar](200) NULL,
 CONSTRAINT [PK_ActivationFunctions] PRIMARY KEY NONCLUSTERED 
(
	[ActivationFunctionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

INSERT INTO [dbo].[ActivationFunctions] (ActivationFunctionId, Lookup, Description, FunctionString)
VALUES 
	(1, 'NullFn', 'Null activation function. Returns zero regardless of input.', 'y = 0'),
	(2, 'BipolarGaussian', 'Bipolar gaussian.\r\nEffective xrange->[-1,1] yrange->[-1,1]', '2*e^(-(input*2.5)^2) - 1'),
	(3, 'BipolarSigmoid', 'Bipolar steepened sigmoid.\r\nEffective xrange->[-1,1] yrange->[-1,1]', 'y = 2.0/(1.0 + exp(-4.9*x)) - 1.0'),
	(4, 'Linear', 'Linear function with clipping.\r\nEffective xrange->[-1,1] yrange[-1,1]', 'y = max(-1,min(1,x))'),
	(5, 'Sine', 'Sine function with doubled period.\r\nEffective xrange->[-Inf,Inf] yrange[-1,1]', 'y = sin(2*x)'),
	(6, 'RbfGaussian', 'Gaussian.\r\nEffective yrange->[0,1]', 'y = e^(-((x-center)*epsilon)^2)'),
	(7, 'Absolute', 'Absolute (magnitude) function with clipping.\r\nEffective xrange->[-1,1] yrange->[0,1]', 'y = min(1, abs(x))'),
	(8, 'AbsoluteRoot', 'Absolute root function with clipping.\r\nEffective xrange->[-1,1] yrange->[0,1]', 'y = min(1, sqrt(abs(x)))'),
	(9, 'Gaussian', 'Gaussian.\r\nEffective xrange->[-1,1] yrange->[0,1]', 'y = e^(-(x*2.5)^2)'),
	(10, 'InverseAbsoluteSigmoid', 'A sigmoid curve produced from the simple/fast arithmetic operations abs, divide and multiply.\r\nEffective xrange->[-1,1] yrange->[0,1]', 'y = 0.5 + (x / (2*(0.2+abs(x))))'),
	(11, 'ReducedSigmoid', 'Simple sigmoid function with a gentler (or reduced) slope compared to the PlainSimple function.\r\n Effective xrange->[-10,10] yrange->[0,1]', 'y = 1/(1+(exp(-0.5*x)))'),
	(12, 'SteepenedSigmoid', 'Plain sigmoid.\r\nEffective xrange->[-5,5] yrange->[0,1]', 'y = 1.0/(1.0 + exp(-4.9*x))'),
	(13, 'SteepenedSigmoidApproximation', 'An approximation of the SteepenedSigmoid function. Faster to calculate but anecdotal evidence suggests using this function gives poorer results than SteepenedSigmoid.\r\nxrange->[-1,1] yrange->[0,1]', 'A fast approximation of y = 1.0/(1.0 + exp(-4.9*x))'),
	(14, 'StepFunction', 'Step function.\r\nEffective xrange->[-Inf,+Inf] yrange->[0,1]', 'y = x<0 ? 0 : 1'),
	(15, 'PlainSigmoid', 'Plain sigmoid.\r\nEffective xrange->[-5,5] yrange->[0,1]', 'y = 1.0/(1.0+(exp(-x)))')

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
