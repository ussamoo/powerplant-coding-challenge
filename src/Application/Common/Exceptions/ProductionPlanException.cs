using System;

namespace Application.Common.Exceptions
{

    public class ProductionPlanException : Exception
    {
        public ProductionPlanException()
            : base()
        {
        }

        public ProductionPlanException(string message)
            : base(message)
        {
        }

        public ProductionPlanException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
