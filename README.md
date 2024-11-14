# BulkBoostDemo

The **BulkBoost** console application is part of the session demo, designed to demonstrate the user multiplexing pattern for Dynamics 365 CE integration scenarios. Follow these steps to set up and execute the demo:

## Setup Instructions

1. **Configure Organization URL**  
   - Open `BulkBoost.Console/Constants/ConnectionStrings.cs`.
   - Update the `Url` parameter with your Dynamics 365 CE organization endpoint.

2. **Create App Registrations in Azure**  
   - Log in to the [Azure Portal](https://portal.azure.com).
   - Register as many applications as needed for your test scenario.
   - Ensure each app registration has API permissions to access your Dynamics 365 CE environment.

3. **Create Application Users in Power Platform Admin Center**  
   - For each app registration, create a corresponding Application User in the [Power Platform Admin Center](https://admin.powerplatform.microsoft.com/).
   - Assign an appropriate security role to each user, ensuring they have permissions for CRUD operations in Dynamics 365 CE.

4. **Set Up Client Credentials**  
   - In `BulkBoost.Console/Constants/ConnectionStrings.cs`, add each app registration’s `Client ID` and `Secret` to the `clientCredentials` list. 
   - Format each entry as an object containing `ClientId` and `ClientSecret`.

5. **Run the Demo**  
   - With the `Url`, `clientCredentials`, and security roles configured, you’re ready to test the multiplexing pattern.
   - Build and run the application to observe performance variations in multi-user parallel operations.

---

This setup guide provides a straightforward path for configuring and running the BulkBoost demo.
