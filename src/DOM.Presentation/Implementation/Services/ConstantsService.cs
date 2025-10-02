using DOM.Presentation.Implementation.Interfaces;
using Microsoft.Extensions.Primitives;

namespace DOM.Presentation.Implementation.Services
{
    public class ConstantsService : IConstantsService
    {
        private readonly IConfiguration _configuration;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public string ConnectionString { get; set; }

        public string AccessToken { get; set; }

        public string UidRegisterLogged { get; set; }

        public string Enviroment { get; set; }

        public string Acronym { get; set; }

        public ConstantsService(
                IConfiguration configuration, 
                IHttpContextAccessor httpContextAccessor
            )
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;

            GetValues();
        }

        private void GetValues()
        {
            Enviroment = _configuration["Enviroment"]!.ToUpper();
            Acronym = _configuration["Acronym"]!.ToUpper();

            ConnectionString = _configuration["SQL_CONNECTION_STRING"]!;

            StringValues? stringValues = _httpContextAccessor.HttpContext?.Request.Headers.FirstOrDefault((KeyValuePair<string, StringValues> x) => x.Key.Equals("Authorization")).Value;
            AccessToken = (stringValues.HasValue ? ((string?)stringValues.GetValueOrDefault()) : null);
            
            if (AccessToken != null)
            {
                AccessToken = AccessToken.Replace("Bearer ", "");
            }

            UidRegisterLogged = _httpContextAccessor.HttpContext?.Items.FirstOrDefault((KeyValuePair<object, object> x) => x.Key.Equals("UserId")).Value?.ToString()!;
        }
    }
}
