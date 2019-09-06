﻿using NFSE.Infra.Data;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Negocio.Tabelas
{
    public class Erro
    {
        public List<NFSE.Domain.Entities.Erro> ConsultarPorCodigoRetorno(int codigoRetorno)
        {
            var SQL = new StringBuilder();

            SQL.AppendLine("SELECT NotaFiscalErroId");
            SQL.AppendLine("      ,UsuarioId");
            SQL.AppendLine("      ,GrvId");
            SQL.AppendLine("      ,CodigoRetorno");
            SQL.AppendLine("      ,MensagemErro");
            SQL.AppendLine("      ,DataHoraCadastro");

            SQL.AppendLine("  FROM " + DataBase.GetNfeDatabase() + ".dbo.tb_nfse_erros");

            SQL.AppendLine(" WHERE CodigoRetorno = @CodigoRetorno");

            SqlParameter[] sqlParameters =
            {
                new SqlParameter("@CodigoRetorno",SqlDbType.Int) { Value = codigoRetorno }
            };

            using (var dataTable = DataBase.Select(SQL, sqlParameters))
            {
                if (dataTable == null)
                {
                    return null;
                }

                return DataTableUtil.DataTableToList<NFSE.Domain.Entities.Erro>(dataTable);
            }
        }

        public List<NFSE.Domain.Entities.Erro> ConsultarPorGrv(int grvId)
        {
            var SQL = new StringBuilder();

            SQL.AppendLine("SELECT NotaFiscalErroId");
            SQL.AppendLine("      ,UsuarioId");
            SQL.AppendLine("      ,GrvId");
            SQL.AppendLine("      ,CodigoRetorno");
            SQL.AppendLine("      ,MensagemErro");
            SQL.AppendLine("      ,DataHoraCadastro");

            SQL.AppendLine("  FROM " + DataBase.GetNfeDatabase() + ".dbo.tb_nfse_erros");

            SQL.AppendLine(" WHERE GrvId = @GrvId");

            SqlParameter[] sqlParameters =
            {
                new SqlParameter("@GrvId",SqlDbType.Int) { Value = grvId }
            };

            using (var dataTable = DataBase.Select(SQL, sqlParameters))
            {
                if (dataTable == null)
                {
                    return null;
                }

                return DataTableUtil.DataTableToList<NFSE.Domain.Entities.Erro>(dataTable);
            }
        }

        public int Cadastrar(NFSE.Domain.Entities.Erro model)
        {
            var SQL = new StringBuilder();

            SQL.AppendLine("INSERT INTO " + DataBase.GetNfeDatabase() + ".dbo.tb_nfse_erros");

            SQL.AppendLine("    (UsuarioId");
            SQL.AppendLine("    ,GrvId");
            SQL.AppendLine("    ,CodigoRetorno");
            SQL.AppendLine("    ,OrigemErro");
            SQL.AppendLine("    ,MensagemErro)");

            SQL.AppendLine("VALUES");

            SQL.AppendLine("    (@UsuarioId");
            SQL.AppendLine("    ,@GrvId");
            SQL.AppendLine("    ,@CodigoRetorno");
            SQL.AppendLine("    ,@OrigemErro");
            SQL.AppendLine("    ,@MensagemErro)");

            SqlParameter[] sqlParameters =
            {
                new SqlParameter("@UsuarioId",SqlDbType.Int) { Value = model.UsuarioId },
                new SqlParameter("@GrvId",SqlDbType.Int) { Value = model.UsuarioId },
                new SqlParameter("@CodigoRetorno",SqlDbType.Int) {Value = model.CodigoRetorno },
                new SqlParameter("@OrigemErro",SqlDbType.Char) {Value = model.OrigemErro },
                new SqlParameter("@MensagemErro",SqlDbType.VarChar) { Value = model.MensagemErro }
            };

            return DataBase.Execute(SQL, sqlParameters);
        }
    }
}