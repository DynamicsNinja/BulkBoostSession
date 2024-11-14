# BulkBoostDemo

BulkBost console applicaiton is part of the session demo where you can test user multiplexing pattern as Dynamics 365 CE integration scenario.

Steps you need to run this demo are sumple:
  1. Edit Url parameter inside BulkBoost.Console/Constants/ConnectionStrings.cs that points to your organization
  2. Create as many app registrations on Azure Portal
  3. Create user for each app registration on Admin Power Apps portal and assign security role to them
  4. Add those Client IDs and Screts in BulkBoost.Console/Constants/ConnectionStrings.cs as clientCredentials list
  5. You are ready to test the solution
