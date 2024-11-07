using System;
using EsercizioCompito8Nov.Model;
using Microsoft.EntityFrameworkCore;

namespace EsercizioCompito8Nov.Data;

        public class ClientiContext : DbContext
        {
            //clienti context è un database context, ovvero il collegamento tra il database e il programma
            //questo fa si che io posso modificare qualsiasi oggetto della classe clienticontext senza modificare direttamente il database
            //quindi alla fine crei una classe e poi ovviamente un oggetto dbcontext solo per non toccare il database direttamente, 
            //ma far si che interagisca con il programma
            //OVVIAMENTE! non è saggio aprire 2 variabili dbcontext insieme... rischi conflitti
            public DbSet<Cliente> Clienti { get; set; } = null!;
            public DbSet<Ordine> Ordini { get; set; } = null!;
            public string DbPath { get; }
        
            public ClientiContext()
            {
                var folder = AppContext.BaseDirectory;
                var path = Path.Combine(folder, "../../../Clienti.db");
                DbPath = path;
            }
        
            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
                => optionsBuilder.UseSqlite($"Data Source={DbPath}");
        
        }
