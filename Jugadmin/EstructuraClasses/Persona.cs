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
        
        /// <summary>
        /// Crea una persona
        /// </summary>
        /// <param name="nom">nom de la persona</param>
        public Persona(string nom, string nomImg,string sexe)
        {
            this.nom = nom;
            this.nomImg = nomImg;
            this.sexe = sexe;
            
        }
        /// <summary>
        /// Crea una persona
        /// </summary>
        public Persona() 
        {
            this.nom = "No establert";
            this.nomImg = "No establert";
            this.sexe = "Masculí";
        }
        /// <summary>
        /// Obté el nom de la persona
        /// </summary>
        public string Nom
        {
            get { return nom;}
            set { this.nom = value; }
        }

        public string Sexe
        {
            get { return sexe; }
            set { sexe = value; }
        }

        //Retorna o asigna un bitmat "imatge" a la persona;
        public string NomImg
        {
            get { return nomImg; }
            set { nomImg = value; }
        }

        /// <summary>
        /// Determina si la persona es un jugador o un entrenador
        /// </summary>
        /// <returns>Retorna si és convidat</returns>
        public abstract bool EsJugador();

    }
}
