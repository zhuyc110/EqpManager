using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using EquipmentManager.ViewModel.Equipment;

namespace EquipmentManager.Converters
{
    [ValueConversion(typeof(EquipmentStatus), typeof(Brush))]
    public class EquipmentStatusToBrushConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var key = value as EquipmentStatus?;
            if (key == null)
            {
                return Brushes.Gray;
            }
            switch (key.Value)
            {
                case EquipmentStatus.Running:
                    return Brushes.DodgerBlue;
                case EquipmentStatus.Down:
                    return Brushes.Red;
                case EquipmentStatus.Pm:
                    return Brushes.OrangeRed;
                default:
                    return Brushes.Gray;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}