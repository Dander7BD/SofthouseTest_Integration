﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkupIntegration
{
    public interface IMLReader : IDisposable
    {
        IMLWriter TranslateTo(IMLWriter output);
    }
}
