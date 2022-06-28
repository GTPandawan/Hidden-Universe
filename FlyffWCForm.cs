using System;
using System.Drawing;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
using System.Text.RegularExpressions;
using System.Threading;
using System.Collections.Generic;
using AutoUpdaterDotNET;

namespace HiddenUniverse_WebClient
{
    public partial class FlyffWCForm : Form 
    {
        // References
        private static FlyffWCForm _instance;
        public ChromiumWebBrowser chromeBrowser;
        public Thread buffThread;
        public bool healWasEnabled;
        private System.Windows.Forms.Timer waitForGameExitTimer;
        private AutoHealTimer autoHealerTimer = new AutoHealTimer();
        private AutoFollowTimer autoFollowTimer = new AutoFollowTimer();
        private AutoBuffTimer autoBuffTimer = new AutoBuffTimer();

        // Configuration Variables
        bool assistMode = false;
        public int autoHealSelectedIndex = -1;
        int[] numberKeyCodes = { 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39 };
        int[] functionKeyCodes = { 0x70, 0x71, 0x72 };        
        public int delaybb = 1500;
        public List<string> selectedBuffSlots { get; set; }

        // initiailization
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
            ArgumentManager.Instance.InitializeArguments();
            if (assistMode) { selectedBuffSlots = new List<string>(); SaveManager.Instance.LoadConfiguration(); }
            InitializeChromium();
        }
        public void EnableAssistMode()
        {
            assistMode = true;
            autoHealBox.Enabled = true;
            autoHealBox.Visible = true;
            autoBuffBox.Visible = true;
            autoBuffTime.Visible = true;
            autoBuffTree.Visible = true;
            autoBuffTree.Enabled = true;
            EnableAutoFollow();
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

        // Auto Heal Methods
        private void autoHealBox_CheckStateChanged(object sender, EventArgs e)
        {
            if (autoHealBox.CheckState == CheckState.Checked) { 
                autoHealBox.BackColor = Color.PeachPuff;
                autoHealTime.Enabled = true;
                autoHealTime.Visible = true;
                autoHealTime.BackColor = Color.PeachPuff;
                if (autoHealSelectedIndex == -1) { autoHealTime.SelectedIndex = 2; } else { autoHealTime.SelectedIndex = autoHealSelectedIndex; }
                autoHealerTimer.autoHealInterval = autoHealSelectedIndex + 1;
                if (autoHealerTimer.Timer == null) { autoHealerTimer.InitTimer(); }
                else if (autoHealerTimer.Timer != null && !autoHealerTimer.Timer.Enabled) { autoHealerTimer.Timer.Interval = autoHealerTimer.autoHealInterval * 1000; autoHealerTimer.Timer.Start(); }
            }
            else { autoHealBox.BackColor = Color.Gray;
                autoHealTime.Enabled = false;
                autoHealTime.Visible = false;
                autoHealTime.BackColor = Color.Gray;
                autoHealerTimer.Timer.Stop();
            }
        }
        private void autoHealTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            autoHealSelectedIndex = autoHealTime.SelectedIndex;
            autoHealerTimer.autoHealInterval = autoHealSelectedIndex + 1;
            if (autoHealerTimer.Timer != null && autoHealerTimer.Timer.Enabled)
            {
                autoHealerTimer.Timer.Interval = autoHealerTimer.autoHealInterval * 1000; // in miliseconds
            }
            else if (autoHealerTimer.Timer != null && !autoHealerTimer.Timer.Enabled)
            {
                autoHealerTimer.Timer.Interval = autoHealerTimer.autoHealInterval * 1000; // in miliseconds
                autoHealerTimer.Timer.Start();
            }
        }
        private void autoHealTime_DropDown(object sender, EventArgs e)
        {
            if (autoHealerTimer.Timer != null && autoHealerTimer.Timer.Enabled) { autoHealerTimer.Timer.Stop(); }
        }
        private void autoHealTime_DropDownClosed(object sender, EventArgs e)
        {
            if (autoHealerTimer.Timer != null && !autoHealerTimer.Timer.Enabled) { autoHealerTimer.Timer.Start(); }
        }
        public void CheckHealBox()//used to enable it after buff ends
        {
            autoHealBox.CheckState = CheckState.Checked;
        }

