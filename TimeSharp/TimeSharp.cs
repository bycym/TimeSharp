using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TimeSharp
{
    public partial class TimeSharp : Form
    {
        enum FormType
        {
            Timer,
            Stopper
        };

        private string appName = "TimeSharp";
        private FormType Setting;
        private int TimerTime;
        private System.Windows.Forms.Timer ATimer;
        private int Ticker;
        private DateTime Dt;

        public TimeSharp()
        {
            Timer();
            InitializeComponent();
            Setting = FormType.Timer;
            LoadInterface();

        }

        private void LoadInterface()
        {
            switch (Setting)
            {
                case FormType.Timer:
                    this.TimerTextBox.Visible = true;
                    this.ResetButton.Text = "Set";
                    this.Text = appName + " - Timer";
                    break;
                case FormType.Stopper:
                    this.TimerTextBox.Visible = false;
                    this.ResetButton.Text = "Reset";
                    this.Text = appName + " - Stopper";
                    break;
            }
        }

        public void Timer()
        {
            Dt = new DateTime();
            ATimer = new System.Windows.Forms.Timer();
            ATimer.Tick += new EventHandler(OnTimedEvent);
            ATimer.Interval = 1 * 1000;
        }

        private void OnTimedEvent(object sender, EventArgs e)
        {
            switch (Setting)
            {
                case FormType.Timer:
                    {
                        if (Ticker <= 0)
                        {
                            ATimer.Enabled = false;
                            ShowNotification();
                            Ticker = TimerTime;
                            ShowTime();
                        }
                        else
                        {
                            --Ticker;
                            ShowTime();
                        }
                    }
                    break;
                case FormType.Stopper:
                    {
                        ++Ticker;
                        ShowTime();
                    }
                    break;
            }
        }

        public void ShowTime()
        {
            this.TimeLabel.Text = Dt.AddSeconds(Ticker).ToString("HH:mm:ss");
        }

        private void StopperToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Setting = FormType.Stopper;
            LoadInterface();
        }

        private void TimerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Setting = FormType.Timer;
            LoadInterface();
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            switch (Setting)
            {
                case FormType.Timer:
                    //Ticker = TimerTime;
                    break;

                case FormType.Stopper:
                    break;
            }
            ShowTime();
            ATimer.Enabled = true;
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            
            switch (Setting)
            {
                case FormType.Timer:
                    if(ATimer.Enabled == false)
                        Ticker = TimerTime;
                    break;

                case FormType.Stopper:
                    break;
            }
            ATimer.Enabled = false;
            ShowTime();
        }

        // Reset / Set button
        private void ResetButton_Click(object sender, EventArgs e)
        {
            ATimer.Enabled = false;
            TimerTime = 0;
            int tmp = 0;
            switch (Setting)
            {
                case FormType.Timer:
                    if (Int32.TryParse(this.TimerTextBox.Text, out tmp))
                    {
                        TimerTime = Ticker = tmp;
                    }
                    else
                    {
                        string TimerBoxText = this.TimerTextBox.Text;
                        if (TimerBoxText.Contains(':'))
                        {
                            string[] tokens = TimerBoxText.Split(':');
                            if (tokens.Length == 2)
                            {
                                if (Int32.TryParse(tokens[0], out tmp))
                                {
                                    TimerTime += tmp * 60 * 60;
                                    if (Int32.TryParse(tokens[1], out tmp))
                                    {
                                        TimerTime += tmp * 60;
                                        Ticker = TimerTime;
                                    }
                                }
                            }
                            else if(tokens.Length == 3)
                            {
                                if (Int32.TryParse(tokens[0], out tmp))
                                {
                                    TimerTime += tmp * 60 * 60;
                                    if (Int32.TryParse(tokens[1], out tmp))
                                    {
                                        TimerTime += tmp * 60;
                                        if (Int32.TryParse(tokens[2], out tmp))
                                        {
                                            TimerTime += tmp;
                                            Ticker = TimerTime;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show(this, "Time should be: hh:mm or hh:mm:ss", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show(this, "Time should be: hh:mm or hh:mm:ss", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        
                    }
                    break;
                case FormType.Stopper:
                    Ticker = 0;
                    break;
            }
            ShowTime();
        }

        private void ShowNotification()
        {
            this.WindowState = FormWindowState.Minimized;
            this.Show();
            this.WindowState = FormWindowState.Normal;

            MessageBox.Show(this, "Timer ended " + DateTime.Now.ToString("HH:mm:ss")
                , "Ended", MessageBoxButtons.OK, MessageBoxIcon.Information);

            var notification = new System.Windows.Forms.NotifyIcon()
            {
                Visible = true,
                Icon = System.Drawing.SystemIcons.Information,
                // optional - BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info,
                // optional - BalloonTipTitle = "My Title",
                BalloonTipText = "My long description...",
            };

            // Display for 5 seconds.
            notification.ShowBalloonTip(5000);

            // This will let the balloon close after it's 5 second timeout
            // for demonstration purposes. Comment this out to see what happens
            // when dispose is called while a balloon is still visible.
            //Thread.Sleep(10000);

            // The notification should be disposed when you don't need it anymore,
            // but doing so will immediately close the balloon if it's visible.
            notification.Dispose();
        }
    }
}
