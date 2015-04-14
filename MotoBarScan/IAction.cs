using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MotoBarScan
{
    public interface IAction
    {
        void ReadArgs(string[] args);

        void Execute();
    }
}
