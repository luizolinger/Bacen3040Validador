using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Bndes3040Revisao
{
    class Program
    {
        static void Main(string[] args)
        {

            Dictionary<string, Operacao> listaOpAnterior = new Dictionary<string, Operacao>();
            Dictionary<string, Operacao> listaOpCorrente = new Dictionary<string, Operacao>();

            // TODO: Criar aplicação para selecionar pastas na interfaces
            string pasta3040MesAnterior = @"C:\Projetos\3040_TESTE\202112";
            string pasta3040MesCorrente = @"C:\Projetos\3040_TESTE\202201";

            DirectoryInfo arquivos3040 = new DirectoryInfo(pasta3040MesCorrente);
            FileInfo[] Files = arquivos3040.GetFiles("*.xml");

            foreach (FileInfo file in Files)
            {
                XmlDocument arquivo3040 = new XmlDocument();
                arquivo3040.Load(file.FullName);

                XmlNodeList nodeList = arquivo3040.SelectNodes("/Doc3040/Cli/Op");
                foreach (XmlNode operacao in nodeList)
                {
                    string modalidadeOperacao = operacao.Attributes["Mod"].Value;

                    if (!VerificarNaoRotativo(modalidadeOperacao))
                    {
                        continue;
                    }

                    string ipoc = operacao.Attributes["IPOC"].Value;
                    listaOpCorrente.Add(ipoc, new Operacao(ipoc, modalidadeOperacao));
                }
            }

            arquivos3040 = new DirectoryInfo(pasta3040MesAnterior);
            Files = arquivos3040.GetFiles("*.xml");

            foreach (FileInfo file in Files)
            {
                XmlDocument arquivo3040 = new XmlDocument();
                arquivo3040.Load(file.FullName);

                XmlNodeList nodeList = arquivo3040.SelectNodes("/Doc3040/Cli/Op");
                foreach (XmlNode operacao in nodeList)
                {
                    string modalidadeOperacao = operacao.Attributes["Mod"].Value;

                    if (!VerificarNaoRotativo(modalidadeOperacao))
                    {
                        continue;
                    }

                    string ipoc = operacao.Attributes["IPOC"].Value;

                    XmlNode nodeInf = operacao.SelectSingleNode("Inf");
                    if (nodeInf != null)
                    {
                        string modalidadeInf = nodeInf.Attributes["Tp"].Value;

                        if (!VerificarOperacaoSaida(modalidadeInf))
                        {
                            listaOpAnterior.Add(ipoc, new Operacao(ipoc, modalidadeOperacao, false));
                        }
                    }
                    else
                    {
                        listaOpAnterior.Add(ipoc, new Operacao(ipoc, modalidadeOperacao, false));
                    }
                }
            }

            // Criar lista de IPOC de operações sem saídas de operações não rotativas de um mês para outro
            var ListaSemSaidas = listaOpAnterior.Keys.Except(listaOpCorrente.Keys);

            Console.WriteLine("IPOCs sem saídas no mês corrente: ");

            int total = 0;
            foreach (string op in ListaSemSaidas)
            {
                Console.WriteLine(op);
                total++;
            }
            Console.WriteLine("Total: " + total);

        }

        public static bool VerificarNaoRotativo(string modalidade)
        {

            // TODO: Verificar as modalidades corretas de não rotativos

            // Cheque especial / capital de giro com prazo maior que 365
            if (modalidade == "0213" || modalidade == "0216")
            {
                return false;
            }

            if (modalidade.Substring(0, 2) == "02" || modalidade.Substring(0, 2) == "14")
            {
                return true;
            }

            return false;
        }

        public static bool VerificarOperacaoSaida(string modalidade)
        {
            if (modalidade.Substring(0, 2) == "03")
            {
                return true;
            }
            return false;
        }
    }
}
