using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Scf.Net.MongoDb;

namespace SmartMonitorSimulator
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //TEST    
            Scf.Net.MongoDb.DbConnectionInfo conn = new Scf.Net.MongoDb.DbConnectionInfo("mongodb://localhost:27017", "", "SmartMonitor");
            MongoDocumentCollection<MongoTestModel> coll = new MongoDocumentCollection<MongoTestModel>();            
            coll.Initialize(conn);

            //MongoTestModel item = new MongoTestModel();
            //item.FirstName = "Balaji";
            //item.LastName = "Allampati";
            //Task.Run(()=> coll.CreateItemAsync(item)).Wait();

            Task<List<MongoTestModel>> task = Task.Run(() => coll.GetAllItemsAsync());
            task.Wait();
            List<MongoTestModel> items = task.Result;

            //Task.Run(() => coll.DeleteItemAsync(items[2].StringId)).Wait();
            items[1].FirstName = "Madhavi";
            items[1].LastName = "Soppadandi";
            Task.Run(() => coll.UpdateItemAsync(items[1])).Wait();

            task = Task.Run(() => coll.GetAllItemsAsync());
            task.Wait();
            items = task.Result;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());
           
        }

    }
}
