using System;
using System.Collections.Generic;
using System.Linq;
using EsercizioCompito8Nov.Data; //uso l'altra sottocartella
using EsercizioCompito8Nov.Model; //così che uso la sottocartella
using Microsoft.EntityFrameworkCore;

namespace EsercizioCompito8Nov
{
    class Program
    {
            static void Main(string[] args)
            {
                Console.WriteLine("Scegli un'azione:");
                Console.WriteLine("1. Popola il database");
                Console.WriteLine("2. Visualizza tutti gli ordini");
                Console.WriteLine("3. Visualizza gli ordini di un cliente specifico");
                Console.WriteLine("4. Modifica un ordine");
                Console.WriteLine("5. Cancella un ordine");
                Console.WriteLine("6. Esci");
            
                int scelta;
                while (!int.TryParse(Console.ReadLine(), out scelta))
                {
                    Console.WriteLine("Scelta non valida. Inserisci un numero.");
                }
            
                using (var db = new ClientiContext()) //using così da non doverlo continuamente ricrearlo ogni volta
                //creando una variabile generica db dopo un po' si "chiude" e viene distrutto.
                {
                    //clienticontext() crea un oggetto uguale (istanza) chiamato db.
                    //db è quindi un clientcontext con le proprietà messe prima
                    //questa istanza è separata, ha infatti le sue proprietà e metodi che possono essere utilizzati per eseguire operazioni
                    //sul database stesso.
                    //quindi crea una copia che si collega alla originale per non modificare la originale direttamente

                    //non puoi usare la classe clientcontext perchè visual è così, quindi devi creare una variabile per richiamare
                    //i vari metodi/proprietà
                    System.Console.WriteLine("Scegli tra 1: popola il database");
                    System.Console.WriteLine("2: visualizza tutti gli ordini");
                    System.Console.WriteLine("3: visualizza gli ordini di un cliente specifico");
                    System.Console.WriteLine("4: modifica un ordine");
                    System.Console.WriteLine("5: cancella un ordine");
                    System.Console.WriteLine("6: esci");
                    switch (scelta)
                    {
                        case 1:
                            PopolaDatabase(db);
                            break;
                        case 2:
                            VisualizzaOrdini(db);
                            break;
                        case 3:
                            Console.Write("Inserisci il cognome del cliente: ");
                            string cognome = Console.ReadLine();
                            Console.Write("Inserisci il nome del cliente: ");
                            string nome = Console.ReadLine();
                            VisualizzaOrdineCliente(db, cognome, nome);
                            break;
                        case 4:
                            Console.Write("Inserisci l'id dell'ordine da modificare: ");
                            int ordineId;
                            while (!int.TryParse(Console.ReadLine(), out ordineId))
                            {
                                Console.WriteLine("Id non valido. Inserisci un numero.");
                            }
                            Console.Write("Inserisci il nuovo totale: ");
                            decimal nuovoTotale;
                            while (!decimal.TryParse(Console.ReadLine(), out nuovoTotale))
                            {
                                Console.WriteLine("Totale non valido. Inserisci un numero.");
                            }
                            ModificaOrdine(db, ordineId, nuovoTotale);
                            break;
                        case 5:
                            Console.Write("Inserisci l'id dell'ordine da cancellare: ");
                            int ordineIdDaCancellare;
                            while (!int.TryParse(Console.ReadLine(), out ordineIdDaCancellare))
                            {
                                Console.WriteLine("Id non valido. Inserisci un numero.");
                            }
                            CancellaOrdine(db, ordineIdDaCancellare);
                            break;
                        case 6:
                            Environment.Exit(0);
                            break;
                        default:
                            Console.WriteLine("Scelta non valida. Riprova.");
                            break;
                    }
                }
                Console.ReadKey();
            }

