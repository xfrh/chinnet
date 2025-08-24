using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Utility.FastDBF
{
    public class DbfDataTruncateException : Exception
    {

        public DbfDataTruncateException(string smessage) : base(smessage)
        {
        }

        public DbfDataTruncateException(string smessage, Exception innerException)
          : base(smessage, innerException)
        {
        }

        public DbfDataTruncateException(SerializationInfo info, StreamingContext context)
          : base(info, context)
        {
        }

    }
}
