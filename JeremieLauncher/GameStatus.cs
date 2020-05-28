﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeremieLauncher
{
    [Flags]
    public enum GameStatus
    {
        NONE=0,
        INSTALLED=1,
        UPDATED=2,
        UNAVAILABLE=4
    }
}
