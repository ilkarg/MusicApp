using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
                _appSystem = new AppSystem(SongListView);
                _appSystem.LoadAllMusic();
                _appSystem.CurrentSong = _appSystem.SongList[0];
                _appSystem.ChangeFocusInSongList();
                _appSystem.CurrentSong.mediaPlayer.MediaOpened += (s, e) =>
                {
                    VolumeSlider.Value = _appSystem.CurrentSong.Volume * 100;
                    SongDurationSlider.Maximum = _appSystem.TimeToSeconds(
                        _appSystem.CurrentSong.Duration["Hours"], 
                        _appSystem.CurrentSong.Duration["Minutes"], 
                        _appSystem.CurrentSong.Duration["Seconds"]
                        );
                    
                    Dictionary<string, string> timeLabelDictionary = new Dictionary<string, string>()
                    {
                        {
                            "HoursLabel", _appSystem.CurrentSong.Duration["Hours"] > 0 
                                ? $"{_appSystem.CurrentSong.Duration["Hours"]}:" 
                                : ""
                        },
                        {
                            "MinutesLabel", _appSystem.CurrentSong.Duration["Minutes"] < 10 
                                ? $"0{_appSystem.CurrentSong.Duration["Minutes"]}:" 
                                : $"{_appSystem.CurrentSong.Duration["Minutes"]}:"
                        },
                        {
                            "SecondsLabel", _appSystem.CurrentSong.Duration["Seconds"] < 10 
                                ? $"0{_appSystem.CurrentSong.Duration["Seconds"]}" 
                                : $"{_appSystem.CurrentSong.Duration["Seconds"]}"
                        }
                    };
            
                    SongDuration.Text = $"{timeLabelDictionary["HoursLabel"]}{timeLabelDictionary["MinutesLabel"]}{timeLabelDictionary["SecondsLabel"]}";
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
            try
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
                        "HoursLabel", timeDictionary["Hours"] > 0
                            ? $"{timeDictionary["Hours"]}:"
                            : ""
                    },
                    {
                        "MinutesLabel", timeDictionary["Minutes"] < 10
                            ? $"0{timeDictionary["Minutes"]}:"
                            : $"{timeDictionary["Minutes"]}:"
                    },
                    {
                        "SecondsLabel", timeDictionary["Seconds"] < 10
                            ? $"0{timeDictionary["Seconds"]}"
                            : $"{timeDictionary["Seconds"]}"
                    }
                };

                if (timeLabelDictionary["HoursLabel"] != "")
                {
                    timeLabelDictionary["HoursLabel"] = timeDictionary["Hours"] < 10
                        ? $"0{timeDictionary["Hours"]}:"
                        : $"{timeDictionary["Hours"]}:";
                }

                PlayedDuration.Text =
                    $"{timeLabelDictionary["HoursLabel"]}{timeLabelDictionary["MinutesLabel"]}{timeLabelDictionary["SecondsLabel"]}";

                if (_appSystem.CurrentSong.IsPlayed)
                {
                    SongDurationSlider.Value += 1;
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.StackTrace, "Ошибка");
            }
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
                    if (SongDurationSlider.Value == SongDurationSlider.Maximum)
                    {
                        SongDurationSlider.Value = 0;
                        PlayedDuration.Text = _appSystem.CurrentSong.Duration["Hours"] > 0
                            ? "00:00:00"
                            : "00:00";
                        _appSystem.CurrentSong.mediaPlayer.Position = new TimeSpan(0, 0, 0);
                    }

                    _appSystem.CurrentSong.StartPlay();
                    PlayButtonLabel.Text = "||";
                    _appSystem.CurrentSong.IsPlayed = true;
                    _timer.Start();
                }

                // Возможный вариант фикса, но помогает не всегда, к сожалению, по среди проигрывания может все равно один раз добавиться лишняя секунда
                /*_appSystem.CurrentSong.mediaPlayer.Position = new TimeSpan(
                    0,
                    _appSystem.CurrentSong.mediaPlayer.Position.Hours,
                    _appSystem.CurrentSong.mediaPlayer.Position.Minutes,
                    _appSystem.CurrentSong.mediaPlayer.Position.Seconds,
                    0
                    );*/
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
            ((Slider)sender).SelectionEnd = e.NewValue;
        }

        private void SoundPlayerProgressChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ((Slider)sender).SelectionEnd = e.NewValue;
            if (SongDurationSlider.Value == SongDurationSlider.Maximum)
            {
                PlayButtonLabel.Text = "|>";
                _timer.Stop();
                _appSystem.CurrentSong.StopPlay();
            }
        }

        private void SongDurationSliderLostMouseCapture(object sender, MouseEventArgs e)
        {
            try
            {
                int seconds = Convert.ToInt32(SongDurationSlider.Value);
                Dictionary<string, int> timeDictionary = _appSystem.SecondsToTime(seconds);
                Dictionary<string, string> timeLabelDictionary = new Dictionary<string, string>()
                {
                    {
                        "HoursLabel", timeDictionary["Hours"] > 0
                            ? $"{timeDictionary["Hours"]}:"
                            : ""
                    },
                    {
                        "MinutesLabel", timeDictionary["Minutes"] < 10
                            ? $"0{timeDictionary["Minutes"]}:"
                            : $"{timeDictionary["Minutes"]}:"
                    },
                    {
                        "SecondsLabel", timeDictionary["Seconds"] < 10
                            ? $"0{timeDictionary["Seconds"]}"
                            : $"{timeDictionary["Seconds"]}"
                    }
                };

                PlayedDuration.Text =
                    $"{timeLabelDictionary["HoursLabel"]}{timeLabelDictionary["MinutesLabel"]}{timeLabelDictionary["SecondsLabel"]}";
                _appSystem.CurrentSong.mediaPlayer.Position = new TimeSpan(
                    timeDictionary["Hours"],
                    timeDictionary["Minutes"],
                    timeDictionary["Seconds"]
                );
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.StackTrace, "Ошибка");
            }
        }

        private void PreviousSongButtonClick(object sender, RoutedEventArgs e) =>
            _appSystem.PreviousSong();

        private void NextSongButtonClick(object sender, RoutedEventArgs e) =>
            _appSystem.NextSong();

        private void SongListViewSelectionChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                _appSystem.ListIndex = SongListView.SelectedIndex;
                _appSystem.CurrentSong = _appSystem.SongList[_appSystem.ListIndex];
                
                VolumeSlider.Value = _appSystem.CurrentSong.Volume * 100;

                Dictionary<string, string> timeLabelDictionary = new Dictionary<string, string>()
                {
                    {
                        "HoursLabel", _appSystem.CurrentSong.Duration["Hours"] > 0
                            ? $"{_appSystem.CurrentSong.Duration["Hours"]}:"
                            : ""
                    },
                    {
                        "MinutesLabel", _appSystem.CurrentSong.Duration["Minutes"] < 10
                            ? $"0{_appSystem.CurrentSong.Duration["Minutes"]}:"
                            : $"{_appSystem.CurrentSong.Duration["Minutes"]}:"
                    },
                    {
                        "SecondsLabel", _appSystem.CurrentSong.Duration["Seconds"] < 10
                            ? $"0{_appSystem.CurrentSong.Duration["Seconds"]}"
                            : $"{_appSystem.CurrentSong.Duration["Seconds"]}"
                    }
                };

                SongDuration.Text =
                    $"{timeLabelDictionary["HoursLabel"]}{timeLabelDictionary["MinutesLabel"]}{timeLabelDictionary["SecondsLabel"]}";
                PlayedDuration.Text = _appSystem.CurrentSong.Duration["Hours"] > 0
                    ? "00:00:00"
                    : "00:00";

                SongDurationSlider.Value = 0;
                SongDurationSlider.Maximum = _appSystem.TimeToSeconds(
                    _appSystem.CurrentSong.Duration["Hours"],
                    _appSystem.CurrentSong.Duration["Minutes"],
                    _appSystem.CurrentSong.Duration["Seconds"]
                );
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.StackTrace, "Ошибка");
            }
        }
    }
}