using System;

namespace Bacen3040Validador.Entities
{
    class MensagemProcessamento
    {
        public string Mensagem { get; set; }

        public void AdicionarMensagem(string mensagem)
        {
            Mensagem += Environment.NewLine + mensagem;
        }
    }
}
