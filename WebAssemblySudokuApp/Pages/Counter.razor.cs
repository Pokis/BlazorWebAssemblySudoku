using Microsoft.AspNetCore.SignalR.Client;

namespace WebAssemblySudokuApp.Pages
{
    public partial class Counter
    {
        private int currentCount = 0;
        private string appInfo = "";
        private HubConnection listenHubConnection;
        private HubConnection publishHubConnection;
        private HubConnection appInfoHubConnection;

        private void IncrementCount()
        {
            currentCount++;
        }

        private async void InvokeSignalR()
        {
            // Initialize the SignalR connection with your local server URL
            listenHubConnection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5001/myHub") // Replace with your SignalR hub URL
                .Build();

            // Subscribe to a method the server can invoke (e.g., ReceiveMessage)
            listenHubConnection.On<string>("ReceiveMessage", (message) =>
            {
                // Handle the received message
                Console.WriteLine("Message from server: " + message);
            });

            // Start the connection
            await listenHubConnection.StartAsync();
        }

        private async void SendToSignalR()
        {
            // Invoke a method on the server (e.g., SendMessage)
            await publishHubConnection.InvokeAsync("SendMessage", "Hello from Blazor!");
        }

        private async void GetApplicationInformation()
        {
            try
            {
                if (appInfoHubConnection is null)
                {
                    appInfoHubConnection = new HubConnectionBuilder()
                        .WithUrl("http://localhost:8084/hubs/ApplicationSupportHub") // Replace with your SignalR hub URL
                        .Build();
                    await appInfoHubConnection.StartAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("failed connecting " + ex.Message);
            }

            try
            {
                appInfo = await appInfoHubConnection.InvokeAsync<string>("getApplicationInformation");
            }
            catch (Exception ex)
            {
                Console.WriteLine("failed getting app info " + ex.Message);
            }
        }
    }
}
