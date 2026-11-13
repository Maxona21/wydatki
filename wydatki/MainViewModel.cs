using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Windows.Input;

namespace wydatki
{

    public class MainViewModel : INotifyPropertyChanged
    {
        private const string SciezkaPliku = "wydatki.json";
        private List<Wydatek> _wszystkieWydatki = new List<Wydatek>();

        public ObservableCollection<Wydatek> WidoczneWydatki { get; set; } = new ObservableCollection<Wydatek>();
        public ObservableCollection<string> PodsumowanieKategorii { get; set; } = new ObservableCollection<string>();

        private decimal _kwota;
        public decimal Kwota { get => _kwota; set { _kwota = value; OnPropertyChanged(); } }

        private string _kategoria = "Jedzenie";
        public string Kategoria { get => _kategoria; set { _kategoria = value; OnPropertyChanged(); } }

        private DateTime _data = DateTime.Today;
        public DateTime Data { get => _data; set { _data = value; OnPropertyChanged(); } }

        private string _opis = string.Empty;
        public string Opis { get => _opis; set { _opis = value; OnPropertyChanged(); } }

        private int _wybranyMiesiac = 0;
        public int WybranyMiesiac
        {
            get => _wybranyMiesiac;
            set { _wybranyMiesiac = value; OnPropertyChanged(); OdswiezWidok(); }
        }

        public List<int> Miesiace { get; } = Enumerable.Range(0, 13).ToList();
        public List<string> DostepneKategorie { get; } = new List<string> { "Jedzenie", "Transport", "Rachunki", "Rozrywka", "Inne" };

        private decimal _sumaWydatkow;
        public decimal SumaWydatkow { get => _sumaWydatkow; set { _sumaWydatkow = value; OnPropertyChanged(); } }

        public ICommand DodajCommand { get; }

        public MainViewModel()
        {

            DodajCommand = new RelayCommend(DodajWydatek);
            WczytajZPliku();
            OdswiezWidok();
        }

        private void DodajWydatek()
        {
            if (Kwota <= 0) return;

            _wszystkieWydatki.Add(new Wydatek
            {
                Kwota = Kwota,
                Kategoria = Kategoria,
                Data = Data,
                Opis = Opis
            });

            Kwota = 0;
            Opis = string.Empty;
            ZapiszDoPliku();
            OdswiezWidok();
        }

        private void OdswiezWidok()
        {
            WidoczneWydatki.Clear();
            var filtrowane = WybranyMiesiac == 0
                ? _wszystkieWydatki
                : _wszystkieWydatki.Where(w => w.Data.Month == WybranyMiesiac);

            foreach (var w in filtrowane)
                WidoczneWydatki.Add(w);

            SumaWydatkow = WidoczneWydatki.Sum(w => w.Kwota);

            PodsumowanieKategorii.Clear();
            var grupy = WidoczneWydatki
                .GroupBy(w => w.Kategoria)
                .Select(g => $"{g.Key}: {g.Sum(x => x.Kwota):C}");

            foreach (var g in grupy)
                PodsumowanieKategorii.Add(g);
        }

        private void ZapiszDoPliku()
        {
            var json = JsonSerializer.Serialize(_wszystkieWydatki);
            File.WriteAllText(SciezkaPliku, json);
        }

        private void WczytajZPliku()
        {
            if (!File.Exists(SciezkaPliku)) return;
            var json = File.ReadAllText(SciezkaPliku);
            _wszystkieWydatki = JsonSerializer.Deserialize<List<Wydatek>>(json) ?? new List<Wydatek>();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}