using System;
using System.Drawing;
using System.Windows.Forms;

namespace SimpleClassicTheme.UI.Controls
{
    public class ColorButton : Button
    {
        private Brush brush;
        private Color color;

        public ColorButton()
        {
            Color = Color.White;
        }

        public event EventHandler<EventArgs> ColorChanged;

        public Color Color
        {
            get => color;
            set
            {
                color = value;
                brush = new SolidBrush(color);

                ColorChanged?.Invoke(this, EventArgs.Empty);

                Invalidate();
            }
        }

        protected override void OnClick(EventArgs e)
        {
            ColorDialog picker = new ColorDialog
            {
                Color = color,
                SolidColorOnly = true,
            };

            Form owner = FindForm();
            if (picker.ShowDialog(owner) == DialogResult.OK)
            {
                Color = picker.Color;
            }

            Focus();
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);

            Rectangle colorSurface = new Rectangle(4, 3, Width - 20, Height - 8);
            if (Enabled)
            {
                pevent.Graphics.FillRectangle(brush, colorSurface);
                pevent.Graphics.DrawRectangle(SystemPens.ControlText, colorSurface);
            }

            int dividerX = Width - 12;
            int dividerX2 = dividerX + 1;
            int dividerY = 3;
            int dividerY2 = dividerY + colorSurface.Height;

            pevent.Graphics.DrawLine(SystemPens.ControlDark, dividerX, dividerY, dividerX, dividerY2);
            pevent.Graphics.DrawLine(SystemPens.ControlLightLight, dividerX2, dividerY, dividerX2, dividerY2);

            float downOffsetY = (Height / 2f) - 1.5f;
            Point downOffset = new Point(Width - 9, (int)downOffsetY);

            Point[] arrowVerteces = new Point[]
            {
                downOffset,
                new Point(downOffset.X + 5, downOffset.Y),
                new Point(downOffset.X + 2, downOffset.Y + 3),
            };

            pevent.Graphics.FillPolygon(Enabled ? SystemBrushes.ControlText : SystemBrushes.GrayText, arrowVerteces);
        }
    }
}