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
        Posicio p;
        string nomEquip;
        int minutsJugats;
        int gols;
        int faltesEntreno;
        string usuariPortal; // portal del federat  
        string contrasenyaPortal; // portal del federat
        
        /// <summary>
        /// Crea un convidat
        /// </summary>
        /// <param name="nom">Caràcter que l'identificarà</param>
        /// <param name="sex">Plus de simpatia sobre el sexe contrari</param>
        public Jugador(string dni): base(dni) {}
        public Jugador() { }
        public Posicio P
        {
            get { return p; }
            set { p = value; }
        }  
        public string NomEquip
        {
            get { return nomEquip; }
            set { nomEquip = value; }
        }
        public int MinutsJugats
        {
            get { return minutsJugats; }
            set { minutsJugats = value; }
        }
        public int Gols
        {
            get { return gols; }
            set { gols = value; }
        }
        public int FaltesEntreno
        {
            get { return faltesEntreno; }
            set { faltesEntreno = value; }
        }
        public string UsuariPortal
        {
            get { return usuariPortal; }
            set { usuariPortal = value; }
        }
        public string ContrasenyaPortal
        {
            get { return contrasenyaPortal; }
            set { contrasenyaPortal = value; }
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
