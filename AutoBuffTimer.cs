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
        public System.Windows.Forms.Timer Timer { get { return timer; } }
        private System.Windows.Forms.Timer threadTimer { get; set; }
        public System.Windows.Forms.Timer ThreadTimer { get { return threadTimer; } }

        public int autoBuffInterval = 0;
        
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
