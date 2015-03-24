using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstructuraClasses
{
    public enum Posicio { Porter, Defensa, Migcampista, Davanter}
    public class Jugador : Persona
    {
        int edat;
        double altura;
        int anyNeixament;
        double pes;
        Posicio p;
        string nomEquip;

        
        /// <summary>
        /// Crea un convidat
        /// </summary>
        /// <param name="nom">Caràcter que l'identificarà</param>
        /// <param name="sex">Plus de simpatia sobre el sexe contrari</param>
        public Jugador(string nom, string sexe, string nomImg, int edat, int anyNeixament): base(nom, nomImg, sexe)
        {
            this.edat = edat;
            this.anyNeixament = anyNeixament;
            this.altura = 0;
            this.pes = 0;
            this.p = Posicio.Migcampista;
        }
        public Jugador()
        {

        }

        public Posicio P
        {
            get { return p; }
            set { p = value; }
        } 
        public int Edat
        {
            get { return edat; }
            set { edat = value; }
        }      

        public int AnyNeixament
        {
            get { return anyNeixament; }
            set { anyNeixament = value; }
        }  

        public double Altura
        {
            get { return altura; }
            set { altura = value; }
        } 

        public double Pes
        {
            get { return pes; }
            set { pes = value; }
        }
        public string NomEquip
        {
            get { return nomEquip; }
            set { nomEquip = value; }
        }
        
        /// <summary>
        /// Retorna que si és un Jugador
        /// </summary>
        /// <returns>Cert</returns>
        public override bool EsJugador()
        {
            return true;
        }


    }
}
