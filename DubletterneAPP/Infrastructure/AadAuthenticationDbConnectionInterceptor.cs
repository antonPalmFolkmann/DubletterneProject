using System.Data.Common;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;

namespace Infrastructure
{
    public class AadAuthenticationDbConnectionInterceptor : DbConnectionInterceptor
    {
        private const string TENANT_ID = "33a5d5c9-1cff-47b3-a604-c8d39aa4931f";
        private const string CLIENT_ID = "94eabc4d-57ab-4d90-b5d8-92dad5c49f2e"; // Azure_SQL
        private const string CLIENT_SECRET = "x~-7Q~th26rtfoBxGJx3aZF0IZAWedtFuXhGM";
        private const string REDIRECT_URI = "http://localhost:7073";
        private const string CONNECTION_STRING = "Server=tcp:dubletterne.database.windows.net,1433;Initial Catalog=Project;Persist Security Info=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False";
        private static readonly string[] scopes = new string[] { "https://database.windows.net/.default" };
        public override async ValueTask<InterceptionResult> ConnectionOpeningAsync(
            DbConnection connection, 
            ConnectionEventData eventData, 
            InterceptionResult result, 
            CancellationToken cancellationToken)
        {
            var sqlConnection = (SqlConnection) connection;

            var connectionStringBuilder = new SqlConnectionStringBuilder(sqlConnection.ConnectionString);
            if (connectionStringBuilder.DataSource.Contains("database.windows.net", StringComparison.OrdinalIgnoreCase) && string.IsNullOrEmpty(connectionStringBuilder.UserID))
            {
                sqlConnection.AccessToken = await GetAccessToken_ClientCredentials();
            }
            
            return await base.ConnectionOpeningAsync(connection,eventData,result,cancellationToken);
        }

        public static async Task<string> GetAccessToken_UserInteractive()
        {
        
            IPublicClientApplication app = PublicClientApplicationBuilder
                .Create(CLIENT_ID)
                .WithAuthority(AzureCloudInstance.AzurePublic, TENANT_ID)
                .WithRedirectUri(REDIRECT_URI)
                .Build();           
        
            string accessToken = string.Empty;
            Console.WriteLine("im here");
            AuthenticationResult authResult = null;
            IEnumerable<IAccount> accounts = await app.GetAccountsAsync();
            
            Console.WriteLine("im here 2");            
            try
            {
                            Console.WriteLine("im here 3");
                authResult = await app.AcquireTokenSilent(scopes, accounts.FirstOrDefault()).ExecuteAsync();
                            Console.WriteLine("im here 4");
                accessToken = authResult.AccessToken;
            } catch (MsalUiRequiredException)
            {
                            Console.WriteLine("im here 5");
                authResult = await app.AcquireTokenInteractive(scopes).ExecuteAsync();
                             Console.WriteLine("im here 6");
                accessToken = authResult.AccessToken;
            } catch (Exception ex)
            {
                Console.WriteLine($"Authentication error: {ex.Message}");
            }
        
            Console.WriteLine($"Access token: {accessToken}\n");

            return accessToken;
        }

        public static string GetToken()
        {
            return GetAccessToken_UserInteractive().Result;
        }

        public static async Task<string> GetAccessToken_ClientCredentials()
        {
        
            IConfidentialClientApplication app = ConfidentialClientApplicationBuilder
                .Create(CLIENT_ID)
                .WithClientSecret(CLIENT_SECRET)
                .WithAuthority(AzureCloudInstance.AzurePublic, TENANT_ID)
                .WithRedirectUri(REDIRECT_URI)
                .Build();
        
            string accessToken = string.Empty;
        
            AuthenticationResult authResult = null;
        
            try
            {
                authResult = await app.AcquireTokenForClient(scopes).ExecuteAsync();
                accessToken = authResult.AccessToken;
            }
            catch (MsalClientException ex)
            {
                Console.Write($"Error obtaining access token: {ex.Message}");
                
            }
        
            return accessToken;
        }
    }
}