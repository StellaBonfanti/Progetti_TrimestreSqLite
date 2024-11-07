using System;

namespace EsercizioCompito8Nov.Model;

    public class Cliente
    {
        public int ClienteId { get; set; }
        public string Cognome { get; set; }
        public string Nome { get; set; }
        public DateTime DataNascita { get; set; }
        public string LuogoNascita { get; set; }
        public ICollection<Ordine> Ordini { get; set; }
    }
