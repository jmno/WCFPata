using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WCFPata
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {
        /*
         * AUTHENTICATION
         * */
        [OperationContract]
        [WebInvoke(Method = "POST",
        ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "login?username={username}&password={password}")]
        string logIn(string username, string password);


        [OperationContract]
        [WebInvoke(Method = "POST",
        ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "logout")]
        void logOut(string token);


        [OperationContract]
        [WebInvoke(Method = "GET",
        ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "isAdmin?token={token}")]
        bool isAdmin(string token);

        [OperationContract]
        [WebInvoke(Method = "GET",
        ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "isLoggedIn?token={token}")]
        bool isLoggedIn(string token);


        /*
         *  END AUTHENTICATION
         * 
         */
        [OperationContract]
        [WebInvoke(Method = "POST",
        ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "carregaXml?token={token}")]
        bool carregaXml(string token, DadosWEB dados);

        [OperationContract]
        [WebInvoke(Method = "GET",
        ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "lerSintomasXML?token={token}")]
        List<SintomaWEB> lerSintomasXML(string token);

        [OperationContract]
        [WebInvoke(Method = "GET",
        ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "getAllPacientes?token={token}")]
        List<PacienteWEB> getAllPacientes(string token);

        [OperationContract]
        [WebInvoke(Method = "GET",
        ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "getAllContas?token={token}")]
        List<ContaWEB> getAllContas(string token);

        [OperationContract]
        [WebInvoke(Method = "GET",
        ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "getAllContasTerapeutas?token={token}")]
        List<ContaWEB> getAllContasTerapeutas(string token);

        [OperationContract]
        [WebInvoke(Method = "Get",
        ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "getAllTerapeutas?token={token}")]
        List<TerapeutaWEB> getAllTerapeutas(string token);

        [OperationContract]
        [WebInvoke(Method = "GET",
        ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "getAllPacientesByTerapeuta?token={token}")]
        List<PacienteWEB> getAllPacientesByTerapeuta(string token);


        [OperationContract]
        [WebInvoke(Method = "GET",
        ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "getNomeTerapeuta?token={token}")]
        String getNomeTerapeuta(string token);

        [OperationContract]
        [WebInvoke(Method = "GET",
        ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "getAllEpisodiosByIDPaciente?token={token}&idPaciente={idPaciente}")]
        List<EpisodioClinicoWEB> getAllEpisodiosByIDPaciente(string token, int idPaciente);

        [OperationContract]
        [WebInvoke(Method = "POST",
        ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "getListaSistemaPericial?token={token}")]
        List<SistemaPericialWEB> getListaSistemaPericial(string token, List<SintomaWEB> lista);

        [OperationContract]
        [WebInvoke(Method = "POST",
        ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "addPaciente?token={token}")]
        bool addPaciente(string token, PacienteWEB paciente);


        [OperationContract]
        [WebInvoke(Method = "POST",
        ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "editPaciente?token={token}")]
        bool editPaciente(string token, PacienteWEB paciente);

        [OperationContract]
        [WebInvoke(Method = "POST",
         BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        bool editTerapeuta(string token, TerapeutaWEB terapeuta, ContaWEB conta);

        [OperationContract]
        [WebInvoke(Method = "POST",
        ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "editConta?token={token}")]
        bool editConta(string token, ContaWEB conta);

        [OperationContract]
        [WebInvoke(Method = "POST",
        ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "addConta?token={token}")]
        bool addConta(string token, ContaWEB conta);

        [OperationContract]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "addTerapeuta?token={token}")]
        bool addTerapeuta(string token, TerapeutaWEB terapeuta, ContaWEB conta);

        [OperationContract]
        [WebInvoke(Method = "POST",
        ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "addEpisodioClinico?token={token}")]
        bool addEpisodioClinico(string token, EpisodioClinicoWEB episodio);

        [OperationContract]
        [WebInvoke(Method = "POST",
        ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "removeTerapeutaFromPaciente?token={token}&idPaciente={idPaciente}")]
        bool removeTerapeutaFromPaciente(string token, int idPaciente);

        [OperationContract]
        [WebInvoke(Method = "POST",
        ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "removeConta?token={token}&idConta={idConta}")]
        string removeConta(string token, int idConta);



    }


    [DataContract]
    public class PacienteWEB
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string nome { get; set; }
        [DataMember]
        public string dataNasc { get; set; }
        [DataMember]
        public string morada { get; set; }
        [DataMember]
        public string cc { get; set; }
        [DataMember]
        public string telefone { get; set; }
        [DataMember]
        public int terapeutaID { get; set; }
        [DataMember]
        public string sexo { get; set; }

    }

    [DataContract]
    public class TerapeutaWEB
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string nome { get; set; }
        [DataMember]
        public string dataNasc { get; set; }
        [DataMember]
        public string telefone { get; set; }
        [DataMember]
        public string cc { get; set; }
        [DataMember]
        public int contaID { get; set; }



    }

    [DataContract]
    public class ContaWEB
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string username { get; set; }
        [DataMember]
        public string password { get; set; }
        [DataMember]
        public Boolean isAdmin { get; set; }

    }


    [DataContract]
    public class SintomaWEB
    {
        [DataMember]
        public string nome { get; set; }

    }

    [DataContract]
    public class DiagnosticoWEB
    {
        [DataMember]
        public string nome { get; set; }
        [DataMember]
        public List<SintomaWEB> listaSintomas { get; set; }
        [DataMember]
        public string orgao { get; set; }
        [DataMember]
        public string tratamento { get; set; }

    }

    [DataContract]
    public class SistemaPericialWEB
    {
        [DataMember]
        public string diagnostico { get; set; }
        [DataMember]
        public string tratamento { get; set; }
        [DataMember]
        public decimal score { get; set; }

    }

    [DataContract]
    public class DadosWEB
    {

        [DataMember]
        public List<SintomaWEB> listaSintomas { get; set; }
        [DataMember]
        public List<DiagnosticoWEB> listaDiagnosticos { get; set; }


    }


    [DataContract]
    public class EpisodioClinicoWEB
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string data { get; set; }
        [DataMember]
        public string diagnostico { get; set; }
        [DataMember]
        public List<SintomaWEB> listaSintomas { get; set; }
        [DataMember]
        public int idPaciente { get; set; }
    }






}
