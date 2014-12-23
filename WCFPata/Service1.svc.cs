using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Globalization;
using System.IO;
using System.Web.Hosting;

namespace WCFPata
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
        
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]

    public class Service1 : IService1
    {

        HandlerBD handler = new HandlerBD();
        private Dictionary<string, ContaWEB> contas;
        private Dictionary<string, Token> tokens;
        private static string FILEPATH;

        public Service1() {

            this.contas = new Dictionary<string, ContaWEB>();
            this.tokens = new Dictionary<string, Token>();
            FILEPATH = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "App_Data", "teste.xml");
        }
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

        private class Token
        {
            private string value;
            private DateTime dataLogin;
            private DateTime dataExpirar;
            private int HORAS;
            private ContaWEB conta;

            public Token(ContaWEB conta) : this(conta, DateTime.Now) { }

            public Token(ContaWEB conta, DateTime dataLogin)
            {
                HORAS = 10;
                this.value = Guid.NewGuid().ToString();
                this.dataLogin = dataLogin;
                this.dataExpirar = dataLogin.AddHours(HORAS);
                this.conta = conta;
            }

            public string Value { get { return value; } }
            public DateTime DataExpirar { get { return dataExpirar; } }
            public ContaWEB Conta { get { return conta; } }
            public string Username { get { return conta.username; } }
            //  public void UpdateTimeout() { UpdateTimeout(240000); }
            // public void UpdateTimeout(long timeout) { this.timeout = Environment.TickCount + timeout; }
            public Boolean isTimeOutExpired() { return dataExpirar < DateTime.Now; }

        }

        //authentication

        public string logIn(String username, String password)
        {
            cleanUpTokens();
            lercontasBD();

            if (!String.IsNullOrEmpty(username) && !String.IsNullOrEmpty(password) && password.Equals(contas[username].password))
            {
                Token tokenObject = new Token(contas[username]);
                tokens.Add(tokenObject.Value, tokenObject);
                return tokenObject.Value;
            }
            else
            {
                throw new ArgumentException("ERROR: invalid username/password combination");
            }
            // return handler.logIn(username, password);
        }

        private void lercontasBD()
        {
            List<ContaWEB> listaContasWeb = new List<ContaWEB>();
            List<Conta> listaConta = handler.getContas();

            foreach (Conta c in listaConta)
            {
                ContaWEB con = new ContaWEB();
                con.username = c.username;
                con.password = c.password;
                con.isAdmin = c.isAdmin;
                con.id = c.Id;
                if (!verificaConta(con))
                    contas.Add(con.username, con);
            }



        }

        private bool verificaConta(ContaWEB con)
        {
            foreach (KeyValuePair<String, ContaWEB> c in contas)
            {
                if (c.Value.username.Equals(con.username))
                    return true;
            }
            return false;
        }

        public void logOut(string token)
        {
            tokens.Remove(token);
            cleanUpTokens();

        }

        public bool isAdmin(string token)
        {
            return tokens[token].Conta.isAdmin;
        }

        public bool isLoggedIn(string token)
        {
            bool res = true;
            try
            {
                checkAuthentication(token, false);
            }
            catch (ArgumentException)
            {
                res = false;
            }
            return res;
        }

        private void cleanUpTokens()
        {
            foreach (Token tokenObject in tokens.Values)
            {

                tokens.Remove(tokenObject.Username);

            }
        }

        private Token checkAuthentication(string token, bool mustBeAdmin)
        {
            Token tokenObject;
            if (String.IsNullOrEmpty(token))
            {
                throw new ArgumentException("Error: invalid token value.");
            }
            try
            {
                tokenObject = tokens[token];
            }
            catch (KeyNotFoundException)
            {
                throw new ArgumentException("Error: user is not logged in /expired session?).");
            }
            if (tokenObject.isTimeOutExpired())
            {
                tokens.Remove(tokenObject.Username);
                throw new Exception("Error: the session has expired. Please Loged in again.");
            }
            if (mustBeAdmin && !tokens[token].Conta.isAdmin)
            {
                throw new ArgumentException("Error: only admins are allowed to perform this operation.");
            }
            return tokenObject;

        }



        //fim authentication



            //Escrever XML NO APPDATA
        public bool carregaXml(string token, DadosWEB dados)
        {
            
            checkAuthentication(token, false);
            bool resultado = false;
            
            try
            {
                OperacoesXML.guardarXML(dados, FILEPATH);
                resultado = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                // resultado = false;
            }


            return resultado;
        }

            //Ler dados XML do APPDATA
        public List<SintomaWEB> lerSintomasXML(string token)
        {
            //
            checkAuthentication(token, false);
            //bool resultado = false;
            List<SintomaWEB> lista = new List<SintomaWEB>();
            try
            {
                lista = OperacoesXML.getListaSintomas(FILEPATH);
                //resultado = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                // resultado = false;
            }


            return lista;
        }
        public List<PacienteWEB> getAllPacientes(string token)
        {
            checkAuthentication(token, false);

            List<PacienteWEB> lista = new List<PacienteWEB>();
            List<Paciente> listaP = handler.getAllPacientes();

            foreach (Paciente p in listaP) 
            {
                PacienteWEB pa = new PacienteWEB();
                pa.cc = p.cc;
                pa.dataNasc = p.dataNasc;
                pa.id = p.Id;
                pa.morada = p.morada;
                pa.nome = p.nome;
                pa.telefone = p.telefone;
                pa.terapeutaID = p.Terapeuta.Id;
                lista.Add(pa);
            }

            return lista;
        }

    }
}
