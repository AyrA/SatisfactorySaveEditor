using SatisfactorySaveEditor.Audio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace SatisfactorySaveEditor
{
    public partial class frmAudioExtract : Form
    {
        const string PAK_FILE = "FactoryGame-WindowsNoEditor.pak";

        private volatile bool Scan = false;
        private volatile bool Found = false;
        private volatile int ThreadCount = 0;
        private volatile string LastPath;
        private object mutex;
        private string DefaultTitle;
        private string AudioDir;
        private Process Player;
        private int AudioIndex = 0;

        public frmAudioExtract()
        {
            AudioDir = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Audio");
            var Files = new string[] {
                @"%ProgramFiles%\Epic Games\SatisfactoryExperimental\FactoryGame\Content\Paks\" + PAK_FILE,
                @"%ProgramFiles%\Epic Games\SatisfactoryEarlyAccess\FactoryGame\Content\Paks\" + PAK_FILE
            }
            .Select(m => Environment.ExpandEnvironmentVariables(m))
            .ToArray();
            InitializeComponent();
            DefaultTitle = Text;
            foreach (var F in Files)
            {
                if (File.Exists(F))
                {
                    Log.Write("AudioExtract: Found pak file in default path: {0}", F);
                    tbResourceFile.Text = F;
                    btnScan.Enabled = true;
                    break;
                }
            }
            Tools.SetupKeyHandlers(this);
            //Resize only horizontally
            MaximumSize = new System.Drawing.Size(int.MaxValue, MinimumSize.Height);
        }

        private void frmAudioExtract_Load(object sender, EventArgs e)
        {
            if (!LoadAudioPlayer())
            {
                Log.Write("AudioExtract: No audio files extracted yet");
                if (!Program.HasQuickPlay)
                {
                    gbAudio.Text = "QuickPlay.exe is missing! Press [F1] to see how to obtain it manually.";
                }
                if (string.IsNullOrEmpty(tbResourceFile.Text))
                {
                    Log.Write("AudioExtract: No pak file found. Presenting user with options");
                    switch (MessageBox.Show(@"The required game file was not found in the default locations.
Do you want to scan all hard drives drives for the file?
It will not scan removable devices and network drives.
Select [YES] if Satisfactory is not installed in the default location.
Select [No] if you know where the game is and want to select the file manually
Select [Cancel] if you did not yet install the game.", "Game File not found", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information))
                    {
                        case DialogResult.Yes:
                            Log.Write("AudioExtract: Scanning entire system");
                            ScanSystem(DriveInfo.GetDrives().Where(m => m.IsReady && m.DriveType == DriveType.Fixed).Select(m => m.RootDirectory.FullName).ToArray());
                            break;
                        case DialogResult.No:
                            Log.Write("AudioExtract: Manual user selection");
                            break;
                        case DialogResult.Cancel:
                            Log.Write("AudioExtract: Closing form");
                            Close();
                            break;
                    }
                }
            }
            else
            {
                Log.Write("AudioExtract: Audio files present. Disabling scan controls");
                btnScan.Enabled = btnSelectFile.Enabled = tbResourceFile.Enabled = false;
            }
        }

        private void frmAudioExtract_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Scan && e.CloseReason == CloseReason.UserClosing)
            {
                Log.Write("AudioExtract: User tried to close on existing scan");
                if (MessageBox.Show("A file scan is currently in progress. Abort the scan and close the window?", "File scan in progress", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    Log.Write("AudioExtract: User chose to continue the scan");
                    e.Cancel = true;
                }
                else
                {
                    Log.Write("AudioExtract: User chose to abort the scan");
                }
            }
            if (!e.Cancel)
            {
                Scan = false;
            }
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            Log.Write("AudioExtract: Manual user selection");
            OFD.Filter = $"Satisfactory game file|{PAK_FILE}|Unreal Engine pak files|*.pak|All files|*.*";
            if (OFD.ShowDialog() == DialogResult.OK)
            {
                Log.Write("AudioExtract: User selected {0}", OFD.FileName);
                tbResourceFile.Text = OFD.FileName;
                btnScan.Enabled = true;
            }
            else
            {
                Log.Write("AudioExtract: User cancelled file selection");
            }
        }

        private void frmAudioExtract_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            Tools.ShowHelp(GetType().Name);
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(AudioDir))
            {
                try
                {
                    Directory.CreateDirectory(AudioDir);
                }
                catch (Exception ex)
                {
                    Log.Write("AudioExtract: Unable to create Audio directory");
                    Log.Write(ex);
                    MessageBox.Show($"Unable to create the output directory. Reason: {ex.Message}", "Can't create directory", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            FileStream FS;
            try
            {
                FS = File.OpenRead(tbResourceFile.Text);
            }
            catch (Exception ex)
            {
                Log.Write("AudioExtract: Unable to open pak file");
                Log.Write(ex);
                MessageBox.Show($"Unable to open the specified file. Reason: {ex.Message}", "Can't open file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //Disable controls
            btnScan.Enabled = false;
            btnSelectFile.Enabled = false;
            tbResourceFile.Enabled = false;

            Log.Write("AudioExtract: Begin audio extraction of {0}", tbResourceFile.Text);
            //Begin audio extraction
            WaveFinder.FindAudio(FS);
            pbFilePos.Maximum = 100;
            Thread T = new Thread(delegate ()
            {
                while (WaveFinder.IsScanning)
                {
                    Invoke((MethodInvoker)delegate
                    {
                        pbFilePos.Value = (int)(FS.Position * 100 / FS.Length);
                    });
                    Thread.Sleep(500);
                }
                Log.Write("AudioExtract: Audio extraction complete. Found {0} files", WaveFinder.WaveFiles.Length);
                using (FS)
                {
                    int Counter = 0;

                    //Order files descending but PCM first, then OGG
                    var FoundFiles = WaveFinder.WaveFiles
                    .Where(m => m.Type == WaveFileType.PCM)
                    .OrderByDescending(m => m.Header.DataSize)
                    .Concat(
                        WaveFinder.WaveFiles
                        .Where(m => m.Type == WaveFileType.OGG)
                        .OrderByDescending(m => m.Header.DataSize))
                    .ToArray();

                    Log.Write("AudioExtract: Exporting {0} files", FoundFiles.Length);

                    Invoke((MethodInvoker)delegate
                    {
                        pbFilePos.Value = 0;
                        pbFilePos.Maximum = FoundFiles.Length;
                    });

                    foreach (var F in FoundFiles)
                    {
                        var FileName = Path.Combine(AudioDir, string.Format("{0:0000}.{1}", ++Counter, F.Type == WaveFileType.PCM ? "wav" : "ogg"));
                        Invoke((MethodInvoker)delegate
                        {
                            pbFilePos.Value = Counter;
                        });
                        FileStream OUT;
                        try
                        {
                            OUT = File.Create(FileName);
                        }
                        catch (Exception ex)
                        {
                            Log.Write("AudioExtract: Unable to export {0}", FileName);
                            Log.Write(ex);
                            //Unable to create output file, skip
                            continue;
                        }
                        using (OUT)
                        {
                            FS.Seek(F.Header.DataOffset, SeekOrigin.Begin);
                            if (F.Type == WaveFileType.PCM)
                            {
                                //Fix values before writing new Header
                                //NOTE: Set different values here for different games
                                F.Header.AudioFormat = 1; //PCM
                                F.Header.BitsPerSample = 16;
                                F.Header.SampleRate = 44100;
                                //Fix computed values
                                F.Header.BlockAlign = (ushort)(F.Header.ChannelCount * F.Header.BitsPerSample / 8);
                                F.Header.ByteRate = F.Header.SampleRate * F.Header.ChannelCount * F.Header.BitsPerSample / 8;
                                F.Header.Write(OUT);
                            }
                            else
                            {
                                //OGG Doesn't needs the RIFF header, just write data
                            }
                            byte[] Data = new byte[F.Header.DataSize];
                            FS.Read(Data, 0, Data.Length);
                            OUT.Write(Data, 0, Data.Length);
                            Log.Write("AudioExtract: Exported {0} file", F.Type);
                        }
                    }
                }
                Invoke((MethodInvoker)delegate
                {
                    pbFilePos.Value = 0;
                    pbFilePos.Maximum = 100;
                    LoadAudioPlayer();
                    //Enable controls again
                    btnScan.Enabled = true;
                    btnSelectFile.Enabled = true;
                    tbResourceFile.Enabled = true;
                });
            });
            T.Start();
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            var Files = Directory.GetFiles(AudioDir);
            if (AudioIndex > 0)
            {
                Play(Files[--AudioIndex]);
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            var Files = Directory.GetFiles(AudioDir);
            if (Files.Length > 0)
            {
                Play(Files[AudioIndex >= 0 && AudioIndex < Files.Length ? AudioIndex : 0]);
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (!Player.HasExited)
            {
                Log.Write("AudioExtract: Closing QuickPlay");
                Player.CloseMainWindow();
                Player.WaitForExit();
            }
            Player.Dispose();
            Player = null;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            var Files = Directory.GetFiles(AudioDir);
            if (AudioIndex < Files.Length - 1)
            {
                Play(Files[++AudioIndex]);
            }
        }

        private void btnOpenDir_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(AudioDir))
            {
                Log.Write("AudioExtract: Browsing {0}", AudioDir);
                Process.Start(AudioDir);
            }
        }

        private bool LoadAudioPlayer()
        {
            if (Directory.Exists(AudioDir))
            {
                if (Program.HasQuickPlay)
                {
                    var Files = Directory.GetFiles(AudioDir);
                    if (Files.Length > 0)
                    {
                        AudioIndex = 0;
                        Play(Files[0]);
                    }
                    gbAudio.Enabled = Files.Length > 0;
                    return Files.Length > 0;
                }
                else
                {
                    Log.Write("AudioExtract: QuickPlay not present");
                }
            }
            gbAudio.Enabled = false;
            return false;
        }

        private void Play(string AudioFile)
        {
            if (Program.HasQuickPlay)
            {
                Log.Write("AudioExtract: Play {0}", AudioFile);
                if (Player == null)
                {
                    Player = Process.Start("QuickPlay.exe", $"\"{AudioFile}\"");
                }
                else
                {
                    Process.Start("QuickPlay.exe", $"\"{AudioFile}\"");
                }
            }
        }

        private void ScanSystem(string[] Root)
        {
            Log.Write("AudioExtract: Initialize system scanner");
            mutex = new object();
            Scan = true;
            Found = false;
            tbResourceFile.Enabled = false;
            btnScan.Enabled = false;
            btnSelectFile.Enabled = false;
            ThreadCount = Root.Length;
            SetText($"Scanning {ThreadCount} Drives...");
            var d = (ParameterizedThreadStart)delegate (object o)
            {
                var Dirs = new Stack<string>();
                Dirs.Push(o.ToString());

                Log.Write("AudioExtract: System scanner {0} start", o);

                while (Dirs.Count > 0 && Scan)
                {
                    var Current = Dirs.Pop();
                    LastPath = Current;
                    try
                    {
                        var Pak = Path.Combine(Current, PAK_FILE);
                        if (File.Exists(Pak))
                        {
                            Log.Write("AudioExtract: System scanner {0} found {1}", o, Pak);
                            lock (mutex)
                            {
                                Scan = false;
                                Found = true;
                                Invoke((MethodInvoker)delegate
                                {
                                    tbResourceFile.Enabled = true;
                                    btnScan.Enabled = true;
                                    btnSelectFile.Enabled = true;
                                    OFD.FileName = Pak;
                                    tbResourceFile.Text = Pak;
                                });
                            }
                        }
                    }
                    catch
                    {
                        //Ignore, probably no access
                    }
                    try
                    {
                        foreach (var SubDir in Directory.EnumerateDirectories(Current))
                        {
                            Dirs.Push(SubDir);
                        }
                    }
                    catch
                    {
                        Log.Write("AudioExtract: System scanner {0} couldn't enumerate {1}", o, Current);
                        //Ignore, probably no access
                    }
                }
                lock (mutex)
                {
                    if (--ThreadCount == 0)
                    {
                        Invoke((MethodInvoker)delegate
                        {
                            SetText(null);
                            if (!Found)
                            {
                                MessageBox.Show($"Unable to find {PAK_FILE} on any of your harddrives.", "File scan", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            }
                        });
                    }
                    else
                    {
                        SetText($"Scanning {ThreadCount} Drives...");
                    }
                    Log.Write("AudioExtract: System scanner {0} ended", o);
                }
            };
            foreach (var R in Root)
            {
                var T = new Thread(d);
                T.IsBackground = true;
                T.Start(R);
            }
            Thread TProgress = new Thread(delegate ()
            {
                while (Scan)
                {
                    Invoke((MethodInvoker)delegate
                    {
                        lock (mutex)
                        {
                            tbResourceFile.Text = LastPath;
                        }
                    });
                    Thread.Sleep(500);
                }
            });
            TProgress.IsBackground = true;
            TProgress.Start();
        }

        private void SetText(string NewTitle)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { SetText(NewTitle); });
            }
            else
            {
                if (string.IsNullOrEmpty(NewTitle))
                {
                    Text = DefaultTitle;
                }
                else
                {
                    Text = $"{DefaultTitle} [{NewTitle}]";
                }
            }
        }
    }
}
