
# Abstract

This is a sample demonstrating how to migrate a local database from Windows Phone to a UWP app using an Azure Cloud Service.

# Problem

If you have created a Windows Phone 8 app with a local database, then migrate to UWP, the data in your local database file is no longer accessible. This is because the underlying data is stored using SqlServer Compact, and this does not run on UWP.

# Proposed Solution

One possible solution is to create an Azure Cloud Service which opens the database file, and returns a text file which you can then use to import back into the UWP app. 

# Project Organization

The project is broken into these components:

* Todolist.Portable: Models the data which all other components will process and present. Here a 'todo item'.
* Todolist.Wp8: Windows Phone 8 (Silverlight) app, with example functionality to add and change todo items.
* Todolist.Uwp: Universal Windows Platform app, with the same functionality. In addition, there is an example migration workflow.
* Todolist.AzureCloudService: The configuration for the azure cloud service used to migrate the data
* Todolist.MigrationWorkerRole: The actual code running in the cloud to do the migration.

# How to try it out

1. Set up an Azure cloud account, and a storage account
2. Add the details into 'app.config' in Todolist.MigrationWorkerRole
3. Add the details into 'app.config' in Todolist.Uwp
4. Build and deploy the azure cloud service
5. Build and deploy the Wp8 version to a Windows 10 phone
6. Launch the app, and create a few todo items
7. Build and deploy the UWP version to the same phone
8. Notice the migration prompt, and press the 'Migrate my data' button
9. Go through the workflow to upload your data
10. The cloud service processes data every hour, so you may have to wait up to an hour
11. Once it's finished migrating, close the migration screen, and notice that the todo items you created in step 6 are now there in your UWP app.
