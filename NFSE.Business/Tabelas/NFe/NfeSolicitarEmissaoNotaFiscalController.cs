﻿using NFSE.Business.Tabelas.DP;
using NFSE.Business.Tabelas.Global;
using NFSE.Business.Util;
using NFSE.Domain.Entities.Global;
using NFSE.Domain.Entities.NFe;
using NFSE.Domain.Enum;
using NFSE.Infra.Data;
using System;

namespace NFSE.Business.Tabelas.NFe
{
    public class NfeSolicitarEmissaoNotaFiscalController
    {
        public string SolicitarEmissaoNotaFiscal(CapaAutorizacaoNfse model)
        {
            DataBase.SystemEnvironment = model.Homologacao ? SystemEnvironment.Development : SystemEnvironment.Production;

            var nfe = new NfeController().ConsultarNotaFiscal(model.GrvId, model.UsuarioId, model.IdentificadorNota, Acao.Solicitação);

            var grv = new GrvController().Selecionar(model.GrvId);

            #region Empresa
            EmpresaEntity Empresa;

            if ((Empresa = new EmpresaController().Selecionar(new EmpresaEntity { EmpresaId = new DepositoController().Selecionar(grv.DepositoId).EmpresaId })) == null)
            {
                new NfeWsErroController().CadastrarErroGenerico(model.GrvId, model.UsuarioId, null, OrigemErro.MobLink, Acao.Retorno, "Empresa associada não encontrada");

                throw new Exception("Empresa associada não encontrada");
            }
            #endregion Empresa

            string resposta;

            try
            {
                resposta = new Tools().PostNfse
                (
                    uri: new NfeConfiguracao().GetRemoteServer() + "?ref=" + model.IdentificadorNota,
                    json: CreateJson(model),
                    token: Empresa.Token
                );
            }
            catch (Exception ex)
            {
                new NfeWsErroController().CadastrarErroGenerico(model.GrvId, model.UsuarioId, nfe.IdentificadorNota, OrigemErro.WebService, Acao.Solicitação, "Ocorreu um erro ao solicitar a Nota Fiscal: " + ex.Message);

                throw new Exception("Ocorreu um erro ao solicitar a Nota Fiscal (" + model.IdentificadorNota + "): " + ex.Message);
            }

            try
            {
                new NfeRetornoSolicitacaoController().Cadastrar(nfe, model, resposta);
            }
            catch (Exception ex)
            {
                new NfeWsErroController().CadastrarErroGenerico(model.GrvId, model.UsuarioId, nfe.IdentificadorNota, OrigemErro.MobLink, Acao.Solicitação, "Ocorreu um erro ao cadastrar a solicitação da Nota Fiscal: " + ex.Message);

                throw new Exception("Ocorreu um erro ao cadastrar a solicitação da Nota Fiscal (" + model.IdentificadorNota + "): " + ex.Message);
            }

            return resposta;
        }

        public string SimularEmissaoNotaFiscal(CapaAutorizacaoNfse model)
        {
            DataBase.SystemEnvironment = SystemEnvironment.Development;

            EmpresaEntity Empresa;

            if ((Empresa = new EmpresaController().Selecionar(new EmpresaEntity { Cnpj = model.Autorizacao.prestador.cnpj } )) == null)
            {
                throw new Exception("Empresa associada não encontrada");
            }

            model.IdentificadorNota = new DetranController().GetDetranSequence("NFE");
            model.UsuarioId = 1;

            try
            {
                return new Tools().PostNfse
                (
                    uri: new NfeConfiguracao().GetRemoteServer() + "?ref=" + model.IdentificadorNota,
                    json: CreateJson(model),
                    token: Empresa.Token
                );
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro ao simular a Nota Fiscal (" + model.IdentificadorNota + "): " + ex.Message);
            }
        }

        private string CreateJson(CapaAutorizacaoNfse model)
        {
            var tools = new Tools();

            string json = tools.ObjToJSON(model.Autorizacao);

            var jsonUtil = new JsonUtil.JsonUtil();

            if (string.IsNullOrWhiteSpace(model.Autorizacao.tomador.cnpj))
            {
                json = jsonUtil.RemoveElement(json, "tomador", "cnpj");
            }
            else
            {
                json = jsonUtil.RemoveElement(json, "tomador", "cpf");
            }

            //if (string.IsNullOrWhiteSpace(model.Autorizacao.servico.codigo_tributario_municipio))
            //{
            //    json = jsonUtil.RemoveElement(json, "servico", "codigo_tributario_municipio");
            //}

            return json;
        }

        //public string ReceberNotaFiscal(Consultar model)
        //{
        //    return new JavaScriptSerializer().Serialize(Consultar_obj(model));
        //}
    }
}