using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using Dapper;

namespace APICotacoes.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CotacoesController : ControllerBase
    {
        [HttpGet]
        public ContentResult Get(
            [FromServices]IConfiguration config)
        {
            using var conexao = new SqlConnection(
                config.GetConnectionString("BaseCotacoes"));

            string valorJSON = conexao.QueryFirst<string>(
                "SELECT Sigla " +
                      ",NomeMoeda " +
                      ",UltimaCotacao " +
                      ",ValorComercial AS 'Cotacoes.FCNuvem.Comercial' " +
                      ",ValorTurismo AS 'Cotacoes.FCNuvem.Turismo' " +
                "FROM dbo.Cotacoes " +
                "ORDER BY NomeMoeda " +
                "FOR JSON PATH, ROOT('Moedas')");
            
            return Content(valorJSON, "application/json");
        }
    }
}