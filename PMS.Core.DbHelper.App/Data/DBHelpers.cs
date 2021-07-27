using Dapper;
using PMS.Core.DbHelper.App.Enums;
using PMS.Core.DbHelper.App.Generics;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Core.DbHelper.App.Data
{
    public class DBHelpers
    {
        public string ConnectionString { get; set; }
        public DBHelpers(string ConString)
        {
            ConnectionString = ConString;
        }

        public async Task<Result> QueryAsyncList<T>(string sqlQuery, object param = null)
        {
            try
            {
                if (string.IsNullOrEmpty(sqlQuery))
                {
                    return new Result() { Status = ResultStatus.Warning, Message = "Query is null", Data = "" };
                }
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    await conn.OpenAsync();
                    var result = await conn.QueryAsync<T>(sqlQuery, param);
                    await conn.CloseAsync();
                    if (result != null)
                        return new Result() { Data = result.ToList(), Status = ResultStatus.Success, Message = "Success" };
                    else
                        return new Result() { Data = null, Status = ResultStatus.NotFound, Message = "Data Not Found" };
                }
            }
            catch (Exception ex)
            {
                return new Result() { Data = "Exception", Status = ResultStatus.Warning, Message = ex.Message };
            }
        }
        public async Task<Result> QueryAsync<T>(string sqlQuery, object param = null)
        {
            try
            {
                if (string.IsNullOrEmpty(sqlQuery))
                {
                    return new Result() { Status = ResultStatus.Warning, Message = "Query is null", Data = "" };
                }
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    await conn.OpenAsync();
                    var result = await conn.QueryAsync<T>(sqlQuery, param);
                    await conn.CloseAsync();
                    if (result != null)
                        return new Result() { Data = result.SingleOrDefault(), Status = ResultStatus.Success, Message = "Success" };

                    else
                        return new Result() { Data = null, Status = ResultStatus.NotFound, Message = "Data Not Found" };
                }
            }
            catch (Exception ex)
            {
                return new Result() { Data = "Exception", Status = ResultStatus.Warning, Message = ex.Message };
            }
        }
        public Result QueryList<T>(string sqlQuery, object param = null)
        {
            try
            {
                if (string.IsNullOrEmpty(sqlQuery))
                {
                    return new Result() { Status = ResultStatus.Warning, Message = "Query is null", Data = "" };
                }
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    var result = conn.Query<T>(sqlQuery, param);
                    if (result != null)
                        return new Result() { Data = result.ToList(), Status = ResultStatus.Success, Message = "Success" };
                    else
                        return new Result() { Data = null, Status = ResultStatus.NotFound, Message = "Data Not Found" };
                }
            }
            catch (Exception ex)
            {
                return new Result() { Data = "Exception", Status = ResultStatus.Warning, Message = ex.Message };
            }
        }
        public Result Query<T>(string sqlQuery, object param = null)
        {
            try
            {
                if (string.IsNullOrEmpty(sqlQuery))
                {
                    return new Result() { Status = ResultStatus.Warning, Message = "Query is null", Data = "" };
                }
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    var result = conn.Query<T>(sqlQuery, param);
                    if (result != null)
                        return new Result() { Data = result.SingleOrDefault(), Status = ResultStatus.Success, Message = "Success" };
                    else
                        return new Result() { Data = null, Status = ResultStatus.NotFound, Message = "Data Not Found" };
                }
            }
            catch (Exception ex)
            {
                return new Result() { Data = "Exception", Status = ResultStatus.Warning, Message = ex.Message };
            }
        }
        public async Task<Result> QueryMultipleAsync(string sqlQuery, object param = null, List<string> tableNames = null)
        {
            try
            {
                if (string.IsNullOrEmpty(sqlQuery))
                {
                    return new Result() { Status = ResultStatus.Warning, Message = "Query is null", Data = "" };
                }
                using (var conn = new SqlConnection(ConnectionString))
                {
                    await conn.OpenAsync();
                    var multi = await conn.QueryMultipleAsync(sqlQuery, param);
                    var result = new Dictionary<string, dynamic>();
                    int i = 0;
                    while (multi.IsConsumed == false)
                    {
                        var item = await multi?.ReadAsync<dynamic>();
                        result.Add(tableNames[i], item);
                        i++;
                    }
                    await conn.CloseAsync();
                    if (result != null)
                        return new Result() { Data = result, Status = ResultStatus.Success, Message = "Success" };

                    else
                        return new Result() { Data = null, Status = ResultStatus.NotFound, Message = "Data Not Found" };
                }
            }
            catch (Exception ex)
            {
                return new Result() { Data = "Exception", Status = ResultStatus.Warning, Message = ex.Message };
            }
        }
        public Result QueryMultiple(string sqlQuery, object param = null, List<string> tableNames = null)
        {
            try
            {
                if (string.IsNullOrEmpty(sqlQuery))
                {
                    return new Result() { Status = ResultStatus.Warning, Message = "Query is null", Data = "" };
                }
                using (var conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    var multi = conn.QueryMultiple(sqlQuery, param);
                    var result = new Dictionary<string, dynamic>();
                    int i = 0;
                    while (multi.IsConsumed == false)
                    {
                        var item = multi?.Read<dynamic>();
                        result.Add(tableNames[i], item);
                        i++;
                    }
                    conn.Close();
                    if (result != null)
                        return new Result() { Data = result, Status = ResultStatus.Success, Message = "Success" };

                    else
                        return new Result() { Data = null, Status = ResultStatus.NotFound, Message = "Data Not Found" };
                }
            }
            catch (Exception ex)
            {
                return new Result() { Data = "Exception", Status = ResultStatus.Warning, Message = ex.Message };
            }
        }
        /// <summary>
        ///  Execute Method use for INSERT UPDATE DELETE
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <param name="param"></param>
        /// <returns>After Execution Return Single Table is Best Practices</returns>
        public async Task<Result> ExecuteAsync<T>(string sqlQuery, object param = null)
        {
            try
            {
                if (string.IsNullOrEmpty(sqlQuery))
                {
                    return new Result() { Status = ResultStatus.Warning, Message = "Query is null", Data = "" };
                }
                else
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        await conn.OpenAsync();
                        using (var transaction = await conn.BeginTransactionAsync())
                        {
                            try
                            {
                                var result = await conn.QueryAsync<T>(sqlQuery, param);
                                await transaction.CommitAsync();
                                await conn.CloseAsync();
                                return new Result()
                                {
                                    Data = result.SingleOrDefault(),
                                    Status = ResultStatus.Success,
                                    Message = "Success"
                                };
                            }
                            catch (Exception ex)
                            {
                                await transaction.RollbackAsync();
                                return new Result()
                                {
                                    Data = "Exception",
                                    Status = ResultStatus.Warning,
                                    Message = ex.Message

                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new Result()
                {
                    Data = "Exception",
                    Status = ResultStatus.Warning,
                    Message = ex.Message

                };
            }
        }

        public async Task<Result> ExecuteAsyncList<T>(string sqlQuery, object param = null)
        {
            try
            {
                if (string.IsNullOrEmpty(sqlQuery))
                {
                    return new Result() { Status = ResultStatus.Warning, Message = "Query is null", Data = "" };
                }
                else
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        await conn.OpenAsync();
                        using (var transaction = await conn.BeginTransactionAsync())
                        {
                            try
                            {
                                var result = await conn.QueryAsync<T>(sqlQuery, param);
                                await transaction.CommitAsync();
                                await conn.CloseAsync();
                                return new Result()
                                {
                                    Data = result.ToList(),
                                    Status = ResultStatus.Success,
                                    Message = "Success"
                                };
                            }
                            catch (Exception ex)
                            {
                                await transaction.RollbackAsync();
                                return new Result()
                                {
                                    Data = "Exception",
                                    Status = ResultStatus.Warning,
                                    Message = ex.Message

                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new Result()
                {
                    Data = "Exception",
                    Status = ResultStatus.Warning,
                    Message = ex.Message

                };
            }
        }
        /// <summary>
        /// Execute Method use for INSERT UPDATE DELETE
        /// </summary>
        /// <param name="sqlQuery"> EXEC SP @parameter  and query like INSERT INTO TABLE (columns) VALUES(@columns), select * from table</param>
        /// <param name="param">Patameter pass param: new { ID = ID } </param>
        /// <returns>After Execution Return Single Table is Best Practices</returns>
        public Result Execute(string sqlQuery, object param = null)
        {
            try
            {
                if (string.IsNullOrEmpty(sqlQuery))
                {
                    return new Result() { Status = ResultStatus.Warning, Message = "Query is null", Data = "" };
                }
                else
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        using (var transaction = conn.BeginTransaction())
                        {
                            try
                            {
                                var result = conn.Query<dynamic>(sqlQuery, param);
                                transaction.Commit();
                                conn.Close();
                                return new Result()
                                {
                                    Data = result.SingleOrDefault(),
                                    Status = ResultStatus.Success,
                                    Message = "Success"
                                };
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                return new Result()
                                {
                                    Data = "Exception",
                                    Status = ResultStatus.Warning,
                                    Message = ex.Message

                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new Result()
                {
                    Data = "Exception",
                    Status = ResultStatus.Warning,
                    Message = ex.Message

                };
            }
        }

        public Result ExecuteList<T>(string sqlQuery, object param = null)
        {
            try
            {
                if (string.IsNullOrEmpty(sqlQuery))
                {
                    return new Result() { Status = ResultStatus.Warning, Message = "Query is null", Data = "" };
                }
                else
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        try
                        {
                            conn.Open();
                            var result = conn.Query<T>(sqlQuery, param);
                            conn.Close();
                            return new Result()
                            {
                                Data = result.ToList(),
                                Status = ResultStatus.Success,
                                Message = "Success"
                            };
                        }
                        catch (Exception ex)
                        {
                            return new Result()
                            {
                                Data = "Exception",
                                Status = ResultStatus.Warning,
                                Message = ex.Message

                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new Result()
                {
                    Data = "Exception",
                    Status = ResultStatus.Warning,
                    Message = ex.Message

                };
            }
        }

        public DataSet GetDataSet(string query)
        {
            DataSet ds = new DataSet(); 
            using (SqlConnection sqlcon=new SqlConnection(ConnectionString))
            {
                using (SqlDataAdapter da = new SqlDataAdapter(query, sqlcon))
                {
                    da.Fill(ds);
                    return ds;
                }
            }
        }
    }

}
