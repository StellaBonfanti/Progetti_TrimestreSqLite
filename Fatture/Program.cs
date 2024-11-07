using System.Runtime.InteropServices;
using System.Text;
using gestioneFattureClienti.Data;
using gestioneFattureClienti.Models;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
 
using var db = new FattureClientiContext();
 
Console.OutputEncoding = Encoding.UTF8;
Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("it-IT");
 
ModalitàOperativa modalitàOperativa = ModalitàOperativa.Nessuna;
//colore di default della console
Console.ForegroundColor = ConsoleColor.Cyan;
//gestione del menu
bool uscitaDalProgramma = false;
do
{
    //gestione della scelta
    bool correctInput = false;
    do
    {
        Console.WriteLine("Inserire la modalità operativa [CreazioneDb, LetturaDb, ModificaFattura, CancellazioneFattura, CancellazioneDb, Nessuna]");
        correctInput = Enum.TryParse(Console.ReadLine(), true, out modalitàOperativa);
        //modalità operativa è un enum che si trova in fondo, in particolare modalitàOperativa è il valore che entri da console mentre
        //ModalitàOperativa è l'enum stesso. quindi quello con la m minuscola è solo per tenere i dati 
        if (correctInput)
        {
            switch (modalitàOperativa)
            {
                case ModalitàOperativa.CreazioneDb:
                    CreazioneDb();
                    break;
                case ModalitàOperativa.LetturaDb:
                    LetturaDb();
                    break;
                case ModalitàOperativa.ModificaFattura:
                    //ModificaFattura();
                    break;
                case ModalitàOperativa.CancellazioneFattura:
                    //CancellazioneFattura();
                    break;
                case ModalitàOperativa.CancellazioneDb:
                    //CancellazioneDb();
                    break;
                default:
                    WriteLineWithColor("Non è stata impostata nessuna modalità operativa", ConsoleColor.Yellow);
                    break;
            }
        }
        if (!correctInput)
        {
            Console.Clear();
            WriteLineWithColor("Il valore inserito non corrisponde a nessuna opzione valida.\nI valori ammessi sono: [Creazione, Lettura, Modifica, Cancellazione, Nessuna]", ConsoleColor.Red);
        }
    } while (!correctInput);
    Console.WriteLine("Uscire dal programma?[Si, No]");
    uscitaDalProgramma = Console.ReadLine()?.StartsWith("si", StringComparison.CurrentCultureIgnoreCase) ?? false;
    //StringComparison.CurrentCultureIgnoreCase serve per fare il confronto ignorando se sono maiuscole o minuscole, restituirà quindi
    //true sia che sia si, che Si, che sI, che SI
    Console.Clear();
} while (!uscitaDalProgramma);
 
 
 
 
 
 
 
 
 
