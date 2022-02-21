using System;
using Bacen3040Validador.Entities;

namespace Bacen3040Validador.Entities
{
    class Operacao
    {
        public OperacaoIpoc Ipoc;
        public string Modalidade { get; set; }
        public bool PossuiSaida { get; set; }
        public DateTime? DtaProxParcela { get; set; }
        public DateTime? DtVencOp { get; set; }
        public int QtdParcelas { get; set; }
        public string CaracEspecial { get; set; }
        public string ClassOp { get; set; }

        public Operacao()
        {

        }

        public Operacao(string ipocXml)
        {
            OperacaoIpoc ipoc = new OperacaoIpoc();
            ipoc.CarregarIpocXml(ipocXml);
            Ipoc = ipoc;
        }

    }
}
