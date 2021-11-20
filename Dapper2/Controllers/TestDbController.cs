
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using Dapper;
using Candal.Models;
using Microsoft.Extensions.Configuration;

namespace Candal.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestDbController : ControllerBase
    {
        private readonly ILogger<TestDbController> _logger;
        private readonly IConfiguration _configuration;

        public TestDbController(ILogger<TestDbController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("users-dapper")]
        public async Task<ActionResult> GetUsersDapper()//[FromBody]
        {
            
            try
            {
                DateTime dt = WriteStaticTimes("Product Start", null);

                using System.Data.SQLite.SQLiteConnection conn =
                    new System.Data.SQLite.SQLiteConnection(_configuration.GetConnectionString("DefaultConnectionSqlite"));

                await conn.OpenAsync();
                IEnumerable<Product> result = await conn.QueryAsync<Product>("Select * From products");//time:00:00:59.6280715
                await conn.CloseAsync();

                DateTime dt1 = WriteStaticTimes("Product QUERY END", dt);

                List<Product> result2 = new List<Product>();
                result2 = result.ToList(); //00:00:00.0408799 

                DateTime dt2 = WriteStaticTimes("Product ToList END", dt1);

                //sort example
                //List<Product> SortedList = result2.OrderBy(fields => fields.Name).ToList(); //time:00:00:58.6235209
                
                // if I want to spread the data to other classes, sub-class 
                //List<Product> result3 = new List<Product>();// time:00:00:01.8605035
                //foreach (var item in result)
                //{
                //    var prod = new Product()
                //    {
                //        Id = item.Id,
                //        Guid = item.Guid,
                //        Name = item.Name,
                //        Price = item.Price,
                //        Validation = item.Validation
                //    };
                //    result3.Add(prod);
                //}
                //////////////////////////////////////////////////////////
                ///
                WriteStaticTimes("Product PROCESS END", dt2);


                return Ok("result2");

            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }

  
        [HttpGet]
        [Route("products-pure-sql")]
        public async Task<ActionResult> GetProductssPureSql()//[FromBody]
        {
            DateTime dt = WriteStaticTimes("Product Start", null);

            try
            {
                using System.Data.SQLite.SQLiteConnection conn =
                     new System.Data.SQLite.SQLiteConnection(_configuration.GetConnectionString("DefaultConnectionSqlite"));

                await conn.OpenAsync();

                //using System.Data.SqlClient.SqlTransaction transaction = conn.BeginTransaction("SampleTransaction");

                using System.Data.SQLite.SQLiteCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM products";
                //cmd.Transaction = transaction;

                //cmd.ExecuteNonQuery();

                using System.Data.SQLite.SQLiteDataReader reader = cmd.ExecuteReader();

                //get columns name
                //var columns = new List<string>();
                //for (int i = 0; i < reader.FieldCount; i++)
                //{
                //    columns.Add(reader.GetName(i));
                //    Type aaa = reader.GetFieldType(i);
                //}

                var result = new List<Product>();
                //mapper
                while (reader != null && await reader.ReadAsync())
                {
                    Product product = new Product();

                    //product.Id = reader.GetInt32(reader.GetOrdinal("id"));
                    //product.Guid = reader.GetString(reader.GetOrdinal("guid"));
                    //product.Name = reader.GetString(reader.GetOrdinal("name"));
                    //product.Price = reader.GetInt32(reader.GetOrdinal("price"));
                    //product.Validation = DateTime.Parse(reader.GetString(reader.GetOrdinal("validation")));

                    product.Id = System.Convert.ToInt32(reader["id"]);
                    product.Guid = System.Convert.ToString(reader["guid"]);
                    product.Name = System.Convert.ToString(reader["name"]);
                    product.Price = System.Convert.ToInt32(reader["price"]);
                    product.Validation = System.Convert.ToDateTime(reader["validation"]);

                    result.Add(product);
                }

                await reader.CloseAsync();

                //transaction.Commit();

                await conn .CloseAsync();

                WriteStaticTimes("Product End", dt);

                //////////////////////
                return Ok("result");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        private DateTime WriteStaticTimes(string msg, DateTime? lastDate)
        {
            DateTime date = DateTime.Now;
            System.Diagnostics.Debug.WriteLine(msg + ": " + date.ToString("MM/dd/yyyy hh:mm:ss.fff"));

            if (lastDate != null)
            {
                TimeSpan interval = date - (DateTime)lastDate;
                System.Diagnostics.Debug.WriteLine(msg + ":INTERVAL:" + interval.ToString());
            }

            return date;
        }
    }

}





//----------------------------------------------------------------------------------------
//conn.open;
//using var trans = conn.BeginTransaction();

//val = "my value";
//conn.Execute("insert into Table(val) values (@val)", new { val }, trans);

//conn.Execute("update Table set val = @val where Id = @id", new { val, id = 1 });

//trans.Commit()

//val = "my value";
//cnn.Execute("insert into Table(val) values (@val)", new { val });

//cnn.Execute("update Table set val = @val where Id = @id", new { val, id = 1 });