static void CreazioneDb()
{
    using FattureClientiContext db = new();
    //verifichiamo se il database esista già
    //https://medium.com/@Usurer/ef-core-check-if-db-exists-feafe6e36f4e
    //https://stackoverflow.com/questions/33911316/entity-framework-core-how-to-check-if-database-exists
    if (db.Database.GetService<IRelationalDatabaseCreator>().Exists())
    //questo è trovato online ed è stato usato così perchè funziona così, infatti chiama una libreria esterna
    {
        WriteLineWithColor("Il database esiste già, vuoi ricrearlo da capo? Tutti i valori precedentemente inseriti verranno persi. [Si, No]", ConsoleColor.Red);
        bool dbErase = Console.ReadLine()?.StartsWith("si", StringComparison.CurrentCultureIgnoreCase) ?? false;
        if (dbErase)
        {
            //cancelliamo il database se esiste
            db.Database.EnsureDeleted();

            //ricreiamo il database a partire dal model (senza dati vuol dire che le tabelle sono vuote)
            db.Database.EnsureCreated();

            //inseriamo i dati nelle tabelle
            PopulateDb(db);
        }
    }
    else //il database non esiste
    {
        //ricreiamo il database a partire dal model (senza dati vuol dire che le tabelle sono vuote)
        db.Database.EnsureCreated();

        //popoliamo il database (ovvero inseriamo i dati)
        PopulateDb(db);
    }
 
    static void PopulateDb(FattureClientiContext db)
    {
        //Creazione dei Clienti - gli id vengono generati automaticamente come campi auto-incremento quando si effettua l'inserimento, tuttavia
        //è bene inserire esplicitamente l'id degli oggetti quando si procede all'inserimento massivo gli elementi mediante un foreach perché
        //EF core potrebbe inserire nel database gli oggetti in un ordine diverso rispetto a quello del foreach
        // https://stackoverflow.com/a/54692592
        // https://stackoverflow.com/questions/11521057/insertion-order-of-multiple-records-in-entity-framework/
        List<Cliente> listaClienti =
        [
            new (){ClienteId=1, RagioneSociale= "Cliente 1", PartitaIVA= "1111111111", Citta = "Napoli", Via="Via dei Mille", Civico= "23", CAP="80100"},
            new (){ClienteId=2, RagioneSociale= "Cliente 2", PartitaIVA= "1111111112", Citta = "Roma", Via="Via dei Fori Imperiali", Civico= "1", CAP="00100"},
            new (){ClienteId=3, RagioneSociale= "Cliente 3", PartitaIVA= "1111111113", Citta = "Firenze", Via="Via Raffaello", Civico= "10", CAP="50100"}
        ];
 
        //Creazione della lista delle fatture
        List<Fattura> listaFatture =
        [
            new (){FatturaId=1, Data= DateTime.Now.Date, Importo = 1200.45m, ClienteId = 1},
            new (){FatturaId=2, Data= DateTime.Now.AddDays(-5).Date, Importo = 3200.65m, ClienteId = 1},
            new (){FatturaId=3, Data= new DateTime(2019,10,20).Date, Importo = 5200.45m, ClienteId = 1},
            new (){FatturaId=4, Data= DateTime.Now.Date, Importo = 5200.45m, ClienteId = 2},
            new (){FatturaId=5, Data= new DateTime(2019,08,20).Date, Importo = 7200.45m, ClienteId = 2}
        ];
        //inserire clienti e fatture
        Console.WriteLine("Inseriamo i clienti nel database");
        listaClienti.ForEach(c => db.Add(c));
        //db è il nostro database, creato prima

        db.SaveChanges();
        Console.WriteLine("Inseriamo le fatture nel database");
        
        listaFatture.ForEach(f => db.Add(f));
        //classico add delle liste
        db.SaveChanges();
    }
}
static void LetturaDb()
{
    System.Console.WriteLine("stampa clienti");
 
    using var db = new FattureClientiContext();
    //fattureclienticontext è una classe
    db.Clienti.ToList().ForEach(c => System.Console.WriteLine(c));
 
    System.Console.WriteLine("stampa fatture");
 
    List<Fattura> listaFatture = [.. db.Fatture];
    List<Cliente> listaClienti = [.. db.Clienti];
    listaFatture.ForEach(f => System.Console.WriteLine(f));
}
static void ModificaFattura()
{
    using var db = new FattureClientiContext();
    Console.WriteLine("inserisci codice fattura da modificare");
    int codiceFatture = int.Parse(Console.ReadLine());
    
    Fattura? fatturaM = db.Fatture.Find(codiceFatture);
    if (fatturaM != null)
    {
        fatturaM.Importo *= 1.05m;
        db.SaveChanges();
    }
    
    db.Fatture.ToList().ForEach(Console.WriteLine);
}

static void CancellazioneFattura()
{
    using var db = new FattureClientiContext();
    Console.WriteLine("inserisci il codice della fattura da cancellare");
    int codiceFatturaDaEliminare = int.Parse(Console.ReadLine());
    
    Fattura? fdelegate = db.Fatture.Find(codiceFatturaDaEliminare);
    if (fdelegate != null)
    {
        db.Fatture.Remove(fdelegate);
        db.SaveChanges();
    }
}

 
 
 
 
 
 
static void WriteLineWithColor(string text, ConsoleColor consoleColor)
{
    ConsoleColor previousColor = Console.ForegroundColor;
    Console.ForegroundColor = consoleColor;
    Console.WriteLine(text);
    Console.ForegroundColor = previousColor;
}
enum ModalitàOperativa
{
    CreazioneDb,
    LetturaDb,
    ModificaFattura,
    CancellazioneFattura,
    CancellazioneDb,
    Nessuna
}