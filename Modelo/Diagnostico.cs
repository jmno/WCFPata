using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PATA
{
    public class Diagnostico
    {
        private string _nome;
        private List<Sintoma> _listSintomas;
        private string _orgao;
        private string _tratamento;


        public List<Sintoma> ListSintomas
        {
            set
            {
                this._listSintomas = value;
            }
            get
            {
                return this._listSintomas;
            }
        }

        public string Nome
        {
            set { this._nome = value; }
            get { return this._nome; }
        }

        public string Tratamento
        {
            set { this._tratamento = value; }
            get { return this._tratamento; }
        }
        public string Orgao
        {
            set { this._orgao = value; }
            get { return this._orgao; }
        }
        

         public Diagnostico(string nome,string orgao,string tratamento, List<Sintoma> listaSintomas) 
        {
            this._nome = nome;
            this._orgao = orgao;
            this._tratamento = tratamento;
            this._listSintomas = listaSintomas;
        }

         public Diagnostico()
         {
         }

         public override String ToString()
         {
             return  Orgao + "|" + Nome + "|"+ Tratamento;
         }

         

    }
}
