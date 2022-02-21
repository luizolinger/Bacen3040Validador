using Bacen3040Validador.Entities;

namespace Bacen3040Validador
{
    class Program
    {
        static void Main(string[] args)
        {

            string pasta3040MesAnterior = @"C:\Projetos\AILOS\3040\202112\01_VIACREDI_REMESSA02";
            string pasta3040MesCorrente = @"C:\Projetos\AILOS\3040\202201\01_VIACREDI_REMESSA01";

            // string pasta3040MesAnterior = @"C:\Projetos\3040_TESTE\202112";
            // string pasta3040MesCorrente = @"C:\Projetos\3040_TESTE\202201";
            // string pasta3040MesAnterior = @"C:\Projetos\3040_TESTE\ANTES";
            // string pasta3040MesCorrente = @"C:\Projetos\3040_TESTE\DEPOIS";
            /*
            string pasta3040MesAnterior = @"C:\Projetos\AILOS\3040\202112\03_AILOS_REMESSA02";
            string pasta3040MesCorrente = @"C:\Projetos\AILOS\3040\202201\03_AILOS_REMESSA01";
            */

            LerArquivos arquivos = new LerArquivos(pasta3040MesAnterior, pasta3040MesCorrente);
            System.Console.WriteLine(arquivos.Mensagens);
        }
    }
}
