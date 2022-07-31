using System;
using System.Collections.Generic;
using System.Windows;

namespace MusicApp;

public class AppSystem
{
    public Song CurrentSong;
    public List<Song> SongList = new List<Song>();
    public bool LMBClicked = false;

    public bool LoadAllMusic()
    {
        try
        {
            MessageBox.Show("LoadAllMusic()", "Debug");
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message, "Ошибка");
            return false;
        }

        return true;
    }

    public bool AddSong()
    {
        try
        {
            MessageBox.Show("AddSong()", "Debug");
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message, "Ошибка");
            return false;
        }

        return true;
    }

    public int TimeToSeconds(int Hours, int Minutes, int Seconds)
    {
        Dictionary<string, int> resultTimeDictionary = new Dictionary<string, int>()
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

        try
        {
            resultTimeDictionary["Hours"] = Hours > 0 ? Hours * 60 * 60 : 0;
            resultTimeDictionary["Minutes"] = Minutes > 0 ? Minutes * 60 : 0;
            resultTimeDictionary["Seconds"] = Seconds;
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.StackTrace, "Ошибка");
            return -1;
        }

        return resultTimeDictionary["Hours"] + resultTimeDictionary["Minutes"] + resultTimeDictionary["Seconds"];
    }
}