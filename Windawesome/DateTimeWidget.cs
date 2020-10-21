using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
namespace Windawesome
{
    public sealed class DateTimeWidget : IFixedWidthWidget
    {
        public Bar bar { get; private set; }
        public Label label { get; private set; }
        public bool isLeft { get; private set; }
        public EventHandler click { get; private set; }

        private readonly Color foreground_color;
        private readonly string str;
        private readonly Timer update_time;
        private readonly string prefix;
        private readonly string suffix;
        private readonly Color background_color;

        public DateTimeWidget(string str, string prefix = " ", string suffix = " ", Color? back_color = null, Color? fore_color = null, int update_time = 30000, EventHandler click = null)
        {
            this.prefix = prefix;
            this.suffix = suffix;
            this.background_color = back_color ?? Color.FromArgb(0xC0, 0xC0, 0xC0);
            this.foreground_color = fore_color ?? Color.Black;
            this.str = str;
            this.click = click;

            this.update_time = new Timer()
            {
                Interval = update_time,
            };
            this.update_time.Tick += (sender, ev) =>
            {
                var old_left = this.label.Left;
                var old_right = this.label.Right;
                var old_width = this.label.Width;
                this.label.Text = (this.prefix + DateTime.Now.ToString(str) + this.suffix);
                this.label.Width = TextRenderer.MeasureText(label.Text, label.Font).Width;
                if (old_width != this.label.Width)
                {
                    this.RepositionControls(old_left, old_right);
                    this.bar.DoFixedWidthWidgetWidthChanged(this);
                }
            };
        }


        public void Dispose()
        {
        }

        public IEnumerable<Control> GetInitialControls(bool isLeft)
        {
            this.isLeft = isLeft;
            return Enumerable.Repeat(this.label, 1);
        }

        public int GetLeft()
        {
            return this.label.Left;
        }

        public int GetRight()
        {
            return this.label.Right;
        }

        public void InitializeWidget(Bar bar)
        {
            this.bar = bar;
            this.label = bar.CreateLabel(this.prefix + DateTime.Now.ToString(str) + this.suffix, 0);
            this.label.TextAlign = ContentAlignment.MiddleCenter;
            this.label.BackColor = this.background_color;
            this.label.ForeColor = this.foreground_color;
            if (this.click != null)
            {
                this.label.Click += this.click;
            }
        }

        public void Refresh()
        {
        }

        public void RepositionControls(int left, int right)
        {
            this.label.Location = this.isLeft ? new Point(left, 0) : new Point(right - this.label.Width, 0);
        }

        public void StaticDispose()
        {
        }

        public void StaticInitializeWidget(Windawesome windawesome)
        {
        }
    }
}
