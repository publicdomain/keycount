// <copyright file="MainForm.cs" company="PublicDomain.is">
//     CC0 1.0 Universal (CC0 1.0) - Public Domain Dedication
//     https://creativecommons.org/publicdomain/zero/1.0/legalcode
// </copyright>
namespace KeyCount
{
    // Directives
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using Gma.System.MouseKeyHook;

    /// <summary>
    /// Description of MainForm.
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        /// The count.
        /// </summary>
        private int count = 0;

        /// <summary>
        /// The key code.
        /// </summary>
        private Keys keyCode;

        /// <summary>
        /// Gets or sets the associated icon.
        /// </summary>
        /// <value>The associated icon.</value>
        private Icon associatedIcon = null;

        /// <summary>
        /// The m global hook.
        /// </summary>
        private IKeyboardMouseEvents m_GlobalHook;

        /// <summary>
        /// The stopwatch.
        /// </summary>
        Stopwatch stopwatch = new Stopwatch();

        /// <summary>
        /// Initializes a new instance of the <see cref="T:KeyCount.MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            // The InitializeComponent() call is required for Windows Forms designer support.
            this.InitializeComponent();

            // Add keys
            foreach (var key in Enum.GetValues(typeof(Keys)))
            {
                // Check if unique
                if (!this.keyComboBox.Items.Contains(key.ToString()))
                {
                    // Add to dropdown list
                    this.keyComboBox.Items.Add(key.ToString());
                }
            }

            /* Set icons */

            // Set associated icon from exe file
            this.associatedIcon = Icon.ExtractAssociatedIcon(typeof(MainForm).GetTypeInfo().Assembly.Location);

            // Set icon for Free Releases @ PublicDomain.is menu item
            this.freeReleasesPublicDomainisToolStripMenuItem.Image = this.associatedIcon.ToBitmap();
        }

        /// <summary>
        /// Handles the start stop button click.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnStartStopButtonClick(object sender, EventArgs e)
        {
            // Check for start
            // Check for Star(t) vs Sto(p)
            if (this.startStopButton.Text.EndsWith("t", StringComparison.InvariantCulture))
            {
                // Set key code
                this.keyCode = (Keys)Enum.Parse(typeof(Keys), this.keyComboBox.SelectedItem.ToString(), true);

                // Start stopwatch
                this.stopwatch.Start();

                // Start timer
                this.activeTimer.Start();

                // Subscribe
                this.Subscribe();

                // Change to stop
                this.startStopButton.Text = "&Stop";
                this.startStopButton.ForeColor = Color.Red;
            }
            else
            {
                // Stop stopwatch
                this.stopwatch.Stop();

                // Stop timer
                this.activeTimer.Stop();

                // Unsubscribe
                this.Unsubscribe();

                // Reset to start
                this.startStopButton.Text = "&Start";
                this.startStopButton.ForeColor = Color.DarkGreen;
            }
        }

        /// <summary>
        /// Handles the new tool strip menu item click.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnNewToolStripMenuItemClick(object sender, EventArgs e)
        {
            // Stop activity
            if (this.startStopButton.Text.EndsWith("p", StringComparison.InvariantCulture))
            {
                this.startStopButton.PerformClick();
            }

            // Reset count
            this.count = 0;

            // Reset stop watch
            this.stopwatch.Reset();

            // Reset labels
            this.countLabel.Text = "0";
            this.countCountToolStripStatusLabel.Text = "00:00:00";
        }

        /// <summary>
        /// Handles the copy tool strip menu item click.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnCopyToolStripMenuItemClick(object sender, EventArgs e)
        {
            // Copy current count
            Clipboard.Clear();
            Clipboard.SetText(this.count.ToString());
        }

        /// <summary>
        /// Handles the options tool strip menu item drop down item clicked.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnOptionsToolStripMenuItemDropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            // Set tool strip menu item
            ToolStripMenuItem toolStripMenuItem = (ToolStripMenuItem)e.ClickedItem;

            // Toggle checked
            toolStripMenuItem.Checked = !toolStripMenuItem.Checked;

            // Set topmost by check box
            this.TopMost = this.alwaysOnTopToolStripMenuItem.Checked;
        }

        /// <summary>
        /// Handles the free releases public domainis tool strip menu item click.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnFreeReleasesPublicDomainisToolStripMenuItemClick(object sender, EventArgs e)
        {
            // Open our website
            Process.Start("https://publicdomain.is");
        }

        /// <summary>
        /// Handles the original thread redditcom tool strip menu item click.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnOriginalThreadRedditcomToolStripMenuItemClick(object sender, EventArgs e)
        {
            // Open original thread
            Process.Start("https://www.reddit.com/r/software/comments/ulp3aa/any_way_to_count_individual_keystrokes/");
        }

        /// <summary>
        /// Handles the source code githubcom tool strip menu item click.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnSourceCodeGithubcomToolStripMenuItemClick(object sender, EventArgs e)
        {
            // TODO Add code
        }

        /// <summary>
        /// Handles the about tool strip menu item click.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnAboutToolStripMenuItemClick(object sender, EventArgs e)
        {
            // TODO Add code
        }

        /// <summary>
        /// Handles the key combo box text changed.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnKeyComboBoxTextChanged(object sender, EventArgs e)
        {
            // Set key code
            this.keyCode = (Keys)Enum.Parse(typeof(Keys), this.keyComboBox.SelectedItem.ToString(), true);
        }

        /// <summary>
        /// Handles the main form load.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnMainFormLoad(object sender, EventArgs e)
        {
            // Focus enter key (13)
            for (int i = 0; i < this.keyComboBox.Items.Count; i++)
            {
                if (Convert.ToInt32((Keys)Enum.Parse(typeof(Keys), this.keyComboBox.Items[i].ToString(), true)) == 13)
                {
                    this.keyComboBox.SelectedIndex = i;

                    return;
                }
            }
        }

        /// <summary>
        /// Subscribe this instance.
        /// </summary>
        public void Subscribe()
        {
            m_GlobalHook = Hook.GlobalEvents();

            m_GlobalHook.KeyDown += OnGlobalHookKeyDown;
        }

        /// <summary>
        /// Unsubscribe this instance.
        /// </summary>
        public void Unsubscribe()
        {
            m_GlobalHook.KeyDown -= OnGlobalHookKeyDown;

            m_GlobalHook.Dispose();
        }

        /// <summary>
        /// Handles the global hook key down.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnGlobalHookKeyDown(object sender, KeyEventArgs e)
        {
            // Check for matching key code
            if (this.keyCode == e.KeyCode)
            {
                // Raise count
                this.count++;

                // Update count label
                this.countLabel.Text = $"{this.count}";
            }
        }

        /// <summary>
        /// Handles the active timer tick.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnActiveTimerTick(object sender, EventArgs e)
        {
            // Show elapsed time
            this.countCountToolStripStatusLabel.Text = this.stopwatch.Elapsed.ToString(@"hh\:mm\:ss");
        }

        /// <summary>
        /// Handles the exit tool strip menu item click.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnExitToolStripMenuItemClick(object sender, System.EventArgs e)
        {
            // Close program
            this.Close();
        }
    }
}
