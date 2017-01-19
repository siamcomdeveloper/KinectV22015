
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.IO;
using System.Windows.Threading;
using System.Threading;
using System.Windows.Forms; //ใช้งาน Libray ของ Coding4Fun เพิ่มดึงค่าความลึกมาแสดงผล
using Microsoft.Kinect;

namespace KinectV22015
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        int gameWidth = 1080;
        int gameHeight = 1920;

        private DateTime _lastTime = DateTime.MaxValue;

        private int _lastFrames;

        private int _totalFrames;

        private const string GestureSaveFileLocation = @"C:\EXD2\";

        List<BitmapImage> LogoImage = new List<BitmapImage>();
        List<BitmapImage> SelectImage = new List<BitmapImage>();
        List<BitmapImage> EasyImage = new List<BitmapImage>();
        List<BitmapImage> NormalImage = new List<BitmapImage>();
        List<BitmapImage> HardImage = new List<BitmapImage>();

        List<BitmapImage> Target1Image = new List<BitmapImage>();
        List<BitmapImage> Target2Image = new List<BitmapImage>();
        List<BitmapImage> Target3Image = new List<BitmapImage>();
        List<BitmapImage> Target4Image = new List<BitmapImage>();
        List<BitmapImage> Target5Image = new List<BitmapImage>();

        List<BitmapImage> TargetDinoImage = new List<BitmapImage>();

        List<BitmapImage> ReadyImage = new List<BitmapImage>();
        List<BitmapImage> StartImage = new List<BitmapImage>();
        List<BitmapImage> FightImage = new List<BitmapImage>();

        List<BitmapImage> TimeImage = new List<BitmapImage>();
        List<BitmapImage> Score1Image = new List<BitmapImage>();
        List<BitmapImage> Score2Image = new List<BitmapImage>();
        List<BitmapImage> Score3Image = new List<BitmapImage>();
        List<BitmapImage> Score4Image = new List<BitmapImage>();
        List<BitmapImage> Score5Image = new List<BitmapImage>();

        List<BitmapImage> LogoOverImage = new List<BitmapImage>();
        List<BitmapImage> GameOverImage = new List<BitmapImage>();
        List<BitmapImage> YouwinImage = new List<BitmapImage>();
        List<BitmapImage> HighscoreImage = new List<BitmapImage>();

        List<BitmapImage> A_ZImage = new List<BitmapImage>();
        List<BitmapImage> NumberImage = new List<BitmapImage>();

        int indexLogoImage = 0;
        int indexSelectImage = 0;
        int indexEasyImage = 0;
        int indexNormalImage = 0;
        int indexHardImage = 0;

        int indexReadyImage = 0;
        int indexStartImage = 0;
        int indexFightImage = 0;
        int indexTimeImage = 0;

        int indexLogoOverImage = 0;
        int indexYouwinImage = 0;
        int indexScoreImage = 0;
        int indexGameOverImage = 0;

        int indexHighscoreImage = 0;
        int indexTarget1Image = 0;
        int indexTarget2Image = 0;
        int indexTarget3Image = 0;
        int indexTarget4Image = 0;
        int indexTarget5Image = 0;

        int indexTarget1ImageTouch = 0;
        int indexTarget2ImageTouch = 0;
        int indexTarget3ImageTouch = 0;
        int indexTarget4ImageTouch = 0;
        int indexTarget5ImageTouch = 0;

        bool flagPressEnter = false;
        DispatcherTimer timer_UpdateLogoImage, timer_UpdateDisplay;

        int GameState = 1;
        int ResetGame = 0, Logo = 1, Select = 2, Selected = 3, SetupTarget = 4, Ready = 5, Start = 6, Fight = 7, GameOver = 8, HighScore = 9;

        int selectEasy = 1, selectNormal = 2, selectHard = 3;

        bool checkplay = false;

        Thread UpdateSelectThread, UpdateEasyThread, UpdateHardThread;//UpdateNormalThread
        Thread UpdateReadyThread, UpdateStartThread, UpdateFightThread;//, UpdateTimeThread;
        Thread UpdateTarget1Thread, UpdateTarget2Thread, UpdateTarget3Thread, UpdateTarget4Thread, UpdateTarget5Thread;

        Thread UpdateLogoOverThread, UpdateYouwinThread, UpdateScoreThread, UpdateGameOverThread, UpdateHighscoreThread;

        DateTime SelectDateTime;
        DateTime EasyDateTime;
        DateTime NormalDateTime;
        DateTime HardDateTime;

        DateTime ReadyDateTime;
        DateTime StartDateTime;
        DateTime FightDateTime;
        DateTime TimeDateTime;

        DateTime LogoOverDateTime;
        DateTime YouwinDateTime;
        DateTime ScoreDateTime;
        DateTime GameOverDateTime;

        DateTime HighscoreDateTime;

        DateTime Target1DateTime;
        DateTime Target2DateTime;
        DateTime Target3DateTime;
        DateTime Target4DateTime;
        DateTime Target5DateTime;

        bool flagSelectTime = true;
        bool flagEasyTime = true;
        bool flagNormalTime = true;
        bool flagHardTime = true;

        bool flagReadyTime = true;
        bool flagStartTime = true;
        bool flagFightTime = true;
        bool flagTimeTime = true;

        bool flagLogoOverTime = true;
        bool flagYouwinTime = true;
        bool flagScoreTime = true;
        bool flagGameOverTime = true;

        bool flagHighscoreTime = true;

        bool flagTarget1Time = true;
        bool flagTarget2Time = true;
        bool flagTarget3Time = true;
        bool flagTarget4Time = true;
        bool flagTarget5Time = true;

        int SelectTime = 0;
        int EasyTime = 0;
        int NormalTime = 0;
        int HardTime = 0;

        int ReadyTime = 0;
        int StartTime = 0;
        int FightTime = 0;
        int TimeTime = 0;

        int LogoOverTime = 0;
        int YouwinTime = 0;
        int ScoreTime = 0;
        int GameOverTime = 0;

        int HighscoreTime = 0;

        int Target1Time = 0;
        int Target2Time = 0;
        int Target3Time = 0;
        int Target4Time = 0;
        int Target5Time = 0;

        bool flagEasy = false, flagNormal = false, flagHard = false, flagEnter = false;

        bool flagReady = false, flagStart = false, flagFight = false, flagTime = false;

        bool flagSelectedSelect = false, flagSelectedEasy = false, flagSelectedHard = false;
        bool flagCheckSuccess = true;
        bool flagTouchTarget1 = false, flagTouchTarget2 = false, flagTouchTarget3 = false, flagTouchTarget4 = false, flagTouchTarget5 = false;

        bool flagTargetSuccess = false, flagReadySuccess = false;
        bool flagCheckSelect = false;

        KinectSensor _sensor;
        MultiSourceFrameReader _reader;
        IList<Body> _bodies;

        float xHandLeft;
        float yHandLeft;
        float zHandLeft;

        float xHandRight;
        float yHandRight;
        float zHandRight;

        int score = 0;

        string PathImages = @"C:\images\";

        DispatcherTimer timer_CountTime;
        int Milli = 0, Second = 0, Min = 0;
        int SecondFinish = 0, MinFinish = 0;
        String strMilli, strSecond, strMin;
        bool flagFinish = false;
        int scoreTarget1 = 0, scoreTarget2 = 0, scoreTarget3 = 0, scoreTarget4 = 0, scoreTarget5 = 0;
        public MainWindow()
        {
            InitializeComponent();

            imagehandLeft.Width = 100;
            imagehandLeft.Height = 100;
            imagehandLeft.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            imagehandLeft.Source = new BitmapImage(new Uri(PathImages + "handleft.png"));
            imagehandLeft.Margin = new System.Windows.Thickness(0, 0, 0, 0);

            imagehandRight.Width = 100;
            imagehandRight.Height = 100;
            imagehandRight.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            imagehandRight.Source = new BitmapImage(new Uri(PathImages + "handright.png"));
            imagehandRight.Margin = new System.Windows.Thickness(100, 100, 0, 0);
           
            // Kinect sensor initialization
            _sensor = KinectSensor.GetDefault();

            if (_sensor != null)
            {
                _sensor.Open();
            }

            _reader = _sensor.OpenMultiSourceFrameReader(FrameSourceTypes.Color |
                                             FrameSourceTypes.Depth |
                                             FrameSourceTypes.Infrared |
                                             FrameSourceTypes.Body);
            _reader.MultiSourceFrameArrived += Reader_MultiSourceFrameArrived;


            foreach (String imagefile in Directory.GetFiles(@"C:\EXD2\Rock\"))
            {
                LogoImage.Add(new BitmapImage(new Uri(imagefile)));
                LogoOverImage.Add(new BitmapImage(new Uri(imagefile)));
            }
            foreach (String imagefile in Directory.GetFiles(@"C:\EXD2\Player Select\"))
            {
                SelectImage.Add(new BitmapImage(new Uri(imagefile)));
            }
            foreach (String imagefile in Directory.GetFiles(@"C:\EXD2\Easy\"))
            {
                EasyImage.Add(new BitmapImage(new Uri(imagefile)));
            }
            foreach (String imagefile in Directory.GetFiles(@"C:\EXD2\2Normal\"))
            {
                NormalImage.Add(new BitmapImage(new Uri(imagefile)));
            }
            foreach (String imagefile in Directory.GetFiles(@"C:\EXD2\3Hard\"))
            {
                HardImage.Add(new BitmapImage(new Uri(imagefile)));
            }

            foreach (String imagefile in Directory.GetFiles(@"C:\DinoWait\"))
            {
                TargetDinoImage.Add(new BitmapImage(new Uri(imagefile)));
            }

            foreach (String imagefile in Directory.GetFiles(@"C:\EXD2\7Number1\"))
            {
                Target1Image.Add(new BitmapImage(new Uri(imagefile)));
            }
            foreach (String imagefile in Directory.GetFiles(@"C:\EXD2\7Number2\"))
            {
                Target2Image.Add(new BitmapImage(new Uri(imagefile)));
            }
            foreach (String imagefile in Directory.GetFiles(@"C:\EXD2\7Number3\"))
            {
                Target3Image.Add(new BitmapImage(new Uri(imagefile)));
            }
            foreach (String imagefile in Directory.GetFiles(@"C:\EXD2\7Number4\"))
            {
                Target4Image.Add(new BitmapImage(new Uri(imagefile)));
            }
            foreach (String imagefile in Directory.GetFiles(@"C:\EXD2\7Number5\"))
            {
                Target5Image.Add(new BitmapImage(new Uri(imagefile)));
            }

            foreach (String imagefile in Directory.GetFiles(@"C:\EXD2\7Number1\"))
            {
                TargetDinoImage.Add(new BitmapImage(new Uri(imagefile)));
            }

            foreach (String imagefile in Directory.GetFiles(@"C:\EXD2\4Ready\"))
            {
                ReadyImage.Add(new BitmapImage(new Uri(imagefile)));
            }
            foreach (String imagefile in Directory.GetFiles(@"C:\EXD2\6Fight\"))
            {
                FightImage.Add(new BitmapImage(new Uri(imagefile)));
            }
            foreach (String imagefile in Directory.GetFiles(@"C:\EXD2\5Start\"))
            {
                StartImage.Add(new BitmapImage(new Uri(imagefile)));
            }

            foreach (String imagefile in Directory.GetFiles(@"C:\EXD2\8Time\"))
            {
                TimeImage.Add(new BitmapImage(new Uri(imagefile)));
            }

            foreach (String imagefile in Directory.GetFiles(@"C:\EXD2\Game Over1\"))
            {
                GameOverImage.Add(new BitmapImage(new Uri(imagefile)));
            }
            foreach (String imagefile in Directory.GetFiles(@"C:\EXD2\You win\"))
            {
                YouwinImage.Add(new BitmapImage(new Uri(imagefile)));
            }
            foreach (String imagefile in Directory.GetFiles(@"C:\EXD2\high score\"))
            {
                HighscoreImage.Add(new BitmapImage(new Uri(imagefile)));
            }
            foreach (String imagefile in Directory.GetFiles(@"C:\EXD2\A_Z\"))
            {
                A_ZImage.Add(new BitmapImage(new Uri(imagefile)));
            }
            foreach (String imagefile in Directory.GetFiles(@"C:\EXD2\Number\"))
            {
                NumberImage.Add(new BitmapImage(new Uri(imagefile)));
            }
            foreach (String imagefile in Directory.GetFiles(@"C:\EXD2\9score1\"))
            {
                Score1Image.Add(new BitmapImage(new Uri(imagefile)));
            }
            foreach (String imagefile in Directory.GetFiles(@"C:\EXD2\9score2\"))
            {
                Score2Image.Add(new BitmapImage(new Uri(imagefile)));
            }
            foreach (String imagefile in Directory.GetFiles(@"C:\EXD2\9score3\"))
            {
                Score3Image.Add(new BitmapImage(new Uri(imagefile)));
            }
            foreach (String imagefile in Directory.GetFiles(@"C:\EXD2\9score4\"))
            {
                Score4Image.Add(new BitmapImage(new Uri(imagefile)));
            }
            foreach (String imagefile in Directory.GetFiles(@"C:\EXD2\9score5\"))
            {
                Score5Image.Add(new BitmapImage(new Uri(imagefile)));
            }

            ImageHighScoreSetUp();
            ImageSetUp();
        }

        private void Timer_Tick_CountTime(object sender, EventArgs e)
        {
            Milli++;
            if (Milli > 59)
            {
                Milli = 0;
                Second++;

                if (Second > 59)
                {
                    Second = 0;
                    Min++;
                }
            }

            if ( Min < 1 && (!flagFinish))
            {
                if (Milli < 10) strMilli = "0" + Milli.ToString();
                else strMilli = Milli.ToString();
                if (Second < 10) strSecond = "0" + Second.ToString();
                else strSecond = Second.ToString();
                if (Min < 10) strMin = "0" + Min.ToString();
                else strMin = Min.ToString();

                labelCountTime.Content = strMin + " : " + strSecond + " : " + strMilli;
            }
            else if (!flagFinish)
            {
                MinFinish = Min;
                SecondFinish = Second;
                flagFinish = true;

                labelCountTime.Content = "01 : 00 : 00";
            }


            if (!flagFinish)
            {
                if (flagTouchTarget1 && flagTouchTarget2 && flagTouchTarget3 && flagTouchTarget4 && flagTouchTarget5)
                {
                    MinFinish = Min;
                    SecondFinish = Second;
                    flagFinish = true;
                }
            }
            else {

                int TimeFinish = (MinFinish * 60) + SecondFinish;
                int TimeNow = (Min * 60) + Second;

                if (TimeNow - TimeFinish > 5)
                {
                    GameState = GameOver;

                    if (!UpdateLogoOverThread.IsAlive)
                        UpdateLogoOverThread.Start();
                    if (!UpdateYouwinThread.IsAlive)
                        UpdateYouwinThread.Start();
                    if (!UpdateScoreThread.IsAlive)
                        UpdateScoreThread.Start();
                    if (!UpdateGameOverThread.IsAlive)
                        UpdateGameOverThread.Start();
                    if (UpdateFightThread.IsAlive)
                        UpdateFightThread.Abort();

                    timer_CountTime.Stop();
                }
            }
        }

        void ImageHighScoreSetUp()
        {
            Name11.Width = 100;
            Name11.Height = 100;
            Name11.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            Name11.Margin = new System.Windows.Thickness(Name11.Width * 6, gameHeight / 2 - Name11.Height * 5, 0, 0);
            Name11.Source = A_ZImage[0];

            Name12.Width = 100;
            Name12.Height = 100;
            Name12.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            Name12.Margin = new System.Windows.Thickness(Name12.Width * 7, gameHeight / 2 - Name12.Height * 5, 0, 0);
            Name12.Source = A_ZImage[0];

            Name13.Width = 100;
            Name13.Height = 100;
            Name13.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            Name13.Margin = new System.Windows.Thickness(Name11.Width * 8, gameHeight / 2 - Name11.Height * 5, 0, 0);
            Name13.Source = A_ZImage[0];


            Score11.Width = 100;
            Score11.Height = 100;
            Score11.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            Score11.Margin = new System.Windows.Thickness(gameWidth / 2 + Name11.Width, gameHeight / 2 - Name11.Height * 5, 0, 0);
            Score11.Source = NumberImage[1];

            Score12.Width = 100;
            Score12.Height = 100;
            Score12.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            Score12.Margin = new System.Windows.Thickness(gameWidth / 2 + Name12.Width * 2, gameHeight / 2 - Name12.Height * 5, 0, 0);
            Score12.Source = NumberImage[5];

            ///////////////

            Name21.Width = 100;
            Name21.Height = 100;
            Name21.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            Name21.Margin = new System.Windows.Thickness(Name11.Width * 6, gameHeight / 2 - Name11.Height * 3, 0, 0);
            Name21.Source = A_ZImage[1];

            Name22.Width = 100;
            Name22.Height = 100;
            Name22.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            Name22.Margin = new System.Windows.Thickness(Name12.Width * 7, gameHeight / 2 - Name12.Height * 3, 0, 0);
            Name22.Source = A_ZImage[1];

            Name23.Width = 100;
            Name23.Height = 100;
            Name23.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            Name23.Margin = new System.Windows.Thickness(Name11.Width * 8, gameHeight / 2 - Name11.Height * 3, 0, 0);
            Name23.Source = A_ZImage[1];


            Score21.Width = 100;
            Score21.Height = 100;
            Score21.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            Score21.Margin = new System.Windows.Thickness(gameWidth / 2 + Name11.Width, gameHeight / 2 - Name11.Height * 3, 0, 0);
            Score21.Source = NumberImage[1];

            Score22.Width = 100;
            Score22.Height = 100;
            Score22.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            Score22.Margin = new System.Windows.Thickness(gameWidth / 2 + Name12.Width * 2, gameHeight / 2 - Name12.Height * 3, 0, 0);
            Score22.Source = NumberImage[0];

            ///////////////////

            Name31.Width = 100;
            Name31.Height = 100;
            Name31.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            Name31.Margin = new System.Windows.Thickness(Name11.Width * 6, gameHeight / 2 - Name12.Height, 0, 0);
            Name31.Source = A_ZImage[2];

            Name32.Width = 100;
            Name32.Height = 100;
            Name32.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            Name32.Margin = new System.Windows.Thickness(Name12.Width * 7, gameHeight / 2 - Name12.Height, 0, 0);
            Name32.Source = A_ZImage[2];

            Name33.Width = 100;
            Name33.Height = 100;
            Name33.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            Name33.Margin = new System.Windows.Thickness(Name11.Width * 8, gameHeight / 2 - Name12.Height, 0, 0);
            Name33.Source = A_ZImage[2];

            Score32.Width = 100;
            Score32.Height = 100;
            Score32.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            Score32.Margin = new System.Windows.Thickness(gameWidth / 2 + Name12.Width * 2, gameHeight / 2 - Name12.Height, 0, 0);
            Score32.Source = NumberImage[9];

            ///////////////////

            Name41.Width = 100;
            Name41.Height = 100;
            Name41.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            Name41.Margin = new System.Windows.Thickness(Name11.Width * 6, gameHeight / 2 + Name11.Height, 0, 0);
            Name41.Source = A_ZImage[3];

            Name42.Width = 100;
            Name42.Height = 100;
            Name42.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            Name42.Margin = new System.Windows.Thickness(Name12.Width * 7, gameHeight / 2 + Name12.Height, 0, 0);
            Name42.Source = A_ZImage[3];

            Name43.Width = 100;
            Name43.Height = 100;
            Name43.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            Name43.Margin = new System.Windows.Thickness(Name11.Width * 8, gameHeight / 2 + Name11.Height, 0, 0);
            Name43.Source = A_ZImage[3];

            Score42.Width = 100;
            Score42.Height = 100;
            Score42.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            Score42.Margin = new System.Windows.Thickness(gameWidth / 2 + Name12.Width * 2, gameHeight / 2 + Name12.Height, 0, 0);
            Score42.Source = NumberImage[6];
        }
        void ImageSetUp()
        {
            imageLogo.Width = 600;
            imageLogo.Height = 600;
            imageLogo.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            imageLogo.Margin = new System.Windows.Thickness(gameWidth / 2 - imageLogo.Width / 2, gameHeight / 2 - imageLogo.Height, 0, 0);

            imageSelect.Width = 400;
            imageSelect.Height = 400;
            imageSelect.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            imageSelect.Margin = new System.Windows.Thickness(gameWidth / 2 - imageSelect.Width / 2, gameHeight / 2 - imageSelect.Height, 0, 0);

            imageEasy.Width = 400;
            imageEasy.Height = 400;
            imageEasy.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            imageEasy.Margin = new System.Windows.Thickness(gameWidth / 2 - imageEasy.Width / 2, gameHeight - imageEasy.Height * 3.5, 0, 0);

            imageHard.Width = 400;
            imageHard.Height = 400;
            imageHard.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            imageHard.Margin = new System.Windows.Thickness(gameWidth / 2 - imageHard.Width / 2, gameHeight - imageHard.Height * 2, 0, 0);

            imageReady.Width = 400;
            imageReady.Height = 400;
            imageReady.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            imageReady.Margin = new System.Windows.Thickness(gameWidth / 2 - imageReady.Width / 2, gameHeight / 2 - imageReady.Height * 3.5, 0, 0);

            imageStart.Width = 400;
            imageStart.Height = 400;
            imageStart.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            imageStart.Margin = new System.Windows.Thickness(gameWidth / 2 - imageStart.Width / 2, gameHeight / 2 - imageStart.Height * 3.5, 0, 0);

            imageFight.Width = 400;
            imageFight.Height = 400;
            imageFight.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            imageFight.Margin = new System.Windows.Thickness(gameWidth / 2 - imageFight.Width / 2, gameHeight / 2 - imageFight.Height * 3.5, 0, 0);

            labelTime.Margin = new System.Windows.Thickness(gameWidth / 2 + (labelCountTime.Width / 2) + (labelTime.Width/2), 20, 0, 0);
            labelCountTime.Margin = new System.Windows.Thickness(gameWidth / 2 + labelCountTime.Width / 2, 20  + labelTime.Height/1.3, 0, 0);
            


            imageLogoOver.Width = 300;
            imageLogoOver.Height = 300;
            imageLogoOver.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            imageLogoOver.Margin = new System.Windows.Thickness(gameWidth / 2 - imageLogoOver.Width / 2, gameHeight / 2 - imageLogoOver.Height * 4.5, 0, 0);

            imageHighscore.Width = 400;
            imageHighscore.Height = 400;
            imageHighscore.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            imageHighscore.Margin = new System.Windows.Thickness(gameWidth / 2 - imageHighscore.Width / 2, gameHeight / 2 - imageHighscore.Height * 2.5, 0, 0);

            //imageTarget1 = new Image();
            imageTarget1.Width = 100;
            imageTarget1.Height = 100;
            imageTarget1.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            imageTarget1.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            imageTarget1.Margin = new System.Windows.Thickness(gameWidth / 2 - imageTarget1.Width * 3, gameHeight/2 - imageTarget1.Height * 3, 0, 0);
            //imageTarget1.Source = Target1Image[40];
            imageTarget1.Source = TargetDinoImage[0];

            //imageTarget2 = new Image();
            imageTarget2.Width = 100;
            imageTarget2.Height = 100;
            imageTarget2.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            imageTarget2.Margin = new System.Windows.Thickness(gameWidth / 2 + imageTarget2.Width * 2, gameHeight / 2 - imageTarget2.Height * 3, 0, 0);
            imageTarget2.Source = TargetDinoImage[0];

            //imageTarget3 = new Image();
            imageTarget3.Width = 100;
            imageTarget3.Height = 100;
            imageTarget3.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            imageTarget3.Margin = new System.Windows.Thickness(gameWidth / 2 - imageTarget3.Width * 3, gameHeight/2 , 0, 0);
            //imageTarget3.Source = Target3Image[40];
            imageTarget3.Source = TargetDinoImage[0];

            //imageTarget4 = new Image();
            imageTarget4.Width = 100;
            imageTarget4.Height = 100;
            imageTarget4.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            imageTarget4.Margin = new System.Windows.Thickness(gameWidth / 2 + imageTarget4.Width * 2, gameHeight / 2, 0, 0);
            //imageTarget4.Source = Target4Image[40];
            imageTarget4.Source = TargetDinoImage[0];

            //imageTarget5 = new Image();
            imageTarget5.Width = 100;
            imageTarget5.Height = 100;
            imageTarget5.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            imageTarget5.Margin = new System.Windows.Thickness(gameWidth / 2, gameHeight/2, 0, 0);
            //imageTarget5.Source = Target5Image[40];
            imageTarget5.Source = TargetDinoImage[0];

            ///////////////
            labelCountTime.Opacity = 0;
            labelTime.Opacity = 0;

            Name11.Opacity = 0;
            Name12.Opacity = 0;
            Name13.Opacity = 0;
            Score11.Opacity = 0;
            Score12.Opacity = 0;

            Name21.Opacity = 0;
            Name22.Opacity = 0;
            Name23.Opacity = 0;
            Score21.Opacity = 0;
            Score22.Opacity = 0;

            Name31.Opacity = 0;
            Name32.Opacity = 0;
            Name33.Opacity = 0;
            Score31.Opacity = 0;
            Score32.Opacity = 0;

            Name41.Opacity = 0;
            Name42.Opacity = 0;
            Name43.Opacity = 0;
            Score41.Opacity = 0;
            Score42.Opacity = 0;

            imageTarget1.Opacity = 0;
            imageTarget2.Opacity = 0;
            imageTarget3.Opacity = 0;
            imageTarget4.Opacity = 0;
            imageTarget5.Opacity = 0;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Forms.Screen s1 = System.Windows.Forms.Screen.AllScreens[1];
            System.Drawing.Rectangle r1 = s1.WorkingArea;
            this.WindowState = System.Windows.WindowState.Normal;
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.Manual;
            this.Top = r1.Top;
            this.Left = r1.Left;

             ThreadSetUp();

            timer_UpdateLogoImage = new System.Windows.Threading.DispatcherTimer();
            timer_UpdateLogoImage.Tick += new EventHandler(Timer_Tick_UpdateLogoImage);
            timer_UpdateLogoImage.Interval = new TimeSpan(0, 0, 0, 0, 20);
            timer_UpdateLogoImage.Start();

            timer_UpdateDisplay = new System.Windows.Threading.DispatcherTimer();
            timer_UpdateDisplay.Tick += new EventHandler(Timer_Tick_UpdateDisplay);
            timer_UpdateDisplay.Interval = new TimeSpan(0, 0, 0, 0, 1);
            timer_UpdateDisplay.Start();


            timer_CountTime = new DispatcherTimer();
            timer_CountTime.Tick += new EventHandler(Timer_Tick_CountTime);
            timer_CountTime.Interval = new TimeSpan(0, 0, 0, 0, 1);
        }

        void ThreadSetUp()
        {
            UpdateSelectThread = new Thread(new ThreadStart(UpdateSelect));
            UpdateSelectThread.IsBackground = true;

            UpdateEasyThread = new Thread(new ThreadStart(UpdateEasy));
            UpdateEasyThread.IsBackground = true;

            UpdateHardThread = new Thread(new ThreadStart(UpdateHard));
            UpdateHardThread.IsBackground = true;

            UpdateTarget1Thread = new Thread(new ThreadStart(UpdateTarget1));
            UpdateTarget1Thread.IsBackground = true;

            UpdateTarget2Thread = new Thread(new ThreadStart(UpdateTarget2));
            UpdateTarget2Thread.IsBackground = true;

            UpdateTarget3Thread = new Thread(new ThreadStart(UpdateTarget3));
            UpdateTarget3Thread.IsBackground = true;

            UpdateTarget4Thread = new Thread(new ThreadStart(UpdateTarget4));
            UpdateTarget4Thread.IsBackground = true;

            UpdateTarget5Thread = new Thread(new ThreadStart(UpdateTarget5));
            UpdateTarget5Thread.IsBackground = true;

            UpdateReadyThread = new Thread(new ThreadStart(UpdateReady));
            UpdateReadyThread.IsBackground = true;

            UpdateStartThread = new Thread(new ThreadStart(UpdateStart));
            UpdateStartThread.IsBackground = true;

            UpdateFightThread = new Thread(new ThreadStart(UpdateFight));
            UpdateFightThread.IsBackground = true;

            UpdateLogoOverThread = new Thread(new ThreadStart(UpdateLogoOver));
            UpdateLogoOverThread.IsBackground = true;

            UpdateYouwinThread = new Thread(new ThreadStart(UpdateYouwin));
            UpdateYouwinThread.IsBackground = true;

            UpdateScoreThread = new Thread(new ThreadStart(UpdateScore));
            UpdateScoreThread.IsBackground = true;

            UpdateGameOverThread = new Thread(new ThreadStart(UpdateGameOver));
            UpdateGameOverThread.IsBackground = true;

            UpdateHighscoreThread = new Thread(new ThreadStart(UpdateHighscore));
            UpdateHighscoreThread.IsBackground = true;
        }

        void Reader_MultiSourceFrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            if (GameState == Fight)
            {
                var reference = e.FrameReference.AcquireFrame();

                // Color
                // ...

                // Depth
                // ...

                // Infrared
                // ...

                // Body
                using (var frame = reference.BodyFrameReference.AcquireFrame())
                {
                    if (frame != null)
                    {
                        _bodies = new Body[frame.BodyFrameSource.BodyCount];

                        frame.GetAndRefreshBodyData(_bodies);

                        foreach (var body in _bodies)
                        {
                            if (body != null)
                            {
                                // Do something with the body...
                                if (body.IsTracked)
                                {
                                    Joint HandTipLeft = body.Joints[JointType.HandTipRight];//HandTipLeft];
                                    Joint HandTipRight = body.Joints[JointType.HandTipLeft];//HandTipRight];

                                    HandTipLeft.Position.X = HandTipLeft.Position.X * -1;
                                    HandTipRight.Position.X = HandTipRight.Position.X * -1;

                                    CameraSpacePoint cameraSpace_HandTipRightPoint = HandTipRight.Position;
                                    ColorSpacePoint HandTipRightPoint = _sensor.CoordinateMapper.MapCameraPointToColorSpace(cameraSpace_HandTipRightPoint);

                                    CameraSpacePoint cameraSpace_HandTipLeftPoint = HandTipLeft.Position;
                                    ColorSpacePoint HandTipLeftPoint = _sensor.CoordinateMapper.MapCameraPointToColorSpace(cameraSpace_HandTipLeftPoint);
                                    
                                    double ScaleX, ScaleY, XR, YR, ZR, XL, YL, ZL;

                                    ScaleX = (imageTarget1.Margin.Left - imageTarget2.Margin.Left) / (860 - 1060);
                                    ScaleY = (imageTarget1.Margin.Top - imageTarget3.Margin.Top) / (540 - 690);

                                    XR = ((HandTipRightPoint.X - 860) * ScaleX) + 240;
                                    YR = ((HandTipRightPoint.Y - 540) * ScaleY) + 660;
                                    ZR = HandTipRight.Position.Z;

                                    ScaleX = (imageTarget1.Margin.Left - imageTarget2.Margin.Left) / (860 - 1060);
                                    ScaleY = (imageTarget1.Margin.Top - imageTarget3.Margin.Top) / (540 - 690);

                                    XL = ((HandTipLeftPoint.X - 860) * ScaleX) + 240;
                                    YL = ((HandTipLeftPoint.Y - 540) * ScaleY) + 660;
                                    ZL = HandTipLeft.Position.Z;
                                    //ScaleX = (GameX0 - GameX1) / (CameraX0 - CameraX1)
                                    //ScaleY = (GameY0 - GameY1) / (CameraY0 - CameraY1)
                                    //X = ((CameraX - CameraOffsetX) * ScaleX) + GameOffsetX
                                    //Y = ((CameraY - CameraOffsetY) * ScaleY) + GameOffsetY
                                    
                                    LabelxHandLeft.Text = "xHLeft = " + HandTipLeftPoint.X.ToString();
                                    LabelyHandLeft.Text = "yHLeft = " + HandTipLeftPoint.Y.ToString();
                                    LabelzHandLeft.Text = "zHLeft = " + HandTipLeft.Position.Z.ToString();

                                    LabelyHandRight.Text = "yHRight = " + HandTipRightPoint.Y.ToString();
                                    LabelzHandRight.Text = "xHRight = " + HandTipRightPoint.X.ToString();
                                    
                                    LabelMaginTargetLeft.Text = "imageTarget1Left = " + imageTarget1.Margin.Left.ToString();
                                    LabelMaginTargetTop.Text = "imageTarget1Top = " + imageTarget1.Margin.Top.ToString();

                                    LabelMaginTargetRight.Text = "imageTarget1Right = " + (imageTarget1.Margin.Left + imageTarget1.Width).ToString();
                                    LabelMaginTargetBottom.Text = "imageTarget1Bottom = " + (imageTarget1.Margin.Top + imageTarget1.Height).ToString();
                                    
                                    if (!flagTouchTarget1 && !flagFinish)
                                    {
                                        if ((imageTarget1.Margin.Left < XR) && (XR < (imageTarget1.Margin.Left + imageTarget1.Width)) &&
                                           (imageTarget1.Margin.Top < YR) && (YR < (imageTarget1.Margin.Top + imageTarget1.Height)) && ZR > 4.1)
                                        {
                                            flagTouchTarget1 = true;
                                            if (!UpdateTarget1Thread.IsAlive)
                                            {
                                                UpdateTarget1Thread = new Thread(UpdateTarget1);
                                                UpdateTarget1Thread.IsBackground = true;
                                                UpdateTarget1Thread.Start();
                                            }
                                        }
                                        else if ((imageTarget1.Margin.Left < XL) && (XL < (imageTarget1.Margin.Left + imageTarget1.Width)) &&
                                           (imageTarget1.Margin.Top < YL) && (YL < (imageTarget1.Margin.Top + imageTarget1.Height)) && ZL > 4.1)
                                        {
                                            flagTouchTarget1 = true;
                                            if (!UpdateTarget1Thread.IsAlive)
                                            {
                                                UpdateTarget1Thread = new Thread(UpdateTarget1);
                                                UpdateTarget1Thread.IsBackground = true;
                                               UpdateTarget1Thread.Start();
                                            }
                                        }

                                        if (flagTouchTarget1)
                                        {
                                            score++;
                                            scoreTarget1 = score;
                                        }
                                    }
                                    ///////////////Target 2
                                    if (!flagTouchTarget2 && !flagFinish)
                                    {
                                        if ((imageTarget2.Margin.Left < XR) && (XR < (imageTarget2.Margin.Left + imageTarget2.Width)) &&
                                           (imageTarget2.Margin.Top < YR) && (YR < (imageTarget2.Margin.Top + imageTarget2.Height)) && ZR > 4.1)
                                        {
                                            //MessageBox.Show("Left Hand Touch Target 2");
                                            flagTouchTarget2 = true;
                                            if (!UpdateTarget2Thread.IsAlive)
                                            {
                                                UpdateTarget2Thread = new Thread(UpdateTarget2);
                                                UpdateTarget2Thread.IsBackground = true;
                                                UpdateTarget2Thread.Start();
                                            }
                                        }
                                        else if ((imageTarget2.Margin.Left < XL) && (XL < (imageTarget2.Margin.Left + imageTarget2.Width)) &&
                                           (imageTarget2.Margin.Top < YL) && (YL < (imageTarget2.Margin.Top + imageTarget2.Height)) && ZL > 4.1)
                                        {
                                            //MessageBox.Show("Right Hand Touch Target 2");
                                            flagTouchTarget2 = true;
                                            if (!UpdateTarget2Thread.IsAlive)
                                            {
                                                UpdateTarget2Thread = new Thread(UpdateTarget2);
                                                UpdateTarget2Thread.IsBackground = true;
                                                UpdateTarget2Thread.Start();
                                            }
                                        }
                                        if (flagTouchTarget2)
                                        {
                                            score++;
                                            scoreTarget2 = score;
                                        }
                                    }

                                    if (!flagTouchTarget3 && !flagFinish)
                                    {
                                        ///////////////Target 3
                                        if ((imageTarget3.Margin.Left < XR) && (XR < (imageTarget3.Margin.Left + imageTarget3.Width)) &&
                                           (imageTarget3.Margin.Top < YR) && (YR < (imageTarget3.Margin.Top + imageTarget3.Height)) && ZR > 4.1)
                                        {
                                            //MessageBox.Show("Left Hand Touch Target 2");
                                            flagTouchTarget3 = true;
                                            if (!UpdateTarget3Thread.IsAlive)
                                            {
                                                UpdateTarget3Thread = new Thread(UpdateTarget3);
                                                UpdateTarget3Thread.IsBackground = true;
                                                UpdateTarget3Thread.Start();
                                            }
                                        }
                                        else if ((imageTarget3.Margin.Left < XL) && (XL < (imageTarget3.Margin.Left + imageTarget3.Width)) &&
                                           (imageTarget3.Margin.Top < YL) && (YL < (imageTarget3.Margin.Top + imageTarget3.Height)) && ZL > 4.1)
                                        {
                                            //MessageBox.Show("Right Hand Touch Target 2");
                                            flagTouchTarget3 = true;
                                            if (!UpdateTarget3Thread.IsAlive)
                                            {
                                                UpdateTarget3Thread = new Thread(UpdateTarget3);
                                                UpdateTarget3Thread.IsBackground = true;
                                                UpdateTarget3Thread.Start();
                                            }
                                        }
                                        if (flagTouchTarget3)
                                        {
                                            score++;
                                            scoreTarget3 = score;
                                        }
                                    }

                                    if (!flagTouchTarget4 && !flagFinish)
                                    {
                                        ///////////////Target4
                                        if ((imageTarget4.Margin.Left < XR) && (XR < (imageTarget4.Margin.Left + imageTarget4.Width)) &&
                                           (imageTarget4.Margin.Top < YR) && (YR < (imageTarget4.Margin.Top + imageTarget4.Height)) && ZR > 4.1)
                                        {
                                            //MessageBox.Show("Left Hand Touch Target 2");
                                            flagTouchTarget4 = true;
                                            if (!UpdateTarget4Thread.IsAlive)
                                            {
                                                UpdateTarget4Thread = new Thread(UpdateTarget4);
                                                UpdateTarget4Thread.IsBackground = true;
                                                UpdateTarget4Thread.Start();
                                            }
                                        }
                                        else if ((imageTarget4.Margin.Left < XL) && (XL < (imageTarget4.Margin.Left + imageTarget4.Width)) &&
                                           (imageTarget4.Margin.Top < YL) && (YL < (imageTarget4.Margin.Top + imageTarget4.Height)) && ZL > 4.1)
                                        {
                                            //MessageBox.Show("Right Hand Touch Target 2");
                                            flagTouchTarget4 = true;
                                            if (!UpdateTarget4Thread.IsAlive)
                                            {
                                                UpdateTarget4Thread = new Thread(UpdateTarget4);
                                                UpdateTarget4Thread.IsBackground = true;
                                                UpdateTarget4Thread.Start();
                                            }
                                        }
                                        if (flagTouchTarget4)
                                        {
                                            score++;
                                            scoreTarget4 = score;
                                        }
                                    }

                                    if (!flagTouchTarget5 && !flagFinish)
                                    {
                                        ///////////////Target5
                                        if ((imageTarget5.Margin.Left < XR) && (XR < (imageTarget5.Margin.Left + imageTarget5.Width)) &&
                                           (imageTarget5.Margin.Top < YR) && (YR < (imageTarget5.Margin.Top + imageTarget5.Height)) && ZR > 4.1)
                                        {
                                            //MessageBox.Show("Left Hand Touch Target 2");
                                            flagTouchTarget5 = true;
                                            if (!UpdateTarget5Thread.IsAlive)
                                            {
                                                UpdateTarget5Thread = new Thread(UpdateTarget5);
                                                UpdateTarget5Thread.IsBackground = true;
                                                UpdateTarget5Thread.Start();
                                            }
                                        }
                                        else if ((imageTarget5.Margin.Left < XL) && (XL < (imageTarget5.Margin.Left + imageTarget5.Width)) &&
                                           (imageTarget5.Margin.Top < YL) && (YL < (imageTarget5.Margin.Top + imageTarget5.Height)) && ZL > 4.1)
                                        {
                                            //MessageBox.Show("Right Hand Touch Target 2");
                                            flagTouchTarget5 = true;
                                            if (!UpdateTarget5Thread.IsAlive)
                                            {
                                                UpdateTarget5Thread = new Thread(UpdateTarget5);
                                                UpdateTarget5Thread.IsBackground = true;
                                                UpdateTarget5Thread.Start();
                                            }
                                        }
                                        if (flagTouchTarget5)
                                        {
                                            score++;
                                            scoreTarget5 = score;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }


        private void Timer_Tick_UpdateLogoImage(object sender, EventArgs e)
        {
            indexLogoImage++;

            if (!flagEnter)
            {
                if (indexLogoImage < 152)//LogoImage.Count)
                    imageLogo.Source = LogoImage[indexLogoImage];
                else
                    indexLogoImage = 80;
            }
            else
            {
                if (indexLogoImage < LogoImage.Count)
                    imageLogo.Source = LogoImage[indexLogoImage];
                else
                {
                    GameState = Select;

                    UpdateSelectThread.Start();

                    timer_UpdateLogoImage.Stop();
                }
            }
        }

        void UpdateSelect()
        {
            bool flagSuccess = false;
            int i = 0;

            while (GameState == Select)
            {
                if (flagSelectTime)
                {
                    SelectDateTime = DateTime.Now; //ให้เก็บค่าปัจจุบันไว้ เพื่อไว้เปรียบเทียบหาค่าเวลาจริงในเกม
                    flagSelectTime = false; //ทำการเล่นแล้ว
                }

                //แสดงค่าเวลาในเกม
                SelectTime = ((DateTime.Now - SelectDateTime).Milliseconds);

                if (SelectTime > 30)
                {
                    i++;
                    if (i < 43)
                    {
                        //imageSelect.Source = SelectImage[indexSelectImage];
                        Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate ()
                        {
                            imageSelect.Margin = new System.Windows.Thickness(imageSelect.Margin.Left, imageSelect.Margin.Top - i, 0, 0);
                        }));
                    }
                    else
                    {
                        if (!flagSuccess)
                        {
                            flagSuccess = true;

                            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate ()
                            {
                                imageEasy.Opacity = 1;
                                //imageNormal.Opacity = 1;
                                imageHard.Opacity = 1;
                            }));

                            UpdateEasyThread.Start();
                            //UpdateNormalThread.Start();
                            UpdateHardThread.Start();
                        }
                    }

                    indexSelectImage++;

                    if (indexSelectImage < 152)//ScoreImage.Count)
                    {
                        Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate ()
                        {
                            imageSelect.Source = SelectImage[indexSelectImage];
                        }));
                    }
                    else
                    {
                        indexSelectImage = 80;
                    }

                    flagSelectTime = true;
                }

            }

            i = 0;
            indexSelectImage = 110;

            while (!flagSelectedSelect && GameState == Selected)
            {
                if (flagSelectTime)
                {
                    SelectDateTime = DateTime.Now; //ให้เก็บค่าปัจจุบันไว้ เพื่อไว้เปรียบเทียบหาค่าเวลาจริงในเกม
                    flagSelectTime = false; //ทำการเล่นแล้ว
                }

                //แสดงค่าเวลาในเกม
                SelectTime = ((DateTime.Now - SelectDateTime).Milliseconds);

                if (SelectTime > 30)
                {
                    i++;
                    if (i < 30)//SelectImage.Count)
                    {
                        indexSelectImage++;

                        if (indexSelectImage < 152)//ScoreImage.Count)
                        {
                            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate ()
                            {
                                imageSelect.Source = SelectImage[indexSelectImage];
                            }));
                        }
                        else
                        {
                            indexSelectImage = 80;
                        }
                        //imageSelect.Source = SelectImage[indexSelectImage];
                        Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate ()
                        {
                            imageSelect.Margin = new System.Windows.Thickness(imageSelect.Margin.Left, imageSelect.Margin.Top + i, 0, 0);
                        }));
                    }
                    else
                    {
                        indexSelectImage++;

                        if (indexSelectImage < SelectImage.Count)
                        {
                            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate ()
                            {
                                imageSelect.Source = SelectImage[indexSelectImage];
                            }));
                        }
                        else
                        {

                            flagSelectedSelect = true;
                            flagCheckSuccess = false;

                            GameState = SetupTarget;

                            UpdateTarget1Thread.Start();
                            UpdateTarget2Thread.Start();
                            UpdateTarget3Thread.Start();
                            UpdateTarget4Thread.Start();
                            UpdateTarget5Thread.Start();

                            UpdateSelectThread.Abort();
                        }
                    }

                    flagSelectTime = true;
                }

            }
        }

        void UpdateEasy()
        {
            bool flagSuccess = false;
            int i = 0;

            while (GameState == Select)
            {
                if (flagEasyTime)
                {
                    EasyDateTime = DateTime.Now; //ให้เก็บค่าปัจจุบันไว้ เพื่อไว้เปรียบเทียบหาค่าเวลาจริงในเกม
                    flagEasyTime = false; //ทำการเล่นแล้ว
                }

                //แสดงค่าเวลาในเกม
                EasyTime = ((DateTime.Now - EasyDateTime).Milliseconds);

                if (EasyTime > 30)
                {
                    indexEasyImage++;

                    if (indexEasyImage < 130)//ScoreImage.Count)
                    {
                        Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate ()
                        {
                            imageEasy.Source = EasyImage[indexEasyImage];
                        }));
                    }
                    else
                    {
                        indexEasyImage = 80;
                    }

                    flagEasyTime = true;
                }
            }

            i = 0;
            indexEasyImage = 80;

            while (flagEasy && GameState == Selected)
            {
                if (flagEasyTime)
                {
                    EasyDateTime = DateTime.Now; //ให้เก็บค่าปัจจุบันไว้ เพื่อไว้เปรียบเทียบหาค่าเวลาจริงในเกม
                    flagEasyTime = false; //ทำการเล่นแล้ว
                }

                //แสดงค่าเวลาในเกม
                EasyTime = ((DateTime.Now - EasyDateTime).Milliseconds);

                if (EasyTime > 30)
                {
                    i++;
                    if (i < 30 && !flagSelectedEasy)//EasyImage.Count)
                    {
                        indexEasyImage++;
                        if (indexEasyImage < 130)//EasyImage.Count)
                        {
                            //imageEasy.Source = EasyImage[indexEasyImage];
                            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate ()
                            {
                                imageEasy.Source = EasyImage[indexEasyImage];
                            }));
                        }
                        else
                        {
                            indexEasyImage = 80;
                        }

                        //imageEasy.Source = EasyImage[indexSelectImage];
                        Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate ()
                        {
                            imageEasy.Margin = new System.Windows.Thickness(imageEasy.Margin.Left, imageEasy.Margin.Top + i, 0, 0);
                        }));
                    }
                    else
                    {
                        indexEasyImage++;

                        if (indexEasyImage < EasyImage.Count) {
                            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate ()
                            {
                                imageEasy.Source = EasyImage[indexEasyImage];
                            }));
                        }
                        else
                        {
                            flagSelectedEasy = true;
                        }
                    }

                    flagEasyTime = true;
                }

            }
        }

        void UpdateHard()
        {
            bool flagSuccess = false;
            int i = 0;

            //while (true)
            while (GameState == Select)
            {
                if (flagHardTime)
                {
                    HardDateTime = DateTime.Now; //ให้เก็บค่าปัจจุบันไว้ เพื่อไว้เปรียบเทียบหาค่าเวลาจริงในเกม
                    flagHardTime = false; //ทำการเล่นแล้ว
                }

                //แสดงค่าเวลาในเกม
                HardTime = ((DateTime.Now - HardDateTime).Milliseconds);

                if (HardTime > 30)
                {
                    indexHardImage++;
                    if (indexHardImage < 130)//HardImage.Count)
                    {
                        //imageHard.Source = HardImage[indexHardImage];
                        Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate ()
                        {
                            imageHard.Source = HardImage[indexHardImage];
                        }));
                    }
                    else
                    {
                        indexHardImage = 80;
                    }
                    flagHardTime = true;
                }
            }

            i = 0;
            indexHardImage = 80;

            while (flagHard && GameState == Selected)
            {
                if (flagHardTime)
                {
                    HardDateTime = DateTime.Now; //ให้เก็บค่าปัจจุบันไว้ เพื่อไว้เปรียบเทียบหาค่าเวลาจริงในเกม
                    flagHardTime = false; //ทำการเล่นแล้ว
                }

                //แสดงค่าเวลาในเกม
                HardTime = ((DateTime.Now - HardDateTime).Milliseconds);

                if (HardTime > 30)
                {
                    i++;
                    if (i < 30 && !flagSelectedHard)//HardImage.Count)
                    {
                        indexHardImage++;
                        if (indexHardImage < 130)//HardImage.Count)
                        {
                            //imageHard.Source = HardImage[indexHardImage];
                            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate ()
                            {
                                imageHard.Source = HardImage[indexHardImage];
                            }));
                        }
                        else
                        {
                            indexHardImage = 80;
                        }

                        //imageHard.Source = HardImage[indexSelectImage];
                        Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate ()
                        {
                            imageHard.Margin = new System.Windows.Thickness(imageHard.Margin.Left, imageHard.Margin.Top - 5, 0, 0);
                        }));
                    }
                    else
                    {
                        indexHardImage++;

                        if (indexHardImage < HardImage.Count)
                        {
                            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate ()
                            {
                                imageHard.Source = HardImage[indexHardImage];
                            }));
                        }
                        else
                        {
                            flagSelectedHard = true;
                        }
                    }

                    flagHardTime = true;
                }

            }
        }

        void UpdateTarget1()
        {
            bool flagSuccess = false;
            flagEnter = false;

            while (GameState == SetupTarget)//true)
            {

                if (flagTarget1Time)
                {
                    Target1DateTime = DateTime.Now; //ให้เก็บค่าปัจจุบันไว้ เพื่อไว้เปรียบเทียบหาค่าเวลาจริงในเกม
                    flagTarget1Time = false; //ทำการเล่นแล้ว
                }

                Target1Time = ((DateTime.Now - Target1DateTime).Milliseconds);

                if (Target1Time > 30)
                {
                    indexTarget1Image++;

                    if (indexTarget1Image < 200)
                    {//LogoImage.Count)
                        Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate ()
                        {
                            imageTarget1.Source = TargetDinoImage[indexTarget1Image];
                        }));
                    }
                    else {
                        if (!flagSuccess)
                        {
                            //imageTarget1.Source = TargetDinoImage[130];
                            GameState = Ready;
                            UpdateReadyThread.Start();
                            flagSuccess = true;
                            //UpdateTarget1Thread.Abort();
                        }
                    }
                    flagTarget1Time = true;
                }
            }

            indexTarget1Image = 263;
            indexTarget1ImageTouch = 16;

            while (GameState != SetupTarget)
            {

                if (flagTarget1Time)
                {
                    Target1DateTime = DateTime.Now; //ให้เก็บค่าปัจจุบันไว้ เพื่อไว้เปรียบเทียบหาค่าเวลาจริงในเกม
                    flagTarget1Time = false; //ทำการเล่นแล้ว
                }

                Target1Time = ((DateTime.Now - Target1DateTime).Milliseconds);

                if (Target1Time > 30)
                {
                    indexTarget1Image++;

                    if (!flagTouchTarget1)
                    {
                        if (indexTarget1Image < 293)//HardImage.Count)
                        {
                            //imageHard.Source = HardImage[indexHardImage];
                            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate ()
                            {
                                imageTarget1.Source = TargetDinoImage[indexTarget1Image];
                            }));
                        }
                        else
                        {
                            indexTarget1Image = 263;
                        }
                    }
                    else {
                        indexTarget1ImageTouch++;

                        if (indexTarget1ImageTouch < Target1Image.Count)
                        {
                            //imageHard.Source = HardImage[indexHardImage];
                            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate ()
                            {
                                switch (scoreTarget1)
                                {
                                    case 1: imageTarget1.Source = Target1Image[indexTarget1ImageTouch]; break;
                                    case 2: imageTarget1.Source = Target2Image[indexTarget1ImageTouch]; break;
                                    case 3: imageTarget1.Source = Target3Image[indexTarget1ImageTouch]; break;
                                    case 4: imageTarget1.Source = Target4Image[indexTarget1ImageTouch]; break;
                                    case 5: imageTarget1.Source = Target5Image[indexTarget1ImageTouch]; break;
                                    default: break;
                                }
                            }));
                        }
                        else
                        {
                            indexTarget1ImageTouch = 27;
                        }
                    }
                    flagTarget1Time = true;
                }
            }
        }

        void UpdateTarget2()
        {
            bool flagSuccess = false;
            flagEnter = false;

            while (GameState == SetupTarget)//true)
            {

                if (flagTarget2Time)
                {
                    Target2DateTime = DateTime.Now; //ให้เก็บค่าปัจจุบันไว้ เพื่อไว้เปรียบเทียบหาค่าเวลาจริงในเกม
                    flagTarget2Time = false; //ทำการเล่นแล้ว
                }

                Target2Time = ((DateTime.Now - Target2DateTime).Milliseconds);

                if (Target2Time > 30)
                {
                    indexTarget2Image++;

                    if (indexTarget2Image < 200)
                    {//LogoImage.Count)
                        Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate ()
                        {
                            imageTarget2.Source = TargetDinoImage[indexTarget2Image];
                        }));
                    }
                    flagTarget2Time = true;
                }
            }

            indexTarget2Image = 263;
            indexTarget2ImageTouch = 16;

            while (GameState != SetupTarget)
            {

                if (flagTarget2Time)
                {
                    Target2DateTime = DateTime.Now; //ให้เก็บค่าปัจจุบันไว้ เพื่อไว้เปรียบเทียบหาค่าเวลาจริงในเกม
                    flagTarget2Time = false; //ทำการเล่นแล้ว
                }

                Target2Time = ((DateTime.Now - Target2DateTime).Milliseconds);

                if (Target2Time > 30)
                {
                    indexTarget2Image++;

                    if (!flagTouchTarget2)
                    {
                        if (indexTarget2Image < 293)//HardImage.Count)
                        {
                            //imageHard.Source = HardImage[indexHardImage];
                            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate ()
                            {
                                imageTarget2.Source = TargetDinoImage[indexTarget2Image];
                            }));
                        }
                        else
                        {
                            indexTarget2Image = 263;
                        }
                    }
                    else {
                        indexTarget2ImageTouch++;

                        if (indexTarget2ImageTouch < Target2Image.Count)
                        {
                            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate ()
                            {
                                switch (scoreTarget2)
                                {
                                    case 1: imageTarget2.Source = Target1Image[indexTarget2ImageTouch]; break;
                                    case 2: imageTarget2.Source = Target2Image[indexTarget2ImageTouch]; break;
                                    case 3: imageTarget2.Source = Target3Image[indexTarget2ImageTouch]; break;
                                    case 4: imageTarget2.Source = Target4Image[indexTarget2ImageTouch]; break;
                                    case 5: imageTarget2.Source = Target5Image[indexTarget2ImageTouch]; break;
                                    default: break;
                                }
                            }));
                        }
                        else
                        {
                            indexTarget2ImageTouch = 27;
                        }
                    }
                    flagTarget2Time = true;
                }
            }
        }

        void UpdateTarget3()
        {
            bool flagSuccess = false;
            flagEnter = false;

            while (GameState == SetupTarget)//true)
            {

                if (flagTarget3Time)
                {
                    Target3DateTime = DateTime.Now; //ให้เก็บค่าปัจจุบันไว้ เพื่อไว้เปรียบเทียบหาค่าเวลาจริงในเกม
                    flagTarget3Time = false; //ทำการเล่นแล้ว
                }

                Target3Time = ((DateTime.Now - Target3DateTime).Milliseconds);

                if (Target3Time > 30)
                {
                    indexTarget3Image++;

                    if (indexTarget3Image < 200)
                    {//LogoImage.Count)
                        Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate ()
                        {
                            imageTarget3.Source = TargetDinoImage[indexTarget3Image];
                        }));
                    }
                    flagTarget3Time = true;
                }
            }

            indexTarget3Image = 263;
            indexTarget3ImageTouch = 16;

            while (GameState != SetupTarget)
            {

                if (flagTarget3Time)
                {
                    Target3DateTime = DateTime.Now; //ให้เก็บค่าปัจจุบันไว้ เพื่อไว้เปรียบเทียบหาค่าเวลาจริงในเกม
                    flagTarget3Time = false; //ทำการเล่นแล้ว
                }

                Target3Time = ((DateTime.Now - Target3DateTime).Milliseconds);

                if (Target3Time > 30)
                {
                    indexTarget3Image++;

                    if (!flagTouchTarget3)
                    {
                        if (indexTarget3Image < 293)//HardImage.Count)
                        {
                            //imageHard.Source = HardImage[indexHardImage];
                            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate ()
                            {
                                imageTarget3.Source = TargetDinoImage[indexTarget3Image];
                            }));
                        }
                        else
                        {
                            indexTarget3Image = 263;
                        }
                    }
                    else {
                        indexTarget3ImageTouch++;

                        if (indexTarget3ImageTouch < Target3Image.Count)
                        {
                            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate ()
                            {
                                switch (scoreTarget3)
                                {
                                    case 1: imageTarget3.Source = Target1Image[indexTarget3ImageTouch]; break;
                                    case 2: imageTarget3.Source = Target2Image[indexTarget3ImageTouch]; break;
                                    case 3: imageTarget3.Source = Target3Image[indexTarget3ImageTouch]; break;
                                    case 4: imageTarget3.Source = Target4Image[indexTarget3ImageTouch]; break;
                                    case 5: imageTarget3.Source = Target5Image[indexTarget3ImageTouch]; break;
                                    default: break;
                                }
                            }));
                        }
                        else
                        {
                            indexTarget3ImageTouch = 27;
                        }
                    }
                    flagTarget3Time = true;
                }
            }
        }

        void UpdateTarget4()
        {
            bool flagSuccess = false;
            flagEnter = false;

            while (GameState == SetupTarget)//true)
            {

                if (flagTarget4Time)
                {
                    Target4DateTime = DateTime.Now; //ให้เก็บค่าปัจจุบันไว้ เพื่อไว้เปรียบเทียบหาค่าเวลาจริงในเกม
                    flagTarget4Time = false; //ทำการเล่นแล้ว
                }

                Target4Time = ((DateTime.Now - Target4DateTime).Milliseconds);

                if (Target4Time > 30)
                {
                    indexTarget4Image++;

                    if (indexTarget4Image < 200)
                    {//LogoImage.Count)
                        Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate ()
                        {
                            imageTarget4.Source = TargetDinoImage[indexTarget4Image];
                        }));
                    }
                    flagTarget4Time = true;
                }
            }

            indexTarget4Image = 263;
            indexTarget4ImageTouch = 16;

            while (GameState != SetupTarget)
            {

                if (flagTarget4Time)
                {
                    Target4DateTime = DateTime.Now; //ให้เก็บค่าปัจจุบันไว้ เพื่อไว้เปรียบเทียบหาค่าเวลาจริงในเกม
                    flagTarget4Time = false; //ทำการเล่นแล้ว
                }

                Target4Time = ((DateTime.Now - Target4DateTime).Milliseconds);

                if (Target4Time > 30)
                {
                    indexTarget4Image++;

                    if (!flagTouchTarget4)
                    {
                        if (indexTarget4Image < 293)//HardImage.Count)
                        {
                            //imageHard.Source = HardImage[indexHardImage];
                            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate ()
                            {
                                imageTarget4.Source = TargetDinoImage[indexTarget4Image];
                            }));
                        }
                        else
                        {
                            indexTarget4Image = 263;
                        }
                    }
                    else {
                        indexTarget4ImageTouch++;

                        if (indexTarget4ImageTouch < Target4Image.Count)
                        {
                            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate ()
                            {
                                switch (scoreTarget4)
                                {
                                    case 1: imageTarget4.Source = Target1Image[indexTarget4ImageTouch]; break;
                                    case 2: imageTarget4.Source = Target2Image[indexTarget4ImageTouch]; break;
                                    case 3: imageTarget4.Source = Target3Image[indexTarget4ImageTouch]; break;
                                    case 4: imageTarget4.Source = Target4Image[indexTarget4ImageTouch]; break;
                                    case 5: imageTarget4.Source = Target5Image[indexTarget4ImageTouch]; break;
                                    default: break;
                                }
                            }));
                        }
                        else
                        {
                            indexTarget4ImageTouch = 27;
                        }
                    }
                    flagTarget4Time = true;
                }
            }
        }

        void UpdateTarget5()
        {
            bool flagSuccess = false;
            flagEnter = false;

            while (GameState == SetupTarget)//true)
            {

                if (flagTarget5Time)
                {
                    Target5DateTime = DateTime.Now; //ให้เก็บค่าปัจจุบันไว้ เพื่อไว้เปรียบเทียบหาค่าเวลาจริงในเกม
                    flagTarget5Time = false; //ทำการเล่นแล้ว
                }

                Target5Time = ((DateTime.Now - Target5DateTime).Milliseconds);

                if (Target5Time > 30)
                {
                    indexTarget5Image++;

                    if (indexTarget5Image < 200)
                    {//LogoImage.Count)
                        Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate ()
                        {
                            imageTarget5.Source = TargetDinoImage[indexTarget5Image];
                        }));
                    }
                    flagTarget5Time = true;
                }
            }

            indexTarget5Image = 263;
            indexTarget5ImageTouch = 16;

            while (GameState != SetupTarget)
            {

                if (flagTarget5Time)
                {
                    Target5DateTime = DateTime.Now; //ให้เก็บค่าปัจจุบันไว้ เพื่อไว้เปรียบเทียบหาค่าเวลาจริงในเกม
                    flagTarget5Time = false; //ทำการเล่นแล้ว
                }

                Target5Time = ((DateTime.Now - Target5DateTime).Milliseconds);

                if (Target5Time > 30)
                {
                    indexTarget5Image++;

                    if (!flagTouchTarget5)
                    {
                        if (indexTarget5Image < 293)//HardImage.Count)
                        {
                            //imageHard.Source = HardImage[indexHardImage];
                            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate ()
                            {
                                imageTarget5.Source = TargetDinoImage[indexTarget5Image];
                            }));
                        }
                        else
                        {
                            indexTarget5Image = 263;
                        }
                    }
                    else {
                        indexTarget5ImageTouch++;

                        if (indexTarget5ImageTouch < Target5Image.Count)
                        {
                            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate ()
                            {
                                switch (scoreTarget5)
                                {
                                    case 1: imageTarget5.Source = Target1Image[indexTarget5ImageTouch]; break;
                                    case 2: imageTarget5.Source = Target2Image[indexTarget5ImageTouch]; break;
                                    case 3: imageTarget5.Source = Target3Image[indexTarget5ImageTouch]; break;
                                    case 4: imageTarget5.Source = Target4Image[indexTarget5ImageTouch]; break;
                                    case 5: imageTarget5.Source = Target5Image[indexTarget5ImageTouch]; break;
                                    default: break;
                                }
                            }));
                        }
                        else
                        {
                            indexTarget5ImageTouch = 27;
                        }
                    }
                    flagTarget5Time = true;
                }
            }
        }

        void UpdateReady()
        {
            bool flagSuccess = false;

            while (true)
            {
                if (flagReadyTime)
                {
                    ReadyDateTime = DateTime.Now; //ให้เก็บค่าปัจจุบันไว้ เพื่อไว้เปรียบเทียบหาค่าเวลาจริงในเกม
                    flagReadyTime = false; //ทำการเล่นแล้ว
                }

                //แสดงค่าเวลาในเกม
                ReadyTime = ((DateTime.Now - ReadyDateTime).Milliseconds);

                if (ReadyTime > 20)
                {
                    indexReadyImage++;
                    if (indexReadyImage < ReadyImage.Count)
                    {
                        Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate ()
                        {
                            imageReady.Source = ReadyImage[indexReadyImage];
                        }));
                    }
                    else
                    {
                        flagReadySuccess = true;
                        flagSuccess = true;

                        GameState = Start;
                        UpdateStartThread.Start();
                        UpdateReadyThread.Abort();
                    }
                    flagReadyTime = true;
                }
            }
        }

        void UpdateStart()
        {
            bool flagSuccess = false;

            while (true)
            {
                if (flagStartTime)
                {
                    StartDateTime = DateTime.Now; //ให้เก็บค่าปัจจุบันไว้ เพื่อไว้เปรียบเทียบหาค่าเวลาจริงในเกม
                    flagStartTime = false; //ทำการเล่นแล้ว
                }

                //แสดงค่าเวลาในเกม
                StartTime = ((DateTime.Now - StartDateTime).Milliseconds);

                if (StartTime > 10)
                {
                    indexStartImage++;
                    if (indexStartImage < StartImage.Count)
                    {
                        Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate ()
                        {
                            imageStart.Source = StartImage[indexStartImage];
                        }));
                    }
                    else
                    {
                        timer_CountTime.Start();

                        UpdateFightThread.Start();

                        //UpdateTimeThread.Start();
                     
                        GameState = Fight;

                        UpdateStartThread.Abort();
                    }
                    flagStartTime = true;
                }
            }
        }

        void UpdateFight()
        {
            bool flagSuccess = false;

            while (true)
            {
                if (flagFightTime)
                {
                    FightDateTime = DateTime.Now; //ให้เก็บค่าปัจจุบันไว้ เพื่อไว้เปรียบเทียบหาค่าเวลาจริงในเกม
                    flagFightTime = false; //ทำการเล่นแล้ว
                }

                //แสดงค่าเวลาในเกม
                FightTime = ((DateTime.Now - FightDateTime).Milliseconds);

                if (FightTime > 20)
                {
                    indexFightImage++;
                    if (indexFightImage < 152)//FightImage.Count)
                    {
                        Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate ()
                        {
                            imageFight.Source = FightImage[indexFightImage];
                        }));
                    }
                    else
                    {
                        indexFightImage = 80;
                    }
                    flagFightTime = true;
                }
            }
        }

        void UpdateLogoOver()
        {
            bool flagSuccess = false;
            
            while (true)
            {
                if (GameState == HighScore)
                {
                    flagSuccess = true;
                    //UpdateLogoOverThread.Abort();
                }

                if (flagLogoOverTime)
                {
                    LogoOverDateTime = DateTime.Now; //ให้เก็บค่าปัจจุบันไว้ เพื่อไว้เปรียบเทียบหาค่าเวลาจริงในเกม
                    flagLogoOverTime = false; //ทำการเล่นแล้ว
                }

                //แสดงค่าเวลาในเกม
                LogoOverTime = ((DateTime.Now - LogoOverDateTime).Milliseconds);

                if (LogoOverTime > 30)
                {
                    indexLogoOverImage++;
                    if (indexLogoOverImage < 152)//LogoOverImage.Count)
                    {
                        Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate ()
                        {
                            imageLogoOver.Source = LogoOverImage[indexLogoOverImage];
                        }));
                    }
                    else
                    {
                        indexLogoOverImage = 80;
                    }
                    flagLogoOverTime = true;
                }
            }
        }

        void UpdateYouwin()
        {
            bool flagSuccess = false;

            while (true)
            {
                if (GameState == HighScore)
                {
                    flagSuccess = true;
                    UpdateYouwinThread.Abort();
                }

                if (flagYouwinTime)
                {
                    YouwinDateTime = DateTime.Now; //ให้เก็บค่าปัจจุบันไว้ เพื่อไว้เปรียบเทียบหาค่าเวลาจริงในเกม
                    flagYouwinTime = false; //ทำการเล่นแล้ว
                }

                //แสดงค่าเวลาในเกม
                YouwinTime = ((DateTime.Now - YouwinDateTime).Milliseconds);

                if (YouwinTime > 30)
                {
                    indexYouwinImage++;
                    if (indexYouwinImage < 152)//YouwinImage.Count)
                    {
                        Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate ()
                        {
                            imageYouwin.Source = YouwinImage[indexYouwinImage];
                        }));
                    }
                    else
                    {
                        indexYouwinImage = 80;
                    }
                    flagYouwinTime = true;
                }
            }
        }

        void UpdateScore()
        {
            bool flagSuccess = false;
            
            while (true)
            {
                if (GameState == HighScore)
                {
                    flagSuccess = true;
                    UpdateScoreThread.Abort();
                }

                if (flagScoreTime)
                {
                    ScoreDateTime = DateTime.Now; //ให้เก็บค่าปัจจุบันไว้ เพื่อไว้เปรียบเทียบหาค่าเวลาจริงในเกม
                    flagScoreTime = false; //ทำการเล่นแล้ว
                }

                //แสดงค่าเวลาในเกม
                ScoreTime = ((DateTime.Now - ScoreDateTime).Milliseconds);

                if (ScoreTime > 30)
                {
                    indexScoreImage++;
                    if (indexScoreImage < 152)//ScoreImage.Count)
                    {
                        Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate ()
                        {
                            if(score==0 || score == 1) imageScore.Source = Score1Image[indexScoreImage];
                            else if (score == 2) imageScore.Source = Score2Image[indexScoreImage];
                            else if(score == 3) imageScore.Source = Score3Image[indexScoreImage];
                            else if(score == 4) imageScore.Source = Score4Image[indexScoreImage];
                            else if(score == 5) imageScore.Source = Score5Image[indexScoreImage];
                        }));
                    }
                    else
                    {
                        indexScoreImage = 80;
                    }
                    flagScoreTime = true;
                }
            }
        }

        void UpdateGameOver()
        {
            bool flagSuccess = false;

            while (true)
            {
                if (flagGameOverTime)
                {
                    GameOverDateTime = DateTime.Now; //ให้เก็บค่าปัจจุบันไว้ เพื่อไว้เปรียบเทียบหาค่าเวลาจริงในเกม
                    flagGameOverTime = false; //ทำการเล่นแล้ว
                }

                //แสดงค่าเวลาในเกม
                GameOverTime = ((DateTime.Now - GameOverDateTime).Milliseconds);

                if (GameOverTime > 30)
                {
                    indexGameOverImage++;
                    if (indexGameOverImage < GameOverImage.Count)
                    {
                        Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate ()
                        {
                            imageGameOver.Source = GameOverImage[indexGameOverImage];
                        }));
                    }
                    else
                    {
                        //indexGameOverImage = 80;
                        GameState = HighScore;
                        UpdateHighscoreThread.Start();
                        UpdateGameOverThread.Abort();

                    }
                    flagGameOverTime = true;
                }
            }
        }

        void UpdateHighscore()
        {
            bool flagSuccess = false;
            flagEnter = false;

            while (true)
            {

                if (flagHighscoreTime)
                {
                    HighscoreDateTime = DateTime.Now; //ให้เก็บค่าปัจจุบันไว้ เพื่อไว้เปรียบเทียบหาค่าเวลาจริงในเกม
                    flagHighscoreTime = false; //ทำการเล่นแล้ว
                }
                
                HighscoreTime = ((DateTime.Now - HighscoreDateTime).Milliseconds);

                if (HighscoreTime > 30)
                {
                    indexHighscoreImage++;

                    if (!flagEnter)
                    {
                        if (indexHighscoreImage < 130)//LogoImage.Count)
                           Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate ()
                            {
                                imageHighscore.Source = HighscoreImage[indexHighscoreImage];
                            }));
                        else
                            indexHighscoreImage = 30;
                    }
                    else {
                        if (indexHighscoreImage < HighscoreImage.Count)
                        {
                            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate ()
                            {
                                imageHighscore.Source = HighscoreImage[indexHighscoreImage];
                            }));
                        }
                        else
                        {
                            GameState = ResetGame;
                            UpdateHighscoreThread.Abort();
                        }
                    }
                    flagHighscoreTime = true;
                }
            }
        }

        private void Timer_Tick_UpdateDisplay(object sender, EventArgs e)
        {
            if (GameState == Logo)
            {
                imageLogo.Opacity = 1;
                imageSelect.Opacity = 0;
                imageEasy.Opacity = 0;
                //imageNormal.Opacity = 0;
                imageHard.Opacity = 0;

                imageLogoOver.Opacity = 0;
                imageHighscore.Opacity = 0;
            }
            else if (GameState == Select && !flagCheckSelect)
            {
                {
                    imageLogo.Opacity = 0;
                    imageSelect.Opacity = 1;
                    imageEasy.Opacity = 0;
                    //imageNormal.Opacity = 0;
                    imageHard.Opacity = 0;
                    flagCheckSelect = true;
                }
            }
            else if (GameState == Selected && !flagCheckSuccess)
            {
                if (flagEasy)
                {
                    if (UpdateSelectThread.IsAlive)
                        UpdateSelectThread.Abort();
                    if (UpdateEasyThread.IsAlive)
                        UpdateEasyThread.Abort();

                    imageEasy.Opacity = 1;
                    //imageNormal.Opacity = 0;
                    imageHard.Opacity = 0;

                    if (!UpdateSelectThread.IsAlive)
                    {
                        UpdateSelectThread = new Thread(UpdateSelect);
                        UpdateSelectThread.IsBackground = true;
                        UpdateSelectThread.Start();
                    }
                    if (!UpdateEasyThread.IsAlive)
                    {
                        UpdateEasyThread = new Thread(UpdateEasy);
                        UpdateEasyThread.IsBackground = true;
                        UpdateEasyThread.Start();
                    }

                    flagCheckSuccess = true;
                }
                else if (flagNormal)
                {
                    imageEasy.Opacity = 0;
                    //imageNormal.Opacity = 1;
                    imageHard.Opacity = 0;
                }
                else if (flagHard)
                {
                    imageEasy.Opacity = 0;
                    //imageNormal.Opacity = 0;
                    imageHard.Opacity = 1;
                }
            }
            else if (GameState == SetupTarget)// && !flagCheckSuccess)
            {
                imageSelect.Opacity = 0;
                imageEasy.Opacity = 0;
                //imageNormal.Opacity = 0;
                imageHard.Opacity = 0;

                imageReady.Opacity = 0;
                imageStart.Opacity = 0;
                imageFight.Opacity = 0;

                //imageTime.Opacity = 0;
                labelCountTime.Opacity = 0;
                labelTime.Opacity = 0;

                imageTarget1.Opacity = 1;
                imageTarget2.Opacity = 1;
                imageTarget3.Opacity = 1;
                imageTarget4.Opacity = 1;
                imageTarget5.Opacity = 1;
            }

            else if (GameState == Ready)
            {
                imageSelect.Opacity = 0;
                imageEasy.Opacity = 0;
                //imageNormal.Opacity = 0;
                imageHard.Opacity = 0;

                imageReady.Opacity = 1;
                imageStart.Opacity = 0;
                imageFight.Opacity = 0;
            }
            else if (GameState == Start)
            {
                imageReady.Opacity = 0;
                imageStart.Opacity = 1;
                imageFight.Opacity = 0;
            }
            else if (GameState == Fight)
            {
                imageStart.Opacity = 0;
                imageFight.Opacity = 1;

                //imageTime.Opacity = 1;
                labelCountTime.Opacity = 1;
                labelTime.Opacity = 1;
            }
            else if (GameState == GameOver)
            {
                imageReady.Opacity = 0;
                imageStart.Opacity = 0;
                imageFight.Opacity = 0;

                //imageTime.Opacity = 0;
                labelCountTime.Opacity = 0;
                labelTime.Opacity = 0;

                imageTarget1.Opacity = 0;
                imageTarget2.Opacity = 0;
                imageTarget3.Opacity = 0;
                imageTarget4.Opacity = 0;
                imageTarget5.Opacity = 0;

                imageLogoOver.Opacity = 1;
                imageYouwin.Opacity = 1;
                imageScore.Opacity = 1;
                imageGameOver.Opacity = 1;
            }
            else if (GameState == HighScore)
            {
                imageHighscore.Opacity = 1;
                imageYouwin.Opacity = 0;
                imageScore.Opacity = 0;
                imageGameOver.Opacity = 0;

                Name11.Opacity = 1;
                Name12.Opacity = 1;
                Name13.Opacity = 1;
                Score11.Opacity = 1;
                Score12.Opacity = 1;

                Name21.Opacity = 1;
                Name22.Opacity = 1;
                Name23.Opacity = 1;
                Score21.Opacity =1;
                Score22.Opacity = 1;

                Name31.Opacity = 1;
                Name32.Opacity = 1;
                Name33.Opacity = 1;
                Score31.Opacity = 1;
                Score32.Opacity = 1;

                Name41.Opacity = 1;
                Name42.Opacity = 1;
                Name43.Opacity = 1;
                Score41.Opacity = 1;
                Score42.Opacity = 1;
            }
            else if(GameState == ResetGame)
            {
                ClearAllThread();
                ClearAllParameter();
                ImageSetUp();
                ThreadSetUp();
                GameState = Logo;
                timer_UpdateLogoImage.Start();
            }
            //else Environment.Exit(0);
        }


        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (GameState == Logo)
            {
                if (e.Key == Key.Enter)
                {
                    flagEnter = true;
                }
            }
            else if (GameState == Select)
            {
                if (e.Key == Key.NumPad1)
                {
                    flagEasy = true;
                    UpdateHardThread.Abort();
                    imageHard.Opacity = 0;
                    GameState = Selected;
                }
                else if (e.Key == Key.NumPad2)
                {
                    flagHard = true;
                    UpdateEasyThread.Abort();
                    imageEasy.Opacity = 0;
                    GameState = Selected;
                }
            }
            else if (GameState == HighScore)
            {
                if (e.Key == Key.Enter)
                {
                    flagEnter = true;
                }
            }
        }

        void ClearAllThread()
        {
            if (UpdateSelectThread.IsAlive)
                UpdateSelectThread.Abort();
            if (UpdateEasyThread.IsAlive)
                UpdateEasyThread.Abort();
            if (UpdateHardThread.IsAlive)
                UpdateHardThread.Abort();

            if (UpdateTarget1Thread.IsAlive)
                UpdateTarget1Thread.Abort();
            if (UpdateTarget2Thread.IsAlive)
                UpdateTarget2Thread.Abort();
            if (UpdateTarget3Thread.IsAlive)
                UpdateTarget3Thread.Abort();
            if (UpdateTarget4Thread.IsAlive)
                UpdateTarget4Thread.Abort();
            if (UpdateTarget5Thread.IsAlive)
                UpdateTarget5Thread.Abort();

            if (UpdateReadyThread.IsAlive)
                UpdateReadyThread.Abort();
            if (UpdateStartThread.IsAlive)
                UpdateStartThread.Abort();
            if (UpdateFightThread.IsAlive)
                UpdateFightThread.Abort();

            if (UpdateLogoOverThread.IsAlive)
                UpdateLogoOverThread.Abort();
            if (UpdateYouwinThread.IsAlive)
                UpdateYouwinThread.Abort();
            if (UpdateScoreThread.IsAlive)
                UpdateScoreThread.Abort();
            if (UpdateGameOverThread.IsAlive)
                UpdateGameOverThread.Abort();

            if (UpdateHighscoreThread.IsAlive)
                UpdateHighscoreThread.Abort();
        }

        void ClearAllParameter()
        {
            indexLogoImage = 0;
            indexSelectImage = 0;
            indexEasyImage = 0;
            indexNormalImage = 0;
            indexHardImage = 0;

            indexReadyImage = 0;
            indexStartImage = 0;
            indexFightImage = 0;
            indexTimeImage = 0;

            indexLogoOverImage = 0;
            indexYouwinImage = 0;
            indexScoreImage = 0;
            indexGameOverImage = 0;

            indexHighscoreImage = 0;

            indexTarget1Image = 0;
            indexTarget2Image = 0;
            indexTarget3Image = 0;
            indexTarget4Image = 0;
            indexTarget5Image = 0;

            indexTarget1ImageTouch = 0;
            indexTarget2ImageTouch = 0;
            indexTarget3ImageTouch = 0;
            indexTarget4ImageTouch = 0;
            indexTarget5ImageTouch = 0;
            
            flagPressEnter = false;

            checkplay = false;

            flagSelectTime = true;
            flagEasyTime = true;
            flagNormalTime = true;
            flagHardTime = true;

            flagReadyTime = true;
            flagStartTime = true;
            flagFightTime = true;
            flagTimeTime = true;

            flagLogoOverTime = true;
            flagYouwinTime = true;
            flagScoreTime = true;
            flagGameOverTime = true;

            flagHighscoreTime = true;

            flagTarget1Time = true;
            flagTarget2Time = true;
            flagTarget3Time = true;
            flagTarget4Time = true;
            flagTarget5Time = true;

            SelectTime = 0;
            EasyTime = 0;
            NormalTime = 0;
            HardTime = 0;

            ReadyTime = 0;
            StartTime = 0;
            FightTime = 0;
            TimeTime = 0;

            LogoOverTime = 0;
            YouwinTime = 0;
            ScoreTime = 0;
            GameOverTime = 0;

            HighscoreTime = 0;

            Target1Time = 0;
            Target2Time = 0;
            Target3Time = 0;
            Target4Time = 0;
            Target5Time = 0;

            flagEasy = false;
            flagNormal = false;
            flagHard = false;
            flagEnter = false;

            flagReady = false;
            flagStart = false;
            flagFight = false;
            flagTime = false;

            flagSelectedSelect = false;
            flagCheckSuccess = true;
            flagTouchTarget1 = false;
            flagTouchTarget2 = false;
            flagTouchTarget3 = false;
            flagTouchTarget4 = false;
            flagTouchTarget5 = false;

            flagSelectedEasy = false;
            flagSelectedHard = false;

            flagTargetSuccess = false;
            flagReadySuccess = false;

            flagCheckSelect = false;

            score = 0;
            Min = 0;
            Second = 0;
            Milli = 0;
            
            flagFinish = false;
            scoreTarget1 = 0;
            scoreTarget2 = 0;
            scoreTarget3 = 0;
            scoreTarget4 = 0;
            scoreTarget5 = 0;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ClearAllThread();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Image image = e.Source as Image;
            if (GameState == Fight)
            {
                if (image.Name == "imageTarget1" && !flagFinish)
                {
                    flagTouchTarget1 = true;
                    score++;
                    scoreTarget1 = score;
                    if (!UpdateTarget1Thread.IsAlive)
                    {
                        UpdateTarget1Thread = new Thread(UpdateTarget1);
                        UpdateTarget1Thread.IsBackground = true;
                        UpdateTarget1Thread.Start();
                    }
                }
                else if (image.Name == "imageTarget2" && !flagFinish)
                {
                    flagTouchTarget2 = true;
                    score++;
                    scoreTarget2 = score;
                    if (!UpdateTarget2Thread.IsAlive)
                    {
                        UpdateTarget2Thread = new Thread(UpdateTarget2);
                        UpdateTarget2Thread.IsBackground = true;
                        UpdateTarget2Thread.Start();
                    }
                }
                else if (image.Name == "imageTarget3" && !flagFinish)
                {
                    flagTouchTarget3 = true;
                    score++;
                    scoreTarget3 = score;
                    if (!UpdateTarget3Thread.IsAlive)
                    {
                        UpdateTarget3Thread = new Thread(UpdateTarget3);
                        UpdateTarget3Thread.IsBackground = true;
                        UpdateTarget3Thread.Start();
                    }
                }
                else if (image.Name == "imageTarget4" && !flagFinish)
                {
                    flagTouchTarget4 = true;
                    score++;
                    scoreTarget4 = score;
                    if (!UpdateTarget4Thread.IsAlive)
                    {
                        UpdateTarget4Thread = new Thread(UpdateTarget4);
                        UpdateTarget4Thread.IsBackground = true;
                        UpdateTarget4Thread.Start();
                    }
                }
                else if (image.Name == "imageTarget5" && !flagFinish)
                {
                    flagTouchTarget5 = true;
                    score++;
                    scoreTarget5 = score;
                    if (!UpdateTarget5Thread.IsAlive)
                    {
                        UpdateTarget5Thread = new Thread(UpdateTarget5);
                        UpdateTarget5Thread.IsBackground = true;
                        UpdateTarget5Thread.Start();
                    }
                }
            }
        }

    }
}
