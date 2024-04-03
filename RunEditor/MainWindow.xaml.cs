
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Input;
using static System.Net.Mime.MediaTypeNames;
namespace RunEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct STARTUPINFO
        {
            public int cb;
            public string lpReserved;
            public string lpDesktop;
            public string lpTitle;
            public uint dwX;
            public uint dwY;
            public uint dwXSize;
            public uint dwYSize;
            public uint dwXCountChars;
            public uint dwYCountChars;
            public uint dwFillAttribute;
            public uint dwFlags;
            public short wShowWindow;
            public short cbReserved2;
            public IntPtr lpReserved2;
            public IntPtr hStdInput;
            public IntPtr hStdOutput;
            public IntPtr hStdError;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct PROCESS_INFORMATION
        {
            public IntPtr hProcess;
            public IntPtr hThread;
            public int dwProcessId;
            public int dwThreadId;
        }
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool CreateProcess(
    string lpApplicationName,
    string lpCommandLine,
    IntPtr lpProcessAttributes,
    IntPtr lpThreadAttributes,
    bool bInheritHandles,
    uint dwCreationFlags,
    IntPtr lpEnvironment,
    string lpCurrentDirectory,
    [In] ref STARTUPINFO lpStartupInfo,
    out PROCESS_INFORMATION lpProcessInformation
);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool CloseHandle(IntPtr hObject);

        public MainWindow()
        {
            InitializeComponent();
        }

        private void RunProcess_Click(object sender, RoutedEventArgs e)
        {
            string content = textBoxContent.Text;
            STARTUPINFO si = new STARTUPINFO();
            si.cb = Marshal.SizeOf(si);
            si.dwFlags = 0x00000001; // STARTF_USESHOWWINDOW
            si.wShowWindow = 1; // SW_SHOWNORMAL

            PROCESS_INFORMATION pi;

            if (!CreateProcess(null, content, IntPtr.Zero, IntPtr.Zero, false, 0x00000010, // CREATE_NEW_CONSOLE
                IntPtr.Zero, null, ref si, out pi))
            {
                MessageBox.Show("failed to execute command");
            }
            CloseHandle(pi.hProcess);
            CloseHandle(pi.hThread);
        }
        private void Unicode_Click(object sender, RoutedEventArgs e)
        {
            string content = textBoxContent.Text;
            StringBuilder unicodeText = new StringBuilder();

            foreach (char c in content)
            {
                unicodeText.Append($"U+{((int)c):X4} ");
            }

            // Unicode表記をテキストボックスに追記
            textBoxContent.AppendText("\n"+unicodeText.ToString());
        }
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && e.Key == Key.Enter)
            {
                RunProcess_Click(sender, e);
            }
            else if ((Keyboard.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt && e.Key == Key.Enter){

            }
        }
    }
}
