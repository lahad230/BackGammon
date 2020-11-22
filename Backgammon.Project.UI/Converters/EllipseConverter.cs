using SharedAssets.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Backgammon.Project.UI.Converters
{
    public class EllipseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Cell currentCell = (Cell)value;
            ObservableCollection<Ellipse> col = new ObservableCollection<Ellipse>();

            for (int i = 0; i < currentCell.SoldiersQuantity; i++)
            {
                Ellipse e = new Ellipse();
                e.Width = 29;
                e.Height = 29;
                if (currentCell.Color)
                {
                    e.Fill = new SolidColorBrush(Colors.Black);
                }
                else
                {
                    e.Fill = new SolidColorBrush(Colors.Cyan);
                }

                col.Add(e);

            }

            return col;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
