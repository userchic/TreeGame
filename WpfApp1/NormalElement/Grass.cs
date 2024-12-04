using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WpfApp1.Elements;

namespace WpfApp1
{
    public class Grass : State
    {
        public override void Handle(Space context)
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("/Drafts/Tent.png", UriKind.Relative);
            bitmap.EndInit();
            context.btn.Content = new Image() { Source= bitmap};
            context.state = new Tent();
        }
    }
}
