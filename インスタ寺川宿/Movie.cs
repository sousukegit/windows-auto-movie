using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using Shell32;

namespace インスタ寺川宿
{
    class Movie
    {

        AxWMPLib.AxWindowsMediaPlayer Player;
        public void SetPlayer(AxWMPLib.AxWindowsMediaPlayer PlayerMovieclass)
        {
            Player = PlayerMovieclass;
        }

        //タッチナンバー初期化
        int touch_num = 0;

        public int Now_touch_num=0;

        public void Play_Main_Movie()
        {       
            
                WMPLib.IWMPPlaylist playlist_loop = Player.playlistCollection.newPlaylist("myplaylist");
                playlist_loop = Set_Main_Movie(playlist_loop);
                Player.currentPlaylist = playlist_loop;
                Player.Ctlcontrols.play();
            
        }

        public void Play_Touch_Movie()
        {

            WMPLib.IWMPPlaylist playlist_touch = Player.playlistCollection.newPlaylist("myplaylist");
            playlist_touch = Set_Touch_Movie(playlist_touch);
            Player.currentPlaylist = playlist_touch;
            Player.Ctlcontrols.play();

        }

        private WMPLib.IWMPPlaylist Set_Main_Movie(WMPLib.IWMPPlaylist playlist)
        {
            //ループ再生を設定
            Player.settings.setMode("loop", true);

            //@"G:\最終課題インスタ\リール＋画像"以下のファイルをすべて取得し、リスト化する
            IEnumerable<string> files_loop = System.IO.Directory.EnumerateFiles(@"E:\最終課題インスタ\リール＋画像", "*", System.IO.SearchOption.AllDirectories);


            // プレイリストを作成
            WMPLib.IWMPPlaylist playlist_loop = Player.playlistCollection.newPlaylist("myplaylist");


            // リストボックスの音楽ファイルをプレイリストに追加

            foreach (string f in files_loop)
            {

                WMPLib.IWMPMedia media = Player.newMedia(f);
                playlist_loop.appendItem(media);

            }
            // プレイリストをメディアプレイヤーに設定
            Player.currentPlaylist = playlist_loop;

            playlist = playlist_loop;

            return playlist;
        }

        private WMPLib.IWMPPlaylist Set_Touch_Movie(WMPLib.IWMPPlaylist playlist)
        {
            //ループ再生やめる
            Player.settings.setMode("loop", false);

            //@"G:\最終課題インスタ\リール＋画像"以下のファイルをすべて取得し、リスト化する
            IEnumerable<string> files = System.IO.Directory.EnumerateFiles(@"E:\最終課題インスタ\タッチ動画", "*", System.IO.SearchOption.AllDirectories);


            //// プレイリストを作成
            WMPLib.IWMPPlaylist playlist_touch = Player.playlistCollection.newPlaylist("myplaylist_touch");


            // リストボックスの音楽ファイルをプレイリストに追加

            foreach (string f in files)
            {

                WMPLib.IWMPMedia media = Player.newMedia(f);
                playlist_touch.appendItem(media);

            }
            // プレイリストをメディアプレイヤーに設定
            Player.currentPlaylist = playlist_touch;
            playlist = playlist_touch;
            return playlist;

        }

        public void Stay_Touch_Movie()
        {
            //待機ループ用のフラグ
            bool flg_loop = true;

            //ループの中でタッチ変数を常に取得し、再生できるようにする
            while (flg_loop)
            {
                Thread.Sleep(1000);
                if (touch_num == 1)
                {
                    Play_Touch_Movie();
                    Thread.Sleep(500);

                    //再生時間だけとまる
                    if (Player.playState == WMPLib.WMPPlayState.wmppsPlaying)
                    {
                        string strDuration = GetMovieDurationText(@"E:\最終課題インスタ\タッチ動画\punishment弾いてみた.mp4");
                        TimeSpan movietime = TimeSpan.Parse(strDuration);
                        Thread.Sleep(movietime);
                        Console.WriteLine("再生時間" + strDuration);
                    }

                    Write_touchnum(0);

                }
            }
        }
        public void Stay_Main_Movie()
        {
            bool flg1 = true;

            while (flg1)
            { //タッチ動画が終わまで待機
                Thread.Sleep(1000);
                if (Player.playState == WMPLib.WMPPlayState.wmppsStopped)
                {
                    //タッチナンバーを0にする
                    int finnum = 0;
                    Set_touchnum(finnum);
                    Play_Main_Movie();

                }
            }
        }


        public int Get_touchnum()
        {   
            //テキストファイルに書き込まれたらセット
            //ここでセンサー側の側の変数返す
            return touch_num;
        }
        public void Set_touchnum(int Set_touchnum)
        {
            //ここでセンサー側の側の変数をいれる  
            //タッチナンバーを代入
            touch_num = Set_touchnum;
            Console.WriteLine("タッチナンバー" + touch_num+"をセット");

        }
        public void Read_touchnum()
        {
            int Read_touchnum;
            string path = @"\\192.168.10.120\pi\share\1.txt";
            if (File.Exists(path))
            {
                Read_touchnum = 1;
            }
            else
            {
                Read_touchnum = 0;
            }
            Console.WriteLine("タッチナンバーは"+Read_touchnum) ;          

            Set_touchnum(Read_touchnum);

        }

        public void Write_touchnum(int num)
        {
            int Write_touchnum = num;
            string path = @"E:\最終課題用タッチテキスト\タッチテキスト.txt";

            Encoding sjisEnc = Encoding.GetEncoding("Shift_JIS");
            StreamWriter writer = new StreamWriter(path, false, sjisEnc);


            writer.WriteLine(Write_touchnum);
            writer.Close();
            writer.Dispose();
        }

        //ファイルの時間を取得するメソット
        public string GetMovieDurationText(string strMovPath)
        {
            FileInfo fi = new FileInfo(strMovPath);
            string strFileName = fi.FullName;
            var shellAppType = Type.GetTypeFromProgID("Shell.Application");
            dynamic shell = Activator.CreateInstance(shellAppType);
            Folder objFolder = shell.NameSpace(Path.GetDirectoryName(strFileName));
            FolderItem folderItem = objFolder.ParseName(Path.GetFileName(strFileName));
            string strDuration = objFolder.GetDetailsOf(folderItem, 27);
            return strDuration;
        }




    }
}
