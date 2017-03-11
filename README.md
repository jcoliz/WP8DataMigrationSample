# Abstract

This is a sample demonstrating how to migrate a local database from Windows Phone to a UWP app using an Azure Cloud Service.

# Problem

If you have created a Windows Phone 8 app with a local database, then migrate to UWP, the data in your local database file is no longer accessible. This is because the underlying data is stored using SqlServer Compact, and this does not run on UWP.

# Proposed Solution

One possible solution is to create an Azure Cloud Service which opens the database file, and returns a text file which you can then use to import back into the UWP app. 
