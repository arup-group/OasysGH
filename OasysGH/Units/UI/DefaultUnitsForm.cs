using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UnitsNet;
using UnitsNet.Units;
using OasysGH.Units.Helpers;
using OasysGH.Units;

namespace OasysGH.Units.UI
{
  public partial class DefaultUnitsForm : Form
  {
    public DefaultUnitsForm()
    {
      InitializeComponent();

      List<string> lengthGeometryAbbreviations = UnitsHelper.GetFilteredAbbreviations(EngineeringUnits.Length);
      
      this.geometrySectionComboBox.DataSource = lengthGeometryAbbreviations;
      this.geometrySectionComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
      this.geometrySectionComboBox.SelectedIndex = lengthGeometryAbbreviations.IndexOf(Length.GetAbbreviation(DefaultUnits.LengthUnitSection));

    }

    private void button1_Click(object sender, EventArgs e)
    {

    }

    private void button3_Click(object sender, EventArgs e)
    {

    }

    private void label11_Click(object sender, EventArgs e)
    {

    }
  }
}
