using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Xml;
using System.Xml.Linq;
using Todolist.Portable.Helpers;
using System.Xml.Serialization;
using Todolist.Portable.Models;
using Todolist.Uwp.Models;

namespace Todolist.Uwp.ViewModels
{
    /// <summary>
    /// Handles migration of Database from WP8 to Win 10
    /// </summary>
    public class MigrationViewModel 
    {
        #region Public Properties

        /// <summary>
        /// Whether there are some Database to migrate
        /// </summary>
        public bool HasDatabase => (MigrationStatus != MigrationStatusEnum.NoMigrationNeeded && MigrationStatus != MigrationStatusEnum.Invalid);

        public bool IsWaiting
        {
            get { return _IsWaiting; }
            set
            {
                _IsWaiting = value;
                SetProperty(nameof(IsWaiting));
            }
        }
        private bool _IsWaiting = false;

        public enum MigrationStatusEnum
        {
            Invalid = 0,
            /// <summary>
            /// No database file was found, so no migration is needed
            /// </summary>
            NoMigrationNeeded,
            /// <summary>
            /// There is a database file, and we have not started to migrate it yet
            /// </summary>
            Starting,
            /// <summary>
            /// We have uploaded the database file, now we are awaiting a response
            /// </summary>
            Waiting,
            /// <summary>
            /// The migration file is ready for us to download from the server
            /// </summary>
            Ready,
            /// <summary>
            /// We have completed the migration, but haven't deleted the file yet, in case we
            /// run into problems and want to try it again
            /// </summary>
            Done
        };

        /// <summary>
        /// Current state of track migration
        /// </summary>
        public MigrationStatusEnum MigrationStatus
        {
            get
            {
                var result = Setting_GetKeyValueWithDefault("MigrationViewModel.TrackMigrationStatus", MigrationStatusEnum.Invalid.ToString());
                return (MigrationStatusEnum)Enum.Parse(typeof(MigrationStatusEnum), result);
            }
            private set
            {
                Setting_SetKey("MigrationViewModel.TrackMigrationStatus", value.ToString());
                SetProperty(nameof(MigrationStatus));
                SetProperty(nameof(HasDatabase));
                SetProperty(nameof(IsDatabaseStarting));
                SetProperty(nameof(IsDatabaseWaiting));
                SetProperty(nameof(IsDatabaseReady));
                SetProperty(nameof(IsDatabaseDone));
            }
        }

        public bool IsDatabaseStarting => MigrationStatus == MigrationStatusEnum.Starting;
        public bool IsDatabaseWaiting => MigrationStatus == MigrationStatusEnum.Waiting;
        public bool IsDatabaseReady => MigrationStatus == MigrationStatusEnum.Ready;
        public bool IsDatabaseDone => MigrationStatus == MigrationStatusEnum.Done;

        /// <summary>
        /// Users' email address. Strongly encouraged!!
        /// </summary>
        public string UserEmail { get; set; }

        #endregion

        #region Commands
        #pragma warning disable 4014
        public ICommand UploadDatabaseCommand => new DelegateCommand(_ => UploadDatabase());
        public ICommand CheckDatabaseCommand => new DelegateCommand(_ => CheckDatabaseAvailable());
        public ICommand DownloadDatabaseCommand => new DelegateCommand(_ => DownloadDatabase());
        public ICommand DeleteDatabaseCommand => new DelegateCommand(_ => DeleteDatabase());
        #pragma warning restore
        #endregion

        #region Public Methods

        /// <summary>
        /// Load the current state of migration, scanning for available files, and checking the
        /// app storage settings
        /// </summary>
        public async Task Load()
        {
            try
            {
                var dir = await Filesystem_Directory(string.Empty);

                if (dir.Contains("Todo.sdf"))
                {
                    if (!HasDatabase)
                        MigrationStatus = MigrationStatusEnum.Starting;
                }
                else
                {
                    MigrationStatus = MigrationStatusEnum.NoMigrationNeeded;
                }
            }
            catch (Exception ex)
            {
                SetError("GM1", ex);
            }
        }

        /// <summary>
        /// Quick check to see if migration needs to be offered to the user
        /// </summary>
        /// <returns></returns>
        public static async Task<bool> NeedsMigrationPrompt()
        {
            var dir = await Filesystem_Directory(string.Empty);

            return dir.Contains(DatabaseFilename);
        }

