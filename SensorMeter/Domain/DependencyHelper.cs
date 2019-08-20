using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SensorMeter.Domain
{
    public class DependencyHelper
    {  private int changeCount = 0;

        private const string tableName = "Inventory";
        private const string statusMessage = "{0} changes have occurred.";

        // The following objects are reused 
        // for the lifetime of the application. 
        private DataSet dataToWatch = null;
        private SqlConnection connection = null;
        private SqlCommand command = null;

        //private bool CanRequestNotifications()
        //{
        //    // In order to use the callback feature of the
        //    // SqlDependency, the application must have
        //    // the SqlClientPermission permission.
        //    try
        //    {
        //        var perm = new SqlClientPermission(PermissionState.Unrestricted);
        //        perm.Demand();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}
        private string GetConnectionString()
        {
            // To avoid storing the connection string in your code, 
            // you can retrive it from a configuration file. 
            return "Data Source=.;Integrated Security=true;" +
              "Initial Catalog=SMChart;Pooling=False;";
        }

        private string GetSQL()
        {
            return "SELECT [MessageID], [JobDetailID], [Pressure], [Time] FROM [dbo].[Messages] WHERE [JobDetailID] = @Key ";
        }
        private SqlCommand GetSQLWithTop()
        {
            var command = new SqlCommand("SELECT TOP(20) MessageID, JobDetailID, Pressure, Time FROM [Messages] WHERE JobDetailID = @Key Order by Time DESC;", connection);

            SqlParameter prm =
                new SqlParameter("@Key", SqlDbType.NVarChar);
            prm.Direction = ParameterDirection.Input;
            prm.DbType = DbType.String;
            prm.Value = "A1";
            command.Parameters.Add(prm);
            return command;
        }

        private void dependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            // This event will occur on a thread pool thread. 
            // Updating the UI from a worker thread is not permitted. 
            // The following code checks to see if it is safe to 
            // update the UI. 
            //ISynchronizeInvoke i = (ISynchronizeInvoke)this;

            // If InvokeRequired returns True, the code 
            // is executing on a worker thread. 
            //if (i.InvokeRequired)
            //{
            //    // Create a delegate to perform the thread switch. 
            //    OnChangeEventHandler tempDelegate =
            //        new OnChangeEventHandler(dependency_OnChange);

            //    object[] args = { sender, e };

            //    // Marshal the data from the worker thread 
            //    // to the UI thread. 
            //    i.BeginInvoke(tempDelegate, args);

            //    return;
            //}

            // Remove the handler, since it is only good 
            // for a single notification. 
            SqlDependency dependency =
                (SqlDependency)sender;

            dependency.OnChange -= dependency_OnChange;

            // At this point, the code is executing on the 
            // UI thread, so it is safe to update the UI. 
            ++changeCount;
            //label1.Text = String.Format(statusMessage, changeCount);

            // Add information from the event arguments to the list box 
            // for debugging purposes only. 
            //listBox1.Items.Clear();
            //listBox1.Items.Add("Info:   " + e.Info.ToString());
            //listBox1.Items.Add("Source: " + e.Source.ToString());
            //listBox1.Items.Add("Type:   " + e.Type.ToString());

            // Reload the dataset that is bound to the grid. 
            GetData();
            GetLimitedData();
        }
        private void GetLimitedData() {

            using (SqlDataAdapter adapter =
                new SqlDataAdapter(GetSQLWithTop()))
            {
                DataSet ds = new DataSet();
                adapter.Fill(ds, tableName);
                var hubContext = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
                hubContext.Clients.All.UpdateChart(1, "Testing");
                //chart1.DataSource = ds.Tables[0];
                //chart1.DataBind();
                //chart2.DataSource = ds.Tables[0];
                //chart2.DataBind();
            }
        }
        private void GetData()
        {
            // Empty the dataset so that there is only 
            // one batch of data displayed. 
            dataToWatch.Clear();

            // Make sure the command object does not already have 
            // a notification object associated with it. 
            command.Notification = null;

            // Create and bind the SqlDependency object 
            // to the command object. 
            SqlDependency dependency =
                new SqlDependency(command);
            dependency.OnChange += new
                OnChangeEventHandler(dependency_OnChange);
            using (var reader = command.ExecuteReader()) { 
                
            }
           
            //chart1.Series[0].XValueMember = "Time";
            //chart1.Series[0].YValueMembers = "Pressure";
            //chart1.ChartAreas[0].AxisX.LabelStyle.Format = "HH:mm:ss";
            //chart2.Series[0].XValueMember = "Time";
            //chart2.Series[0].YValueMembers = "Pressure";
            //chart2.ChartAreas[0].AxisX.LabelStyle.Format = "HH:mm:ss";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            changeCount = 0;
            //label1.Text = String.Format(statusMessage, changeCount);
            if (connection == null)
            {
                connection = new SqlConnection(GetConnectionString());
                connection.Open();
            }
            // Remove any existing dependency connection, then create a new one. 
            SqlDependency.Stop(GetConnectionString());
            SqlDependency.Start(GetConnectionString());

            

            if (command == null)
            {
                // GetSQL is a local procedure that returns 
                // a paramaterized SQL string. You might want 
                // to use a stored procedure in your application. 
                command = new SqlCommand(GetSQL(), connection);

                SqlParameter prm =
                    new SqlParameter("@Key", SqlDbType.NVarChar);
                prm.Direction = ParameterDirection.Input;
                prm.DbType = DbType.String;
                prm.Value = "A1";
                command.Parameters.Add(prm);
            }
            if (dataToWatch == null)
            {
                dataToWatch = new DataSet();
            }
            GetLimitedData();
            GetData();
        }
        //private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        //{
        //    SqlDependency.Stop(GetConnectionString());
        //    if (connection != null)
        //    {
        //        connection.Close();
        //    }
        //}
        public DependencyHelper()
        {

            button1_Click(this, new EventArgs());

        }    
    }
}