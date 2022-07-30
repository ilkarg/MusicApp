using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace MusicApp
{
    public partial class MainWindow : Window
    {
        private readonly AppSystem _appSystem;
        private DispatcherTimer _timer = new DispatcherTimer();
        
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                _appSystem = new AppSystem();
                _appSystem.LoadAllMusic();
                _appSystem.SongList.Add(new Song("Тестовая песня", "./Music/test.mp3"));
                _appSystem.CurrentSong = _appSystem.SongList[0];
                _appSystem.CurrentSong.mediaPlayer.MediaOpened += (s, e) =>
                {
                    VolumeSlider.Value = _appSystem.CurrentSong.Volume * 100;
                    
                    if (_appSystem.CurrentSong.Duration["Hours"] > 0)
                    {
                        SongDuration.Text =
                            String.Format("{0}:{1}:{2}", 
                                _appSystem.CurrentSong.Duration["Hours"] < 10 ? $"0{_appSystem.CurrentSong.Duration["Hours"]}" : _appSystem.CurrentSong.Duration["Hours"], 
                                _appSystem.CurrentSong.Duration["Minutes"] < 10 ? $"0{_appSystem.CurrentSong.Duration["Minutes"]}" : _appSystem.CurrentSong.Duration["Minutes"],
                                _appSystem.CurrentSong.Duration["Seconds"] < 10 ? $"0{_appSystem.CurrentSong.Duration["Seconds"]}" : _appSystem.CurrentSong.Duration["Seconds"]
                            );
                    }
                    else
                    {
                        SongDuration.Text = String.Format("{0}:{1}",
                            _appSystem.CurrentSong.Duration["Minutes"] < 10 ? $"0{_appSystem.CurrentSong.Duration["Minutes"]}" : _appSystem.CurrentSong.Duration["Minutes"],
                            _appSystem.CurrentSong.Duration["Seconds"] < 10 ? $"0{_appSystem.CurrentSong.Duration["Seconds"]}" : _appSystem.CurrentSong.Duration["Seconds"]
                        );
                    }
                };
                _timer.Interval = TimeSpan.FromSeconds(1);
                _timer.Tick += TimerTick;
            }
            catch (Exception exception)
            {
                MessageBox.Show($"{exception.StackTrace}", "Ошибка");
            }
        }

        private void TimerTick(object sender, EventArgs e)
        {
            Dictionary<string, int> timeDictionary = new Dictionary<string, int>()
            {
                {
                    "Hours", _appSystem.CurrentSong.mediaPlayer.Position.Hours
                },
                {
                    "Minutes", _appSystem.CurrentSong.mediaPlayer.Position.Minutes
                },
                {
                    "Seconds", _appSystem.CurrentSong.mediaPlayer.Position.Seconds
                }
            };

            Dictionary<string, string> timeLabelDictionary = new Dictionary<string, string>()
            {
                {
                    "HoursLabel", timeDictionary["Hours"] > 0 ? $"{timeDictionary["Hours"]}:" : ""
                },
                {
                    "MinutesLabel", timeDictionary["Minutes"] < 10 ? $"0{timeDictionary["Minutes"]}:" : $"{timeDictionary["Minutes"]}:"
                },
                {
                    "SecondsLabel", timeDictionary["Seconds"] < 10 ? $"0{timeDictionary["Seconds"]}" : $"{timeDictionary["Seconds"]}"
                }
            };

            if (timeLabelDictionary["HoursLabel"] != "")
            {
                timeLabelDictionary["HoursLabel"] = timeDictionary["Hours"] < 10 ? $"0{timeDictionary["Hours"]}:" : $"{timeDictionary["Hours"]}:";
            }

            PlayedDuration.Text = $"{timeLabelDictionary["HoursLabel"]}{timeLabelDictionary["MinutesLabel"]}{timeLabelDictionary["SecondsLabel"]}";
        }

        private void PlaySoundButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_appSystem.CurrentSong.IsPlayed)
                {
                    _appSystem.CurrentSong.PausePlay();
                    PlayButtonLabel.Text = "|>";
                    _appSystem.CurrentSong.IsPlayed = false;
                    _timer.Stop();
                }
                else
                {
                    _appSystem.CurrentSong.StartPlay();
                    PlayButtonLabel.Text = "||";
                    _appSystem.CurrentSong.IsPlayed = true;
                    _timer.Start();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.StackTrace, "Ошибка");
            }
        }

        private void AddSoundButtonClick(object sender, RoutedEventArgs e) =>
            _appSystem.AddSong();

        private void SoundVolumeChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _appSystem.CurrentSong.SetVolume(VolumeSlider.Value / 100);
            ((Slider)sender).SelectionEnd=e.NewValue;
        }
    }
}