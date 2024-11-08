using System;
 
namespace gestioneFattureClienti.Models;
 
public class Fattura
{
    public int FatturaId { get; set; }
    public DateTime Data { get; set; }
    public decimal Importo { get; set; }
    public int ClienteId { get; set; }
    public Cliente Cliente { get; set; } = null!;
 
    public override string ToString()
    {
        return $"{{{nameof(FatturaId)} = {FatturaId}, " +
            $"{nameof(Data)} = {Data.ToShortDateString()}, " +
            $"{nameof(Importo)} = {Importo}, " +
            $"{nameof(ClienteId)} = {ClienteId}}}";
    }
}