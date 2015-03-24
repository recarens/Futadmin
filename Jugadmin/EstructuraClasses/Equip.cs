using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstructuraClasses
{
    public class Equip : IEnumerable<Jugador>
    {
        string nom;
        int tipus;
        string categoria;
        int numJugadors;
        Dictionary<string, Jugador> equip;

        public string Nom
        {
            get { return nom; }
            set { nom = value; }
        }

        public Equip(string nom,int tipus, string categoria)
        {
            this.nom = nom;
            this.tipus = tipus;
            this.categoria = categoria;
            equip = new Dictionary<string, Jugador>();
        }
        public Equip()
        {
            equip = new Dictionary<string, Jugador>();
        }

        /// <summary>
        /// Obtè el número total de persones
        /// </summary>
        public int NumJugadors
        {
            get
            {
                return numJugadors;
            }
            set
            {
                numJugadors = value;
            }
        }

        public int Tipus
        {
            get { return tipus; }
            set { tipus = value; }
        }

        public string Categoria
        {
            get { return categoria; }
            set { categoria = value; }
        }

        /// <summary>
        /// Afegeix una persona a la taula
        /// </summary>
        /// <param name="conv">Convidat a afegir</param>
        public void Afegir(Jugador j)
        {
            if (!equip.ContainsKey(j.Nom))
            {
                equip.Add(j.Nom.ToString().ToLower(), j);
                numJugadors++;
            }
        }

        public void Eliminar(Jugador j)
        {
            equip.Remove(j.Nom.ToString().ToLower());
            numJugadors--;
        }

        public void Eliminar(string nom)
        {
            nom = nom.ToLower();
            equip.Remove(nom);
            numJugadors--;
        }

        public IEnumerator<Jugador> GetEnumerator()
        {
            return equip.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
