using BulkBoost.Model;

namespace BulkBoost.Console.Constants
{
    public static class ConnectionStrings
    {
        const string Url = "https://YOUR_ORG.crm4.dynamics.com";

        public static List<ClientCredentials> clientCredentials = [
            new ClientCredentials {
                ClientId = "YOUR_CLIENT_ID_1",
                ClientSecret = "YOUR_SECRET_1",
            },
            new ClientCredentials {
                ClientId = "YOUR_CLIENT_ID_2",
                ClientSecret = "YOUR_SECRET_2",
            },
            new ClientCredentials {
                ClientId = "YOUR_CLIENT_ID_3",
                ClientSecret = "YOUR_SECRET_4",
            },
            new ClientCredentials {
                ClientId = "YOUR_CLIENT_ID_4",
                ClientSecret = "YOUR_SECRET_4",
            },
            new ClientCredentials {
                ClientId = "YOUR_CLIENT_ID_5",
                ClientSecret = "YOUR_SECRET_5",
            },
        ];

        private static readonly List<string> connectionStringsList = clientCredentials
            .Select(e => $"AuthType=ClientSecret;Url={Url};ClientId={e.ClientId};ClientSecret={e.ClientSecret};RequireNewInstance=true")
            .ToList();
        public static List<string> Get(int number)
        {

            if (number > clientCredentials.Count)
            {
                throw new ArgumentException("Number of connections requested is greater than the number of available connections");
            }

            return connectionStringsList.Take(number).ToList();
        }
        public static List<string> GetAll()
        {
            return connectionStringsList.ToList();
        }

        public static int Count()
        {
            return connectionStringsList.Count;
        }
    }
}