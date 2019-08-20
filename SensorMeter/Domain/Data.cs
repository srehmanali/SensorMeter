using SensorMeter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SensorMeter.Domain
{
    public class Data
    {
        public IEnumerable<Navbar> navbarItems()
        {
            var menu = new List<Navbar>();
            menu.Add(new Navbar { Id = 1, nameOption = "Setup", controller = "Home", action = "Setup", imageClass = "fa fa-wrench fa-fw", status = true, isParent = false, parentId = 0 });
            menu.Add(new Navbar { Id = 2, nameOption = "Process", controller = "Home", action = "Process", imageClass = "fa fa-dashboard fa-fw", status = true, isParent = false, parentId = 0 });
            menu.Add(new Navbar { Id = 3, nameOption = "Report", controller = "Home", action = "Report", imageClass = "fa fa-bar-chart-o fa-fw", status = true, isParent = false, parentId = 0 });
            menu.Add(new Navbar { Id = 4, nameOption = "Settings", controller = "Home", action = "Settings", imageClass = "fa fa-cogs fa-fw", status = true, isParent = false, parentId = 0 });

            return menu.ToList();
        }
    }
}