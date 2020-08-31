using System;
using System.Windows.Data;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Soccen.Models
{
    public class RowToIndexConvertor : MarkupExtension, IValueConverter
    {
        static RowToIndexConvertor convertor;

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            DataGridRow dataGridRow = value as DataGridRow;
            DataGridRow row = dataGridRow;

            if (row != null)
            {
                return row.GetIndex() + 1;
            }
            else
            {
                return -1;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (convertor == null)
            {
                convertor = new RowToIndexConvertor();
            }

            return convertor;
        }


        public RowToIndexConvertor()
        {

        }
    }
}
