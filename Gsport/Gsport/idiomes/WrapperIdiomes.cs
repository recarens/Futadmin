using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Resources;
using System.Reflection;

namespace Gsport.idiomes
{
    class WrapperIdiomes
    {
        private static ObjectDataProvider m_provider;

        public WrapperIdiomes()
        {
        }

        //torna una instancia nova dels nostres recurssos d'idioma
        public idiomes GetResourceInstance()
        {
            return new idiomes();
        }

        //torna ObjectDataProvider en us.
        public static ObjectDataProvider ResourceProvider
        {
            get
            {
                if (m_provider == null)
                    m_provider = (ObjectDataProvider)App.Current.FindResource("IdiomesRes");
                return m_provider;
            }
        }

        //cambia la cultura aplicada en els recursos y refresca la propietat ResourceProvider.
        public static void ChangeCulture(CultureInfo culture)
        {
            Properties.Resources.Culture = culture;
            ResourceProvider.Refresh();
        }
    }
}
