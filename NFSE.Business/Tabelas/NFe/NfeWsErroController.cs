﻿using NFSE.Domain.Entities.NFe;
using NFSE.Infra.Data;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace NFSE.Business.Tabelas.NFe
{
    public class NfeWsErroController
    {
        public List<NfeWsErroModel> Listar(NfeWsErroModel model)
        {
            var SQL = new StringBuilder();

            SQL.AppendLine("SELECT ErroId");
            SQL.AppendLine("      ,GrvId");
            SQL.AppendLine("      ,IdentificadorNota");
            SQL.AppendLine("      ,UsuarioId");
            SQL.AppendLine("      ,Acao");
            SQL.AppendLine("      ,OrigemErro");
            SQL.AppendLine("      ,Status");
            SQL.AppendLine("      ,CodigoErro");
            SQL.AppendLine("      ,MensagemErro");
            SQL.AppendLine("      ,CorrecaoErro");
            SQL.AppendLine("      ,DataHoraCadastro");

            SQL.AppendLine("  FROM dbo.tb_dep_nfe_ws_erros");

            SQL.AppendLine(" WHERE 1 = 1");

            if (model.ErroId > 0)
            {
                SQL.AppendLine("   AND ErroId = " + model.ErroId);
            }

            if (model.GrvId > 0)
            {
                SQL.AppendLine("   AND GrvId = " + model.GrvId);
            }

            if (model.IdentificadorNota > 0)
            {
                SQL.AppendLine("   AND IdentificadorNota = " + model.IdentificadorNota);
            }

            using (var dataTable = DataBase.Select(SQL))
            {
                return dataTable == null ? null : DataTableUtil.DataTableToList<NfeWsErroModel>(dataTable);
            }
        }

        public NfeWsErroModel Selecionar(NfeWsErroModel model)
        {
            var list = Listar(model);

            return list?.FirstOrDefault();
        }

        public int Cadastrar(NfeWsErroModel model)
        {
            var SQL = new StringBuilder();

            SQL.AppendLine("INSERT INTO dbo.tb_dep_nfe_ws_erros");
            
            SQL.AppendLine("    (GrvId");
            SQL.AppendLine("    ,IdentificadorNota");
            SQL.AppendLine("    ,UsuarioId");
            SQL.AppendLine("    ,Acao");
            SQL.AppendLine("    ,OrigemErro");
            SQL.AppendLine("    ,Status");
            SQL.AppendLine("    ,CodigoErro");
            SQL.AppendLine("    ,MensagemErro");
            SQL.AppendLine("    ,CorrecaoErro)");

            SQL.AppendLine("VALUES");

            SQL.AppendLine("    (@GrvId");
            SQL.AppendLine("    ,@IdentificadorNota");
            SQL.AppendLine("    ,@UsuarioId");
            SQL.AppendLine("    ,@Acao");
            SQL.AppendLine("    ,@OrigemErro");
            SQL.AppendLine("    ,@Status");
            SQL.AppendLine("    ,@CodigoErro");
            SQL.AppendLine("    ,@MensagemErro");
            SQL.AppendLine("    ,@CorrecaoErro)");

            if (model.Status != null)
            {
                model.Status = model.Status.ToUpper().Trim();

                if (model.Status.Length > 30)
                {
                    model.Status = model.Status.Substring(0, 30);
                }
            }

            if (model.CodigoErro != null)
            {
                model.CodigoErro = model.CodigoErro.ToUpper().Trim();

                if (model.CodigoErro.Length > 30)
                {
                    model.CodigoErro = model.CodigoErro.Substring(0, 30);
                }
            }

            if (model.MensagemErro != null)
            {
                model.MensagemErro = model.MensagemErro.Trim();

                if (model.MensagemErro.Length > 1000)
                {
                    model.MensagemErro = model.MensagemErro.Substring(0, 1000);
                }
            }

            if (model.CorrecaoErro != null)
            {
                model.CorrecaoErro = model.CorrecaoErro.Trim();

                if (model.CorrecaoErro.Length > 1000)
                {
                    model.CorrecaoErro = model.CorrecaoErro.Substring(0, 1000);
                }
            }

            SqlParameter[] sqlParameters =
            {
                new SqlParameter("@GrvId", SqlDbType.Int) { Value = model.GrvId },
                new SqlParameter("@IdentificadorNota", SqlDbType.Int) { Value = model.IdentificadorNota },
                new SqlParameter("@UsuarioId", SqlDbType.Int) { Value = model.UsuarioId },
                new SqlParameter("@Acao", SqlDbType.Char) { Value = model.Acao },
                new SqlParameter("@OrigemErro", SqlDbType.Char) { Value = model.OrigemErro },
                new SqlParameter("@Status", SqlDbType.VarChar) { Value = model.Status },
                new SqlParameter("@CodigoErro", SqlDbType.VarChar) { Value = model.CodigoErro },
                new SqlParameter("@MensagemErro", SqlDbType.VarChar) { Value = model.MensagemErro },
                new SqlParameter("@CorrecaoErro", SqlDbType.VarChar) { Value = model.CorrecaoErro }
            };

            return DataBase.ExecuteScalar(SQL, sqlParameters);
        }
    }
}