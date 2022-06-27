﻿using System;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;

namespace HiddenUniverse_WebClient
{
    class SaveManager
    {
        private static SaveManager _instance;
        FlyffWCForm mainForm = FlyffWCForm.Instance;

        private SaveManager()
        {

        }
        public static SaveManager Instance { get { if (_instance == null) { _instance = new SaveManager(); } return _instance; } }
        public void SaveConfiguration()
        {
            int autoHealSelectedIndex = mainForm.autoHealSelectedIndex;
            List<string> buffTree = mainForm.selectedBuffSlots;
            List<string> config = new List<string>();
            int configItems = buffTree.Count + 1;
            config.Add(autoHealSelectedIndex.ToString());
            foreach (string b in buffTree) { config.Add(b); }
            File.WriteAllLines(ArgumentManager.assistfsConfigPath, config);
        }

        public void LoadConfiguration()
        {
            if (File.Exists(ArgumentManager.assistfsConfigPath))
            {
                string[] config = File.ReadAllLines(ArgumentManager.assistfsConfigPath);
                int autoHealSelectedIndex = Int32.Parse(config[0]);
                mainForm.autoHealSelectedIndex = autoHealSelectedIndex;
                mainForm.autoBuffTreeCheckItem(config); 

            }
        }
    }
}
