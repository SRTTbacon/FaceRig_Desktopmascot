﻿using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media.Imaging;

namespace FaceRig_Vtuber
{
    public partial class MainWindow : Window
    {
        [DllImport("User32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private extern static bool PrintWindow(IntPtr hwnd, IntPtr hDC, uint nFlags);
        public MainWindow()
        {
            InitializeComponent();
            MouseLeftButtonDown += (sender, e) => { DragMove(); };
            SavePicture();
            if (File.Exists(Directory.GetCurrentDirectory() + "/Location_T.dat"))
            {
                StreamReader str1 = new StreamReader(Directory.GetCurrentDirectory() + "/Location_T.dat");
                Top = int.Parse(str1.ReadLine());
                str1.Close();
                StreamReader str2 = new StreamReader(Directory.GetCurrentDirectory() + "/Location_L.dat");
                Left = int.Parse(str2.ReadLine());
                str2.Close();
            }
            else
            {
                Top = 775;
                Left = 1425;
            }
            Location_Save();
        }
        private async void SavePicture()
        {
            try
            {
                var pLst = Process.GetProcessesByName("FaceRig");
                var handle = pLst.First().MainWindowHandle;
                while (true)
                {
                    Bitmap img = new Bitmap(640, 480);
                    Graphics memg = Graphics.FromImage(img);
                    PrintWindow(handle, memg.GetHdc(), 0);
                    memg.Dispose();
                    img.MakeTransparent(Color.FromArgb(0, 255, 0));
                    MemoryStream stream = new MemoryStream();
                    img.Save(stream, ImageFormat.Png);
                    img.Dispose();
                    BitmapImage bmpImage = new BitmapImage();
                    bmpImage.BeginInit();
                    bmpImage.CacheOption = BitmapCacheOption.OnLoad;
                    bmpImage.StreamSource = stream;
                    bmpImage.EndInit();
                    stream.Close();
                    ImageView.Source = bmpImage;
                    await System.Threading.Tasks.Task.Delay(25);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("FaceRigが起動されていません。" + e.Message);
                Close();
            }
        }
        async void Location_Save()
        {
            while (true)
            {
                await System.Threading.Tasks.Task.Delay(1000);
                StreamWriter stw = File.CreateText(Directory.GetCurrentDirectory() + "/Location_T.dat");
                stw.WriteLine(Top);
                stw.Close();
                StreamWriter stw2 = File.CreateText(Directory.GetCurrentDirectory() + "/Location_L.dat");
                stw2.WriteLine(Left);
                stw2.Close();
            }
        }
    }
}