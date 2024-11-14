using BulkBoost.Helpers;
using BulkBoost.Model;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;

namespace BulkBoost.Service
{
    public class MultiplexingServiceClient
    {
        private readonly object _lock = new();
        private List<Connection> Connections { get; set; } = new List<Connection>();
        private List<string> ConnectionStrings { get; set; } = new List<string>();

        public MultiplexingServiceClient(List<string> connectionStrings)
        {
            if (connectionStrings.Count == 0)
            {
                throw new ArgumentException("No connection strings provided.");
            }

            ConnectionOptimizer.OptimizeConnection();

            ConnectionStrings = connectionStrings;
        }

        #region Public Methods
        public void Execute(OrganizationRequest request, int numberOfThreads = 20, int batchSize = 10)
        {
            Initialize(numberOfThreads);

            switch (request)
            {
                case CreateMultipleRequest createMultipleRequest:
                    RequestExecutor.ExecuteCreateMultiple(createMultipleRequest, numberOfThreads, batchSize, GetServiceClient);
                    break;
                case UpdateMultipleRequest updateMultipleRequest:
                    RequestExecutor.ExecuteUpdateMultiple(updateMultipleRequest, numberOfThreads, batchSize, GetServiceClient);
                    break;
                case UpsertMultipleRequest deleteMultipleRequest:
                    throw new NotImplementedException();
                case ExecuteMultipleRequest executeMultipleRequest:
                    RequestExecutor.ExecuteExecuteMultiple(executeMultipleRequest, numberOfThreads, batchSize, GetServiceClient);
                    break;
                default:
                    throw new ArgumentException("Request type not supported.");
            }

            DisposeConnections();
        }
        #endregion

        #region Private Methods

        private ServiceClient GetServiceClient()
        {
            lock (_lock)
            {
                var connection = Connections.OrderBy(e => e.Counter).First();
                return connection.GetServiceClient();
            }
        }

        private void Initialize(int numberOfThreads)
        {
            List<Connection> baseConnections = new List<Connection>();

            foreach (var connectionString in ConnectionStrings)
            {
                var connection = new Connection(connectionString);
                baseConnections.Add(connection);
                Connections.Add(connection);
            }

            for (int i = 0; i < numberOfThreads - baseConnections.Count; i++)
            {
                var baseConnectionIndex = i % baseConnections.Count;
                var connection = baseConnections[baseConnectionIndex];
                var clonedConnection = connection.Clone();
                Connections.Add(clonedConnection);
            }
        }

        private void DisposeConnections()
        {
            foreach (var connection in Connections)
            {
                connection.ServiceClient.Dispose();
            }
        }

        #endregion    
    }
}