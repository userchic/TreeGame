using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Elements;

namespace WpfApp1
{
    public abstract class State
    {
        public abstract void Handle(Space context);
    }
}
