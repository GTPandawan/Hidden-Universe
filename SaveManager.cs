using System;
using System.Windows.Forms;
using System.IO;

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
        public void SaveConfiguration(int buffListCount, CheckedListBox autoBuffList,  int numberKeyCodesLength)
        {
            int configItems = buffListCount + 1;
            string[] config = new string[configItems];
            int lst = 0;
            int autoHealSelectedIndex = mainForm.autoHealSelectedIndex;
            config[lst] = autoHealSelectedIndex.ToString();
            lst++;
            for (int i = 0; i < numberKeyCodesLength; i++)
            {
                if (autoBuffList.GetItemChecked(i))
                {
                    config[lst] = i.ToString();
                    lst++;
                }
            }
            File.WriteAllLines(ArgumentManager.assistfsConfigPath, config);
        }

        public void LoadConfiguration()
        {
            if (File.Exists(ArgumentManager.assistfsConfigPath))
            {
                string[] config = File.ReadAllLines(ArgumentManager.assistfsConfigPath);
                int autoHealSelectedIndex;
                Int32.TryParse(config[0], out autoHealSelectedIndex);
                mainForm.autoHealSelectedIndex = autoHealSelectedIndex;
                for (int i = 1; i < config.Length; i++)
                {
                    int c;
                    Int32.TryParse(config[i], out c);
                    mainForm.autoBuffListCheckItem(c);
                }
            }

        }
    }
}
