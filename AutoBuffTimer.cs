using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiddenUniverse_WebClient
{
    internal class AutoBuffTimer
    {
        private System.Windows.Forms.Timer timer { get; set; }
        private System.Windows.Forms.Timer countdownTimer { get; set; }
        public System.Windows.Forms.Timer Timer { get { return timer; } }
        public System.Windows.Forms.Timer CountdownTimer { get { return countdownTimer; } }
        private System.Windows.Forms.Timer threadTimer { get; set; }
        public System.Windows.Forms.Timer ThreadTimer { get { return threadTimer; } }

        public int autoBuffInterval = 0;
        public float cdelta = 0;
        public int lastTimerIInterval;
        
        public void InitTimer()
        {
            if (autoBuffInterval > 0)
            {
                timer = new System.Windows.Forms.Timer();
                timer.Tick += new EventHandler(Timer_Tick);
                timer.Interval = autoBuffInterval; // in miliseconds
                timer.Start();
            }
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            FlyffWCForm.Instance.initiateBuff();
        }

        public void InitCountdown()
        {
            countdownTimer = new System.Windows.Forms.Timer();
            countdownTimer.Tick += new EventHandler(CountdownTimer_Tick);
            countdownTimer.Interval = 100;
            countdownTimer.Start();
        }
        private void CountdownTimer_Tick(object sender, EventArgs e)
        {
            if (timer == null || !timer.Enabled )
            {
                FlyffWCForm.Instance.autoBuffText.Enabled = false;
                FlyffWCForm.Instance.autoBuffText.Visible = false;
                countdownTimer.Stop();
                return;
            }
            if (lastTimerIInterval != autoBuffInterval || cdelta == autoBuffInterval)
            {
                cdelta = 0;
                lastTimerIInterval = autoBuffInterval;
            }
            cdelta = cdelta + countdownTimer.Interval;
            string cdsec = ((timer.Interval - cdelta) / 1000).ToString("0.0");
            FlyffWCForm.Instance.autoBuffText.Text = cdsec + " Seconds";
        }
        public void InitThreadTimer()
        {
            threadTimer = new System.Windows.Forms.Timer();
            threadTimer.Tick += new EventHandler(ThreadTimer_Tick);
            threadTimer.Interval = 1000; // in miliseconds
            threadTimer.Start();
        }
        private void ThreadTimer_Tick(object sender, EventArgs e)
        {
            if (!FlyffWCForm.Instance.buffThread.IsAlive && FlyffWCForm.Instance.healWasEnabled)
            {
                FlyffWCForm.Instance.autoHealBox.Checked = true;
                threadTimer.Stop();
                return;
            }
            else if (!FlyffWCForm.Instance.buffThread.IsAlive) { threadTimer.Stop(); }
        }
    }
}
