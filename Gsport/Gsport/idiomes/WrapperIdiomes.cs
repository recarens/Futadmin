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

        //devuelve una instancia nueva de nuestros recursos.
        public idiomes GetResourceInstance()
        {
            return new idiomes();
        }

        //Esta propiedad devuelve el ObjectDataProvider en uso.
        public static ObjectDataProvider ResourceProvider
        {
            get
            {
                if (m_provider == null)
                    m_provider = (ObjectDataProvider)App.Current.FindResource("IdiomesRes");
                return m_provider;
            }
        }

        //Este método cambia la cultura aplicada a los recursos y refresca la propiedad ResourceProvider.
        public static void ChangeCulture(CultureInfo culture)
        {
            Properties.Resources.Culture = culture;
            ResourceProvider.Refresh();
        }
    }
}