        /// <summary>
        /// Upload the Database file to the service
        /// </summary>
        public async Task<string> UploadDatabase()
        {
            string result = string.Empty;
            IsWaiting = true;

            try
            {
                // http://stackoverflow.com/questions/1365407/c-sharp-code-to-validate-email-address
                const string pattern = @"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$";

                if (string.IsNullOrEmpty(UserEmail) || !Regex.IsMatch(UserEmail, pattern))
                {
                    //SetMessage(App.GetResourceString("Migration_NeedEmail/Text"));
                }
                else
                {
                    AzureStorage_Initialize();

                    var dt = DateTime.Now.ToString("yyyy-MM-dd-HH-mm");
                    var email = Uri.EscapeDataString(UserEmail);
                    MigrationFileKey = $"{email}-{dt}";
                    using (var stream = await Filesystem_OpenFileForRead(DatabaseFilename))
                    {
                        await AzureStorage_UploadToBlob(BlobContainer, $"{MigrationFileKey}.sdf", stream);
                    }

                    MigrationStatus = MigrationStatusEnum.Waiting;
                    result = MigrationFileKey;
                }
            }
            catch (Exception ex)
            {
                SetError("GM4", ex);
            }
            finally
            {
                IsWaiting = false;
            }

            return result;
        }

        /// <summary>
        /// Check the service to see whether Database are available
        /// </summary>
        public async Task<bool> CheckDatabaseAvailable()
        {
            bool exists = false;
            try
            {
                IsWaiting = true;
                AzureStorage_Initialize();
                exists = await AzureStorage_DoesBlobExist(BlobContainer, $"{MigrationFileKey}.xml");

                if (exists)
                    MigrationStatus = MigrationStatusEnum.Ready;
                else
                    SetMessage(("Migration_Message_Waiting/Text"));

            }
            catch (Exception ex)
            {
                SetError("GM5", ex);
            }
            IsWaiting = false;

            return exists;
        }

        /// <summary>
        /// Download Database from the service and import them into our file system
        /// </summary>
        public async Task DownloadDatabase()
        {
            try
            {
                IsWaiting = true;
                var migration_output = $"Temp/{MigrationFileKey}.xml";

                // Download the migrated file
                using (var outstream = await Filesystem_OpenFileForOverwrite(migration_output))
                {
                    await AzureStorage_DownloadBlob(BlobContainer, $"{MigrationFileKey}.xml", outstream);
                }

                // Import the migrated file
                using (var stream = await Filesystem_OpenFileForRead(migration_output))
                {
                    var xs = new XmlSerializer(typeof(TodoItemList));
                    var list = (TodoItemList)xs.Deserialize(stream);

                    using (var db = new TodoDbContext())
                    {
                        await db.Items.AddRangeAsync(list);
                        db.SaveChanges();
                    }
                }

                MigrationStatus = MigrationStatusEnum.Done;
            }
            catch (Exception ex)
            {
                SetError("GM6", ex);
            }
            IsWaiting = false;
        }

        /// <summary>
        /// Delete the Database file
        /// </summary>
        public void DeleteDatabase()
        {
            try
            {
                Filesystem_Delete(DatabaseFilename);
                MigrationStatus = MigrationStatusEnum.NoMigrationNeeded;
                SetMessage(("Migration_Message_DatabaseDeleted/Text"));
            }
            catch (Exception ex)
            {
                SetError("GM7", ex);
            }
        }

        #endregion

        #region Internal Properties

        private string MigrationFileKey
        {
            get
            {
                var result = Setting_GetKeyValueWithDefault("MigrationViewModel.MigrationFileKey", string.Empty);
                return result;
            }
            set
            {
                Setting_SetKey("MigrationViewModel.MigrationFileKey", value);
            }
        }
        private static readonly string BlobContainer = "todomigration";
        private static readonly string DatabaseFilename = "Todo.sdf";

        #endregion

        #region Platform Layer

        private static string Setting_GetKeyValueWithDefault(string k, string v)
        {
            throw new NotImplementedException();
        }

        private static void Setting_SetKey(string k, string v)
        {
            throw new NotImplementedException();
        }

        private static Task<IEnumerable<string>> Filesystem_Directory(string path)
        {
            throw new NotImplementedException();
        }

        private static void Filesystem_Delete(string path)
        {
            throw new NotImplementedException();
        }
        private static Task<Stream> Filesystem_OpenFileForOverwrite(string path)
        {
            throw new NotImplementedException();
        }

        private static Task<Stream> Filesystem_OpenFileForRead(string path)
        {
            throw new NotImplementedException();
        }

        private void AzureStorage_Initialize()
        {
            throw new NotImplementedException();
        }

        private Task<bool> AzureStorage_DoesBlobExist(string blobContainer, string v)
        {
            throw new NotImplementedException();
        }
        private Task AzureStorage_DownloadBlob(string blobContainer, string v, Stream outstream)
        {
            throw new NotImplementedException();
        }

        private Task AzureStorage_UploadToBlob(string blobContainer, string v, Stream stream)
        {
            throw new NotImplementedException();
        }

        private void SetError(string v, Exception ex)
        {
            throw new NotImplementedException();
        }

        private void SetMessage(string v)
        {
            throw new NotImplementedException();
        }
        private void SetProperty(string v)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}