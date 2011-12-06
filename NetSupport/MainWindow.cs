using System;
using System.Diagnostics;
using System.DirectoryServices;
using System.Drawing;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Resources;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;
using Tamir.SharpSsh.java.util;
using Tamir.SharpSsh.jsch;

namespace NetSupport
{
    public partial class MainWindow : Form
    {
        private const string rdpRegKey = @"System\CurrentControlSet\Control\Terminal Server";

        private Session sshSession = null;
        private Properties.Settings config = Properties.Settings.Default;
        private bool sessionActive = false;
        ResourceManager langRes = null;
        ServerAddress srvAddrDiag = null;

        public MainWindow()
        {
            SplashScreen.ShowSplashScreen();

            InitializeComponent();

            langRes = new ResourceManager("NetSupport.Language", typeof(MainWindow).Assembly);
            srvAddrDiag = new ServerAddress();
        }

        #region Win32

        const int ERROR_ACCOUNT_RESTRICTION = 1327;

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool CloseHandle(IntPtr hObject);

        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool LogonUser(
            string lpszUsername,
            string lpszDomain,
            string lpszPassword,
            int dwLogonType,
            int dwLogonProvider,
            out IntPtr phToken
            );

        public enum LogonType
        {
            /// <summary>
            /// This logon type is intended for users who will be interactively using the computer, such as a user being logged on  
            /// by a terminal server, remote shell, or similar process.
            /// This logon type has the additional expense of caching logon information for disconnected operations; 
            /// therefore, it is inappropriate for some client/server applications,
            /// such as a mail server.
            /// </summary>
            LOGON32_LOGON_INTERACTIVE = 2,

            /// <summary>
            /// This logon type is intended for high performance servers to authenticate plaintext passwords.

            /// The LogonUser function does not cache credentials for this logon type.
            /// </summary>
            LOGON32_LOGON_NETWORK = 3,

            /// <summary>
            /// This logon type is intended for batch servers, where processes may be executing on behalf of a user without 
            /// their direct intervention. This type is also for higher performance servers that process many plaintext
            /// authentication attempts at a time, such as mail or Web servers. 
            /// The LogonUser function does not cache credentials for this logon type.
            /// </summary>
            LOGON32_LOGON_BATCH = 4,

            /// <summary>
            /// Indicates a service-type logon. The account provided must have the service privilege enabled. 
            /// </summary>
            LOGON32_LOGON_SERVICE = 5,

            /// <summary>
            /// This logon type is for GINA DLLs that log on users who will be interactively using the computer. 
            /// This logon type can generate a unique audit record that shows when the workstation was unlocked. 
            /// </summary>
            LOGON32_LOGON_UNLOCK = 7,

            /// <summary>
            /// This logon type preserves the name and password in the authentication package, which allows the server to make 
            /// connections to other network servers while impersonating the client. A server can accept plaintext credentials 
            /// from a client, call LogonUser, verify that the user can access the system across the network, and still 
            /// communicate with other servers.
            /// NOTE: Windows NT:  This value is not supported. 
            /// </summary>
            LOGON32_LOGON_NETWORK_CLEARTEXT = 8,

            /// <summary>
            /// This logon type allows the caller to clone its current token and specify new credentials for outbound connections.
            /// The new logon session has the same local identifier but uses different credentials for other network connections. 
            /// NOTE: This logon type is supported only by the LOGON32_PROVIDER_WINNT50 logon provider.
            /// NOTE: Windows NT:  This value is not supported. 
            /// </summary>
            LOGON32_LOGON_NEW_CREDENTIALS = 9,
        }

        public enum LogonProvider
        {
            /// <summary>
            /// Use the standard logon provider for the system. 
            /// The default security provider is negotiate, unless you pass NULL for the domain name and the user name 
            /// is not in UPN format. In this case, the default provider is NTLM. 
            /// NOTE: Windows 2000/NT:   The default security provider is NTLM.
            /// </summary>
            LOGON32_PROVIDER_DEFAULT = 0,
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Returns a string in the native language from the dictionary.
        /// </summary>
        /// <param name="keyName">The key name.</param>
        /// <returns>The translated text.</returns>
        private string GetCaption(string keyName)
        {
            return langRes.GetString(keyName, Thread.CurrentThread.CurrentCulture);
        }

