using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Speech.Synthesis;
using System.Speech.Recognition;
using System.IO;

namespace wSpeech
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            David.IsChecked = true;
            PauseButton.Visibility = Visibility.Hidden;
            ResumeButton.Visibility = Visibility.Hidden;
        }
        SpeechSynthesizer Speaker = new SpeechSynthesizer();
        SpeechRecognitionEngine Recognizer = new SpeechRecognitionEngine();
        Grammar dictationGrammar = new DictationGrammar();
        
        
        
        OpenFileDialog FileDialog = new OpenFileDialog
        {
            Title = "Open a Text File",
            InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            Filter = "Text File (*.txt)|*.txt|All Files (*.*)|*.*",
            RestoreDirectory = true
        };
        SaveFileDialog SaveDialog = new SaveFileDialog
        {
            Title = "Save to Audio File",
            InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic),
            Filter = "Wave File (*.wav)|*.wav",
            RestoreDirectory = true
        };
       
        public void Play(object sender, EventArgs e)
        {
            Speaker.GetInstalledVoices();
                if (David.IsChecked == true)
                {
                    Speaker.SelectVoice("Microsoft David Desktop");
                }
                else if (Zira.IsChecked == true)
                {
                    Speaker.SelectVoice("Microsoft Zira Desktop");
                }
                else if (Hazel.IsChecked == true)
                {
                    Speaker.SelectVoice("Microsoft Hazel Desktop");
                }
                Speaker.Rate = Convert.ToInt32(SpeedControl.Value * 10) / 25;
                Speaker.Volume = Convert.ToInt32(VolumeControl.Value) * 10;
                Speaker.SpeakAsync(TextBox.Text);
                PauseButton.Visibility = Visibility.Visible;
                PlayButton.Visibility = Visibility.Hidden;
                Speaker.SpeakCompleted += Speaker_SpeakCompleted;
        }

        void Pause(Object sender, EventArgs e)
        {
            Speaker.Pause();
            ResumeButton.Visibility = Visibility.Visible;
            PauseButton.Visibility = Visibility.Hidden;
        }

        void Resume(object sender, EventArgs e)
        {
            Speaker.Resume();
            PauseButton.Visibility = Visibility.Visible;
            ResumeButton.Visibility = Visibility.Hidden;
        }

        public void Stop(object sender, EventArgs e)
        {
            Speaker.SpeakAsyncCancelAll();
            PlayButton.Visibility = Visibility.Visible;
            PauseButton.Visibility = Visibility.Hidden;
            ResumeButton.Visibility = Visibility.Hidden;
        }

        private void Speaker_SpeakCompleted(object sender, SpeakCompletedEventArgs e)
        {
            PlayButton.Visibility = Visibility.Visible;
            PauseButton.Visibility = Visibility.Hidden;
            ResumeButton.Visibility = Visibility.Hidden;
        }

        private void Copy(object sender, EventArgs e)
        {
                System.Windows.Clipboard.SetText(TextBox.Text);
        }

        private void Cut(object sender, EventArgs e)
        {
            System.Windows.Clipboard.SetText(TextBox.Text);
            TextBox.Text = null;
        }

        private void Paste(object sender, EventArgs e)
        {
            TextBox.Text = System.Windows.Clipboard.GetText();
        }

        private void Clear(object sender, EventArgs e)
        {
            TextBox.Text=null;
        }
        private void Hear(object sender, EventArgs e)
        {
            Recognizer.LoadGrammar(dictationGrammar);
            Recognizer.SetInputToDefaultAudioDevice();
            RecognitionResult result = Recognizer.Recognize();
            System.Windows.Clipboard.SetText(result.Text);
            Recognizer.UnloadAllGrammars();
        }

        private void Open(object sender, EventArgs e)
        {
           
            if (FileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                String text = File.ReadAllText(FileDialog.FileName);
                TextBox.Text = text;
            }
        }

        private void Save(Object sender, EventArgs e)
        {
            
            if (SaveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FileStream File = new FileStream(SaveDialog.FileName,FileMode.Create,FileAccess.Write);
                Speaker.SetOutputToWaveStream(File);
                Speaker.Speak(TextBox.Text);
                File.Close();
            }
        }

        private void Update(Object sender, EventArgs e)
        {
            
        }

        private void About(Object sender, EventArgs e)
        {
            AboutBox About = new AboutBox();
            About.ShowDialog();
        }
        
    }
}
