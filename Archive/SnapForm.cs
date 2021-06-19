using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HDshared {
    public class SnapForm : Form {
        private static Screen ScreenLeft = Screen.PrimaryScreen;
        private static Screen ScreenRight = Screen.PrimaryScreen;

        public static void SetupArea() {
            ScreenLeft = Screen.PrimaryScreen;
            ScreenRight = Screen.PrimaryScreen;
            foreach (Screen screen in Screen.AllScreens) {
                if (screen.WorkingArea.X < ScreenLeft.WorkingArea.X)
                    ScreenLeft = screen;

                if (screen.WorkingArea.X > ScreenRight.WorkingArea.X)
                    ScreenRight = screen;
            }
        }

        public void MoveAbove(Form parentForm) {
            this.Left = parentForm.Left;
            this.Top = parentForm.Top;
            Screen thisScreen = Screen.FromControl(parentForm);

            if (this.Left > thisScreen.WorkingArea.X + (thisScreen.WorkingArea.Width / 2))
                this.Left = parentForm.Left - this.Width + parentForm.Width;
            else
                this.Left = parentForm.Left;

            this.Top = parentForm.Top - this.Height;

            //Clamp to screen edge
            if (this.Left < thisScreen.WorkingArea.X)
                this.Left = thisScreen.WorkingArea.X;

            if (this.Right > thisScreen.WorkingArea.Right)
                this.Left = thisScreen.WorkingArea.Right - this.Width;
        }

        public void MoveBelow(Form parentForm) {
            this.Left = parentForm.Left;
            Screen thisScreen = Screen.FromControl(parentForm);

            if (this.Left > thisScreen.WorkingArea.X + (thisScreen.WorkingArea.Width / 2))
                this.Left = parentForm.Left - this.Width + parentForm.Width;
            else
                this.Left = parentForm.Left;

            this.Top = parentForm.Bottom;

            //Clamp to screen edge
            if (this.Left < thisScreen.WorkingArea.X)
                this.Left = thisScreen.WorkingArea.X;

            if (this.Right > thisScreen.WorkingArea.Right)
                this.Left = thisScreen.WorkingArea.Right - this.Width;
        }

        public void MoveToScreenLeftBottomLeft() {
            this.Left = ScreenLeft.WorkingArea.Left - 7;
            this.Top = ScreenLeft.WorkingArea.Top + ScreenLeft.WorkingArea.Height - this.Size.Height + 7;
        }

        public void MoveToScreenRightBottomRight() {
            this.Left = ScreenRight.WorkingArea.Left + ScreenRight.WorkingArea.Width - this.Size.Width + 7;
            this.Top = ScreenRight.WorkingArea.Top + ScreenRight.WorkingArea.Height - this.Size.Height + 7;
        }

        public void MoveToScreenLeftTopRight() {
            this.Left = ScreenLeft.WorkingArea.Left + ScreenLeft.WorkingArea.Width - this.Size.Width + 7;
            this.Top = ScreenLeft.WorkingArea.Top;
        }

        public void MoveToScreenRightTopLeft() {
            this.Left = ScreenRight.WorkingArea.Left - 7;
            this.Top = ScreenRight.WorkingArea.Top;
        }

        public void MoveToScreenLeftBottomRight() {
            this.Left = ScreenLeft.WorkingArea.Left + ScreenLeft.WorkingArea.Width - this.Size.Width + 7;
            this.Top = ScreenLeft.WorkingArea.Top + ScreenLeft.WorkingArea.Height - this.Size.Height + 7;
        }

        public void MoveToScreenRightBottomLeft() {
            this.Left = ScreenRight.WorkingArea.Left - 7;
            this.Top = ScreenRight.WorkingArea.Top + ScreenRight.WorkingArea.Height - this.Size.Height + 7;
        }

        public void MoveToScreenPrimaryTopLeft() {
            this.Left = Screen.PrimaryScreen.WorkingArea.Left - 7;
            this.Top = Screen.PrimaryScreen.WorkingArea.Top;
        }

        public void MoveToScreenPrimaryBottomLeft() {
            this.Left = Screen.PrimaryScreen.WorkingArea.Left - 7;
            this.Top = Screen.PrimaryScreen.WorkingArea.Top + Screen.PrimaryScreen.WorkingArea.Height - this.Size.Height + 7;
        }
    }
}
