using System;

namespace EsercizioCompito8Nov.Model;

    public class Ordine
    {
        public int OrdineId { get; set; }
        public decimal ImportoTotale { get; set; }
        public DateTime DataOrdine { get; set; }
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }
    }
