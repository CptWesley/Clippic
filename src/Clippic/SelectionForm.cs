using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Clippic
{
    /// <summary>
    /// Form for making the selection.
    /// </summary>
    /// <seealso cref="Form" />
    public class SelectionForm : Form
    {
        private Point selectionStart;
        private bool selectionStarted;

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectionForm"/> class.
        /// </summary>
        public SelectionForm()
        {
            FormBorderStyle = FormBorderStyle.None;
            ShowInTaskbar = false;
            BackColor = Color.Red; // Does not work for all colours. Thank you Microsoft.
            TransparencyKey = BackColor;
            DoubleBuffered = true;

            MouseDown += StartSelection;
            MouseUp += StopSelection;
            Load += WindowLoaded;
            MouseMove += MouseMoved;
            Paint += Painting;
        }

        /// <summary>
        /// Event fired when the window is loaded.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void WindowLoaded(object sender, EventArgs e)
        {
            Size = SystemInformation.VirtualScreen.Size;
            Location = SystemInformation.VirtualScreen.Location;
        }

        /// <summary>
        /// Event fired when the mouse moves.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void MouseMoved(object sender, MouseEventArgs e)
        {
            Cursor.Current = Cursors.Cross;
            Invalidate(); // Force redraw of form.
        }

        /// <summary>
        /// Event fired when the selection starts.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void StartSelection(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                selectionStart = GetGlobalLocation(e.Location);
                selectionStarted = true;
                MouseMoved(sender, e);
            }
            else
            {
                Close();
            }
        }

        /// <summary>
        /// Event fired when the selection ends.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void StopSelection(object sender, MouseEventArgs e)
        {
            if (selectionStarted && e.Button == MouseButtons.Left)
            {
                Point selectionEnd = GetGlobalLocation(e.Location);
                int xMin = Math.Min(selectionStart.X, selectionEnd.X);
                int xMax = Math.Max(selectionStart.X, selectionEnd.X);
                int yMin = Math.Min(selectionStart.Y, selectionEnd.Y);
                int yMax = Math.Max(selectionStart.Y, selectionEnd.Y);

                if (xMax - xMin > 0 && yMax - yMin > 0)
                {
                    Bitmap bmp = new Bitmap(xMax - xMin, yMax - yMin);
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        g.CopyFromScreen(xMin, yMin, 0, 0, bmp.Size, CopyPixelOperation.SourceCopy);
                    }

                    Clipboard.SetImage(bmp);
                }
            }

            Close();
        }

        private void Painting(object sender, PaintEventArgs e)
        {
            if (selectionStarted)
            {
                Point relativeStart = PointToClient(selectionStart);
                Point currentLocation = PointToClient(Cursor.Position);
                int xMin = Math.Min(relativeStart.X, currentLocation.X);
                int xMax = Math.Max(relativeStart.X, currentLocation.X);
                int yMin = Math.Min(relativeStart.Y, currentLocation.Y);
                int yMax = Math.Max(relativeStart.Y, currentLocation.Y);
                int width = xMax - xMin;
                int height = yMax - yMin;
                e.Graphics.DrawRectangle(Pens.LightGray, xMin, yMin, width, height);
                e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
                e.Graphics.DrawString($"{width}, {height}", new Font("Arial", 7), Brushes.LightGray, currentLocation);
            }
        }

        /// <summary>
        /// Gets the global location of a point in the window.
        /// </summary>
        /// <param name="point">The point in the window.</param>
        /// <returns>The global location of the point in the window.</returns>
        private Point GetGlobalLocation(Point point)
            => new Point(point.X + Location.X, point.Y + Location.Y);
    }
}
