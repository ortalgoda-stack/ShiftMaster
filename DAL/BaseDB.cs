using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public abstract class BaseDB
    {
        protected readonly string connectionString;

        public BaseDB(string connectionString)
        {
            this.connectionString = connectionString;
        }
    }
}
