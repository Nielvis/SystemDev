using System.Net;

namespace DOM.Presentation.Implementation.Interfaces
{
    public interface IDbService
    {
        int Execute(string Query, string Database = "master");

        List<T> Select<T>(string Query);

        (HttpStatusCode StatusCode, List<T> Data) Procedure<T>(string Database, string Procedure, Dictionary<string, object> Parameters);

    }
}
