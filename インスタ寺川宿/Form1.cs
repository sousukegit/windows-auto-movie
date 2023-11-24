using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Timers;
using System.IO;

namespace インスタ寺川宿
{
    public partial class Form1 : Form
    {
        //Movieクラス使用できるようインスタンス化
        Movie movie = new Movie();

        //タッチナンバー初期化
        

        int Now_min;
        int Now_hour;

        
        public Form1()
        {
            InitializeComponent();
            //controlの表示を隠す
            Player.uiMode = "none";
            //ループ再生設定
            Player.settings.setMode("loop", true);

            //PlayerクラスをMovieクラスで参照できるようにする
             movie.SetPlayer(Player);



            //モニターオンとメイン動画再生のタイマーの開始
            timer1.Interval = 1000;
            timer1.Start();

            //現在のタッチナンバーを常に取得する
            timer2.Interval = 1000;
            timer2.Start();

            //モニターオフとメイン動画停止のタイマーのスパン
            timer3.Interval = 1000;
            

        }

        private void Player_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            movie.Play_Main_Movie();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Player.Ctlcontrols.stop();
        }


        private void button3_Click(object sender, EventArgs e)
        {
            
            movie.Play_Touch_Movie();                       

            ////task2はtasak1が終わるまで待機
            Task task2 = Task.Run(() =>
            {
                bool flg1 = true;

                while (flg1)
                {
                    Thread.Sleep(1000);

                    if (Player.playState == WMPLib.WMPPlayState.wmppsStopped)
                    {
                        flg1 = false;
                        movie.Play_Main_Movie();
                    }
                }
            });
        }


        private void Main_Movie_Sysytem()
        {

            //  メインムービー
            Task task1 = Task.Run(() =>
            {
                movie.Play_Main_Movie();
            });


            //Task2はタッチするまで、そして20時するまで待機させておく
            Task task2 = Task.Run(() =>
            {
                movie.Stay_Touch_Movie();               
            });

            Task task3 = Task.Run(() =>
            {
                //ここもムービクラスに
                movie.Stay_Main_Movie();
               
            });           

        }

       
        private void button4_Click(object sender, EventArgs e)
        {
            Moniter.PowerOn();

        }
        private void button5_Click(object sender, EventArgs e)
        {
            Moniter.PowerOff();
            Thread.Sleep(2000);
            Moniter.PowerOn();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Main_Movie_Sysytem();
        }
                     

        private void button7_Click(object sender, EventArgs e)
        {
            ////タッチすると1になる
            
            movie.Write_touchnum(1);

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
            //時刻取得
            Now_hour = DateTime.Now.Hour;
            Now_min = DateTime.Now.Minute;

            //8時0分に動画開始
            
            if (Now_hour==11&&Now_min == 11)
            {   
                //動画再生されたあとtimer1実行しなくてもいいので止める
                timer1.Stop();
                //タイマー３で時刻取得して、オフタイマー待機をスタート
                timer3.Start();
                //モニターオン、動画システム再生
                Moniter.PowerOn();
                Main_Movie_Sysytem();
                
            }
           

        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            //1秒ごとにテキストボックスの数字を読み込んでセットする必要がある
            movie.Read_touchnum();
            
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            //時刻取得
            Now_hour = DateTime.Now.Hour;
            Now_min = DateTime.Now.Minute;


            //20時0分にストップ
            
            if (Now_hour == 11 && Now_min ==12)
            {
                //オフされたあとtimer3実行しなくてもいいので止める
                timer3.Stop();
                //タイマー1で時刻取得して、オンタイマー待機をスタート
                timer1.Start();
                //モニターオフ、動画停止
                Player.Ctlcontrols.stop();
                Thread.Sleep(1000);
                Moniter.PowerOff();
            }


        }

        private void button8_Click(object sender, EventArgs e)
        {
            Moniter.PowerSave();
        }
    }
}
