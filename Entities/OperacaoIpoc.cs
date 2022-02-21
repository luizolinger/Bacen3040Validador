using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bacen3040Validador.Entities
{
    class OperacaoIpoc
    {
        // 8 posições
        public string CnpjInstituicao { get; set; }
        
        // 4 posições
        public string Modalidade { get; set; }

        // 1 posição
        public char TipoCliente { get; set; }

        // Depende TipoCliente (11, ou 8 ou 14)
        public string CodigoCliente { get; set; }

        // 40 posições
        public string CodigoContrato { get; set; }

        public void CarregarIpocXml(string ipocXml)
        {
            CnpjInstituicao = ipocXml.Substring(0, 8);
            Modalidade = ipocXml.Substring(8, 4);
            TipoCliente = char.Parse(ipocXml.Substring(12, 1));
            if (TipoCliente == '1')
            {
                CodigoCliente = ipocXml.Substring(13, 11);
                CodigoContrato = ipocXml.Substring(24);
            } else if (TipoCliente == '2')
            {
                CodigoCliente = ipocXml.Substring(13, 8);
                CodigoContrato = ipocXml.Substring(21);
            } else
            {
                CodigoCliente = ipocXml.Substring(13, 14);
                CodigoContrato = ipocXml.Substring(27);
            }
        }

        public override string ToString()
        {
            return CnpjInstituicao
                + Modalidade
                + TipoCliente
                + CodigoCliente
                + CodigoContrato;
        }
    }
}
