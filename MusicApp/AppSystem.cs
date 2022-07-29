using System;
using System.Collections.Generic;
using System.Windows;

namespace MusicApp;

public class AppSystem
{
    public Song CurrentSong;
    public List<Song> SongList = new List<Song>();

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
}