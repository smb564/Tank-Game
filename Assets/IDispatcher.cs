﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets
{
    interface IDispatcher
    {
        void invoke(Action function);
    }

}