        // Auto Buff Methods
        private void autoBuffBox_Click(object sender, EventArgs e)
        {
            initiateBuff();
        }
        private void autoBuffTree_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Nodes.Count > 0 ) // If this is a parent node being checked
            {
                foreach (TreeNode childNode in e.Node.Nodes) // check/uncheck children nodes if there are any
                {
                    if (childNode.Checked != childNode.Parent.Checked) // Making sure no duplication of entries upon config load
                    {
                        childNode.Checked = childNode.Parent.Checked;
                    }
                }
            }
            else if (e.Node.Parent.Checked) // This is a child node and it's parent is checked
            {
                bool parentCheck = false;
                foreach (TreeNode child in e.Node.Parent.Nodes) // verify if parent needs to be unchecked (if all childrens are unchecked)
                {
                    if (child.Checked) { parentCheck = true; break; }
                }
                if (!parentCheck) { e.Node.Parent.Checked = false; }
            }
            else // a child node and it's parent is not checked
            {
                bool parentCheck = true;
                foreach (TreeNode child in e.Node.Parent.Nodes) // verify if parent needs to be checked (if all childrens are checked)
                {
                    if (!child.Checked) { parentCheck = false; break; }
                }
                if (parentCheck) { e.Node.Parent.Checked = true; }
            }
            if (e.Node.Checked && e.Node.Parent != null) // Add to list if checked
            {
                string cn = e.Node.Parent.Index.ToString() + "x" + e.Node.Index;
                selectedBuffSlots.Add(cn);                
            }
            else if (!e.Node.Checked && e.Node.Parent != null) // Remove from list if not checked
            {
                string cn = e.Node.Parent.Index.ToString() + "x" + e.Node.Index;
                selectedBuffSlots.Remove(cn);
            }
            selectedBuffSlots.Sort(); // Sorts the slots by placement and not by user selection order.
            if (selectedBuffSlots.Count > 0 && !autoBuffBox.Enabled) // Enable Buffbox
            {
                autoBuffBox.Enabled = true;
                autoBuffBox.BackColor = Color.PeachPuff;
                autoBuffTime.Enabled = true;
                autoBuffTime.SelectedIndex = 0;
            }
            else if (selectedBuffSlots.Count <= 0 && autoBuffBox.Enabled) // Disable Boffbox
            {
                autoBuffBox.Enabled = false;
                autoBuffBox.BackColor = Color.Gray;
                autoBuffTime.Enabled = false;
                autoBuffTime.SelectedIndex = 0;
            }
        }
        public void initiateBuff()
        {
            if (autoHealerTimer.Timer != null && autoHealerTimer.Timer.Enabled)
            {
                autoHealerTimer.Timer.Stop();
                autoHealBox.Checked = false;
                healWasEnabled = true; ;
            }
            else { healWasEnabled = false; }
            if (buffThread != null && buffThread.IsAlive) { buffThread.Abort(); }
            else
            {
                buffThread = new Thread(AutoBuff);
                buffThread.Start();
                if (autoBuffTimer.ThreadTimer == null) { autoBuffTimer.InitThreadTimer(); }
                else { autoBuffTimer.ThreadTimer.Start(); }
            }
        }
        private void AutoBuff()
        {
            Thread.Sleep(1500); // wait before buff sequence (in case another action is in progress)
            foreach (string str in selectedBuffSlots.ToArray())
            {
                int fKeyIndex, nKeyIndex;
                AutoBuffStringConvert(str,out fKeyIndex,out nKeyIndex);
                sendKeyCodeToBrowser(functionKeyCodes[fKeyIndex]);
                Thread.Sleep(75); // delay between switching hotbar & sending a buff command
                sendKeyCodeToBrowser(numberKeyCodes[nKeyIndex]);
                Thread.Sleep(delaybb);
            }
        }
        private void autoBuffTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            GroupCollection gc = RegexCheck.Test(autoBuffTime.GetItemText(autoBuffTime.SelectedItem), "Every ([0-9]{1,2}) minutes");
            if (gc != null)
            {
                var interval = Int32.Parse(gc[1].Value);
                if (autoBuffTimer.Timer == null) { autoBuffTimer.autoBuffInterval = interval * 60000; autoBuffTimer.InitTimer(); }
                else if (autoBuffTimer.Timer != null && autoBuffTimer.Timer.Enabled) { autoBuffTimer.Timer.Interval = autoBuffTimer.autoBuffInterval = interval * 60000; }
                else if (autoBuffTimer.Timer != null && !autoBuffTimer.Timer.Enabled) { autoBuffTimer.Timer.Interval = autoBuffTimer.autoBuffInterval = interval * 60000; autoBuffTimer.Timer.Start(); }
                if (autoBuffTimer.CountdownTimer == null) { autoBuffText.Enabled = true; autoBuffText.Visible = true; autoBuffTimer.InitCountdown(); } // starts countdown
                else if (autoBuffTimer.CountdownTimer != null && !autoBuffTimer.CountdownTimer.Enabled) { autoBuffTimer.cdelta = 0; autoBuffText.Enabled = true; autoBuffText.Visible = true; autoBuffTimer.CountdownTimer.Start(); } // resume countdown
            }
            else
            {
                if (autoBuffTimer.Timer != null && autoBuffTimer.Timer.Enabled) { autoBuffTimer.Timer.Stop(); }
            }
        }
        public void autoBuffTreeCheckItem (string[] config)
        {
            for (int i = 1; i < config.Length; i++) // check all saved items
            {
                int fKeyIndex, nKeyIndex;
                AutoBuffStringConvert(config[i], out fKeyIndex, out nKeyIndex);
                autoBuffTree.Nodes[fKeyIndex].Nodes[nKeyIndex].Checked = true;
            }
            for (int i = 0; i < autoBuffTree.Nodes.Count; i++)
            {
                bool parentCheck = true;
                foreach (TreeNode child in autoBuffTree.Nodes[i].Nodes)
                {
                    if (!child.Checked) { parentCheck = false; break; }
                }
                if (parentCheck) { autoBuffTree.Nodes[i].Checked = true; }
            }
        }
        private void AutoBuffStringConvert(string str, out int fKeyIndex, out int nKeyIndex)
        {
            var split = str.Split("x".ToCharArray());
            fKeyIndex = Int32.Parse(split[0]);
            nKeyIndex = Int32.Parse(split[1]);
        }
        private void autoBuffTime_DropDown(object sender, EventArgs e)
        {
            if (autoHealerTimer.Timer != null && autoHealerTimer.Timer.Enabled) { autoHealerTimer.Timer.Stop(); }
        }

        private void autoBuffTime_DropDownClosed(object sender, EventArgs e)
        {
            if (autoHealerTimer.Timer != null && !autoHealerTimer.Timer.Enabled && autoHealTime.Enabled) { autoHealerTimer.Timer.Start(); }
        }

        // Auto Follow Methods
        private void autoFollowBox_CheckStateChanged(object sender, EventArgs e)
        {
            if (autoFollowBox.Checked)
            {
                autoFollowBox.BackColor = Color.PeachPuff;
                if (autoFollowTimer.Timer == null)
                {
                    autoFollowTimer.InitTimer();
                }
                else { autoFollowTimer.Timer.Start(); }
            }
            else if (!autoFollowBox.Checked)
            {
                autoFollowBox.BackColor = Color.Gray;
                if (autoFollowTimer.Timer != null) { autoFollowTimer.Timer.Stop(); }
            }
        }

        // Leech Mode / Assist Mode
        public void EnableAutoFollow()
        {
            autoFollowBox.Visible = true;
            autoFollowBox.Enabled = true;
        }
        public void sendKeyCodeToBrowser(int keyCodeHex)
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

        // Updates
        private void CheckForUpdates()
        {
            AutoUpdater.Synchronous = true;
            AutoUpdater.Mandatory = true;
            AutoUpdater.AppTitle = "Hidden Universe WebClient";
            // Currently not working since this is a private repo and it requires authentication\token
            AutoUpdater.Start("https://raw.githubusercontent.com/HiddenUniverse/Hidden-Universe/main/version.xml");
        }
        
        // Game Exit
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
                        if (m.Success) { if (assistMode) { SaveManager.Instance.SaveConfiguration(); ; } Application.Exit(); }
                    }
                });
            }
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Cef.Shutdown();
        }

    }
}
