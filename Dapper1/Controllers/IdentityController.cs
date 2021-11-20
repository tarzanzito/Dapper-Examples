
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

//<PackageReference Include = "Microsoft.Extensions.Configuration" Version = "5.0.0" />
//<PackageReference Include = "Microsoft.Extensions.Configuration.Json" Version = "5.0.0" />

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IdentityController : ControllerBase
    {
        private readonly ILogger<IdentityController> _logger;
        private readonly IConfiguration _configuration;

        public IdentityController(ILogger<IdentityController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("get-all-users")]
        public async Task<ActionResult> GetAllUsers()
        {
            using IDbConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            //using SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            //await conn.OpenAsync();

            string query = "SELECT * FROM AspNetUsers";
            var result = await conn.QueryAsync<MyIdentityUser>(query);

            //await conn.CloseAsync();

            return this.Ok(result);
        }

        [HttpGet]
        [Route("get-user-by-name/{userName}")]
        public async Task<ActionResult> GetUserByName([FromRoute] string userName)
        {
            using SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            //await conn.OpenAsync();

            string query = "SELECT * FROM AspNetUsers WHERE NormalizedUserName = @UserName";
            //QueryAsync<T>(string sql, object param = null, IDbTransaction transaction = null, int ? commandTimeout = null, CommandType ? commandType = null);
            
            var result = await conn.QuerySingleOrDefaultAsync<MyIdentityUser>(query, new { @userName = userName.ToUpper() });
            //var result = await conn.QueryFirstOrDefaultAsync<MyIdentityUser>(query, new { @userName = userName.ToUpper() });

            //await conn.CloseAsync();

            return this.Ok(result);
        }

        [HttpPost]
        [Route("insert-user")]
        public async Task<ActionResult> InsertUser([FromBody] MyIdentityUser myIdentityUser)
        {
            myIdentityUser.Id = Guid.NewGuid().ToString();

            using SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            //await conn.OpenAsync();

            IDbTransaction trans = await conn.BeginTransactionAsync();

            string sql = "INSERT INTO AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail ,EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount) VALUES (@Id, @UserName, @NormalizedUserName, @Email, @NormalizedEmail, @EmailConfirmed, @PasswordHash, @SecurityStamp, @ConcurrencyStamp, @PhoneNumber, @PhoneNumberConfirmed, @TwoFactorEnabled, @LockoutEnd, @LockoutEnabled, @AccessFailedCount)";
            //@Id, @UserName, @NormalizedUserName, @Email, @NormalizedEmail, @EmailConfirmed, @PasswordHash, @SecurityStamp, @ConcurrencyStamp, @PhoneNumber, @PhoneNumberConfirmed, @TwoFactorEnabled, @LockoutEnd, @LockoutEnabled, @AccessFailedCount)";

            var affectedRows = await conn.ExecuteAsync(sql, myIdentityUser); //because sql colmuns names and fields class name are quals

            trans.Commit();
            //await conn.CloseAsync();

            return this.Ok(myIdentityUser);
        }

        [HttpPut]
        [Route("update-user")]
        public async Task<ActionResult> UpdateUser([FromBody] MyIdentityUser myIdentityUser)
        {
            using SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            //await conn.OpenAsync();

            string sql = "UPDATE AspNetUsers SET UserName = @UserName, NormalizedUserName = @NormalizedUserName, Email = @Email, NormalizedEmail = @NormalizedEmail, EmailConfirmed = @EmailConfirmed, PasswordHash = @PasswordHash, SecurityStamp = @SecurityStamp, ConcurrencyStamp = @ConcurrencyStamp, PhoneNumber = @PhoneNumber, PhoneNumberConfirmed = @PhoneNumberConfirmed, TwoFactorEnabled = @TwoFactorEnabled, LockoutEnd = @LockoutEnd, LockoutEnabled = @LockoutEnabled, AccessFailedCount = @AccessFailedCount WHERE Id = @Id";
            //@Id, @UserName, @NormalizedUserName, @Email, @NormalizedEmail, @EmailConfirmed, @PasswordHash, @SecurityStamp, @ConcurrencyStamp, @PhoneNumber, @PhoneNumberConfirmed, @TwoFactorEnabled, @LockoutEnd, @LockoutEnabled, @AccessFailedCount)";

            var affectedRows = await conn.ExecuteAsync(sql, myIdentityUser); //because sql colmuns names and fields class name are quals

            //await conn.CloseAsync();

            return this.Ok(myIdentityUser);
        }

        [HttpDelete]
        [Route("delete-user/{id}")]
        public async Task<ActionResult> Delete([FromRoute] string id)
        {
            using SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            //await conn.OpenAsync();

            var parms = new { @Id = id };
            string sql = "DELETE FROM AspNetUsers WHERE Id = @Id";
            var affectedRows = await conn.ExecuteAsync(sql, parms);

            //await conn.CloseAsync();

            return this.Ok(id);
        }

        [HttpGet]
        [Route("get-all-view1")]
        public async Task<ActionResult> GetAllView1()
        {
            using SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            //await conn.OpenAsync();

            string query = "SELECT * FROM View_1";
            var result = await conn.QueryAsync<MyView1>(query);

            //await conn.CloseAsync();

            return this.Ok(result);
        }

        [HttpGet]
        [Route("get-all-view1-childs")]
        public async Task<ActionResult> GetAllView1Childs()
        {
            using SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            //await conn.OpenAsync();

            string query = "SELECT * FROM View_1";
            IEnumerable<MyView1> result = await conn.QueryAsync<MyView1>(query);

            //await conn.CloseAsync();

            //////////////////////////////////////////////////////////////////
            // transform basic list into: One User -> Multi Roles - One/Many
            //////////////////////////////////////////////////////////////////
            List<MyView1Child> resultList = new List<MyView1Child>();

            string lastParentKey = "";
            MyView1Child parent = null;

            foreach (MyView1 item in result)
            {
                if (item.Id != lastParentKey)
                {
                    parent = new MyView1Child() //map
                    {
                        Id = item.Id,
                        UserName = item.UserName,
                        NormalizedUserName = item.NormalizedUserName,
                        Email = item.Email,
                        NormalizedEmail = item.NormalizedEmail,
                        PhoneNumber = item.PhoneNumber,
                        Roles = new List<MyViewRole>()
                    };
                    resultList.Add(parent);
                    lastParentKey = item.UserId;
                }

                if (item.RoleId != null)
                {
                    MyViewRole child = new MyViewRole() //map
                    {
                        RoleId = item.RoleId,
                        NormalizedName = item.NormalizedName,
                        Name = item.Name
                    };
                    parent.Roles.Add(child);
                }
            }

            return this.Ok(resultList);
            //return this.Ok(result);
        }


        //todo
        //cmd.CommandText = "SELECT SCOPE_IDENTITY() AS LAST_ID";  //nao resulta!!!
        //object idNew = cmd.ExecuteScalar();

        [HttpGet]
        [Route("get-all-view1-childs2")]
        public async Task<ActionResult> GetAllView1Childs_Dapperr()
        {

              //https://dapper-tutorial.net/result-multi-mapping

            using SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            //await conn.OpenAsync();

             string query = @"SELECT A.Id, A.UserName, A.NormalizedUserName, A.Email, A.NormalizedEmail, A.PhoneNumber, 
            B.UserId, B.RoleId, C.Name, C.NormalizedName
            FROM AspNetRoles C INNER JOIN AspNetUserRoles B ON C.Id = B.RoleId RIGHT OUTER JOIN
            AspNetUsers A ON B.UserId = A.Id ORDER BY  B.UserId";

            //string query = "SELECT * FROM View_1";

            var orderDictionary = new Dictionary<string, MyView1Child>(); //V1
            MyView1Child lastOrderEntry = null; //V2 IEnumerable
            string lastKey = ""; //V2
            IEnumerable<MyView1Child> result = await conn.QueryAsync<MyView1Child, MyViewRole, MyView1Child>(query,
                (parent, child) =>
                {
                    //V2- WITH ORDER BY ID - SORTED
                    if (parent.Id != lastKey)
                        parent.Roles = new List<MyViewRole>();
                    else
                        parent = lastOrderEntry;

                    if (child != null)
                        parent.Roles.Add(child);

                    if (parent.Id != lastKey)
                    {
                        lastOrderEntry = parent;
                        lastKey = parent.Id;
                        return parent;
                    }
                    else
                        return null;

                    ////V1 - WITHOUT ORDER BY ID - RANDOM ORDER
                    //MyView1Child orderEntry;
                    //bool isNew = false;
                    //if (!orderDictionary.TryGetValue(parent.Id, out orderEntry))
                    //{
                    //    orderEntry = parent;
                    //    orderEntry.Roles = new List<MyViewRole>();
                    //    orderDictionary.Add(orderEntry.Id, orderEntry);
                    //    isNew = true;
                    //}

                    //if (child != null)
                    //    orderEntry.Roles.Add(child);

                    //if (isNew)
                    //    return orderEntry;
                    //else
                    //    return null;

                }, splitOn: "RoleId") //onde muda de classe
                ;

            //await conn.CloseAsync();
            
            List<MyView1Child> list = result.ToList();
            //list.RemoveAll(item => item == null);
            return this.Ok(list);
        }
    }
}

//https://balta.io/blog/dapper-entity-framework-core


//https://dapper-tutorial.net/knowledge-base/52217304/adding-multimap-children-into-parent-with-dapper
//https://dapper-tutorial.net/knowledge-base/52277829/understanding-dapper-spliton




//User newUser = conn.QuerySingle<User>(
//                                insertUserSql,
//                                new
//                                {
//                                    Username = "lorem ipsum",
//                                    Phone = "555-123",
//                                    Email = "lorem ipsum"
//                                },
//                                tran);



