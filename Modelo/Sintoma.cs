using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PATA
{

    public class Sintoma

    {
        private string _nome;


        public Sintoma(string nome) 
        {
            this._nome = nome;
        }

          public string Nome
        {
            set { this._nome = value; }
            get { return this._nome; }
        }

     

        public override String ToString()
        {
            return " Nome:" + Nome;
        }
    }

   
}
