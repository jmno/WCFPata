using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Windows;


namespace PATA
{
    public class Dados
    {
        public Boolean isEmpty = true;
        public List<Sintoma> _listSintomas;
        public List<Diagnostico> _listDiagnosticos;

        public Dados()
        {
            this._listSintomas = new List<Sintoma>();
            this._listDiagnosticos = new List<Diagnostico>();
            isEmpty = false;
        }

        public List<Sintoma> ListSintomas
        {
            set
            {
                this._listSintomas = value;
                this.isEmpty = false;
            }
            get
            {
                return this._listSintomas;
            }
        }

        public List<Diagnostico> ListDiagnosticos
        {
            set
            {
                this._listDiagnosticos = value;
                this.isEmpty = false;
            }
            get
            {
                return this._listDiagnosticos;
            }
        }

        public Boolean IsEmpty
        {
            set
            {
                this.isEmpty = value;
            }
            get
            {
                return this.isEmpty;
            }
        }
   

        public override String ToString()
        {
            String c = "";
            foreach (Sintoma p in _listSintomas)
            {
                c += p.ToString();
            }
            foreach (Diagnostico o in _listDiagnosticos)
            {
                c += o.ToString();
            }
            return c;
        }

       

    }
}
