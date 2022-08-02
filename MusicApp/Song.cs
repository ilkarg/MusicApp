using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace MusicApp;

public class Song
{
    public string? Name { get; set; }
    public string? Path { get; set; }
    public Dictionary<string, int> Duration { get; set; }
    public double Volume { get; set; }
    public bool IsPlayed { get; set; }
    public bool IsEnded { get; set; }
    public MediaPlayer mediaPlayer;

    public Song(string? name, string? path)
    {
        try
        {
            Name = name;
            Path = path;
            IsPlayed = false;
            IsEnded = false;
            Duration = new Dictionary<string, int>()
            {
                {
                    "Hours", 0
                },
                {
                    "Minutes", 0
                },
                {
                    "Seconds", 0
                }
            };
            mediaPlayer = new MediaPlayer();
            mediaPlayer.Open(new Uri(Path, UriKind.RelativeOrAbsolute));
            mediaPlayer.MediaFailed += (s, e) => MessageBox.Show(e.ErrorException.Message, "Ошибка");
            mediaPlayer.MediaOpened += (s, e) =>
            {
                SetVolume(1);
                
                Duration["Hours"] = mediaPlayer.NaturalDuration.TimeSpan.Hours;
                Duration["Minutes"] = mediaPlayer.NaturalDuration.TimeSpan.Minutes;
                Duration["Seconds"] = mediaPlayer.NaturalDuration.TimeSpan.Seconds;

                Volume = mediaPlayer.Volume;
            };
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.StackTrace, "Ошибка");
        }
    }

    public bool StartPlay()
    {
        try
        {
            mediaPlayer.Play();
            IsPlayed = true;
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.StackTrace, "Ошибка");
            return false;
        }
        
        return true;
    }

    public bool PausePlay()
    {
        try
        {
            mediaPlayer.Pause();
            IsPlayed = false;
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.StackTrace, "Ошибка");
            return false;
        }

        return true;
    }

    public bool StopPlay()
    {
        try
        {
            mediaPlayer.Stop();
            IsPlayed = false;
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.StackTrace, "Ошибка");
            return false;
        }

        return true;
    }

    public bool SetVolume(double volume)
    {
        try
        {
            mediaPlayer.Volume = volume;
            Volume = volume;
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.StackTrace, "Ошибка");
            return false;
        }

        return true;
    }
}