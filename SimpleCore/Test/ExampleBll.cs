﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCore.Test
{
    public class ExampleBll : ExampleBusinessBll<Example>
    {
        internal ExampleBll(ExceptionEventHandler onException)
        {
            base.OnException += onException;
        }
    }
}