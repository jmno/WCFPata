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
        UriTemplate = "getAllPacientesByTerapeuta?token={token}")]
        List<PacienteWEB> getAllPacientesByTerapeuta(string token);

        [OperationContract]
        [WebInvoke(Method = "GET",
        ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "getAllEpisodiosByIDPaciente?token={token}&idPaciente={idPaciente}")]
        List<EpisodioClinicoWEB> getAllEpisodiosByIDPaciente(string token, int idPaciente);

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

    }

    [DataContract]
    public class ContaWeb
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
