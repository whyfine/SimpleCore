using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCore.Test
{
    public class CoreProvider
    {
        private CoreProvider() { }

        [ThreadStatic]
        private static CoreProvider _provider = new CoreProvider();
        public static CoreProvider Provider
        {
            get
            {
                if (_provider == null) _provider = new CoreProvider();
                return _provider;
            }
        }

        public ExampleBll ExampleBll = new ExampleBll(Business_OnException);

        static void Business_OnException(object sender, DbContextExceptionEventArgs e)
        {
            //write log
        }
    }
}
