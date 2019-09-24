﻿namespace NFSE.Domain.Entities.DP
{
    public class FaturamentoAssociadoCnaeEntity
    {
        public string NumeroFormularioGrv { get; set; }

        public int GrvId { get; set; }

        public int AtendimentoId { get; set; }

        public int? CnaeId { get; set; }

        public string Cnae { get; set; }

        public int? ListaServicoId { get; set; }

        public string Servico { get; set; }

        public string ServicoAssociadoDescricao { get; set; }

        public string DescricaoConfiguracaoNfe { get; set; }

        public char FlagEnviarValorIss { get; set; }

        public char FlagEnviarInscricaoEstadual { get; set; }

        public decimal TotalComDesconto { get; set; }
    }
}