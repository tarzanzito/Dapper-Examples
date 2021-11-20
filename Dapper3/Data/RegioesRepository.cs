using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Dapper;
//using Slapper;
//using StackExchange.Profiling;
//using StackExchange.Profiling.Data;

namespace APIRegioes.Data
{
    public class RegioesRepository
    {
        private readonly IConfiguration _configuration;

        public RegioesRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IEnumerable<Regiao> Get(string codRegiao = null)
        {
            //var conexao = new ProfiledDbConnection(new SqlConnection(
            //    _configuration.GetConnectionString("BaseDadosGeograficos")),
            //    MiniProfiler.Current);

            SqlConnection conexao = new SqlConnection(_configuration.GetConnectionString("BaseDadosGeograficos"));

            bool queryWithParameter = !String.IsNullOrWhiteSpace(codRegiao);
            var sqlCmd =
                "SELECT R.IdRegiao, " +
                        "R.CodRegiao, " +
                        "R.NomeRegiao, " +
                        "E.SiglaEstado AS Estados_SiglaEstado, " +
                        "E.NomeEstado AS Estados_NomeEstado, " +
                        "E.NomeCapital AS Estados_NomeCapital " +
                "FROM dbo.Regioes R " +
                "INNER JOIN dbo.Estados E " +
                    "ON E.IdRegiao = R.IdRegiao " +
                (queryWithParameter ? $"WHERE (R.CodRegiao = @CodigoRegiao) " : String.Empty) +
                "ORDER BY R.NomeRegiao, E.NomeEstado";
            
            object paramQuery = null;
            if (queryWithParameter)
                paramQuery = new { CodigoRegiao = codRegiao };
            var dados = conexao.Query<dynamic>(sqlCmd, paramQuery); //IEnumerable<object> 

            //AutoMapper.Configuration.AddIdentifier(
            //    typeof(Regiao), "IdRegiao");
            //AutoMapper.Configuration.AddIdentifier(
            //    typeof(Estado), "SiglaEstado");

            //return (AutoMapper.MapDynamic<Regiao>(dados)
            //    as IEnumerable<Regiao>).ToArray();

            return null;
        }



        public IEnumerable<Regiao> GetMeu(string codRegiao = null)
        {
            SqlConnection conexao = new SqlConnection(_configuration.GetConnectionString("BaseDadosGeograficos"));

            bool queryWithParameter = !String.IsNullOrWhiteSpace(codRegiao);
            var sqlCmd =
                "SELECT R.IdRegiao, " +
                        "R.CodRegiao, " +
                        "R.NomeRegiao, " +
                        "E.SiglaEstado, " +
                        "E.NomeEstado, " +
                        "E.NomeCapital " +
                "FROM dbo.Regioes R " +
                "INNER JOIN dbo.Estados E " +
                    "ON E.IdRegiao = R.IdRegiao " +
                (queryWithParameter ? $"WHERE (R.CodRegiao = @CodigoRegiao) " : String.Empty) +
                "ORDER BY R.NomeRegiao, E.NomeEstado";

            object paramQuery = null;
            if (queryWithParameter)
                paramQuery = new { CodigoRegiao = codRegiao };
            var dados = conexao.Query<RegiaoEstado>(sqlCmd, paramQuery); //IEnumerable<object> 


            //////////////////
            List<Regiao> list1 = new List<Regiao>();

            int currIdMaster = 0;
            Regiao currRegiao = null ;

            foreach (RegiaoEstado item in dados)
            {
                if (item.IdRegiao != currIdMaster)
                {
                    currRegiao = new Regiao()
                    {
                        IdRegiao = item.IdRegiao,
                        CodRegiao = item.CodRegiao,
                        NomeRegiao = item.NomeRegiao,
                        Estados = new List<Estado>()
                    };
                    list1.Add(currRegiao);
                    currIdMaster = item.IdRegiao;
                }

                Estado estado = new Estado()
                {
                    SiglaEstado = item.SiglaEstado,
                    NomeCapital = item.NomeCapital,
                    NomeEstado = item.NomeEstado
                };
                currRegiao.Estados.Add(estado);
            }

            ////////////////////////

            return list1;
        }

        public IEnumerable<dynamic> GetMeu2(string codRegiao = null)
        {
            SqlConnection conexao = new SqlConnection(_configuration.GetConnectionString("BaseDadosGeograficos"));

            bool queryWithParameter = !String.IsNullOrWhiteSpace(codRegiao);
            var sqlCmd =
                "SELECT R.IdRegiao, " +
                        "R.CodRegiao, " +
                        "R.NomeRegiao, " +
                        "E.SiglaEstado, " +
                        "E.NomeEstado, " +
                        "E.NomeCapital " +
                "FROM dbo.Regioes R " +
                "INNER JOIN dbo.Estados E " +
                    "ON E.IdRegiao = R.IdRegiao " +
                (queryWithParameter ? $"WHERE (R.CodRegiao = @CodigoRegiao) " : String.Empty) +
                "ORDER BY R.NomeRegiao, E.NomeEstado";

            object paramQuery = null;
            if (queryWithParameter)
                paramQuery = new { CodigoRegiao = codRegiao };
            var dados = conexao.Query<dynamic>(sqlCmd, paramQuery); //IEnumerable<object> 


            //////////////////
            List<Regiao> list1 = new List<Regiao>();

            int currIdMaster = 0;
            Regiao currRegiao = null;

            foreach (RegiaoEstado item in dados)
            {
                if (item.IdRegiao != currIdMaster)
                {
                    currRegiao = new Regiao()
                    {
                        IdRegiao = item.IdRegiao,
                        CodRegiao = item.CodRegiao,
                        NomeRegiao = item.NomeRegiao,
                        Estados = new List<Estado>()
                    };
                    list1.Add(currRegiao);
                    currIdMaster = item.IdRegiao;
                }

                Estado estado = new Estado()
                {
                    SiglaEstado = item.SiglaEstado,
                    NomeCapital = item.NomeCapital,
                    NomeEstado = item.NomeEstado
                };
                currRegiao.Estados.Add(estado);
            }

            ////////////////////////

            return list1;
        }

    }
}




//foreach (var item2 in item) //KeyValuePair
//{

//    string aaa = item2.Key;
//    string bbb = (string) item2.Value;

////Dapper.SqlMapper..DapperRow xx = item as 
//System.Reflection.PropertyInfo[] a_oPropertiesX = item.GetType().GetProperties();
//    var data = (IDictionary<string, object>)item;
//    //{ { DapperRow, IdRegiao = '1', CodRegiao = 'CO', NomeRegiao = 'Centro-Oeste', Estados_SiglaEstado = 'DF', Estados_NomeEstado = 'Distrito Federal', Estados_NomeCapital = 'Brasília'} }
//    string aa = "";

//    aa = "";
//}
