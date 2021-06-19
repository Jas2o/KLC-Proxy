using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KLCProxy {
    public class MenuButton : Button {
        [DefaultValue(null)]
        public ContextMenuStrip Menu { get; set; }

        [DefaultValue(false)]
        public bool ShowMenuUnderCursor { get; set; }

        protected override void OnMouseDown(MouseEventArgs mevent) {
            base.OnMouseDown(mevent);

            if (Menu != null && mevent.Button == MouseButtons.Left) {
                Point menuLocation = new Point(0, 0);
                Rectangle testArea = new Rectangle(this.Parent.Parent.Parent.Bounds.X, this.Parent.Parent.Parent.Bounds.Y, 10, 10);

                foreach (Screen screen in Screen.AllScreens) {
                    if (screen.Bounds.IntersectsWith(testArea)) {
                        int checkX = screen.Bounds.X + (screen.Bounds.Width / 2);
                        int checkY = screen.Bounds.Y + (screen.Bounds.Height / 2);

                        if (testArea.Y > checkY) {
                            if (testArea.X > checkX) {
                                Menu.DefaultDropDownDirection = ToolStripDropDownDirection.AboveLeft;
                                menuLocation = new Point(0, Height);
                            } else {
                                Menu.DefaultDropDownDirection = ToolStripDropDownDirection.AboveRight;
                                menuLocation = new Point(Width, Height);
                            }
                        } else {
                            if (testArea.X > checkX) {
                                Menu.DefaultDropDownDirection = ToolStripDropDownDirection.BelowLeft;
                                menuLocation = new Point(0, 0);
                            } else {
                                Menu.DefaultDropDownDirection = ToolStripDropDownDirection.BelowRight;
                                menuLocation = new Point(Width, 0);
                            }
                        }

                        break;
                    }
                }
                //--

                if (ShowMenuUnderCursor) {
                    //menuLocation = mevent.Location;
                }/* else {
                    menuLocation = new Point(Width, 0);
                    //menuLocation = new Point(0, Height);
                }*/

                Menu.Show(this, menuLocation, Menu.DefaultDropDownDirection);
            }
        }

        protected override void OnPaint(PaintEventArgs pevent) {
            base.OnPaint(pevent);

            if (Menu != null) {
                int arrowX = ClientRectangle.Width - 14;
                int arrowY = ClientRectangle.Height / 2 - 1;

                Brush brush = Enabled ? SystemBrushes.ControlText : SystemBrushes.ControlDark;
                Point[] arrows = new Point[] { new Point(arrowX, arrowY), new Point(arrowX + 7, arrowY), new Point(arrowX + 3, arrowY + 4) };
                pevent.Graphics.FillPolygon(brush, arrows);
            }
        }
    }
}
