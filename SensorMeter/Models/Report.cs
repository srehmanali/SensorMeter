using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace SensorMeter.Models
{
    public class Report
    {
        public DataSet GetData() {

            var ds = new DataSet("ReportData");
            var dt = new DataTable("ChartData");
            dt.Columns.Add("Col1");
            dt.Columns.Add("Col2");
            ds.Tables.Add(dt);
            return ds;

        }
    }
}
