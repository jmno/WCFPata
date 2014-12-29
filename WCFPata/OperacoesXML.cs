using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace WCFPata
{
    public class OperacoesXML
    {

        
       

        public static List<ContaWEB> lerContasFicheiroXml(String path) 
        {
            List<ContaWEB> lista = new List<ContaWEB>();
            String xpath = "//Conta";
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            XmlNodeList nodeList = doc.SelectNodes(xpath);
            foreach (XmlNode node in nodeList)
            {
                ContaWEB c = new ContaWEB();
                c.username = node.ChildNodes[0].InnerText.ToString();
                c.password = node.ChildNodes[1].InnerText.ToString();
                c.isAdmin = Boolean.Parse(node.ChildNodes[2].InnerText.ToString());
                c.id = Convert.ToInt32(node.ChildNodes[3].InnerText.ToString());
                lista.Add(c);
                    

            }

            return lista;

        }
        public static void guardarXML(DadosWEB dados, String xmlPath)
        {

            if (dados != null)
            {

                XmlTextWriter escritor = new XmlTextWriter(xmlPath, System.Text.Encoding.UTF8);
                escritor.Formatting = Formatting.Indented;
                escritor.WriteStartDocument();
                escritor.WriteStartElement("PATA");
                escritor.WriteStartElement("ListaDeSintomas");

                foreach (SintomaWEB p in dados.listaSintomas)
                {
                    escritor.WriteStartElement("Sintoma");
                    escritor.WriteElementString("nome", p.nome);
                    escritor.WriteEndElement();
                }
                escritor.WriteEndElement();
                escritor.WriteStartElement("DiagnosticosETratamentos");
                foreach (DiagnosticoWEB o in dados.listaDiagnosticos)
                {
                    escritor.WriteStartElement("DiagnosticoETratamento");
                    escritor.WriteElementString("Orgao", o.orgao);
                    escritor.WriteElementString("Diagnostico", o.nome);
                    escritor.WriteElementString("Tratamento", o.tratamento);
                    escritor.WriteStartElement("ListaSintomas");
                    foreach (SintomaWEB s in o.listaSintomas)
                    {
                        escritor.WriteElementString("sintoma", s.nome);
                    }
                    escritor.WriteEndElement();
                    escritor.WriteEndElement();
                }
                escritor.WriteEndElement();
                escritor.WriteEndElement();
                escritor.WriteEndDocument();
                escritor.Close();
                
            }
            
        }

        public static String xpathExpression(String xmlPath, String xpath)
        {
            string res = String.Empty;
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlPath);
            XmlNodeList nodeList = doc.SelectNodes(xpath);
            foreach (XmlNode node in nodeList)
            {
                res += node.InnerXml;

            }
            return res;
        }

        public static List<SintomaWEB> getListaSintomas(String xmlPath)
        {
            List<SintomaWEB> lista = new List<SintomaWEB>();
            String xpath = "//ListaDeSintomas/Sintoma/nome";
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlPath);
            XmlNodeList nodeList = doc.SelectNodes(xpath);
            foreach (XmlNode node in nodeList)
            {
                SintomaWEB s = new SintomaWEB();
                s.nome = node.InnerText.ToString();
                lista.Add(s);

            }

            return lista;

        }


        public static List<SistemaPericialWEB> getListaSistemaPericial(List<SintomaWEB> lista, String xmlPath) 
        {
            List<SistemaPericialWEB> listaFinal = new List<SistemaPericialWEB>();


            String xpath = "";
            List<DiagnosticoWEB> listaDiagnosticos = new List<DiagnosticoWEB>();
            List<SintomaWEB> listaSintomas;
            List<Int32> bedjoras = new List<Int32>();
            DiagnosticoWEB diagAux;

            foreach (SintomaWEB s in lista)
            {
                xpath = "//ListaSintomas/sintoma[text()=\"" + s.nome + "\"]/../..";
                XmlDocument doc = new XmlDocument();
                doc.Load(xmlPath);
                XmlNodeList nodeList = doc.SelectNodes(xpath);
                foreach (XmlNode node in nodeList)
                {
                    listaSintomas = new List<SintomaWEB>();
                    foreach (XmlNode x in node.ChildNodes[3].ChildNodes)
                    {
                        SintomaWEB sin = new SintomaWEB();
                        sin.nome = x.InnerText;
                        listaSintomas.Add(sin);
                    }
                    diagAux = new DiagnosticoWEB();
                    diagAux.listaSintomas = listaSintomas;
                    diagAux.orgao = node.ChildNodes[0].InnerText;
                    diagAux.nome = node.ChildNodes[1].InnerText;
                    diagAux.tratamento = node.ChildNodes[2].InnerText;


                    //TESTE


                    if (exists(listaDiagnosticos, diagAux))
                    {
                        int indice = getIndice(listaDiagnosticos, diagAux);
                        bedjoras[indice] += 1;
                    }

                    else
                    {
                        listaDiagnosticos.Add(diagAux);
                        bedjoras.Add(1);
                    }
                }


            }

            foreach (DiagnosticoWEB d in listaDiagnosticos)
            {
                int aux = listaDiagnosticos.IndexOf(d);
                int count = bedjoras[aux];
                int auxcontagem = d.listaSintomas.Count();
                decimal bla = count / Convert.ToDecimal(auxcontagem);
                decimal resultado = Math.Round(bla * 100, 1);
                SistemaPericialWEB sis = new SistemaPericialWEB();
                sis.diagnostico = d.nome + " - " + d.orgao;
                sis.tratamento = d.tratamento;
                sis.score = resultado;
                listaFinal.Add(sis);

            }

            listaFinal.Sort(new ComparacaoResultados());

             

            return listaFinal;
        }

        //public static List<string> procuraSintomas(List<string> lista, String xmlPath)
        //{
        //    List<String> listaFinal = new List<string>();
        //    String xpath = "";
        //    List<Diagnostico> listaDiagnosticos = new List<Diagnostico>();
        //    List<Sintoma> listaSintomas;
        //    List<Int32> bedjoras = new List<Int32>();
        //    Diagnostico diagAux;

        //    foreach (String s in lista)
        //    {
        //        xpath = "//ListaSintomas/sintoma[text()=\"" + s + "\"]/../..";
        //        XmlDocument doc = new XmlDocument();
        //        doc.Load(xmlPath);
        //        XmlNodeList nodeList = doc.SelectNodes(xpath);
        //        foreach (XmlNode node in nodeList)
        //        {
        //            listaSintomas = new List<Sintoma>();
        //            foreach (XmlNode x in node.ChildNodes[3].ChildNodes)
        //            {
        //                listaSintomas.Add(new Sintoma(x.InnerText));
        //            }
        //            diagAux = new Diagnostico(node.ChildNodes[1].InnerText, node.ChildNodes[0].InnerText, node.ChildNodes[2].InnerText, listaSintomas);


        //            //TESTE


        //            if (exists(listaDiagnosticos, diagAux))
        //            {
        //                int indice = getIndice(listaDiagnosticos, diagAux);
        //                bedjoras[indice] += 1;
        //            }

        //            else
        //            {
        //                listaDiagnosticos.Add(diagAux);
        //                bedjoras.Add(1);
        //            }
        //        }


        //    }

        //    foreach (Diagnostico d in listaDiagnosticos)
        //    {
        //        int aux = listaDiagnosticos.IndexOf(d);
        //        int count = bedjoras[aux];
        //        int auxcontagem = d.ListSintomas.Count();
        //        decimal bla = count / Convert.ToDecimal(auxcontagem);
        //        decimal resultado = Math.Round(bla * 100, 1);
        //        listaFinal.Add(resultado + "|" + d.ToString());

        //    }

        //    listaFinal.Sort(new ComparacaoResultados());

        //    return listaFinal;
        //}

        public static int getIndice(List<DiagnosticoWEB> lista, DiagnosticoWEB d)
        {
            if (lista.Count > 0)
            {
                foreach (DiagnosticoWEB diagnostico in lista)
                {
                    if (diagnostico.nome.Equals(d.nome) && diagnostico.orgao.Equals(d.orgao))
                    {
                        return lista.IndexOf(diagnostico);
                    }
                }
            }
            return -1;
        }

        public static Boolean exists(List<DiagnosticoWEB> lista, DiagnosticoWEB d)
        {
            if (lista.Count > 0)
            {
                foreach (DiagnosticoWEB diagnostico in lista)
                {
                    if (diagnostico.nome.Equals(d.nome) && diagnostico.orgao.Equals(d.orgao))
                    {
                        return true;
                    }

                }
            }
            return false;

        }

        public class ComparacaoResultados : IComparer<SistemaPericialWEB>
        {
            public int Compare(SistemaPericialWEB x, SistemaPericialWEB y)
            {
               
                decimal x1 = x.score;
                decimal y2 = y.score;
                //return x.CompareTo(y2);


                if (x1 > y2)
                    return -1;
                if (x1 <= y2)
                    return 0;

                return 1;
            }
        }




        //public static bool verificaXSD(string xsdPath, string xmlPath)
        //{
        //    Boolean _isValid = false;


        //    try
        //    {
        //        XmlReaderSettings settings = new XmlReaderSettings();
        //        settings.Schemas.Add(null, xsdPath);
        //        settings.ValidationType = ValidationType.Schema;

        //        XmlReader reader = XmlReader.Create(xmlPath, settings);
        //        XmlDocument document = new XmlDocument();
        //        document.Load(reader);

        //        ValidationEventHandler eventHandler = new ValidationEventHandler(ValidationEventHandler);

        //        _isValid = true;
        //        document.Validate(eventHandler);

        //    }
        //    catch (Exception ex)
        //    {
        //        _isValid = false;

        //    }

        //    return _isValid;
        //}
        //private static void ValidationEventHandler(object sender, ValidationEventArgs e)
        //{
        //    switch (e.Severity)
        //    {
        //        case XmlSeverityType.Error:
        //            System.Windows.Forms.MessageBox.Show("Error: " + e.Message + Environment.NewLine);
        //            break;
        //        case XmlSeverityType.Warning:
        //            System.Windows.Forms.MessageBox.Show("Warning: " + e.Message + Environment.NewLine);
        //            break;
        //    }
        //}
    }
}