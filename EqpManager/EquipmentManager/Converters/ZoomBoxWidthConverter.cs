﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace EquipmentManager.Converters
{
    [ValueConversion(typeof(double), typeof(double))]
    public class ZoomBoxWidthConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var parentWidth = value as double?;
            if (parentWidth.HasValue)
            {
                return Math.Max(0, parentWidth.Value - DEFAULT_MARGIN);
            }

            return DEFAULT_WIDTH;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion

        #region Fields

        private const int DEFAULT_WIDTH = 400;
        private const int DEFAULT_MARGIN = 20;

        #endregion
    }
}