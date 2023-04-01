using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using NAudio.CoreAudioApi;
using NAudio.Wave;

namespace SoundTyperWinform
{
    public partial class Form1 : Form
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;

        private readonly List<MMDevice> devices;
        private readonly List<WaveOutEvent> players;
        private readonly LowLevelKeyboardProc keyboardProc;

        private IntPtr hookId = IntPtr.Zero;

        private readonly WaveMixerStream32 mixer;

        public Form1()
        {
            InitializeComponent();
            devices = new List<MMDevice>();
            var enumerator = new MMDeviceEnumerator();
            var devicesCollection = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);
            foreach (var device in devicesCollection)
            {
                devices.Add(device);
                audioDeviceComboBox.Items.Add(device.FriendlyName);
            }

            // Load the settings from the application settings file
            if (Properties.Settings.Default.AudioDevice != -1)
            {
                audioDeviceComboBox.SelectedIndex = Properties.Settings.Default.AudioDevice;
            }

            if (!string.IsNullOrEmpty(Properties.Settings.Default.SoundFilePath))
            {
                textBoxFilePath.Text = Properties.Settings.Default.SoundFilePath;
            }

            players = new List<WaveOutEvent>();
            for (int i = 0; i < 5; i++) // create 5 instances of WaveOutEvent
            {
                players.Add(new WaveOutEvent());
            }

            keyboardProc = HookCallback;
        }


        private IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (var curProcess = System.Diagnostics.Process.GetCurrentProcess())
            using (var curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            try
            {
                if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
                {
                    var keyCode = Marshal.ReadInt32(lParam);
                    if (IsAlphanumeric(keyCode) || IsNumber(keyCode) || IsPunctuation(keyCode))
                    {
                        string activeWindow = GetActiveWindowTitle();
                        ActiveWindowTextBox.Text = activeWindow;
                        if (GetActiveWindowTitle().ToLower() == "eos")
                        {
                            var deviceIndex = audioDeviceComboBox.SelectedIndex;
                            if (deviceIndex >= 0)
                            {
                                var deviceId = devices[deviceIndex].ID;
                                var audioFile = new AudioFileReader(textBoxFilePath.Text);
                                audioFile.Volume = 1f; // set the volume to maximum (1.0)
                                var player = players.Find(p => !p.PlaybackState.Equals(PlaybackState.Playing)); // find an available player
                                if (player != null)
                                {
                                    player.DeviceNumber = deviceIndex;
                                    player.Init(audioFile);
                                    player.Play();
                                }
                            }
                        }
                    }
                }
                return CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
            }
            catch
            {
                return CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
            }
        }

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int GetWindowText(IntPtr hWnd, System.Text.StringBuilder lpString, int nMaxCount);

        private string GetActiveWindowTitle()
        {
            var hwnd = GetForegroundWindow();
            var sb = new StringBuilder(256);
            if (GetWindowText(hwnd, sb, 256) > 0)
            {
                return sb.ToString();
            }
            return null;
        }

        private bool IsAlphanumeric(int vkCode)
        {
            return ((vkCode >= 0x41 && vkCode <= 0x5A) || (vkCode >= 0x61 && vkCode <= 0x7A))
                && !(vkCode >= 0x70 && vkCode <= 0x87);
        }

        private bool IsNumber(int vkCode)
        {
            return vkCode >= 0x30 && vkCode <= 0x39;
        }

        private bool IsPunctuation(int keyCode)
        {
            var key = (Keys)keyCode;
            return key == Keys.Oemcomma || key == Keys.OemPeriod || key == Keys.OemQuestion ||
                   key == Keys.Oemtilde || key == Keys.OemOpenBrackets || key == Keys.OemCloseBrackets ||
                   key == Keys.OemMinus || key == Keys.Oemplus || key == Keys.OemBackslash ||
                   key == Keys.OemSemicolon || key == Keys.OemQuotes || key == Keys.OemPipe;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            hookId = SetHook(keyboardProc);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Save the selected values to the application settings file
            if (audioDeviceComboBox.SelectedIndex >= 0)
            {
                Properties.Settings.Default.AudioDevice = audioDeviceComboBox.SelectedIndex;
            }
            else
            {
                Properties.Settings.Default.AudioDevice = -1;
            }

            if (!string.IsNullOrEmpty(textBoxFilePath.Text))
            {
                Properties.Settings.Default.SoundFilePath = textBoxFilePath.Text;
            }
            else
            {
                Properties.Settings.Default.SoundFilePath = string.Empty;
            }

            // Save the settings
            Properties.Settings.Default.Save();

            UnhookWindowsHookEx(hookId);
            foreach (var player in players)
            {
                player?.Dispose();
            }
        }

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            // Show a file dialog to select the audio file
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Wave files (*.wav)|*.wav";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                textBoxFilePath.Text = openFileDialog.FileName;
            }
        }

        private void audioDeviceComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (audioDeviceComboBox.SelectedIndex >= 0)
            {
                Properties.Settings.Default.AudioDevice = audioDeviceComboBox.SelectedIndex;
            }
            else
            {
                Properties.Settings.Default.AudioDevice = -1;
            }
        }

        private void textBoxFilePath_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBoxFilePath.Text))
            {
                Properties.Settings.Default.SoundFilePath = textBoxFilePath.Text;
            }
            else
            {
                Properties.Settings.Default.SoundFilePath = string.Empty;
            }
        }
    }
}
