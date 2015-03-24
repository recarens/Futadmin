using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstructuraClasses
{
    public abstract class Persona
    {
        string dni;
        string nom;
        string cognoms;
        string nomImg;
        string sexe;
        DateTime dtInscripcio;
        DateTime dtNeixament;   
        int anyNeixament;
        string targetaSanitaria;
        string malaltiaOAlergia;    
        string mobil;    
        string telefon;
        string correuElectronic;
        string nSoci;  
        int edat;
        double pes;
        double altura;     

        /// <summary>
        /// Crea una persona
        /// </summary>
        /// <param name="nom">nom de la persona</param>
        public Persona(string dni)
        {
            this.dni = dni;
        }
        /// <summary>
        /// Crea una persona
        /// </summary>
        public Persona() 
        {
            this.dni = "No te dni";
        }

        #region PROPIETATS
        public string NSoci
        {
            get { return nSoci; }
            set { nSoci = value; }
        }
        public int Edat
        {
            get { return edat; }
            set { edat = value; }
        }
        public double Altura
        {
            get { return altura; }
            set { altura = value; }
        }
        public string CorreuElectronic
        {
            get { return correuElectronic; }
            set { correuElectronic = value; }
        }
        public double Pes
        {
            get { return pes; }
            set { pes = value; }
        }
        public string Dni
        {
            get { return dni; }
            set { dni = value; }
        }
        /// <summary>
        /// Obté el nom de la persona
        /// </summary>
        public string Nom
        {
            get { return nom;}
            set { this.nom = value; }
        }
        //Retorna o asigna un bitmat "imatge" a la persona;
        public string NomImg
        {
            get { return nomImg; }
            set { nomImg = value; }
        }
        public int AnyNeixament
        {
            get { return anyNeixament; }
            set { anyNeixament = value; }
        }
        public string Cognoms
        {
            get { return cognoms; }
            set { cognoms = value; }
        }

        public DateTime DtInscripcio
        {
            get { return dtInscripcio; }
            set { dtInscripcio = value; }
        }

        public DateTime DtNeixament
        {
            get { return dtNeixament; }
            set { dtNeixament = value; }
        }
        public string TargetaSanitaria
        {
            get { return targetaSanitaria; }
            set { targetaSanitaria = value; }
        }
        public string Telefon
        {
            get { return telefon; }
            set { telefon = value; }
        }
        public string Sexe
        {
            get { return sexe; }
            set { sexe = value; }
        }
        public string Mobil
        {
            get { return mobil; }
            set { mobil = value; }
        }
        public string MalaltiaOAlergia
        {
            get { return malaltiaOAlergia; }
            set { malaltiaOAlergia = value; }
        }
        #endregion
        /// <summary>
        /// Determina si la persona es un jugador o un entrenador
        /// </summary>
        /// <returns>Retorna si és convidat</returns>
        public abstract bool EsJugador();

    }
}