        /// <summary>
        /// Updates the status labels with given text and text color.
        /// </summary>
        /// <param name="label">The Label component.</param>
        /// <param name="text">The text as string.</param>
        /// <param name="color">The Color object.</param>
        private delegate void SetStatusLabelCallback(Label label, string text, Color color);
        private void SetStatusLabel(Label label, string text, Color color)
        {
            if (label.InvokeRequired)
            {
                SetStatusLabelCallback ssc = new SetStatusLabelCallback(SetStatusLabel);

                this.Invoke(ssc, label, text, color);
            }
            else
            {
                label.Text = text;
                label.ForeColor = color;
            }
        }

        #endregion

        #region SSH

        /// <summary>
        /// Connect to SSH-Server and open TCP tunnel.
        /// </summary>
        /// <returns>Returns true on success, else false.</returns>
        private bool BuildTunnel()
        {
            JSch jsch = new JSch();

            try
            {
                // Set authentication info
                sshSession = jsch.getSession(textBoxUsername.Text, config.SupportHost, config.SupportPort);
                sshSession.setPassword(textBoxPassword.Text);
                // Build configuration and disable host key check
                Hashtable configs = new Hashtable();
                configs.put("StrictHostKeyChecking", "no");
                sshSession.setConfig(configs);
                // Try to connect
                sshSession.connect();
                // Create TCP tunnel
                sshSession.setPortForwardingR(config.FwdRemotePort, config.FwdLocalHost, config.FwdLocalPort);

                sessionActive = true;
                return true;
            }
            catch (JSchException)
            {
                MessageBox.Show(GetCaption("serverLoginError"), GetCaption("loginFailed"),
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            return false;
        }

        /// <summary>
        /// Closes the SSH session.
        /// </summary>
        private void BreakTunnel()
        {
            if (sshSession != null)
                if (sshSession.isConnected())
                    sshSession.disconnect();

            sessionActive = false;
        }

        #endregion

        #region Modules

        /// <summary>
        /// Checks if the terminal server service is active and running.
        /// </summary>
        /// <returns></returns>
        private bool CheckTermService()
        {
            ServiceController rdpSrv = new ServiceController("TermService");
            bool running = false;

            switch (rdpSrv.Status)
            {
                case ServiceControllerStatus.Running:
                    SetStatusLabel(labelTSServ, GetCaption("running"), Color.Green);
                    running = true;
                    break;
                case ServiceControllerStatus.Stopped:
                    SetStatusLabel(labelTSServ, GetCaption("stopped"), Color.Red);
                    break;
                case ServiceControllerStatus.Paused:
                    SetStatusLabel(labelTSServ, GetCaption("paused"), Color.Yellow);
                    break;
                case ServiceControllerStatus.StopPending:
                    SetStatusLabel(labelTSServ, GetCaption("stopping"), Color.Yellow);
                    break;
                case ServiceControllerStatus.StartPending:
                    SetStatusLabel(labelTSServ, GetCaption("starting"), Color.Yellow);
                    break;
                default:
                    SetStatusLabel(labelTSServ, GetCaption("statusChanging"), Color.Yellow);
                    break;
            }

            return running;
        }

        /// <summary>
        /// Tries to connect to the support server.
        /// </summary>
        /// <returns>Returns true on success, else false.</returns>
        private bool SupportServerAvailable
        {
            get
            {
                bool success = false;

                using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    try
                    {
                        var waitEvent = new ManualResetEvent(false);

                        // This gets called if the connection succeeded within the timeout
                        AsyncCallback connectFinished = delegate(IAsyncResult target)
                        {
                            success = true;
                            waitEvent.Set();
                        };

                        // Try to connect asynchronous
                        socket.BeginConnect(config.SupportHost,
                            config.SupportPort,
                            connectFinished,
                            null);

                        // Wait for callback to succeed
                        waitEvent.WaitOne(config.Timeout);

                        //socket.Connect(config.SupportHost, config.SupportPort);

                        return success;
                    }
                    catch (SocketException) { return success; }
                    finally { socket.Close(); }
                }
            }
        }

        /// <summary>
        /// Checks if Terminal Server is running.
        /// </summary>
        private bool IsRdpBound
        {
            get
            {
                IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
                IPEndPoint[] ipEndPoints = ipGlobalProperties.GetActiveTcpListeners();

                foreach (IPEndPoint ipendp in ipEndPoints)
                {
                    if (ipendp.Port == config.FwdLocalPort)
                        return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Checks if an open RDP session exists.
        /// </summary>
        private bool IsRdpUserConnected
        {
            get
            {
                IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
                TcpConnectionInformation[] tcpConnectionInfo = ipGlobalProperties.GetActiveTcpConnections();

                foreach (TcpConnectionInformation tcpInfo in tcpConnectionInfo)
                {
                    if (tcpInfo.LocalEndPoint.Port == config.FwdLocalPort)
                        return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Gets or sets the registry value to allow remote connections.
        /// </summary>
        private int fDenyTSConnections
        {
            set
            {
                RegistryKey fDenyTSConnections =
                    Registry.LocalMachine.OpenSubKey(rdpRegKey, true);
                fDenyTSConnections.SetValue("fDenyTSConnections", value, RegistryValueKind.DWord);
                fDenyTSConnections.Close();
            }

            get
            {
                RegistryKey fDenyTSConnections =
                    Registry.LocalMachine.OpenSubKey(rdpRegKey);
                int value = int.Parse(fDenyTSConnections.GetValue("fDenyTSConnections").ToString());
                fDenyTSConnections.Close();
                return value;
            }
        }

        /// <summary>
        /// Checks if the current user has a password set.
        /// </summary>
        private bool PasswordRequired
        {
            get
            {
                IntPtr phToken;

                // http://www.pinvoke.net/default.aspx/advapi32/LogonUser.html
                bool loggedIn = LogonUser(Environment.UserName,
                    null,
                    "",
                    (int)LogonType.LOGON32_LOGON_INTERACTIVE,
                    (int)LogonProvider.LOGON32_PROVIDER_DEFAULT,
                    out phToken);

                int error = Marshal.GetLastWin32Error();

                if (phToken != IntPtr.Zero)
                    // http://www.pinvoke.net/default.aspx/kernel32/CloseHandle.html
                    CloseHandle(phToken);

                // 1327 = empty password
                if (loggedIn || error == ERROR_ACCOUNT_RESTRICTION)
                    return false;
                else
                    return true;
            }
        }

        /// <summary>
        /// Enables all terminal server services and opens firewall rules.
        /// </summary>
        private void EnableTerminalServer()
        {
            uint retries = 0;

            while (retries++ < 3)
            {
                try
                {
                    // Start Terminal Service
                    ServiceController rdpSrv = new ServiceController("TermService");
                    if (rdpSrv.Status != ServiceControllerStatus.Running)
                        rdpSrv.Start();

                    Process netShell = new Process();

                    // Basic start info
                    netShell.StartInfo.FileName = "netsh";
                    netShell.StartInfo.UseShellExecute = false;
                    netShell.StartInfo.CreateNoWindow = true;
                    netShell.StartInfo.RedirectStandardOutput = true;

                    string fwCmd = string.Empty;
                    // Since Windows Vista the "firewall" command is deprecated
                    if (Environment.OSVersion.Version.Major <= 5)
                        fwCmd = "firewall";
                    else
                        fwCmd = "advfirewall firewall";

                    // Enable remote administration
                    netShell.StartInfo.Arguments =
                        string.Format("{0} set service remoteadmin enable", fwCmd);
                    netShell.Start();
                    netShell.WaitForExit();
                    //textBlockStdout.Text = netShell.StandardOutput.ReadToEnd();

                    // Enable remote assistance and remote desktop
                    netShell.StartInfo.Arguments =
                        string.Format("{0} set service remotedesktop enable", fwCmd);
                    netShell.Start();
                    netShell.WaitForExit();
                    //textBlockStdout.Text = netShell.StandardOutput.ReadToEnd();

                    // Enable the Remote Desktop Firewall Rule
                    netShell.StartInfo.Arguments =
                        string.Format("{0} set service type = remotedesktop mode = enable", fwCmd);
                    netShell.Start();
                    netShell.WaitForExit();

                    // Allow incoming connections
                    fDenyTSConnections = 0;

                    // Check if server is listening
                    if (!IsRdpBound)
                    {
                        // This isn't really the most intelligent solution but simple
                        Thread.Sleep(300);
                        continue;
                    }

                    // Return if no error
                    return;
                }
                catch { continue; }
            }

            MessageBox.Show(GetCaption("termStartFailedMsg"), GetCaption("termStartFailed"),
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Runs a bunch of tests to validate all necessary services, ports and so on.
        /// </summary>
        /// <returns></returns>
        private bool CheckEnvironment()
        {
            bool success = true;

            // Check if terminal server service is up
            success = CheckTermService();

            // Check registry value if connections are allowed
            if (fDenyTSConnections == 0)
                SetStatusLabel(labelRConAllowed, GetCaption("yes"), Color.Green);
            else
            {
                SetStatusLabel(labelRConAllowed, GetCaption("no"), Color.Red);
                success = false;
            }

            // Check if TCP server is listening
            if (IsRdpBound)
                SetStatusLabel(labelTSSock, GetCaption("bound"), Color.Green);
            else
            {
                success = false;
                SetStatusLabel(labelTSSock, GetCaption("unbound"), Color.Red);
            }

            // Check if the support (ssh) server is reachable
            if (SupportServerAvailable)
            {
                SetStatusLabel(labelSupSrvAvail, GetCaption("yes"), Color.Green);
                groupBoxSshAccount.Enabled = true;
            }
            else
            {
                SetStatusLabel(labelSupSrvAvail, GetCaption("no"), Color.Red);
                SetStatusLabel(labelEnvStatus, GetCaption("srvNotFound"), Color.Red);
                buttonSwitchOn.Visible = false;
                success = false;
                groupBoxSshAccount.Enabled = false;
                srvAddrDiag.textBoxHost.Text = config.SupportHost;
                srvAddrDiag.textBoxPort.Text = config.SupportPort.ToString();

                if (srvAddrDiag.ShowDialog() == DialogResult.OK)
                {
                    SetStatusLabel(labelEnvStatus, GetCaption("srvChecking"), Color.Blue);
                    config.SupportHost = srvAddrDiag.textBoxHost.Text;
                    config.SupportPort = int.Parse(srvAddrDiag.textBoxPort.Text);

                    success = CheckEnvironment();
                }
            }

            // Check if current user has password set
            bool winAccOk = PasswordRequired;

            // Change controls to show user what to do now
            if (winAccOk)
            {
                SetStatusLabel(labelWinPwSet, GetCaption("yes"), Color.Green);
                groupBoxWinAccount.Enabled = false;
            }
            else
            {
                SetStatusLabel(labelWinPwSet, GetCaption("no"), Color.Red);
                SetStatusLabel(labelEnvStatus, GetCaption("setWinPw"), Color.Red);
                buttonSwitchOn.Visible = false;
                textBoxWinPasswd.Focus();
                groupBoxWinAccount.Enabled = true;
                groupBoxSshAccount.Enabled = false;
            }

            if (winAccOk)
            {
                // Windows account and support server are ok, now validate the other checks...
                if (success)
                {
                    SetStatusLabel(labelEnvStatus, GetCaption("allFine"), Color.Green);
                    buttonSwitchOn.Enabled = false;
                    buttonSwitchOn.Visible = false;
                    groupBoxSshAccount.Enabled = true;
                }
                else
                {
                    SetStatusLabel(labelEnvStatus, GetCaption("componentError"), Color.Red);
                    buttonSwitchOn.Enabled = true;
                    buttonSwitchOn.Visible = true;
                    groupBoxSshAccount.Enabled = false;
                }
            }

            return success;
        }

        #endregion

        #region Events

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (sessionActive)
            {
                MessageBox.Show(GetCaption("stillConnected"), GetCaption("disconnectFirst"),
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                e.Cancel = true;
                return;
            }

            config.Username = textBoxUsername.Text;
            config.Password = textBoxPassword.Text;
            config.Save();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            labelWinUserName.Text = Environment.UserName;
            textBoxUsername.Text = config.Username;
            textBoxPassword.Text = config.Password;

            CheckEnvironment();
        }

        private void buttonSwitchOn_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            (sender as Button).Enabled = false;
            groupBoxWinAccount.Enabled = false;
            groupBoxSshAccount.Enabled = false;

            backgroundWorkerFix.RunWorkerAsync();
        }

        private void backgroundWorkerFix_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            EnableTerminalServer();
        }

        private void backgroundWorkerFix_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            CheckEnvironment();
            Cursor = Cursors.Default;
        }

        private void checkBoxConnect_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as CheckBox).Checked && !sessionActive)
            {
                if (string.IsNullOrEmpty(textBoxUsername.Text) || string.IsNullOrEmpty(textBoxPassword.Text))
                {
                    (sender as CheckBox).Checked = false;
                    MessageBox.Show(GetCaption("validNamePw"), GetCaption("loginFault"),
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    if (!BuildTunnel())
                    {
                        (sender as CheckBox).Checked = false;
                        SetStatusLabel(labelConnectionStat,
                            GetCaption("connectionFailed"),
                            Color.Red);
                        return;
                    }
                    else
                    {
                        (sender as CheckBox).Text = GetCaption("disconnect");
                        SetStatusLabel(labelConnectionStat,
                            GetCaption("connectionOk"),
                            Color.Green);
                    }
                }

            }

            if (!(sender as CheckBox).Checked && sessionActive)
            {
                if (IsRdpUserConnected)
                {
                    (sender as CheckBox).Checked = true;
                    if (MessageBox.Show(GetCaption("supporterConnected"), GetCaption("activeSession"),
                            MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                        return;
                }

                BreakTunnel();
                (sender as CheckBox).Checked = false;
                (sender as CheckBox).Text = GetCaption("connect");
                SetStatusLabel(labelConnectionStat,
                    GetCaption("disconnectOk"),
                    Color.Blue);
            }
        }

        private void buttonSenWinPwd_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            groupBoxWinAccount.Enabled = false;
            backgroundWorkerSetWinPw.RunWorkerAsync();
        }

        private void backgroundWorkerSetWinPw_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            string path = string.Format("WinNT://{0}/{1}", Environment.MachineName, Environment.UserName);

            try
            {
                using (DirectoryEntry userEntry = new DirectoryEntry(path))
                {
                    object[] password = new object[] { textBoxWinPasswd.Text };
                    object ret = userEntry.Invoke("SetPassword", password);
                    userEntry.CommitChanges();
                }
            }
            catch
            {
                MessageBox.Show(GetCaption("winPwSetFailedMsg"), GetCaption("winPwSetFailed"), 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void backgroundWorkerSetWinPw_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            CheckEnvironment();
            Cursor = Cursors.Default;
        }

        private void textBoxPassword_Enter(object sender, EventArgs e)
        {
            (sender as TextBox).SelectAll();
        }

        private void MainWindow_KeyUp(object sender, KeyEventArgs e)
        {
            bool ok = true;

            if (e.KeyCode == Keys.F10)
            {
                groupBoxSshAccount.Enabled = false;

                srvAddrDiag.textBoxHost.Text = config.SupportHost;
                srvAddrDiag.textBoxPort.Text = config.SupportPort.ToString();

                if (srvAddrDiag.ShowDialog() == DialogResult.OK)
                {
                    config.SupportHost = srvAddrDiag.textBoxHost.Text;
                    config.SupportPort = int.Parse(srvAddrDiag.textBoxPort.Text);

                    ok = CheckEnvironment();
                }

                if (ok)
                {
                    groupBoxSshAccount.Enabled = true;
                    textBoxUsername.Focus();
                }
            }
        }

        private void MainWindow_Shown(object sender, EventArgs e)
        {
            SplashScreen.CloseForm();
        }

        #endregion
    }
}
