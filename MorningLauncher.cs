using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MorningLauncher
{
    /// <summary>
    /// Morning Launcher - A simple app to launch multiple applications at startup
    /// </summary>
    public class MorningLauncherForm : Form
    {
        private const string CONFIG_FILE = "apps.txt";

        // UI Controls
        private TabControl tabControl;
        private TabPage runTab;
        private TabPage settingsTab;
        private Button runButton;
        private ListBox appsListBox;
        private Button addButton;
        private Button removeButton;
        private Button browseButton;
        private Label instructionLabel;
        private Label settingsLabel;

        // Data
        private List<string> applicationPaths;

        public MorningLauncherForm()
        {
            applicationPaths = new List<string>();
            InitializeComponent();
            LoadApplicationPaths();
            RefreshListBox();
        }

        private void InitializeComponent()
        {
            // Form settings
            this.Text = "Morning Launcher";
            this.Size = new Size(600, 450);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Tab Control
            tabControl = new TabControl();
            tabControl.Dock = DockStyle.Fill;

            // Run Tab
            runTab = new TabPage("Launch");

            instructionLabel = new Label();
            instructionLabel.Text = "Click the RUN button below to launch all configured applications.\r\n\r\nThe launcher will close automatically after starting all apps.\r\n\r\nGo to the Settings tab to manage your application list.";
            instructionLabel.Location = new Point(20, 20);
            instructionLabel.Size = new Size(540, 100);
            instructionLabel.Font = new Font("Segoe UI", 10F);

            runButton = new Button();
            runButton.Text = "RUN ALL APPLICATIONS";
            runButton.Location = new Point(150, 150);
            runButton.Size = new Size(280, 80);
            runButton.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            runButton.BackColor = Color.FromArgb(0, 120, 212);
            runButton.ForeColor = Color.White;
            runButton.FlatStyle = FlatStyle.Flat;
            runButton.FlatAppearance.BorderSize = 0;
            runButton.Cursor = Cursors.Hand;
            runButton.Click += RunButton_Click;

            runTab.Controls.Add(instructionLabel);
            runTab.Controls.Add(runButton);

            // Settings Tab
            settingsTab = new TabPage("Settings");

            settingsLabel = new Label();
            settingsLabel.Text = "Manage Application List:";
            settingsLabel.Location = new Point(20, 20);
            settingsLabel.Size = new Size(400, 25);
            settingsLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);

            appsListBox = new ListBox();
            appsListBox.Location = new Point(20, 50);
            appsListBox.Size = new Size(540, 250);
            appsListBox.Font = new Font("Consolas", 9F);
            appsListBox.HorizontalScrollbar = true;

            addButton = new Button();
            addButton.Text = "Add Path Manually";
            addButton.Location = new Point(20, 320);
            addButton.Size = new Size(150, 35);
            addButton.Click += AddButton_Click;

            browseButton = new Button();
            browseButton.Text = "Browse & Add...";
            browseButton.Location = new Point(180, 320);
            browseButton.Size = new Size(150, 35);
            browseButton.Click += BrowseButton_Click;

            removeButton = new Button();
            removeButton.Text = "Remove Selected";
            removeButton.Location = new Point(340, 320);
            removeButton.Size = new Size(150, 35);
            removeButton.Click += RemoveButton_Click;

            settingsTab.Controls.Add(settingsLabel);
            settingsTab.Controls.Add(appsListBox);
            settingsTab.Controls.Add(addButton);
            settingsTab.Controls.Add(browseButton);
            settingsTab.Controls.Add(removeButton);

            // Add tabs to control
            tabControl.TabPages.Add(runTab);
            tabControl.TabPages.Add(settingsTab);

            // Add tab control to form
            this.Controls.Add(tabControl);
        }

        private void LoadApplicationPaths()
        {
            applicationPaths.Clear();

            if (!File.Exists(CONFIG_FILE))
            {
                return;
            }

            try
            {
                string[] lines = File.ReadAllLines(CONFIG_FILE);
                foreach (string line in lines)
                {
                    string trimmed = line.Trim();
                    if (!string.IsNullOrEmpty(trimmed))
                    {
                        applicationPaths.Add(trimmed);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading configuration:\r\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveApplicationPaths()
        {
            try
            {
                File.WriteAllLines(CONFIG_FILE, applicationPaths);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving configuration:\r\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshListBox()
        {
            appsListBox.Items.Clear();
            foreach (string path in applicationPaths)
            {
                appsListBox.Items.Add(path);
            }

            // Update run button text
            if (applicationPaths.Count == 0)
            {
                runButton.Text = "NO APPLICATIONS CONFIGURED";
                runButton.Enabled = false;
            }
            else
            {
                runButton.Text = $"RUN ALL ({applicationPaths.Count}) APPLICATIONS";
                runButton.Enabled = true;
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            using (var inputForm = new Form())
            {
                inputForm.Text = "Add Application Path";
                inputForm.Size = new Size(500, 150);
                inputForm.StartPosition = FormStartPosition.CenterParent;
                inputForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                inputForm.MaximizeBox = false;
                inputForm.MinimizeBox = false;

                var label = new Label();
                label.Text = "Enter full path to executable:";
                label.Location = new Point(10, 10);
                label.Size = new Size(460, 20);

                var textBox = new TextBox();
                textBox.Location = new Point(10, 35);
                textBox.Size = new Size(460, 25);

                var okButton = new Button();
                okButton.Text = "Add";
                okButton.DialogResult = DialogResult.OK;
                okButton.Location = new Point(310, 70);
                okButton.Size = new Size(75, 30);

                var cancelButton = new Button();
                cancelButton.Text = "Cancel";
                cancelButton.DialogResult = DialogResult.Cancel;
                cancelButton.Location = new Point(395, 70);
                cancelButton.Size = new Size(75, 30);

                inputForm.Controls.AddRange(new Control[] { label, textBox, okButton, cancelButton });
                inputForm.AcceptButton = okButton;
                inputForm.CancelButton = cancelButton;

                if (inputForm.ShowDialog() == DialogResult.OK)
                {
                    string path = textBox.Text.Trim();
                    if (!string.IsNullOrEmpty(path))
                    {
                        applicationPaths.Add(path);
                        SaveApplicationPaths();
                        RefreshListBox();
                    }
                }
            }
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Select Application to Add";
                openFileDialog.Filter = "Executable Files (*.exe)|*.exe|All Files (*.*)|*.*";
                openFileDialog.Multiselect = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    foreach (string fileName in openFileDialog.FileNames)
                    {
                        if (!applicationPaths.Contains(fileName))
                        {
                            applicationPaths.Add(fileName);
                        }
                    }
                    SaveApplicationPaths();
                    RefreshListBox();
                }
            }
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            if (appsListBox.SelectedIndex >= 0)
            {
                int index = appsListBox.SelectedIndex;
                applicationPaths.RemoveAt(index);
                SaveApplicationPaths();
                RefreshListBox();
            }
            else
            {
                MessageBox.Show("Please select an application to remove.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void RunButton_Click(object sender, EventArgs e)
        {
            if (applicationPaths.Count == 0)
            {
                MessageBox.Show("No applications configured. Please add applications in the Settings tab.",
                    "No Applications", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int successCount = 0;
            int failCount = 0;
            List<string> failedApps = new List<string>();

            foreach (string appPath in applicationPaths)
            {
                try
                {
                    Process.Start(appPath);
                    successCount++;
                }
                catch (Exception ex)
                {
                    failCount++;
                    failedApps.Add($"{appPath}\r\n  Error: {ex.Message}");
                }
            }

            // Show summary if there were failures
            if (failCount > 0)
            {
                string message = $"Launched {successCount} application(s) successfully.\r\n" +
                                $"{failCount} application(s) failed to launch:\r\n\r\n" +
                                string.Join("\r\n\r\n", failedApps) +
                                "\r\n\r\nThe launcher will now close.";
                MessageBox.Show(message, "Launch Complete", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            // Close the application
            Application.Exit();
        }

        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MorningLauncherForm());
        }
    }
}
