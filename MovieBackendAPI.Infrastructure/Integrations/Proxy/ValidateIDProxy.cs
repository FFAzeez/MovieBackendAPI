//using BPM.Setup.Infrastructure.Integrations.Auth;
//using BPM.Setup.Infrastructure.Integrations.Client;
//using BPM.Setup.Infrastructure.Integrations.Interfaces;
//using InsureService.Domain.BindingModels;
//using InsureService.Domain.Const;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;
//using System.Collections.Generic;
//using System.Net.Http;
//using System.Threading.Tasks;

//namespace BPM.Setup.Infrastructure.Integrations.Proxy
//{
//    public class ValidateIDProxy : IValidateIDProxy
//    {
//        private readonly ILogger<NotificationProxy> _logger;
//        private readonly HttpClientUtil _httpClient;
//        private readonly AuthenticationProxy _authProxy;
//        private readonly IHttpContextAccessor _httpAccessor;
//        private readonly IConfiguration _configuration;


//        private string _clientID;
//        private string _clientSecret;

//        public ValidateIDProxy(ILogger<NotificationProxy> logger,
//            IConfiguration configuration,
//            HttpClientUtil httpClient,
//            IHttpContextAccessor httpAccessor,
//            AuthenticationProxy authProxy
//            )
//        {
//            _logger = logger;
//            _httpClient = httpClient;
//            _httpAccessor = httpAccessor;
//            _configuration = configuration;
//            _authProxy = authProxy;

//            // set env values
//            _clientID = _configuration["InsureApi:ClientId"];
//            _clientSecret = _configuration["InsureApi:ClientSecret"];
//        }
//        public async Task<ServiceResponse<string>> ValidatIdentity(ValidateID payload)
//        {
//            _logger.LogInformation($"'{nameof(ValidatIdentity)}' called in '{nameof(ValidateIDProxy)}'.");

//            var header = new Dictionary<string, string>();
//            var value = _httpAccessor?.HttpContext?.Request?.Headers?.GetCommaSeparatedValues("Authorization");

//            var token = await _authProxy.GetToken(
//                _configuration["OmnichannelGateway:AuthorizationURL"],
//                _clientID,
//                _clientSecret
//                );
//            header.Add("Authorization", "Bearer " + token);

//            var url = string.Format("{0}{1}", _configuration["OmnichannelGateway:URLBase"], _configuration["ExternalApiUrls:ValidateIdUrl"]);
//            _logger.LogInformation($"Validate ID url: {url}");
//            _logger.LogInformation("Validate ID Payload: " + payload);


//            var res = await _httpClient.PostJSONAsync(url, payload, headers: header, cookies: null);
//            var content = await res.GetStringAsync();
//            return new ServiceResponse<string> { StatusCode = ResponseCode.SUCCESSFUL, StatusMessage = content, ResponseObject = content.ToString() };
//        }
//    }
//}
