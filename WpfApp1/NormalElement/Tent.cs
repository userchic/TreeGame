using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using WpfApp1.Elements;

namespace WpfApp1.NormalElements
{
    public class Tent : State
    {
        public override void Handle(Space context)
        {
            context.btn.Background = new SolidColorBrush(Color.FromRgb(221, 221, 221));
            context.btn.Content = null;
            context.state = new Empty();
        }
    }
}
