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
using System.Diagnostics;
using System.Web.Management;

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
        private static string FILEPATHXSD;

        public Service1()
        {

            this.contas = new Dictionary<string, ContaWEB>();
            this.tokens = new Dictionary<string, Token>();
            FILEPATH = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "App_Data", "teste.xml");
            FILEPATHXSD = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "App_Data", "teste.xsd");
            verificarDadosLogin();
            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-PT");
            //Paciente p = new Paciente();
            //p.dataNasc = DateTime.Now;
            //p.nome = "joaquim" + DateTime.Now.ToString();
            //p.cc = "1234";
            //p.morada = "menfoeifn";
            //p.sexo = "H";
            //p.telefone = "5555";
            //p.Terapeuta = handler.getTerapeutaByID(2);
            //handler.addPaciente(p);

        }
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
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
                HORAS = 1;
                this.value = Guid.NewGuid().ToString();
                this.dataLogin = dataLogin;
                this.dataExpirar = dataLogin.AddHours(HORAS);
                this.conta = conta;
            }

            public Token(ContaWEB conta, DateTime dataExpirar, DateTime dataLogin, string value)
            {

                this.value = value;
                this.dataExpirar = dataExpirar;
                this.conta = conta;
                this.dataLogin = dataLogin;
            }

      
            public string Value { get { return value; } set { this.value = value; } }
            public DateTime DataExpirar { get { return dataExpirar; } }
            public DateTime DataLogin { get { return dataLogin; } }
            public int Horas { get { return HORAS; } }

            public ContaWEB Conta { get { return conta; } }
            public string Username { get { return conta.username; } }


            //  public void UpdateTimeout() { UpdateTimeout(240000); }
            // public void UpdateTimeout(long timeout) { this.timeout = Environment.TickCount + timeout; }
            public Boolean isTimeOutExpired() { return dataExpirar < DateTime.Now; }

        }

        public List<TokenWeb> getTokens()
        {
            List<TokenWeb> listaTokensWeb = new List<TokenWeb>();
            foreach (KeyValuePair<String, Token> token in tokens)
            {
                TokenWeb tokenWeb = new TokenWeb();
                tokenWeb.conta = token.Value.Conta;
                tokenWeb.dataExpirar = token.Value.DataExpirar;
                tokenWeb.dataLogin = token.Value.DataLogin;
                tokenWeb.horas = token.Value.Horas;
                tokenWeb.value = token.Value.Value;

                tokenWeb.isTimedOutExpired = token.Value.isTimeOutExpired();
                listaTokensWeb.Add(tokenWeb);
            }

            return listaTokensWeb;
        }

        //authentication

        public string logIn(String username, String password)
        {
            cleanUpTokens(username);
            lercontasBD();

            if (!String.IsNullOrEmpty(username) && !String.IsNullOrEmpty(password) && password.Equals(contas[username].password))
            {
                Token tokenObject = new Token(contas[username]);
                tokens.Add(tokenObject.Value, tokenObject);

                DadosLogin d = new DadosLogin();
                d.Property1 = tokenObject.Value;
                d.dataLogin = tokenObject.DataLogin;
                d.dataExpirar = tokenObject.DataExpirar;
                d.idConta = tokenObject.Conta.id;
                handler.addDadoLogin(d);
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

        private void verificarDadosLogin()
        {
            try
            {
                List<DadosLogin> listaDadosLogin = handler.getListaDadosLogin();

                if (listaDadosLogin.Count > 0)
                {
                    foreach (DadosLogin d in listaDadosLogin)
                    {
                        if (isDadoLoginExpirado(d))
                        {
                            handler.removerDadosLogin(d.idConta);
                        }
                        else
                        {
                            Conta c = handler.getContaByID(d.idConta);
                            ContaWEB conta = new ContaWEB();
                            conta.id = c.Id;
                            conta.username = c.username;
                            conta.password = c.password;
                            conta.isAdmin = c.isAdmin;

                            Token t = new Token(conta, d.dataExpirar, d.dataLogin, d.Property1);

                            tokens.Add(d.Property1, t);
                        }
                    }
                }
            }
            catch (Exception) {
                throw new ArgumentException("ERROR: getting dados login");

            }

        }

        public bool isDadoLoginExpirado(DadosLogin d)
        {
            return DateTime.Now > d.dataExpirar;



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

        private void cleanUpTokens(string username)
        {
            string token = getToken(username);
            if (!token.Equals("NADA"))
            {
                Token t = tokens[token];
                handler.removerDadosLogin(t.Conta.id);
                tokens.Remove(token);
                
               

            }
            
        }

        private String getToken(string username)
        {
            string final = "NADA";
            foreach (KeyValuePair<String, Token> t in tokens)
            {
                if (t.Value.Username.Equals(username))
                    final = t.Value.Value;
            }
            return final;
        }

        private Token checkAuthentication(string token, bool mustBeAdmin)
        {
            Token tokenObject;
            if (String.IsNullOrEmpty(token))
            {
                new LogEvent("Error Is Null or Empty").Raise();
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
            try
            {
                checkAuthentication(token, false);
            }
            catch
            {
                throw new FaultException("Erro Token");
            }
            bool resultado = false;

            try
            {
                OperacoesXML.guardarXML(dados, FILEPATH,FILEPATHXSD);
                resultado = true;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
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
                pa.dataNasc = p.dataNasc.Day.ToString() + "/" + p.dataNasc.Month.ToString() + "/" + p.dataNasc.Year.ToString();
                pa.id = p.Id;
                pa.morada = p.morada;
                pa.nome = p.nome;
                pa.telefone = p.telefone;
                pa.sexo = p.sexo;

                try
                {

                    pa.terapeutaID = p.Terapeuta.Id;
                }
                catch (Exception e)
                {
                    lista.Add(pa);
                    continue;
                }
                lista.Add(pa);

            }

            return lista;
        }


        public List<ContaWEB> getAllContas(string token)
        {
            checkAuthentication(token, false);

            List<ContaWEB> listaContasWEB = new List<ContaWEB>();
            List<Conta> listaContas = handler.getAllContas();

            foreach (Conta conta in listaContas)
            {
                ContaWEB c = new ContaWEB();

                c.id = conta.Id;
                c.username = conta.username;
                c.password = conta.password;
                c.isAdmin = conta.isAdmin;
                listaContasWEB.Add(c);
            }

            return listaContasWEB;
        }

        public List<ContaWEB> getAllContasTerapeutas(string token)
        {
            checkAuthentication(token, false);

            List<ContaWEB> listaContasWEB = new List<ContaWEB>();
            List<Conta> listaContas = handler.getAllContasTerapeutas();

            foreach (Conta conta in listaContas)
            {
                ContaWEB c = new ContaWEB();

                c.id = conta.Id;
                c.username = conta.username;
                c.password = conta.password;
                c.isAdmin = conta.isAdmin;
                listaContasWEB.Add(c);
            }

            return listaContasWEB;
        }

        public List<TerapeutaWEB> getAllTerapeutas(string token)
        {
            checkAuthentication(token, false);

            List<TerapeutaWEB> listaTerapeutaWEB = new List<TerapeutaWEB>();
            List<Terapeuta> listaTerapeutas = handler.getAllTerapeutas();

            foreach (Terapeuta terapeuta in listaTerapeutas)
            {
                TerapeutaWEB t = new TerapeutaWEB();

                t.id = terapeuta.Id;
                t.nome = terapeuta.nome;
                t.dataNasc = terapeuta.dataNasc.Day.ToString() + "/" + terapeuta.dataNasc.Month.ToString() + "/" + terapeuta.dataNasc.Year.ToString();
                t.cc = terapeuta.cc;
                t.telefone = terapeuta.telefone;
                t.contaID = terapeuta.Conta.Id;

                listaTerapeutaWEB.Add(t);
            }

            return listaTerapeutaWEB;
        }



        public List<PacienteWEB> getAllPacientesByTerapeuta(string token)
        {

            checkAuthentication(token, false);

            int idConta = Convert.ToInt32(tokens[token].Conta.id.ToString());
            List<PacienteWEB> lista = new List<PacienteWEB>();
            List<Paciente> listaP = handler.getAllPacientesByTerapeuta(idConta);

            foreach (Paciente p in listaP)
            {
                PacienteWEB pa = new PacienteWEB();
                pa.cc = p.cc;
                pa.dataNasc = p.dataNasc.Day.ToString() + "/" + p.dataNasc.Month.ToString() + "/" + p.dataNasc.Year.ToString();
                pa.id = p.Id;
                pa.morada = p.morada;
                pa.nome = p.nome;
                pa.telefone = p.telefone;
                pa.terapeutaID = p.Terapeuta.Id;
                pa.sexo = p.sexo;
                lista.Add(pa);
            }

            return lista;
        }
        public String getNomeTerapeuta(string token)
        {

            checkAuthentication(token, false);
            String resultado = "Apenas para Terapeutas";
            int idConta = Convert.ToInt32(tokens[token].Conta.id.ToString());
            Boolean admin = Convert.ToBoolean(tokens[token].Conta.isAdmin);
            if (!admin)
            {
                Terapeuta t = handler.getTerapeutaByID(idConta);
                resultado = t.nome;
            }
            return resultado;
        }
        public List<EpisodioClinicoWEB> getAllEpisodiosByIDPaciente(string token, int idPaciente)
        {
            checkAuthentication(token, false);

            List<EpisodioClinicoWEB> lista = new List<EpisodioClinicoWEB>();
            List<EpisodioClinico> listaE = handler.getAllEpisodiosByIDPaciente(idPaciente);

            foreach (EpisodioClinico p in listaE)
            {
                EpisodioClinicoWEB pa = new EpisodioClinicoWEB();
                pa.data = p.data.Day.ToString() + "/" + p.data.Month.ToString() + "/" + p.data.Year.ToString();
                pa.diagnostico = p.diagnostico;
                pa.id = p.Id;
                pa.idPaciente = p.Paciente.Id;
                lista.Add(pa);
            }


            return lista;

        }

        public List<SistemaPericialWEB> getListaSistemaPericial(string token, List<SintomaWEB> lista)
        {
            checkAuthentication(token, false);

            List<SistemaPericialWEB> listaFinal = new List<SistemaPericialWEB>();

            listaFinal = OperacoesXML.getListaSistemaPericial(lista, FILEPATH);
            return listaFinal;

        }

        public bool addPaciente(string token, PacienteWEB paciente)
        {
            checkAuthentication(token, false);
            bool resultado = false;
            int idConta = Convert.ToInt32(tokens[token].Conta.id.ToString());
            Paciente p = new Paciente();
            p.cc = paciente.cc;
            p.dataNasc = getData(paciente.dataNasc.ToString());
            p.morada = paciente.morada;
            p.nome = paciente.nome;
            p.telefone = paciente.telefone;
            p.sexo = paciente.sexo;
            p.Terapeuta = handler.getTerapeutaByID(idConta);
            resultado = handler.addPaciente(p);

            return resultado;
        }

        public bool addPacienteClienteAdmin(string token, PacienteWEB paciente)
        {
            checkAuthentication(token, false);
            bool resultado = false;
            Paciente p = new Paciente();
            if (paciente.terapeutaID != 0)
            {
                p.Terapeuta = handler.getTerapeutaByHisID(paciente.terapeutaID);
            }

            p.cc = paciente.cc;
            p.dataNasc = getData(paciente.dataNasc.ToString());
            p.morada = paciente.morada;
            p.nome = paciente.nome;
            p.telefone = paciente.telefone;
            p.sexo = paciente.sexo;


            resultado = handler.addPaciente(p);

            return resultado;
        }

        public bool editPaciente(string token, PacienteWEB paciente)
        {
            checkAuthentication(token, false);
            bool resultado = false;

            int idConta = Convert.ToInt32(tokens[token].Conta.id.ToString());
            Paciente p = new Paciente();
            p.cc = paciente.cc;
            p.dataNasc = getData(paciente.dataNasc.ToString());
            p.morada = paciente.morada;
            p.nome = paciente.nome;
            p.telefone = paciente.telefone;
            p.Terapeuta = handler.getTerapeutaByID(idConta);
            p.Id = paciente.id;
            p.sexo = paciente.sexo;
            resultado = handler.editPaciente(p);

            return resultado;
        }

        public bool editPacienteClienteAdmin(string token, PacienteWEB paciente)
        {
            checkAuthentication(token, false);
            bool resultado = false;
            Paciente p = new Paciente();
            if (paciente.terapeutaID == 0)
            {
                handler.removeTerapeutaFromPaciente(paciente.id);

            }
            else
            {
                p.Terapeuta = handler.getTerapeutaByHisID(paciente.terapeutaID);
            }

            p.cc = paciente.cc;
            p.dataNasc = getData(paciente.dataNasc.ToString());
            p.morada = paciente.morada;
            p.nome = paciente.nome;
            p.telefone = paciente.telefone;

            p.Id = paciente.id;
            p.sexo = paciente.sexo;
            resultado = handler.editPacienteClienteAdmin(p);

            return resultado;
        }

        public bool editTerapeuta(string token, TerapeutaWEB terapeuta, ContaWEB conta)
        {
            checkAuthentication(token, false);
            bool resultado = false;

            Conta contaBd = new Conta();
            contaBd.Id = conta.id;
            contaBd.isAdmin = conta.isAdmin;
            contaBd.username = conta.username;
            contaBd.password = conta.password;

            Terapeuta t = new Terapeuta();
            t.Id = terapeuta.id;
            t.nome = terapeuta.nome;
            t.cc = terapeuta.cc;
            t.dataNasc = getData(terapeuta.dataNasc);
            t.telefone = terapeuta.telefone;

            resultado = handler.editTerapeuta(t, contaBd);

            return resultado;
        }

        public bool editConta(string token, ContaWEB conta)
        {
            checkAuthentication(token, false);
            bool resultado = false;

            Conta c = new Conta();
            c.Id = conta.id;
            c.username = conta.username;
            c.password = conta.password;
            c.isAdmin = conta.isAdmin;

            resultado = handler.editConta(c);


            return resultado;
        }

        public bool removeTerapeuta(string token, int idContaTerapeuta, int idTerapeuta)
        {
            bool resultado = false;



            checkAuthentication(token, false);
            resultado = handler.removeTerapeuta(idContaTerapeuta, idTerapeuta);



            return resultado;



        }

        public DateTime getData(string data)
        {
            DateTime result = new DateTime();
            DateTime date = new DateTime();
            if (DateTime.TryParseExact(data, "dd'/'MM'/'yyyy",
                           CultureInfo.InvariantCulture,
                           DateTimeStyles.None,
                           out date))
                result = date;



            return result;
        }

        public bool addConta(string token, ContaWEB conta)
        {
            bool resultado = false;
            checkAuthentication(token, false);
            int res = -1;
            Conta c = new Conta();
            c.username = conta.username;
            c.password = conta.password;
            c.isAdmin = conta.isAdmin;
            try
            {
                res = handler.addConta(c);
                if (res != -1)
                    resultado = true;
            }

            catch
            {
                resultado = false;

            }
            return resultado;
        }

        public bool addTerapeuta(string token, TerapeutaWEB terapeuta, ContaWEB conta)
        {
            bool resultado = false;
            int idConta = -1;
            checkAuthentication(token, false);

            Conta c = new Conta();
            c.username = conta.username;
            c.password = conta.password;
            c.isAdmin = conta.isAdmin;
            idConta = handler.addConta(c);

            if (idConta != -1)
            {
                Terapeuta t = new Terapeuta();
                t.Conta = handler.getContaByID(idConta);
                t.nome = terapeuta.nome;
                t.cc = terapeuta.cc;
                t.dataNasc = getData(terapeuta.dataNasc);
                t.telefone = terapeuta.telefone;
                resultado = handler.addTerapeuta(t);
            }

            return resultado;
        }
        public bool addEpisodioClinico(string token, EpisodioClinicoWEB episodio)
        {
            checkAuthentication(token, false);
            bool resultado = false;
            EpisodioClinico e = new EpisodioClinico();
            e.data = getData(episodio.data);
            e.diagnostico = episodio.diagnostico;
            e.Paciente = handler.getPacienteByID(episodio.idPaciente);

            resultado = handler.addEpisodio(e);

            return resultado;
        }

        public bool removeTerapeutaFromPaciente(string token, int idPaciente)
        {
            checkAuthentication(token, false);
            bool resultado = false;

            resultado = handler.removeTerapeutaFromPaciente(idPaciente);

            return resultado;

        }

        public bool removePaciente(string token, int idPaciente)
        {
            bool resultado = false;
            checkAuthentication(token, false);

            bool resRemAllEp = handler.removeAllEpisodiosFromPaciente(idPaciente);
            if (resRemAllEp)
            {
                resultado = handler.removePaciente(idPaciente);
            }

            return resultado;

        }

        public String removeConta(string token, int idConta)
        {

            checkAuthentication(token, false);
            String resultado = "Não é possível apagar a sua própria conta!";
            int id = Convert.ToInt32(tokens[token].Conta.id.ToString());
            bool admin = Convert.ToBoolean(tokens[token].Conta.isAdmin);

            if (id != idConta && admin)
            {
                Conta c = handler.getContaByID(idConta);
                if (c.isAdmin)
                {
                    bool res = handler.removeConta(idConta);
                    if (res)
                    {
                        resultado = "Administrador removido com sucesso";
                    }
                    else
                    {
                        resultado = "Não foi possível remover a conta";
                    }
                }
                else
                {
                    resultado = "A conta que está a tentar remover não é de administrador!";
                }

            }
            return resultado;
        }


        public class LogEvent : WebRequestErrorEvent
        {
            public LogEvent(string message)
                : base(null, null, 100001, new Exception(message))
            {
            }
        }
    }



}
