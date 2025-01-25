namespace paintApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            using System;
            using System.Drawing;
            using System.Windows.Forms;

namespace SimpleDrawingApp
    {
        public class DrawingApp : Form
        {
            private bool isDrawing = false;
            private Point startPoint;
            private Bitmap canvasBitmap;
            private Graphics canvasGraphics;
            private Pen drawingPen = new Pen(Color.Black, 2);
            private ToolStripComboBox colorPicker;
            private ToolStripComboBox shapePicker;
            private string selectedShape = "Freehand";

            public DrawingApp()
            {
                // Initialize the form
                this.Text = "Simple Drawing Application";
                this.Size = new Size(800, 600);
                this.StartPosition = FormStartPosition.CenterScreen;

                // Create a menu strip
                MenuStrip menuStrip = new MenuStrip();

                // File menu
                ToolStripMenuItem fileMenu = new ToolStripMenuItem("File");
                ToolStripMenuItem saveMenuItem = new ToolStripMenuItem("Save", null, SaveFile);
                ToolStripMenuItem openMenuItem = new ToolStripMenuItem("Open", null, OpenFile);
                fileMenu.DropDownItems.Add(saveMenuItem);
                fileMenu.DropDownItems.Add(openMenuItem);

                menuStrip.Items.Add(fileMenu);

                // Tools menu
                ToolStripMenuItem toolsMenu = new ToolStripMenuItem("Tools");

                // Color picker
                colorPicker = new ToolStripComboBox();
                colorPicker.Items.AddRange(new string[] { "Black", "Red", "Green", "Blue", "Yellow" });
                colorPicker.SelectedIndex = 0;
                colorPicker.SelectedIndexChanged += (s, e) => ChangePenColor();

                // Shape picker
                shapePicker = new ToolStripComboBox();
                shapePicker.Items.AddRange(new string[] { "Freehand", "Line", "Rectangle", "Ellipse" });
                shapePicker.SelectedIndex = 0;
                shapePicker.SelectedIndexChanged += (s, e) => selectedShape = shapePicker.SelectedItem.ToString();

                toolsMenu.DropDownItems.Add(new ToolStripLabel("Color:"));
                toolsMenu.DropDownItems.Add(colorPicker);
                toolsMenu.DropDownItems.Add(new ToolStripSeparator());
                toolsMenu.DropDownItems.Add(new ToolStripLabel("Shape:"));
                toolsMenu.DropDownItems.Add(shapePicker);

                menuStrip.Items.Add(toolsMenu);

                // Add the menu strip to the form
                this.Controls.Add(menuStrip);
                this.MainMenuStrip = menuStrip;

                // Initialize the canvas
                canvasBitmap = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
                canvasGraphics = Graphics.FromImage(canvasBitmap);
                canvasGraphics.Clear(Color.White);

                // Handle drawing events
                this.MouseDown += StartDrawing;
                this.MouseMove += Draw;
                this.MouseUp += StopDrawing;
                this.Paint += RefreshCanvas;
            }

            private void StartDrawing(object sender, MouseEventArgs e)
            {
                if (e.Button == MouseButtons.Left)
                {
                    isDrawing = true;
                    startPoint = e.Location;
                }
            }

            private void Draw(object sender, MouseEventArgs e)
            {
                if (isDrawing && selectedShape == "Freehand")
                {
                    canvasGraphics.DrawLine(drawingPen, startPoint, e.Location);
                    startPoint = e.Location;
                    this.Invalidate();
                }
            }

            private void StopDrawing(object sender, MouseEventArgs e)
            {
                if (isDrawing)
                {
                    if (selectedShape == "Line")
                    {
                        canvasGraphics.DrawLine(drawingPen, startPoint, e.Location);
                    }
                    else if (selectedShape == "Rectangle")
                    {
                        canvasGraphics.DrawRectangle(drawingPen, GetRectangle(startPoint, e.Location));
                    }
                    else if (selectedShape == "Ellipse")
                    {
                        canvasGraphics.DrawEllipse(drawingPen, GetRectangle(startPoint, e.Location));
                    }

                    isDrawing = false;
                    this.Invalidate();
                }
            }

            private void RefreshCanvas(object sender, PaintEventArgs e)
            {
                e.Graphics.DrawImage(canvasBitmap, Point.Empty);
            }

            private void ChangePenColor()
            {
                string selectedColor = colorPicker.SelectedItem.ToString();
                drawingPen.Color = Color.FromName(selectedColor);
            }

            private void SaveFile(object sender, EventArgs e)
            {
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "PNG Files|*.png";
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        canvasBitmap.Save(saveFileDialog.FileName);
                    }
                }
            }

            private void OpenFile(object sender, EventArgs e)
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "PNG Files|*.png";
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        canvasBitmap = new Bitmap(openFileDialog.FileName);
                        canvasGraphics = Graphics.FromImage(canvasBitmap);
                        this.Invalidate();
                    }
                }
            }

            private Rectangle GetRectangle(Point p1, Point p2)
            {
                return new Rectangle(
                    Math.Min(p1.X, p2.X),
                    Math.Min(p1.Y, p2.Y),
                    Math.Abs(p1.X - p2.X),
                    Math.Abs(p1.Y - p2.Y)
                );
            }

            [STAThread]
            public static void Main()
            {
                Application.EnableVisualStyles();
                Application.Run(new DrawingApp());
            }
        }
    }

}
    }
}
