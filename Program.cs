using Bacen3040Validador.Entities;

namespace Bacen3040Validador
{
    class Program
    {
        static void Main(string[] args)
        {

            string pasta3040MesAnterior = @"C:\Projetos\3040_TESTE\202112";
            string pasta3040MesCorrente = @"C:\Projetos\3040_TESTE\202201";

            LerArquivos arquivos = new LerArquivos(pasta3040MesAnterior, pasta3040MesCorrente);
            System.Console.WriteLine(arquivos.Mensagens);
        }
    }
}
