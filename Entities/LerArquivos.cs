using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Globalization;

namespace Bacen3040Validador.Entities
{
    public class LerArquivos
    {
        public string Mensagens { get; set; }
        public LerArquivos()
        {

        }

        public LerArquivos(string pasta3040MesAnterior, string pasta3040MesCorrente)
        {
            LerArquivosXMLs(pasta3040MesAnterior, pasta3040MesCorrente);
        }

        private void LerArquivosXMLs(string pasta3040MesAnterior, string pasta3040MesCorrente)
        {

            MensagemProcessamento info = new MensagemProcessamento();

            Dictionary<string, Operacao> listaOpAnterior = new Dictionary<string, Operacao>();
            Dictionary<string, Operacao> listaOpCorrente = new Dictionary<string, Operacao>();

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

                    DateTime? dtaProxParcela = null;
                    if (operacao.Attributes["DtaProxParcela"] != null)
                    {
                        if (operacao.Attributes["DtaProxParcela"].ToString() != "")
                        {
                            dtaProxParcela = DateTime.ParseExact(operacao.Attributes["DtaProxParcela"].Value, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        }
                    }

                    DateTime? dtVencOp = null;
                    if (operacao.Attributes["dtVencOp"] != null)
                    {
                        if (operacao.Attributes["dtVencOp"].ToString() != "")
                        {
                            dtaProxParcela = DateTime.ParseExact(operacao.Attributes["dtVencOp"].Value, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        }
                    }

                    int qdParcelas = 0;
                    if (operacao.Attributes["QtdParcelas"] != null)
                    {
                        qdParcelas = int.Parse(operacao.Attributes["QtdParcelas"].Value);
                    }

                    string caracEspecial = "";
                    if (operacao.Attributes["CaracEspecial"] != null)
                    {
                        caracEspecial = operacao.Attributes["CaracEspecial"].Value;
                    }

                    string classOp = "";
                    if (operacao.Attributes["ClassOp"] != null)
                    {
                        classOp = operacao.Attributes["ClassOp"].Value;
                    }

                    string ipoc = operacao.Attributes["IPOC"].Value;

                    Operacao op = new Operacao(ipoc);
                    op.Modalidade = modalidadeOperacao;
                    op.DtaProxParcela = dtaProxParcela;
                    op.DtVencOp = dtVencOp;
                    op.QtdParcelas = qdParcelas;
                    op.CaracEspecial = caracEspecial;
                    op.ClassOp = classOp;

                    listaOpCorrente.Add(ipoc, op);
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

                    DateTime? dtaProxParcela = null;
                    if (operacao.Attributes["DtaProxParcela"] != null)
                    {
                        if (operacao.Attributes["DtaProxParcela"].ToString() != "")
                        {
                            dtaProxParcela = DateTime.ParseExact(operacao.Attributes["DtaProxParcela"].Value, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        }
                    }

                    DateTime? dtVencOp = null;
                    if (operacao.Attributes["dtVencOp"] != null)
                    {
                        if (operacao.Attributes["dtVencOp"].ToString() != "")
                        {
                            dtaProxParcela = DateTime.ParseExact(operacao.Attributes["dtVencOp"].Value, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        }
                    }

                    int qdParcelas = 0;
                    if (operacao.Attributes["QtdParcelas"] != null)
                    {
                        qdParcelas = int.Parse(operacao.Attributes["QtdParcelas"].Value);
                    }

                    string caracEspecial = "";
                    if (operacao.Attributes["CaracEspecial"] != null)
                    {
                        caracEspecial = operacao.Attributes["CaracEspecial"].Value;
                    }

                    string classOp = "";
                    if (operacao.Attributes["ClassOp"] != null)
                    {
                        classOp = operacao.Attributes["ClassOp"].Value;
                    }


                    if (nodeInf != null)
                    {
                        string modalidadeInf = nodeInf.Attributes["Tp"].Value;

                        if (!VerificarOperacaoSaida(modalidadeInf))
                        {
                            Operacao op = new Operacao(ipoc);
                            op.Modalidade = modalidadeOperacao;
                            op.DtaProxParcela = dtaProxParcela;
                            op.DtVencOp = dtVencOp;
                            op.QtdParcelas = qdParcelas;
                            op.CaracEspecial = caracEspecial;
                            op.ClassOp = classOp;

                            listaOpAnterior.Add(ipoc, op);
                        }
                    }
                    else
                    {
                        Operacao op = new Operacao(ipoc);
                        op.Modalidade = modalidadeOperacao;
                        op.DtaProxParcela = dtaProxParcela;
                        op.DtVencOp = dtVencOp;
                        op.QtdParcelas = qdParcelas;
                        op.CaracEspecial = caracEspecial;
                        op.ClassOp = classOp;

                        listaOpAnterior.Add(ipoc, op);
                    }
                }
            }

            // Criar lista de IPOC de operações sem saídas de operações não rotativas de um mês para outro
            var ListaSemSaidas = listaOpAnterior.Keys.Except(listaOpCorrente.Keys);

            info.AdicionarMensagem("IPOCs sem saídas no mês corrente: ");

            int total = 0;
            foreach (string op in ListaSemSaidas)
            {
                info.AdicionarMensagem(op);
                total++;
            }
            info.AdicionarMensagem("Total: " + total);

/*
            var ListaCaracEspecialAlterada = listaOpCorrente
                .Where(p => listaOpAnterior[p.Key].CaracEspecial != p.Value.CaracEspecial)
                .Select(p => p.Key);
            
            total = 0;
            info.AdicionarMensagem("Característica especial alterada: ");
            foreach (string op in ListaCaracEspecialAlterada)
            {
                info.AdicionarMensagem(op);
                total++;
            }
            info.AdicionarMensagem("Total: " + total);
*/

            total = 0;
            info.AdicionarMensagem("Característica especial alterada: ");
            foreach (var lista in listaOpCorrente)
            {
                if (listaOpAnterior.ContainsKey(lista.Key)) {
                    if (lista.Value.CaracEspecial != listaOpAnterior[lista.Key].CaracEspecial)
                    {
                        info.AdicionarMensagem(lista.Key);
                        total++;
                    }
                }
            }
            info.AdicionarMensagem("Total: " + total);

            total = 0;
            info.AdicionarMensagem("Data última parcela alterada: ");
            foreach (var lista in listaOpCorrente)
            {
                if (listaOpAnterior.ContainsKey(lista.Key))
                {
                    if (lista.Value.DtVencOp != null
                        && lista.Value.DtVencOp != listaOpAnterior[lista.Key].DtVencOp)
                    {
                        info.AdicionarMensagem(lista.Key);
                        total++;
                    }
                }
            }
            info.AdicionarMensagem("Total: " + total);

            total = 0;
            info.AdicionarMensagem("Operações com riscos alterados: ");
            foreach (var lista in listaOpCorrente)
            {
                if (listaOpAnterior.ContainsKey(lista.Key))
                {
                    if (lista.Value.ClassOp != null
                        && lista.Value.ClassOp != listaOpAnterior[lista.Key].ClassOp)
                    {
                        info.AdicionarMensagem(lista.Key
                            + ";De: "
                            + listaOpAnterior[lista.Key].ClassOp
                            + ";Para:"
                            + lista.Value.ClassOp);
                        total++;
                    }
                }
            }
            info.AdicionarMensagem("Total: " + total);

            Mensagens = info.Mensagem;
            
        }

        private bool VerificarNaoRotativo(string modalidade)
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

        private static bool VerificarOperacaoSaida(string modalidade)
        {
            if (modalidade.Substring(0, 2) == "03")
            {
                return true;
            }
            return false;
        }
    }
}
