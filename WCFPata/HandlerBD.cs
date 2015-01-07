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

        public List<Conta> getAllContas()
        {
            return modelo.ContaSet.Where(x => x.isAdmin == true).ToList();
        }

        public List<Conta> getAllContasTerapeutas()
        {
            return modelo.ContaSet.Where(x => x.isAdmin == false).ToList();
        }

        public List<Terapeuta> getAllTerapeutas()
        {
            return modelo.TerapeutaSet.ToList();
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

        public bool editTerapeuta(Terapeuta t,Conta c)
        {

            try
            {

                Conta conta = getContaByID(c.Id);
                conta.username = c.username;
                conta.password = c.password;

                modelo.SaveChanges();

                Terapeuta terapeuta = modelo.TerapeutaSet.Where(i => i.Id == t.Id).First();
                terapeuta.nome = t.nome;
                terapeuta.cc = t.cc;
                terapeuta.dataNasc = t.dataNasc;
                terapeuta.telefone = t.telefone;
                modelo.SaveChanges();
                Terapeuta t1 = modelo.TerapeutaSet.Where(i => i.Id == terapeuta.Id).First();

                return true;
            }
            catch { return false; }
        }

        public bool editConta(Conta c)
        {
            try
            {
                Conta conta = modelo.ContaSet.Where(x => x.Id == c.Id).First();
                conta.username = c.username;
                conta.password = c.password;
                modelo.SaveChanges();

                return true;
            }
            catch { return false; }
        }

        public int addConta(Conta conta)
        {
            int id = -1;
            try
            {
               modelo.ContaSet.Add(conta);
                modelo.SaveChanges();

                id=conta.Id;
                return id;
            }
            catch
            {
                return id;
            }

        }

        public Conta getContaByID(int idConta)
        {

            return modelo.ContaSet.Where(i => i.Id == idConta).First();
        }
        public Paciente getPacienteByID(int idPaciente)
        {

            return modelo.PacienteSet.Where(i => i.Id == idPaciente).First();
        }

        public bool addTerapeuta(Terapeuta t)
        {

            try
            {
                modelo.TerapeutaSet.Add(t);
                modelo.SaveChanges();
                return true;
            }
            catch
            {
                return false;
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

        public bool removeTerapeutaFromPaciente(int idPaciente)
        {
            try
            {
                Paciente p = modelo.PacienteSet.Where(i => i.Id == idPaciente).First();
                modelo.Entry(p).Reference(c => c.Terapeuta).CurrentValue = null;
                modelo.SaveChanges();

                return true;
            }
            catch { return false; }
        }

        public bool removeConta(int idConta)
        {

            try
            {

            

                Conta c = modelo.ContaSet.Where(x => x.Id == idConta).First();

                modelo.ContaSet.Remove(c);
                modelo.SaveChanges();
                return true;
            }
            catch { return false; }

        }

        public bool removeTerapeuta(int idContaTerapeuta, int idTerapeuta)
        {
            bool result=false;
            
            try
            {
                List<Paciente> listaPacientes = getAllPacientesByTerapeuta(idContaTerapeuta).ToList();

                foreach (Paciente p in listaPacientes)
                {
                    if (p.Terapeuta.Id == idTerapeuta)
                    {
                        removeTerapeutaFromPaciente(p.Id);
                    }
                }
              
                    Terapeuta t = modelo.TerapeutaSet.Where(x=>x.Id==idTerapeuta).First();
                    modelo.TerapeutaSet.Remove(t);
                    modelo.SaveChanges();
                    removeConta(idContaTerapeuta);
                    result = true;
              
             

                
               
            }
            catch { result = false; }
            return result;
        }





    }
}