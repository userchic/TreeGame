using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using WpfApp1.Elements;

namespace WpfApp1.NormalElements
{
    public class Empty : State
    {
        public override void Handle(Space context)
        {
            context.btn.Background = new SolidColorBrush(Color.FromRgb(0, 255, 0));
            context.state = new Grass();
        }
    }
}
