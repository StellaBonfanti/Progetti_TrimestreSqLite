using System;
using EsercizioCompito8Nov.Model;
using Microsoft.EntityFrameworkCore;

namespace EsercizioCompito8Nov.Data;

        public class ClientiContext : DbContext
        {
            //clienti context è un database context (context = ambiente), ovvero il collegamento tra il database e il programma
            //questo fa si che io posso modificare qualsiasi oggetto della classe clienticontext senza modificare direttamente il database
            //OVVIAMENTE! non è saggio aprire 2 variabili dbcontext insieme... rischi conflitti
            //questo definisce tutte le proprietà di un QUALSIASI clientcontext
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