        static void PopolaDatabase(ClientiContext db)
        {
            // Creazione dei Clienti
            List<Cliente> listaClienti =
            [
                new (){ Cognome= "Rossi", Nome= "Mario", DataNascita= new DateTime(1980, 1, 1), LuogoNascita= "Roma"},
                new (){Cognome= "Bianchi", Nome= "Luca", DataNascita= new DateTime(1985, 6, 15), LuogoNascita= "Milano"},
                new (){Cognome= "Verdi", Nome= "Giovanni", DataNascita= new DateTime(1990, 3, 20), LuogoNascita= "Napoli"}
            ];
        
            // Creazione della lista degli Ordini
            List<Ordine> listaOrdini =
            [
                new (){DataOrdine= DateTime.Now.Date, ImportoTotale = 1200.45m, ClienteId = 1},
                new (){DataOrdine= DateTime.Now.AddDays(-5).Date, ImportoTotale = 3200.65m, ClienteId = 1},
                new (){DataOrdine= new DateTime(2019,10,20).Date, ImportoTotale = 5200.45m, ClienteId = 1},
                new (){DataOrdine= DateTime.Now.Date, ImportoTotale = 5200.45m, ClienteId = 2},
                new (){DataOrdine= new DateTime(2019,08,20).Date, ImportoTotale = 7200.45m, ClienteId = 2}
            ];
        
            // Inserire clienti e ordini
            Console.WriteLine("Inseriamo i clienti nel database");
            //per ogni cliente dentro listaclienti, si aggiunge al database
            listaClienti.ForEach(c => db.Add(c));
            db.SaveChanges();
            Console.WriteLine("Inseriamo gli ordini nel database");
            //per ogni ordine dentro listaOrdini, si aggiunge al database
            listaOrdini.ForEach(o => db.Add(o));
            db.SaveChanges();
        }

        static void VisualizzaOrdini(ClientiContext context)
        {
            //il context gestisce le operazioni di accesso dati del database 
            //un contesto del database è un oggetto che rappresenta la connessione al database stesso 
            //e fornisce metodi per eseguire query, inserire, modificare o cancellare dati, e altro ancora.
            //In pratica, è come un "intermediario" tra il tuo programma e il database

            //qui crei un context che ti permette di accedere ai dati del database
            var ordini = context.Ordini.ToList();
            foreach (var ordine in ordini)
            {
                Console.WriteLine($"Ordine {ordine.OrdineId}: {ordine.ImportoTotale} - {ordine.DataOrdine} - Cliente {ordine.ClienteId}");
            }
        }

        static void VisualizzaOrdineCliente(ClientiContext context, string cognome, string nome)
        {
            var cliente = context.Clienti.FirstOrDefault(c => c.Cognome == cognome && c.Nome == nome);
            if (cliente != null)
            {
                var ordini = context.Ordini.Where(o => o.ClienteId == cliente.ClienteId).ToList();
                foreach (var ordine in ordini)
                {
                    Console.WriteLine($"Ordine {ordine.OrdineId}: {ordine.ImportoTotale} - {ordine.DataOrdine}");
                }
            }
            else
            {
                Console.WriteLine("Cliente non trovato");
            }
        }
        static void ModificaOrdine(ClientiContext db, int ordineId, decimal nuovoTotale)
        {

            var ordine = db.Ordini.Find(ordineId); //cerca rispetto ad una chiave primaria, una chiave univoca per identificare le
            //righe del database ed è arbitraria (ovvero la scegli tu rispetto alle proprietà)
            //ad esempio: un nome è impossibile renderlo chiave primaria, perchè potrebbe non essere univoco. Un id, invece, si
            if (ordine != null)
            {
                ordine.ImportoTotale = nuovoTotale;
                db.SaveChanges();
                Console.WriteLine("Ordine modificato con successo!");
            }
            else
            {
                Console.WriteLine("Ordine non trovato!");
            }
        }
        
        static void CancellaOrdine(ClientiContext db, int ordineId)
        {
            var ordine = db.Ordini.Find(ordineId);
            //db.ordini è la tabella del database chiamata "ordini", io cerco dentro questa tabella
            //un ordine con una chiave UNIVOCA (in questo caso l'id) e lo metto in una variabile, così che poi puo essere utilizzato
            if (ordine != null)
            {
                db.Ordini.Remove(ordine);
                db.SaveChanges();
                Console.WriteLine("Ordine cancellato con successo!");
            }
            else
            {
                Console.WriteLine("Ordine non trovato!");
            }
        }

    }

}
