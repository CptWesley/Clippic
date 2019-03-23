using System.Drawing;
using System.Windows.Forms;

namespace Clippic
{
    /// <summary>
    /// Form for making the selection.
    /// </summary>
    /// <seealso cref="Form" />
    public class SelectionForm : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectionForm"/> class.
        /// </summary>
        public SelectionForm()
        {
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            ShowInTaskbar = false;
            BackColor = Color.Red; // Does not work for all colours. Thank you Microsoft.
            TransparencyKey = BackColor;
        }
    }
}
