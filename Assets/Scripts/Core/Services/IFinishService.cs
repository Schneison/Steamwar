﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steamwar
{
    public interface IFinishService : IService
    {
        IEnumerator Finish();
    }
}
