using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace WCFPata
{
    public class HandlerBD
    {

        ModelContainer modelo = new ModelContainer();
        /*
         * Autenticacao
         * */

        public String logIn(string username, string password)
        {
            String texto = "";
            Conta c = new Conta();
            try
            {
                c = modelo.ContaSet.Where(i => i.username == username).First();
                if (c.password == password)
                    texto = "S";
                else
                    texto = "N";
            }
            catch (Exception e)
            {
                texto = "User not available";
                Console.WriteLine(e.ToString());
            }

            return texto;
        }

        /**/

        public List<Conta> getContas()
        {
            List<Conta> listaContas = new List<Conta>();
            listaContas = modelo.ContaSet.ToList();

            return listaContas;
        }

        public List<Paciente> getAllPacientes()
        {
            return modelo.PacienteSet.ToList();
        }

        public List<Paciente> getAllPacientesByTerapeuta(int idConta)
        {
            int idTerapeuta = modelo.TerapeutaSet.Where(i => i.Conta.Id == idConta).First().Id;
            return modelo.PacienteSet.Where(i => i.Terapeuta.Id == idTerapeuta).ToList();
        }


        public List<EpisodioClinico> getAllEpisodiosByIDPaciente(int idPaciente)
        {

            return modelo.EpisodioClinicoSet.Where(i => i.Paciente.Id == idPaciente).ToList();
        }

        public bool addPaciente(Paciente p)
        {

            try
            {
                modelo.PacienteSet.Add(p);
                modelo.SaveChanges();
                return true;
            }
            catch { return false; }
        }

        public Terapeuta getTerapeutaByID(int idConta)
        {
            return modelo.TerapeutaSet.Where(i => i.Conta.Id == idConta).First();
        }



        public bool editPaciente(Paciente p)
        {

            try
            {
                Paciente paciente = modelo.PacienteSet.Single(i => i.Id == p.Id);
                paciente.morada = p.morada;
                paciente.telefone = p.telefone;
                paciente.nome = p.nome;
                paciente.cc = p.cc;
                paciente.sexo = p.sexo;
                paciente.dataNasc = p.dataNasc;
                modelo.SaveChanges();
                return true;
            }
            catch { return false; }
        }

        public void addConta(Conta conta)
        {
           
             modelo.ContaSet.Add(conta);
                modelo.SaveChanges();
        }

        public Conta getContaByID(int idConta)
        {
           
            return modelo.ContaSet.Where(i => i.Id == idConta).First();
        }

        public string addTerapeuta(Terapeuta t)
        {

            try
            {
                modelo.TerapeutaSet.Add(t);
                modelo.SaveChanges();
                return "ok";
            }
            catch(Exception e)
            {
                return "Excecao "+e.StackTrace+"\n"+e;
            }
        }

        public bool addEpisodio(EpisodioClinico e)
        {

            try
            {
                modelo.EpisodioClinicoSet.Add(e);
                modelo.SaveChanges();
                return true;
            }
            catch { return false; }
        }



    }
}