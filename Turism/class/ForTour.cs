using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Turism
{
    public partial class Tour
    {
        public string CheckActual
        {
            get
            {
                if (IsActual == true)
                {
                    return "Актуален";
                }
                else
                {
                    return "Не актуален";
                }
            }
        }
        public SolidColorBrush ForegroundActual
        {
            get
            {
                if (IsActual == true)
                {
                    return Brushes.Green;
                }
                else
                {
                    return new SolidColorBrush(Color.FromRgb(227, 30, 36));
                }
            }
        }
    }
}
