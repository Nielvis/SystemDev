namespace DOM.Presentation.Implementation.Interfaces
{
    public interface IConstantsService
    {
        string ConnectionString { get; set; }

        string AccessToken { get; set; }

        string UidRegisterLogged { get; set; }

        string Enviroment { get; set; }

        string Acronym { get; set; }
    }
}
