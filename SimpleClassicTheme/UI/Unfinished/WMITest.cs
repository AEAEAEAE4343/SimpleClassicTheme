using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleClassicTheme.Forms
{
	public partial class WMITest : Form
	{
		public WMITest()
		{
			InitializeComponent();
		}

		private void WMITest_Load(object sender, EventArgs e)
		{
            ManagementObjectSearcher os_searcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem");
            foreach (ManagementObject mobj in os_searcher.Get())
            {
                foreach (PropertyData j in mobj.Properties)
                {
                    try
                    {
                        object val = j.Value;
                        if (val != null)
                            listView1.Items.Add(j.Name).SubItems.Add(val.ToString());
                        else 
                            listView1.Items.Add(j.Name).SubItems.Add("System.Management.PropertyData.Value.**get** returned null.");
                    }
                    catch (Exception ex)
                    {
                        listView1.Items.Add(j.Name).SubItems.Add(ex.Message);
                    }
                }
            }

            foreach (ColumnHeader col in listView1.Columns)
            {
                col.Width = -2;
            }
        }
    }
}
