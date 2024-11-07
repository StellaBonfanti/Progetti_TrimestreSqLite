using System;
using gestioneFattureClienti.Models;
using Microsoft.EntityFrameworkCore;
 
namespace gestioneFattureClienti.Data;
 
public class FattureClientiContext:DbContext
//Il contesto (DbContext) in Entity Framework è come una "finestra" tra il tuo programma e il database. 
//Permette di accedere ai dati, aggiungere nuove informazioni, modificarle o eliminarle, tutto in modo semplice. 
//In pratica, definisce quali tabelle (e i dati che contengono) il programma può usare, 
//e come queste tabelle sono mappate a oggetti (classi) nel codice.

//"Mappare a oggetti" significa che ogni tabella del database è associata a una classe nel codice. 
//Ad esempio, se hai una tabella Clienti nel database, avrai una classe Cliente nel codice che rappresenta i dati di quella tabella.
{
    public DbSet<Cliente> Clienti {get; set;} = null!;
    public DbSet<Fattura> Fatture {get; set;} = null!;
    public string DbPath{get; }
    public FattureClientiContext()
    {
        var folder = AppContext.BaseDirectory;
        //AppContext.BaseDirectory è una proprietà che restituisce il percorso della directory in cui 
        //si trova la applicazione. 
        //In pratica, ti dà la posizione della cartella in cui è stato avviato il programma
 
        var path = Path.Combine(folder, "../../../FattureClienti.db");
        DbPath = path;
        //crea una variabile dbpath che è la unione del folder in cui mi trovo + fattureclienti.db (così ho il progetto)
        //(se cambio pc tutto rimane uguale perchè tanto tu hai fatto basedirectory + nome del progetto)
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        =>optionsBuilder.UseSqlite($"Data Source={DbPath}");
}