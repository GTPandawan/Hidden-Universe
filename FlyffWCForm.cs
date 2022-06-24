using System;
using System.Drawing;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
using System.Text.RegularExpressions;
using System.Threading;
using AutoUpdaterDotNET;

namespace HiddenUniverse_WebClient
{
    public partial class FlyffWCForm : Form
    {
        private static FlyffWCForm _instance;
        public ChromiumWebBrowser chromeBrowser;
        private Thread buffThread;
        private System.Windows.Forms.Timer waitForGameExitTimer, autoHealTimer, autoFollowTimer;
        bool assistMode = false;
        bool autoFollow = false;
        int autoHealInterval = 0;
        public int autoHealSelectedIndex = -1;
        int autoBuffListCheckedCount = 0;
        int autoBuffInterval = 2;
        int[] numberKeyCodes = { 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39 };

        public FlyffWCForm()
        {
            if (_instance == null) { _instance = this; }
            InitializeComponent();
            CheckForUpdates();
            SetArguments();
        }
        public static FlyffWCForm Instance { get { return _instance; } }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Shown += new EventHandler(Form1_Shown);
        }
        private void SetArguments()
        {
            FixBuffListSize();
            ArgumentManager.Instance.InitializeArguments();
            if (assistMode) { SaveManager.Instance.LoadConfiguration(); }
            InitializeChromium();
        }
        private void FixBuffListSize()
        {
            autoBuffList.ClientSize = new Size(autoBuffList.ClientSize.Width, autoBuffList.GetItemRectangle(0).Height * autoBuffList.Items.Count);
        }

