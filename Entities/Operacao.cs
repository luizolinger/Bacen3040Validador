namespace Bacen3040Validador
{
    class Operacao
    {
        public string Ipoc { get; set; }
        public string Modalidade { get; set; }
        public bool PossuiSaida { get; set; }
        public Operacao()
        {

        }

        public Operacao(string ipoc, string modalide)
        {
            Ipoc = ipoc;
            Modalidade = modalide;
        }

        public Operacao(string ipoc, string modalidade, bool possuiSaida) : this(ipoc, modalidade)
        {
            PossuiSaida = possuiSaida;
        }

    }
}
