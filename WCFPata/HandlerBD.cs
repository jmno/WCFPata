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

    }
}