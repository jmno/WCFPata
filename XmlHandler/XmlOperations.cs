using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Windows;
using Dados = PATA.Dados;
using Sintoma = PATA.Sintoma;
using Diagnostico = PATA.Diagnostico;
using System.IO;
using System.Xml.Schema;

namespace XmlHandler
{
    public class XmlOperations
    {
        public static void saveXML(Dados dados, String xmlPath)
        {
            
            if (!dados.isEmpty)
            {

                XmlTextWriter escritor = new XmlTextWriter(xmlPath, System.Text.Encoding.UTF8);
                escritor.Formatting = Formatting.Indented;
                escritor.WriteStartDocument();
                escritor.WriteStartElement("PATA");
                escritor.WriteStartElement("ListaDeSintomas");

                foreach (Sintoma p in dados._listSintomas)
                {
                    escritor.WriteStartElement("Sintoma");
                    escritor.WriteElementString("nome", p.Nome);
                    escritor.WriteEndElement();
                }
                escritor.WriteEndElement();
                escritor.WriteStartElement("DiagnosticosETratamentos");
                foreach (Diagnostico o in dados.ListDiagnosticos)
                {
                    escritor.WriteStartElement("DiagnosticoETratamento");
                    escritor.WriteElementString("Orgao", o.Orgao);
                    escritor.WriteElementString("Diagnostico", o.Nome);
                    escritor.WriteElementString("Tratamento", o.Tratamento);
                    escritor.WriteStartElement("ListaSintomas"); 
                    foreach (Sintoma s in o.ListSintomas)
                    {
                        escritor.WriteElementString("sintoma", s.Nome);
                    }
                    escritor.WriteEndElement();
                    escritor.WriteEndElement();
                }
                escritor.WriteEndElement();
                escritor.WriteEndElement();
                escritor.WriteEndDocument();
                escritor.Close();
                System.Windows.Forms.MessageBox.Show("XML Salvo com Sucesso!");
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Tem de Ler Primeiro os Dados do Excel!");

            }
        }

        public static String xpathExpression(String xmlPath, String xpath) 
        {
             string res = String.Empty;
             XmlDocument doc = new XmlDocument();
             doc.Load(xmlPath);
             XmlNodeList nodeList = doc.SelectNodes(xpath);
             foreach(XmlNode node in nodeList)
                 {
                     res += node.InnerXml;
                
                 }
             return res;
        }

        public static List<String> listaSintomas(String xmlPath)
        {
            List<String> lista = new List<String>();
            String xpath = "//ListaDeSintomas/Sintoma/nome";
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlPath);
            XmlNodeList nodeList = doc.SelectNodes(xpath);
            foreach (XmlNode node in nodeList)
            {
                lista.Add(node.InnerXml);

            }

            return lista;

        }




        public static List<string> procuraSintomas(List<string> lista, String xmlPath)
        {
            List<String> listaFinal = new List<string>();
            String xpath = "";
            List<Diagnostico> listaDiagnosticos = new List<Diagnostico>();
            List<Sintoma> listaSintomas;
            List<Int32> bedjoras = new List<Int32>();
            Diagnostico diagAux;
       
            foreach (String s in lista) 
            {
                xpath = "//ListaSintomas/sintoma[text()=\""+s+"\"]/../..";
                XmlDocument doc = new XmlDocument();
                doc.Load(xmlPath);
                XmlNodeList nodeList = doc.SelectNodes(xpath);
                foreach (XmlNode node in nodeList)
                {
                    listaSintomas = new List<Sintoma>();
                    foreach (XmlNode x in node.ChildNodes[3].ChildNodes)
                    {
                        listaSintomas.Add(new Sintoma(x.InnerText));
                    }
                    diagAux = new Diagnostico(node.ChildNodes[1].InnerText, node.ChildNodes[0].InnerText, node.ChildNodes[2].InnerText, listaSintomas);
                

                    //TESTE

                  
                    if (exists(listaDiagnosticos,diagAux))
                    {
                        int indice = getIndice(listaDiagnosticos,diagAux);
                        bedjoras[indice] += 1;
                    }

                    else
                    {
                        listaDiagnosticos.Add(diagAux);
                        bedjoras.Add(1);
                    }
            }
                

            }

            foreach (Diagnostico d in listaDiagnosticos)
            {
                int aux=listaDiagnosticos.IndexOf(d);
                int count = bedjoras[aux];
                int auxcontagem = d.ListSintomas.Count();
                decimal bla = count/Convert.ToDecimal(auxcontagem);
                decimal resultado =Math.Round(bla*100,1);
                listaFinal.Add(resultado+"|"+d.ToString());
               
            }

            listaFinal.Sort(new ComparacaoResultados());       

            return listaFinal;
        }

        public static int getIndice(List<Diagnostico> lista, Diagnostico d)
        {
            if (lista.Count > 0)
            {
                foreach (Diagnostico diagnostico in lista)
                {
                    if (diagnostico.Nome.Equals(d.Nome) && diagnostico.Orgao.Equals(d.Orgao))
                    {
                        return lista.IndexOf(diagnostico);
                    } 
                }
            }
            return -1;
        }

        public static Boolean exists(List<Diagnostico> lista, Diagnostico d)
        {
            if (lista.Count > 0)
            {
                foreach (Diagnostico diagnostico in lista)
                {
                    if (diagnostico.Nome.Equals(d.Nome) && diagnostico.Orgao.Equals(d.Orgao))
                    {
                        return true;
                    }

                }
            }
            return false;
            
        }

        public class ComparacaoResultados : IComparer<string>
        {
            public int Compare(String x, String y)
            {
                String[] linhas;

                linhas = Convert.ToString(x).Split('|');
                String[] linhas2;
                linhas2 = Convert.ToString(y).Split('|');

                decimal x1 = Convert.ToDecimal(linhas[0]);
                decimal y2 = Convert.ToDecimal(linhas2[0]);
                //return x.CompareTo(y2);


                if (x1 > y2)
                    return -1;
                if (x1 <= y2)
                    return 0;

                return 1;
            }
        }
       

        

        public static bool verificaXSD(string xsdPath, string xmlPath)
        {
            Boolean _isValid = false;


            try
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.Schemas.Add(null, xsdPath);
                settings.ValidationType = ValidationType.Schema;

                XmlReader reader = XmlReader.Create(xmlPath, settings);
                XmlDocument document = new XmlDocument();
                document.Load(reader);

                ValidationEventHandler eventHandler = new ValidationEventHandler(ValidationEventHandler);

                _isValid = true;
                document.Validate(eventHandler);

            }
            catch (Exception ex)
            {
                _isValid = false;
              
            }

            return _isValid;
        }
        private static void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            switch (e.Severity)
            {
                case XmlSeverityType.Error:
                    System.Windows.Forms.MessageBox.Show("Error: " + e.Message + Environment.NewLine);
                    break;
                case XmlSeverityType.Warning:
                   System.Windows.Forms.MessageBox.Show("Warning: " + e.Message + Environment.NewLine);
                    break;
            }
        }
    }
}
