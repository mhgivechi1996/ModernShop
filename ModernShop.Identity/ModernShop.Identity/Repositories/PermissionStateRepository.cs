using Dapr.Client;
using Identity.API.Models.Dto;
using Microsoft.Extensions.Logging;

namespace Identity.API.Repositories
{
    public class PermissionStateRepository : IPermissionStateRepository
    {
        private const string DAPR_STORE_NAME = "statestore";
        private readonly DaprClient _daprClient;
        private readonly ILogger logger;

        public PermissionStateRepository(DaprClient daprClient, ILogger<PermissionStateRepository> logger)
        {
            _daprClient = daprClient;
            this.logger = logger;
        }

        public async Task SaveUserPermissionsState(Guid userId, List<UserPermissionDto> permissions)
        {
          
            //await _daprClient.SaveStateAsync<string>(DAPR_STORE_NAME,"test", "Mohsen");

            foreach (var item in permissions)
            {
                var stateKey = (userId.ToString() + item.AppName).ToLower();
                logger.LogInformation("Write State Key:");
                logger.LogInformation(stateKey);
                await _daprClient.SaveStateAsync<List<string>>(DAPR_STORE_NAME,stateKey, item.PermissioKeys);
            }
        }

        public async Task<List<UserPermissionDto>> GetUserPermissionsState(Guid userId)
        {
            var result = new List<UserPermissionDto>();

            var appNames = new List<string>{ "catalog", "filserver" };
            foreach (var appName in appNames)
            {
                var stateKey = $"{userId}{appName}";

                logger.LogInformation("Get State Key:");
                logger.LogInformation(stateKey.ToLower());

                var appPermissions= await _daprClient.GetStateAsync<List<string>>(DAPR_STORE_NAME, stateKey.ToLower());
                result.Add(new UserPermissionDto
                {
                    AppName = appName,
                    PermissioKeys = appPermissions,
                });
            }
          

            return result;
        }
    }
}
