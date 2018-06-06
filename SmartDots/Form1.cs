using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Dots_OnForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            DoubleBuffered = true;

            pictureBox1 = new PictureBox
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };

            Controls.Add(pictureBox1);
        }

        private PictureBox pictureBox1 = new PictureBox();

        public void ResetCanvas()
        {
            Graphics g = pictureBox1.CreateGraphics();
            g.Clear(Color.White);
        }

        public void DrawTarget(Point point)
        {
            Graphics g = pictureBox1.CreateGraphics();
            g.FillEllipse(new SolidBrush(Color.Black), new Rectangle(point, new Size(10, 10)));
        }

        public void WriteToCanvas(string sentence, Point location)
        {
            Graphics g = pictureBox1.CreateGraphics();
            g.DrawString(sentence, new Font("Arial", 10), new SolidBrush(Color.Red), location);
        }

        internal void RenderPopulation(Population dotPopulation, int time)
        {
            System.Diagnostics.Debug.WriteLine("Image at time: " + time);
            Graphics g = pictureBox1.CreateGraphics();

            KeyValuePair<Brush, Point>[] snapshot = dotPopulation.GetSnapshotAt(time);
            foreach (KeyValuePair<Brush, Point> p in snapshot)
            {
                g.FillEllipse(p.Key, new Rectangle(p.Value, new Size(10, 10)));
            }
        }
    }
}
