using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.File;
using Scf.Net.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scf.Net.BlobStorage
{
    public static class BlobFile
    {
        private static CloudStorageAccount storageAccount = null;
        private static CloudFileClient fileClient = null;
        private static CloudFileShare fileShare = null;

        private static string _connectionString = "";
        private static string _shareName = "";

        private static IEventFeedback _feedback = null;

        public static bool Initialize(string shareName, string connectionString, IEventFeedback feedback)
        {
            _connectionString = connectionString;
            _shareName = shareName;
            _feedback = feedback;

            try
            {
                storageAccount = CloudStorageAccount.Parse(_connectionString);
                fileClient = storageAccount.CreateCloudFileClient();
                fileShare = GetFileShare();
            }
            catch(Exception ex)
            {
                if (_feedback != null)
                    _feedback.OnException(fileClient, ex);

                return false;
            }

            return true;
        }

        public static void Terminate()
        {

        }

        private static bool CreateFileShare()
        {
            try
            {
                CloudFileShare fileShare = fileClient.GetShareReference(_shareName);

                fileShare.CreateIfNotExists();
            }
            catch(Exception ex)
            {
                if (_feedback != null)
                    _feedback.OnException(fileClient, ex);

                return false;
            }

            return true;
        }

        public static bool DeleteFileShare()
        {
            try
            {
                // Create the CloudTable that represents the "people" table.
                CloudFileShare fileShare = fileClient.GetShareReference(_shareName);

                // Delete the table it if exists.
                fileShare.DeleteIfExists();
            }
            catch(Exception ex)
            {
                if (_feedback != null)
                    _feedback.OnException(fileClient, ex);

                return false;
            }

            return true;
        }

        public static List<string> ListFiles()
        {
            List<string> files = new List<string>();

            CloudFileShare share = GetFileShare();
            if (share.Exists())
            {
                CloudFileDirectory directory = share.GetRootDirectoryReference();
                if(directory.Exists())
                {
                    IEnumerable<IListFileItem> list = directory.ListFilesAndDirectories();
                    foreach(IListFileItem item in list)
                    {
                        string filename = Path.GetFileName(item.Uri.ToString());
                        files.Add(filename);
                    }
                }
            }

            return files;
        }

        public static string GetFileAsString(string filename)
        {
            string fileContents = "";

            CloudFileShare share = GetFileShare();
            if(share.Exists())
            {                
                CloudFileDirectory directory = share.GetRootDirectoryReference();
                if(directory.Exists())
                {
                    CloudFile file = directory.GetFileReference(filename);
                    if(file.Exists())
                    {
                        fileContents = file.DownloadText();
                    }
                }
            }

            return fileContents;
        }

        private static CloudFileShare GetFileShare()
        {
            try
            {
                CreateFileShare();

                return fileClient.GetShareReference(_shareName);
            }
            catch(Exception ex)
            {
                if (_feedback != null)
                    _feedback.OnException(fileClient, ex);
            }

            return null;
        }

        private static CloudFileDirectory GetFileDirectory(string folderPath)
        {
            CloudFileDirectory directory = null;
            try
            {
                CloudFileShare fileShare = GetFileShare();

                CloudFileDirectory root = fileShare.GetRootDirectoryReference();

                directory = root.GetDirectoryReference(folderPath);
            }
            catch(Exception ex)
            {
                if (_feedback != null)
                    _feedback.OnException(fileClient, ex);
            }

            return directory;
        }

        public static bool UploadFile(string localFilePath)
        {
            try
            {
                CloudFileShare fileShare = GetFileShare();

                CloudFileDirectory root = fileShare.GetRootDirectoryReference();

                string filename = Path.GetFileName(localFilePath);

                CloudFile file = root.GetFileReference(filename);
                if (file != null)
                {
                    file.UploadFromFile(localFilePath);
                }
            }
            catch (Exception ex)
            {
                if (_feedback != null)
                    _feedback.OnException(fileClient, ex);

                return false;
            }

            return true;
        }

        public static bool UploadFile(string folderPath, string localFilePath)
        {
            try
            {                
                CloudFileDirectory directory = GetFileDirectory(folderPath);

                string filename = Path.GetFileName(localFilePath);

                CloudFile file = directory.GetFileReference(filename);
                if (file != null)
                {
                    file.UploadFromFile(localFilePath);
                }
            }
            catch (Exception ex)
            {
                if (_feedback != null)
                    _feedback.OnException(fileClient, ex);

                return false;
            }

            return true;
        }

        public static bool UploadFiles(string folderPath, List<string> localFilePaths)
        {
            bool result = true;

            try
            {
                foreach (string filepath in localFilePaths)
                {
                    result = (result && UploadFile(folderPath, filepath));
                }
            }
            catch (Exception ex)
            {
                if (_feedback != null)
                    _feedback.OnException(fileClient, ex);

                result = false;
            }

            return result;
        }

        public static bool DeleteFile(string folderPath, string filename)
        {
            try
            {
                CloudFileDirectory directory = GetFileDirectory(folderPath);

                CloudFile file = directory.GetFileReference(filename);

                file.DeleteIfExists();
            }
            catch (Exception ex)
            {
                if (_feedback != null)
                    _feedback.OnException(fileClient, ex);

                return false;
            }

            return true;
        }

        public static bool DeleteFolder(string folderPath)
        {
            try
            {
                CloudFileDirectory directory = GetFileDirectory(folderPath);

                directory.DeleteIfExists();
            }
            catch (Exception ex)
            {
                if (_feedback != null)
                    _feedback.OnException(fileClient, ex);
                return false;
            }

            return true;
        }

        public static string DownloadFile(string folderPath, string fileName)
        {
            try
            {
                string localFile = Path.GetTempFileName();

                CloudFileDirectory directory = GetFileDirectory(folderPath);

                CloudFile file = directory.GetFileReference(fileName);

                file.DownloadToFile(localFile, FileMode.Create);

                return localFile;
            }
            catch (Exception ex)
            {
                if (_feedback != null)
                    _feedback.OnException(fileClient, ex);
            }

            return "";
        }

        public static bool DownloadFolder(string folderPath, string localFolder)
        {
            try
            {
                CloudFileDirectory directory = GetFileDirectory(folderPath);

                List<IListFileItem> results = new List<IListFileItem>();

                results = directory.ListFilesAndDirectories().ToList();

                foreach (IListFileItem fileItem in results)
                {
                    DownloadFile(folderPath, Path.Combine(localFolder, fileItem.Uri.ToString()));
                }
            }
            catch (Exception ex)
            {
                if (_feedback != null)
                    _feedback.OnException(fileClient, ex);

                return false;
            }

            return true;
        }

        public static bool DownloadFileShare(string localFolder)
        {
            try
            {
                CloudFileShare share = GetFileShare();

                List<IListFileItem> results = new List<IListFileItem>();
                FileContinuationToken token = null;

                Task.Run(async () =>
                {
                    do
                    {
                        FileResultSegment resultSegment = await share.GetRootDirectoryReference().ListFilesAndDirectoriesSegmentedAsync(token);
                        results.AddRange(resultSegment.Results);
                        token = resultSegment.ContinuationToken;
                    }
                    while (token != null);
                });
            }
            catch (Exception ex)
            {
                if (_feedback != null)
                    _feedback.OnException(fileClient, ex);

                return false;
            }

            return true;
        }
    }
}
