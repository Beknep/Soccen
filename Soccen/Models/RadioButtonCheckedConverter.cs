using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace Soccen.Models
{
    public class RadioButtonCheckedConverter : MarkupExtension, IValueConverter

    {
        static RadioButtonCheckedConverter convertor;

        public object Convert(object value, Type targetType, object parameter,

            System.Globalization.CultureInfo culture)

        {

            return value.Equals(parameter);

        }


        public object ConvertBack(object value, Type targetType, object parameter,

            System.Globalization.CultureInfo culture)

        {

            return value.Equals(true) ? parameter : Binding.DoNothing;

        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (convertor == null)
            {
                convertor = new RadioButtonCheckedConverter();
            }

            return convertor;
        }

        public RadioButtonCheckedConverter()
        {

        }

    }

}
