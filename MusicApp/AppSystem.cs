using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.VisualStyles;
using Hardcodet.Wpf.TaskbarNotification;
using ListView = System.Windows.Controls.ListView;
using MessageBox = System.Windows.MessageBox;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace MusicApp;

public class AppSystem
{
    public Song CurrentSong;
    public List<Song> SongList = new List<Song>();
    public int ListIndex = 0;
    private ListView _songListView;

    public AppSystem(ListView songListView)
    {
        try
        {
            _songListView = songListView;
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.StackTrace, "Ошибка");
        }
    }

    public bool CheckExistsMusicDir()
    {
        try
        {
            string currentDir = Directory.GetCurrentDirectory();
            if (!Directory.Exists(currentDir + "/Music"))
            {
                Directory.CreateDirectory(currentDir + "/Music");
            }
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.StackTrace, "Ошибка");
            return false;
        }

        return true;
    }

    public bool LoadAllMusic()
    {
        try
        {
            string[] files = Directory.GetFiles("Music");
            string filename = "";

            foreach (string file in files)
            {
                if (file.EndsWith(".mp3"))
                {
                    filename = ReplaceFileName(file);
                    SongList.Add(new Song(filename, file));
                    _songListView.Items.Add(filename);
                }
            }
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
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Browse Audio Files";
            openFileDialog.Filter = "MP3 Files(*.mp3)|*.mp3";
            openFileDialog.CheckFileExists = true;
            openFileDialog.CheckPathExists = true;
            openFileDialog.Multiselect = true;
            openFileDialog.ShowDialog();

            string filename = "";
            string path = Directory.GetCurrentDirectory() + "/Music/";

            foreach (string file in openFileDialog.FileNames)
            {
                filename = ReplaceFileName(file);
                File.Copy(file, path + filename + ".mp3");
                SongList.Add(new Song(filename, path + filename + ".mp3"));
                _songListView.Items.Add(filename);
            }
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message, "Ошибка");
            return false;
        }

        return true;
    }

    public string ReplaceFileName(string file)
    {
        string filename = "";
        
        try
        {
            if (file.LastIndexOf('/') != -1)
            {
                filename = file.Substring(file.LastIndexOf('/')).Replace("/", "").Replace(".mp3", "");
            }
            else
            {
                filename = file.Substring(file.LastIndexOf('\\')).Replace(@"\", "").Replace(".mp3", "");
            }
        }
        catch (Exception exception)
        {
            
            return "Error";
        }

        return filename;
    }

    public bool NextSong()
    {
        try
        {
            ListIndex = ListIndex >= SongList.Count - 1 ? 0 : ListIndex + 1;
            CurrentSong = SongList[ListIndex];
            ChangeFocusInSongList();
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.StackTrace, "Ошибка");
            return false;
        }

        return true;
    }

    public bool PreviousSong()
    {
        try
        {
            ListIndex = ListIndex <= 0 ? SongList.Count - 1 : ListIndex - 1;
            CurrentSong = SongList[ListIndex];
            ChangeFocusInSongList();
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.StackTrace, "Ошибка");
            return false;
        }

        return true;
    }

    public bool ChangeFocusInSongList()
    {
        try
        {
            _songListView.SelectedIndex = ListIndex;
            _songListView.ScrollIntoView(ListIndex);
            _songListView.Focus();
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.StackTrace, "Ошибка");
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

    public Dictionary<string, int> SecondsToTime(int seconds)
    {
        Dictionary<string, int> timeDictionary = new Dictionary<string, int>()
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
            timeDictionary["Hours"] = seconds / 60 / 60;
            timeDictionary["Minutes"] = (seconds / 60) - (timeDictionary["Hours"] * 60);
            timeDictionary["Seconds"] = seconds % 60;
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.StackTrace, "Ошибка");
            return null;
        }

        return timeDictionary;
    }
}