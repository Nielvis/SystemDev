using DOM.Presentation.Implementation.Interfaces;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;

namespace DOM.Presentation.Implementation.Services
{
    public class DbService : IDbService
    {
        private readonly ILogger<DbService> _logger;

        private readonly IConstantsService _constantsService;

        public DbService(
                ILogger<DbService> logger,
                IConstantsService constantsService
            )
        {
            _logger = logger;
            _constantsService = constantsService;
        }

        public int Execute(string Query, string Database = "master")
        {
            Query = SanitizeSpecificSqlQuery(Query);

            _logger.LogInformation($"[{_constantsService.Acronym}({_constantsService.Enviroment})] > Query: {Query}");

            try
            {
                var myConn = new SqlConnection(_constantsService.ConnectionString.Replace("master", Database));
                var myCommand = new SqlCommand(Query, myConn);
                try
                {
                    myConn.Open();
                    myCommand.ExecuteNonQuery();
                }
                catch (Exception Ex)
                {
                    _logger.LogError($"[{_constantsService.Acronym}({_constantsService.Enviroment})] > Query: {Query}");
                    _logger.LogError($"[{_constantsService.Acronym}({_constantsService.Enviroment})] > {Ex}");

                    return 0;
                }
                finally
                {
                    if (myConn.State == ConnectionState.Open)
                    {
                        myConn.Close();
                    }
                }
                return (int)HttpStatusCode.Created;
            }
            catch (Exception Ex)
            {
                _logger.LogError($"[{_constantsService.Acronym}({_constantsService.Enviroment})] > Query: {Query}");
                _logger.LogError($"[{_constantsService.Acronym}({_constantsService.Enviroment})] > {Ex.Message}");
            }
            return 0;
        }

        public List<T> Select<T>(string Query)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_constantsService.ConnectionString))
                {
                    connection.Open();

                    SqlCommand select = new SqlCommand(Query, connection);
                    select.CommandTimeout = 0;
                    SqlDataReader reader = select.ExecuteReader();
                    var employeeList = new ReflectionPopulator<T>().CreateList(reader);

                    return employeeList.Cast<T>().ToList();
                }
            }
            catch (Exception Ex)
            {
                _logger.LogError($"[{_constantsService.Acronym}({_constantsService.Enviroment})] > Query: {Query}");
                _logger.LogError($"[{_constantsService.Acronym}({_constantsService.Enviroment})] > {Ex.Message}");

                return new List<T>();
            }
        }

        private class ReflectionPopulator<T>
        {
            public virtual List<T> CreateList(SqlDataReader reader)
            {
                var results = new List<T>();
                var properties = typeof(T).GetProperties();

                while (reader.Read())
                {
                    var item = Activator.CreateInstance<T>();
                    foreach (var property in typeof(T).GetProperties())
                    {
                        try
                        {
                            if (!reader.IsDBNull(reader.GetOrdinal(property.Name)))
                            {
                                Type convertTo = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                                property.SetValue(item, Convert.ChangeType(reader[property.Name], convertTo), null);
                            }
                        }
                        catch (Exception Ex)
                        {
                            _ = Ex.Message;
                        }
                    }
                    results.Add(item);
                }
                return results;
            }
        }

        public (HttpStatusCode StatusCode, List<T> Data) Procedure<T>(string Database, string Procedure, Dictionary<string, object> Parameters)
        {
            var StatusCode = HttpStatusCode.OK;

            var ListParameter = new List<string>();

            foreach (var parameter in Parameters)
            {
                ListParameter.Add($"{parameter.Key} = '{parameter.Value}'");
            }
            var Query = $"use {Database}; EXEC {Procedure} " + String.Join(", ", ListParameter);

            var GetDataSet = GetReferenceTablesDataSet(Query);
            var CollectionDataSet = GetDataSet.Result.Ds.Tables;

            DataTable status = CollectionDataSet["Status"]!;

            var Status = status.Rows[0]["Status"].ToString()!;
            var Message = status.Rows[0]["Message"].ToString()!;
            var Quantity = status.Rows[0]["Quantity"].ToString()!;

            StatusCode = (HttpStatusCode)Convert.ToInt32(status.Rows[0]["StatusCode"].ToString()!);

            if (!Status.Equals("OK"))
            {
                return (StatusCode, null)!;
            }

            return (StatusCode, ConvertDataTable<T>(CollectionDataSet["Data"]!));
        }


        private List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            if (dt is not null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    T item = GetItem<T>(row);
                    data.Add(item);
                }
            }
            return data;
        }

        private T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }

        private async Task<(bool success, Exception exception, DataSet Ds)> GetReferenceTablesDataSet(string q)
        {
            DataSet ds = new();

            try
            {
                var adapter = new SqlDataAdapter();
                await using SqlConnection cn = new SqlConnection(_constantsService.ConnectionString);
                var command = new SqlCommand(q, cn);
                adapter.SelectCommand = command;

                adapter.Fill(ds);

                ds.Tables[0].TableName = "Status";

                if (ds.Tables.Count > 1)
                {
                    ds.Tables[1].TableName = "Data";
                }

                return (true, null, ds);
            }
            catch (Exception localException)
            {
                return (false, localException, null);
            }
        }

        private string SanitizeSpecificSqlQuery(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            string pattern = @"(WHERE\s+Texto\s*=\s*')([^']*)'([^']*)'";
            Match match = Regex.Match(input, pattern, RegexOptions.IgnoreCase);

            if (match.Success)
            {
                string part1 = match.Groups[1].Value;
                string middlePart = match.Groups[2].Value;
                string lastPart = match.Groups[3].Value;

                string newMiddlePart = middlePart.Replace("'", "`");

                return $"{part1}{newMiddlePart}'{lastPart}";
            }

            return input;
        }
    }
}
