//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WCFPata
{
    using System;
    using System.Collections.Generic;
    
    public partial class EpisodioClinico
    {
        public EpisodioClinico()
        {
            this.Sintoma = new HashSet<Sintoma>();
        }
    
        public int Id { get; set; }
        public System.DateTime data { get; set; }
        public string diagnostico { get; set; }
    
        public virtual ICollection<Sintoma> Sintoma { get; set; }
        public virtual Paciente Paciente { get; set; }
    }
}
