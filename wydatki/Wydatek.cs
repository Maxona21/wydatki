using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wydatki
{
    public class Wydatek
    {
        public decimal Kwota { get; set; }
        public string Kategoria { get; set; } = "Jedzenie";
        public DateTime Data { get; set; } = DateTime.Today;
        public string Opis { get; set; } = "";

    }
}