        public void EnableAssistMode()
        {
            assistMode = true;
            autoHealBox.Enabled = true;
            autoHealBox.Visible = true;
            autoBuffList.Visible = true;
            autoBuffList.Enabled = true;
            autoBuffBox.Visible = true;
            autoFollowBox.Visible = true;
            autoFollowBox.Enabled = true;
        }
        public void InitializeChromium()
        {
            CefSettings settings = new CefSettings();
            Cef.EnableHighDPISupport();
            settings.CachePath = ArgumentManager.profilePath;
            settings.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/96.0.4664.110 Safari/537.36 /CefSharp Browser" + Cef.CefSharpVersion;
            Cef.Initialize(settings);
            chromeBrowser = new ChromiumWebBrowser("https://universe.flyff.com/play");
            this.Controls.Add(chromeBrowser);
            chromeBrowser.Dock = DockStyle.Fill;

        }
        private void Form1_Shown(Object sender, EventArgs e)
        {
            InitWaitForGameExitTimer();
        }
        public void WaitForGameExit()
        {
            string pat = "Restart the Game";
            Regex r = new Regex(pat, RegexOptions.IgnoreCase);
                if (chromeBrowser.IsBrowserInitialized)
                {
                    var task = chromeBrowser.GetTextAsync();
                task.ContinueWith(t =>
                {
                    if (!t.IsFaulted)
                    {
                        var response = t.Result;
                        Match m = r.Match(response);
                        if (m.Success) { if (assistMode) { SaveManager.Instance.SaveConfiguration(autoBuffListCheckedCount, this.autoBuffList, numberKeyCodes.Length); ; } Application.Exit(); }
                    }
                });
                }
        }
        public void InitWaitForGameExitTimer()
        {
            waitForGameExitTimer = new System.Windows.Forms.Timer();
            waitForGameExitTimer.Tick += new EventHandler(waitForGameExitTimer_Tick);
            waitForGameExitTimer.Interval = 1000; // in miliseconds
            waitForGameExitTimer.Start();
        }
        private void waitForGameExitTimer_Tick(object sender, EventArgs e)
        {
            Thread t = new Thread(WaitForGameExit);
            t.Start();
        }
        private void autoHealBox_CheckStateChanged(object sender, EventArgs e)
        {
            if (autoHealBox.CheckState == CheckState.Checked) { 
                autoHealBox.BackColor = Color.PeachPuff;
                autoHealTime.Enabled = true;
                autoHealTime.Visible = true;
                autoHealTime.BackColor = Color.PeachPuff;
                if (autoHealSelectedIndex == -1) { autoHealTime.SelectedIndex = 2; } else { autoHealTime.SelectedIndex = autoHealSelectedIndex; }
                if (autoHealTimer == null || !autoHealTimer.Enabled) { initAutoHealTimer(); }
                else { autoHealTimer.Start(); }
            }
            else { autoHealBox.BackColor = Color.Gray;
                autoHealTime.Enabled = false;
                autoHealTime.Visible = false;
                autoHealTime.BackColor = Color.Gray;
                autoHealTimer.Stop();
            }
        }
        private void initAutoHealTimer()
        {
            autoHealTimer = new System.Windows.Forms.Timer();
            autoHealTimer.Tick += new EventHandler(autoHealTimer_Tick);
            autoHealTimer.Interval = autoHealInterval * 1000; // in miliseconds
            autoHealTimer.Start();
        }
        private void autoHealTimer_Tick(object sender, EventArgs e)
        {
            autoHealTimer.Interval = autoHealInterval * 1000;
            sendKeyCodeToBrowser(0x43);
        }
        private void autoHealTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            autoHealSelectedIndex = autoHealTime.SelectedIndex;
            autoHealInterval = autoHealTime.SelectedIndex + 1;
            if (autoHealTimer != null && autoHealTimer.Enabled) {
                autoHealTimer.Stop();
                autoHealTimer.Interval = autoHealInterval * 1000; // in miliseconds
                autoHealTimer.Start();
            }
        }
        private void autoBuffList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked) { autoBuffListCheckedCount++; }
            else if (e.NewValue == CheckState.Unchecked) { autoBuffListCheckedCount--; }
            if (autoBuffListCheckedCount > 0) { autoBuffBox.Enabled = true; autoBuffBox.BackColor = Color.PeachPuff; }
            else { autoBuffBox.Enabled = false; autoBuffBox.BackColor = Color.Gray; }
        }
        private void autoBuffBox_Click(object sender, EventArgs e)
        {
            if (autoHealTimer != null && autoHealTimer.Enabled)
            {
                autoHealTimer.Stop();
                autoHealBox.Checked = false;
            }

            int[] codesToSend = new int[autoBuffListCheckedCount];
            int lst = 0;
            for (int i = 0; i < numberKeyCodes.Length; i++)
            {
                if (autoBuffList.GetItemChecked(i))
                {
                    codesToSend[lst] = numberKeyCodes[i];
                    lst++;
                }
            }
            if (buffThread != null && buffThread.IsAlive) { buffThread.Abort(); }
            else {
                buffThread = new Thread(AutoBuff);
                buffThread.Start(codesToSend);
            }
            
        }

        public void autoBuffListCheckItem (int i)
        {
            autoBuffList.SetItemChecked(i, true);
        }

        private void AutoBuff(object obj)
        {
            Thread.Sleep(1500);
            int[] numberKeyCodes = (int[])obj;
            foreach (int keyCode in numberKeyCodes) { sendKeyCodeToBrowser(keyCode); Thread.Sleep(autoBuffInterval * 1000); }
        }
        private void sendKeyCodeToBrowser(int keyCodeHex)
        {
            KeyEvent k = new KeyEvent();
            k.WindowsKeyCode = keyCodeHex;
            k.FocusOnEditableField = false;
            k.IsSystemKey = false;
            k.Type = KeyEventType.KeyDown;
            chromeBrowser.GetBrowser().GetHost().SendKeyEvent(k);
            k.Type = KeyEventType.KeyUp;
            chromeBrowser.GetBrowser().GetHost().SendKeyEvent(k);
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Cef.Shutdown();
        }

        private void autoFollowBox_CheckStateChanged(object sender, EventArgs e)
        {
            if (autoFollowBox.Checked) { 
                autoFollow = true;
                autoFollowBox.BackColor = Color.PeachPuff;
                if (autoFollowTimer == null) { 
                    initAutoFollowTimer(); } 
                else { autoFollowTimer.Start(); } 
            }
            else if (!autoFollowBox.Checked) { 
                autoFollow = false;
                autoFollowBox.BackColor = Color.Gray;
                if (autoFollowTimer != null) { autoFollowTimer.Stop(); } 
            }
        }
        private void initAutoFollowTimer()
        {
            autoFollowTimer = new System.Windows.Forms.Timer();
            autoFollowTimer.Tick += new EventHandler(autoFollowTimer_Tick);
            autoFollowTimer.Interval = 5000; // in miliseconds
            autoFollowTimer.Start();
        }
        private void autoFollowTimer_Tick(object sender, EventArgs e)
        {
            if (autoFollow) { sendKeyCodeToBrowser(0x5A); }
        }
        private void CheckForUpdates()
        {
            AutoUpdater.Synchronous = true;
            AutoUpdater.Mandatory = true;
            AutoUpdater.AppTitle = "Hidden Universe WebClient";
            // Currently not working since this is a private repo and it requires authentication\token
            AutoUpdater.Start("https://raw.githubusercontent.com/HiddenUniverse/Hidden-Universe/main/version.xml");
        }
    }
}
