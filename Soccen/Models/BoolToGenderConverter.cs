using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace Soccen.Models
{
    public class BoolToGenderConverter : MarkupExtension, IValueConverter
    {

        static BoolToGenderConverter convertor;

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if(value != null) { 
                if (value.ToString() == "0") return "Жінка";
                else return "Чоловік";
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (convertor == null)
            {
                convertor = new BoolToGenderConverter();
            }

            return convertor;
        }


        public BoolToGenderConverter()
        {

        } 
    }

   
}
