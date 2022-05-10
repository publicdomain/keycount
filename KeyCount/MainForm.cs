// <copyright file="MainForm.cs" company="PublicDomain.is">
//     CC0 1.0 Universal (CC0 1.0) - Public Domain Dedication
//     https://creativecommons.org/publicdomain/zero/1.0/legalcode
// </copyright>
namespace KeyCount
{
    // Directives
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

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
        /// Gets or sets the associated icon.
        /// </summary>
        /// <value>The associated icon.</value>
        private Icon associatedIcon = null;

        /// <summary>
        /// Registers the hot key.
        /// </summary>
        /// <returns><c>true</c>, if hot key was registered, <c>false</c> otherwise.</returns>
        /// <param name="hWnd">H window.</param>
        /// <param name="id">Identifier.</param>
        /// <param name="fsModifiers">Fs modifiers.</param>
        /// <param name="vk">Vk.</param>
        [DllImport("User32")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

        /// <summary>
        /// Unregisters the hot key.
        /// </summary>
        /// <returns><c>true</c>, if hot key was unregistered, <c>false</c> otherwise.</returns>
        /// <param name="hWnd">H window.</param>
        /// <param name="id">Identifier.</param>
        [DllImport("User32")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        /// <summary>
        /// The wm hotkey.
        /// </summary>
        private static int WM_HOTKEY = 0x0312;

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
        /// Window procedure.
        /// </summary>
        /// <param name="m">M.</param>
        protected override void WndProc(ref Message m)
        {
            // Process message
            base.WndProc(ref m);

            // Check for hotkey press
            if (m.Msg == WM_HOTKEY)
            {
                // Raise count
                this.count++;

                // Update count label
                this.countLabel.Text = $"{this.count}";
            }
        }
        /// <summary>
        /// Handles the start stop button click.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnStartStopButtonClick(object sender, EventArgs e)
        {
            // TODO Add code
        }

        /// <summary>
        /// Handles the new tool strip menu item click.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnNewToolStripMenuItemClick(object sender, EventArgs e)
        {
            // TODO Add code
        }

        /// <summary>
        /// Handles the copy tool strip menu item click.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnCopyToolStripMenuItemClick(object sender, EventArgs e)
        {
            // TODO Add code
        }

        /// <summary>
        /// Handles the options tool strip menu item drop down item clicked.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnOptionsToolStripMenuItemDropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            // TODO Add code
        }

        /// <summary>
        /// Handles the free releases public domainis tool strip menu item click.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnFreeReleasesPublicDomainisToolStripMenuItemClick(object sender, EventArgs e)
        {
            // TODO Add code
        }

        /// <summary>
        /// Handles the original thread redditcom tool strip menu item click.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnOriginalThreadRedditcomToolStripMenuItemClick(object sender, EventArgs e)
        {
            // TODO Add code
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
            // TODO Add code
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
        /// Processes the hotkey registration.
        /// </summary>
        /// <param name="registerHotkey">If set to <c>true</c> register hotkey.</param>
        private void ProcessHotkeyRegistration(bool registerHotkey)
        {
            // Try to unregister the key
            try
            {
                // Unregister the hotkey
                UnregisterHotKey(this.Handle, 0);
            }
            catch
            {
                // Let it fall through
            }

            // Halt on unregister
            if (!registerHotkey)
            {
                // Halt flow
                return;
            }

            // Hotkey registration
            try
            {
                // Register the hotkey with selected modifiers
                RegisterHotKey(this.Handle, 0, 0, Convert.ToInt32((Keys)Enum.Parse(typeof(Keys), this.keyComboBox.SelectedItem.ToString(), true)));
            }
            catch
            {
                // Let it fall through
            }
        }

        /// <summary>
        /// Handles the exit tool strip menu item click.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnExitToolStripMenuItemClick(object sender, System.EventArgs e)
        {
            // TODO Add code
        }
    }
}
