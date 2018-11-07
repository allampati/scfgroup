using Scf.Net.Azure.ResourceManager;
using Scf.Net.Base;
using Scf.Net.BlobStorage;
using Scf.Net.ServiceBusQueue;
using SmartMonitorMessages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmartMonitorSimulator
{
    public partial class FormMain : Form, IEventFeedback, IEventReceiver
    {
        private Timer msgTimer = new Timer();
        private bool processingMsgs = false;

        public FormMain()
        {
            InitializeComponent();

            comboBoxGpsType.SelectedIndex = 1;

            SmartMonitorSystem.Instance.Initialize();

            listViewReceivedMsgs.Columns[listViewReceivedMsgs.Columns.Count - 1].Width = -2;

            TestBlobStorage();

            msgTimer.Tick += new EventHandler(OnMessageTimer);
            msgTimer.Interval = 1000;
            msgTimer.Start();
        }

        private void checkBoxGpsStartStop_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxGpsStartStop.Checked)
            {
                checkBoxGpsStartStop.Text = "Stop";

                GpsSimulationInfo info = new GpsSimulationInfo();
                info.Interval = int.Parse(textBoxGpsInterval.Text)*1000; // seconds
                info.Type = comboBoxGpsType.SelectedIndex;

                SmartMonitorSystem.Instance.StartGpsSimulation(info);
            }
            else
            {
                SmartMonitorSystem.Instance.StopGpsSimulation();
                checkBoxGpsStartStop.Text = "Start";
            }
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            msgTimer.Stop();

            SmartMonitorSystem.Instance.Terminate();
        }

        private void OnMessageTimer(object sender, EventArgs args)
        {
            if (processingMsgs)
                return;

            List<BaseMessage> list = SmartMonitorSystem.Instance.GetReceivedMessages();
            if (list == null)
                return;

            processingMsgs = true;

            foreach (BaseMessage msg in list)
            {
                string[] columns = new string[2];

                columns[0] = msg.GetType().Name;

                switch (columns[0])
                {
                    case "TextMessage":
                        columns[1] = ((TextMessage)msg).Text;
                        break;
                }

                ListViewItem item = new ListViewItem(columns);
                listViewReceivedMsgs.Items.Add(item);
            }

            processingMsgs = false;
        }

        private void TestBlobStorage()
        {
            string clientId = "a4d70ac6-0f59-46f3-8930-664d902b0630";
            string clientSecret = "Z+dKKzbIe1+5AHPSTabb2wLS5zoxcKr3DFrigyuFapk=";
            string tenantId = "8b7a6c35-5f18-4862-8500-570e70cad771";
            string subscriptionId = "d34f0ec2-634d-4992-aac3-849adb7a4fb3";
            string resourceGroup = "SmartMonitor";
            string storageName = "smartmonitorblobstorage";

            //BlobTable<Account>.Initialize("DefaultEndpointsProtocol=https;AccountName=smartmonitorblobstorage;AccountKey=xr1shReh6DKdkOsQ6m0TZRH9r2LfnssFm8EyocYF5fp0Pte5dsSBrRlRWIh9bKljL10JIbAZjI4nu4Tk2eueQg==;EndpointSuffix=core.windows.net");

            //BlobManager.Initialize("DefaultEndpointsProtocol=https;AccountName=smartmonitorblobstorage;AccountKey=xr1shReh6DKdkOsQ6m0TZRH9r2LfnssFm8EyocYF5fp0Pte5dsSBrRlRWIh9bKljL10JIbAZjI4nu4Tk2eueQg==;EndpointSuffix=core.windows.net");

            //string containerName = "images";
            //BlobManager.CreateContainer(containerName);
            //BlobManager.UploadImage(containerName, @"C:\Users\Madhavi\Desktop\yandere_shota_by_choukogirl-dathtn8.jpg");
            //BlobManager.ListImages(containerName);

            //BlobManager.Terminate();

            //Receiver.Initialize("Endpoint=sb://gpsservicebus.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=+fWik6NOT3P6zUDDPArBnTWioERtjZInXrZrkPVXRio=","gpseventqueue", this);

            //Receiver.Terminate();
            //Account entity = new Account();
            //entity.CompanyName = "Test Organization";
            //entity.Address = "Address";
            //entity.City = "Houston";
            //entity.State = "TX";
            //entity.ZipCode = "77477";
            //entity.Phone = "111-222-1234";
            //entity.Email = "someone@gmail.com";
            //BlobTable<Account>.Insert(entity);

            //List<Account> list = BlobTable<Account>.GetAllItems();

            //BlobTable<Account>.Terminate();

            //object azure = AzureAccount.Authenticate(clientId, clientSecret, tenantId, subscriptionId);

            //List<string> regions = AzureAccount.Regions();

            //ResourceGroupManager.Instance.Initialize(azure, this);

            //ResourceGroupManager.Instance.Create("TestGroup", regions[2]);

            //ResourceGroupManager.Instance.Terminate();

            //ResourceDeploymentManager.Instance.Initialize(azure, this);

            ////ResourceDeploymentManager.Instance.DeployNotificationHub("SmartMonitorPush", "SmartMonitorNotifyHubs", "Free", "Free", regions[2], resourceGroup);

            //ResourceDeploymentManager.Instance.GetAllDeployments();

            //ResourceDeploymentManager.Instance.Terminate();

            //BlobStorageManager.Instance.Initialize(azure, this);

            //BlobStorageManager.Instance.Create(storageName, regions[6], resourceGroup);

            //BlobStorageManager.Instance.Terminate();

            //ResourceGroupManager.Instance.CreateResourceGroup(resourceGroup, regions[6]);

            //ResourceInfo info = ResourceGroupManager.Instance.GetResourceGroupInfo(resourceGroup);

            //string connectionString = "DefaultEndpointsProtocol=https;AccountName=smartmonitor;AccountKey=MKpOVzi0Q2e+M4qpgMj3sz9uRVSeeu6ZDKxuV0+YWwWJu4mvbzdO2E4FmdiOur0DmHk+tTU6L8SPBSVvdyN7NQ==;EndpointSuffix=core.windows.net";
            //string accessKey = "MKpOVzi0Q2e+M4qpgMj3sz9uRVSeeu6ZDKxuV0+YWwWJu4mvbzdO2E4FmdiOur0DmHk+tTU6L8SPBSVvdyN7NQ==";

            //FileShareManager.Instance.Initialize(connectionString, accessKey);

            //string filename = @"C:\Users\Madhavi\Desktop\Test.docx";

            //FileShareManager.Instance.UploadFile("Documents", "Word", filename);

            //////List<BaseTableItem> list = TableManager.Instance.GetAllItems("Account", new Account());            
            ////BlobManager.Instance.UploadImage("photos", "", @"C:\Users\Madhavi\Desktop\yandere_shota_by_choukogirl-dathtn8.jpg");

            ////BlobManager.Instance.DownloadImage("photos","", @"yandere_shota_by_choukogirl-dathtn8.jpg");

            //////List<BlobItem> list = BlobManager.Instance.GetAllItems("photos", new BlobItem());

            //////Account entity = new Account();
            //////entity.CompanyName = "Test Organization";
            //////entity.Address = "Address";
            //////entity.City = "Houston";
            //////entity.State  = "TX";
            //////entity.ZipCode = "77477";
            //////entity.Phone = "111-222-1234";
            //////entity.Email = "someone@gmail.com";
            //////TableManager.Instance.CreateTable("Account");
            //////TableManager.Instance.InsertItem("Account", entity);

            //FileShareManager.Instance.Terminate();
        }


        public bool OnMessage(object sender, ReceivedMessageEventArgs args)
        {
            return true;
        }

        public void OnComplete(object sender, TaskEventArgs args)
        {

        }

        public void OnError(object sender, InternalErrorEventArgs args)
        {

        }

        public void OnException(object sender, Exception exception)
        {

        }

        public void OnStatus(object sender, SendStatusEventArgs args)
        {
        }
    }
}
