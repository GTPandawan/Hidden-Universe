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
        private System.Windows.Forms.Timer delaybbTimer { get; set; }
        public System.Windows.Forms.Timer DelaybbTimer { get { return delaybbTimer; } }
        private System.Windows.Forms.Timer cdTimer { get; set; }
        public System.Windows.Forms.Timer CDTimer { get { return cdTimer; } }

        public int autoBuffInterval = 0;
        public int currentBuffIndex = 0;
        private string[] selectedBuffsSlots;
        int[] numberKeyCodes = { 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39 };
        int[] functionKeyCodes = { 0x70, 0x71, 0x72 };
        public static int delaybb = 1750;
        public DateTime cdStart, abStart;

        public void InitTimer()
        {
            if (autoBuffInterval > 0)
            {
                if (abStart == null) { abStart = DateTime.Now; }
                timer = new System.Windows.Forms.Timer();
                timer.Tick += new EventHandler(Timer_Tick);
                timer.Interval = 200; // in miliseconds
                timer.Start();
            }
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (abStart == null) { abStart = DateTime.Now; }
            DateTime dt = DateTime.Now;
            TimeSpan ts = dt.Subtract(abStart);
            if (ts.TotalMilliseconds >= autoBuffInterval) { FlyffWCForm.Instance.initiateBuff(); abStart = DateTime.Now; }
        }
        public void InitDelaybbTimer()
        {
            delaybbTimer = new System.Windows.Forms.Timer();
            delaybbTimer.Tick += new EventHandler(DelaybbTimer_Tick);
            delaybbTimer.Interval = delaybb;
            delaybbTimer.Start();
        }
        private void DelaybbTimer_Tick(object sender, EventArgs e)
        {
            if (currentBuffIndex == 0) { selectedBuffsSlots = FlyffWCForm.Instance.selectedBuffSlots.ToArray(); }
            int fKeyIndex, nKeyIndex;
            FlyffWCForm.Instance.AutoBuffStringConvert(selectedBuffsSlots[currentBuffIndex], out fKeyIndex, out nKeyIndex);
            FlyffWCForm.Instance.sendKeyCodeToBrowser(functionKeyCodes[fKeyIndex]);
            Task.Delay(75);// delay between switching hotbar & sending a buff command
            FlyffWCForm.Instance.sendKeyCodeToBrowser(numberKeyCodes[nKeyIndex]);
            currentBuffIndex++;
            if (currentBuffIndex == selectedBuffsSlots.Length && FlyffWCForm.Instance.healWasEnabled)
            {
                currentBuffIndex = 0;
                FlyffWCForm.Instance.autoHealBox.Checked = true;
                delaybbTimer.Stop();
                return;
            }
            else if (currentBuffIndex == selectedBuffsSlots.Length) { currentBuffIndex = 0; delaybbTimer.Stop(); }
        }
        public void InitCDTimer()
        {
            if (cdStart == default(DateTime)) { cdStart = DateTime.Now; }
            cdTimer = new System.Windows.Forms.Timer();
            cdTimer.Tick += new EventHandler(CDTimer_Tick);
            cdTimer.Interval = 100; // in miliseconds
            cdTimer.Start();
        }
        private void CDTimer_Tick(object sender, EventArgs e)
        {
            if (cdStart == default(DateTime)) { cdStart = DateTime.Now; }
            DateTime dt = DateTime.Now;
            TimeSpan ts = dt.Subtract(cdStart);
            var cdsec = (autoBuffInterval - ts.TotalMilliseconds + delaybb) / 1000;
            FlyffWCForm.Instance.autoBuffCD.Text = cdsec.ToString("0.00") + " Seconds";
            if (ts.TotalMilliseconds - delaybb >= autoBuffInterval) { abStart = cdStart = DateTime.Now; }
        }
    }
}
