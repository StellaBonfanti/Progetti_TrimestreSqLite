using System;
using System.ComponentModel.DataAnnotations;

namespace EsempioCarTruck.Model;

public class Car1
{
    [Key]
    public int Targa { get; set; }
    public string Make { get; set; }
    public string Model { get; set; }
}
