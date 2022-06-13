using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Encryption
{
    public partial class MainWindow : Window
    {
        private Bitmap bitmap;
        private string selectedFileName;
        private int charCounter, colorCounter;
        private static string StringToBinary(string data)
        {
            byte[] buf = Encoding.UTF8.GetBytes(data);
            StringBuilder sb = new StringBuilder(buf.Length * 8);

            foreach (byte b in buf)
            {
                sb.Append(Convert.ToString(b, 2).PadLeft(8, '0'));
            }
            return sb.ToString();
        }

        private static string BinaryToString(string data)
        {
            List<Byte> byteList = new List<Byte>();

            for (int i = 0; i < data.Length; i += 8)
            {
                byteList.Add(Convert.ToByte(data.Substring(i, 8), 2));
            }
            return Encoding.UTF8.GetString(byteList.ToArray());
        }

        private void ChangeColor(char color, ref System.Drawing.Color pixel)
        {
            switch (color)
            {
                case 'R':
                    pixel = System.Drawing.Color.FromArgb(pixel.R - 1, pixel.G, pixel.B);
                    break;
                case 'G':
                    pixel = System.Drawing.Color.FromArgb(pixel.R, pixel.G - 1, pixel.B);
                    break;
                case 'B':
                    pixel = System.Drawing.Color.FromArgb(pixel.R, pixel.G, pixel.B - 1);
                    break;
            }
        }

        private void EncodeBit(ref Bitmap bmp, string text, int i, int k, ref bool done)
        {
            char currentColor = Colors.SelectionBoxItem.ToString().ToUpper()[colorCounter];
            System.Drawing.Color currentPixel = bmp.GetPixel(k, i);

            if (text[text.Length - charCounter - 1] == '0' &&
                Convert.ToInt32(currentPixel.GetType().GetProperty(currentColor.ToString()).GetValue(currentPixel, null)) % 2 == 1)
            {
                ChangeColor(currentColor, ref currentPixel);
                bmp.SetPixel(k, i, currentPixel);
            }
            else if (text[text.Length - charCounter - 1] == '1' &&
                Convert.ToInt32(currentPixel.GetType().GetProperty(currentColor.ToString()).GetValue(currentPixel, null)) % 2 == 0)
            {
                ChangeColor(currentColor, ref currentPixel);
                bmp.SetPixel(k, i, currentPixel);

            }

            colorCounter++;
            charCounter--;
            if (charCounter == 0) done = true;
        }

        private void DecodeBit(ref string result, ref byte end, ref int byteCounter, ref bool done, int k, int i)
        {
            char currentColor = Colors.SelectionBoxItem.ToString().ToUpper()[colorCounter];
            System.Drawing.Color currentPixel = bitmap.GetPixel(k, i);
            int bit = Convert.ToInt32(currentPixel.GetType().GetProperty(currentColor.ToString()).GetValue(currentPixel, null)) % 2;
            result += bit;
            end <<= 1;
            end += (byte)bit;
            byteCounter++;

            colorCounter++;
            if (byteCounter == 8)
            {
                byteCounter = 0;
                if (end == 0)
                {
                    done = true;
                }
            }
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ChooseImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Изображение";
            dlg.Filter = "Image files (*.bmp;*.jpg;*.jpeg)|*.bmp;*.jpg;*.jpeg|All Files (*.*)|*.*";
            dlg.RestoreDirectory = true;

            if (dlg.ShowDialog() == true)
            {
                selectedFileName = dlg.FileName;
                BitmapImage bitmapImg = new BitmapImage();
                bitmapImg.BeginInit();
                bitmapImg.UriSource = new Uri(selectedFileName);
                bitmapImg.EndInit();
                ImageContainer.Source = bitmapImg;

                using (MemoryStream outStream = new MemoryStream())
                {
                    BitmapEncoder enc = new BmpBitmapEncoder();
                    enc.Frames.Add(BitmapFrame.Create(bitmapImg));
                    enc.Save(outStream);
                    this.bitmap = new Bitmap(outStream);
                }
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((bool)Encrypt.IsChecked)
                {
                    if (Text.Text.Length > 0)
                    {
                        Bitmap bitmapConvert = new Bitmap(bitmap);
                        bitmapConvert.Save(selectedFileName.Substring(0, selectedFileName.LastIndexOf('.')) + "_encrypted_tmp.bmp", ImageFormat.Bmp);   //format to bmp
                        bitmapConvert.Dispose();    //free resources
                        Bitmap copycopy = new Bitmap(selectedFileName.Substring(0, selectedFileName.LastIndexOf('.')) + "_encrypted_tmp.bmp");
                        Bitmap bitmapCopy = new Bitmap(copycopy);   //working with copy in bmp format
                        copycopy.Dispose();
                        if (File.Exists(selectedFileName.Substring(0, selectedFileName.LastIndexOf('.')) + "_encrypted_tmp.bmp"))   //deleting temporary copy
                            File.Delete(selectedFileName.Substring(0, selectedFileName.LastIndexOf('.')) + "_encrypted_tmp.bmp");

                        string binaryText = StringToBinary(Text.Text) + "00000000"; //marking the end of the message
                        charCounter = binaryText.Length - 1;
                        colorCounter = 0;
                        bool done = false;

                        switch (Direction.SelectedIndex)    //corner where we are starting from
                        {
                            case 0:
                                for (int i = 0; i < bitmap.Height; i++)
                                {
                                    if (done) break;
                                    for (int k = 0; k < bitmap.Width;)
                                    {
                                        EncodeBit(ref bitmapCopy, binaryText, i, k, ref done); //hiding our bit of information in pixel's R/G/B 
                                        if (done) break;
                                        if (colorCounter == 3)  //moving to next pixel
                                        {
                                            k++;
                                            colorCounter = 0;
                                        }

                                    }
                                }
                                break;
                            case 1:
                                for (int i = bitmap.Height - 1; i >= 0; i--)
                                {
                                    if (done) break;
                                    for (int k = bitmap.Width - 1; k >= 0;)
                                    {
                                        EncodeBit(ref bitmapCopy, binaryText, i, k, ref done);
                                        if (done) break;
                                        if (colorCounter == 3)
                                        {
                                            k--;
                                            colorCounter = 0;
                                        }

                                    }
                                }
                                break;
                            case 2:
                                for (int k = bitmap.Width - 1; k >= 0; k--)
                                {
                                    if (done) break;
                                    for (int i = 0; i < bitmap.Height;)
                                    {
                                        EncodeBit(ref bitmapCopy, binaryText, i, k, ref done);
                                        if (done) break;
                                        if (colorCounter == 3)
                                        {
                                            i++;
                                            colorCounter = 0;
                                        }

                                    }
                                }
                                break;
                            case 3:
                                for (int k = 0; k < bitmap.Width; k++)
                                {
                                    if (done) break;
                                    for (int i = bitmap.Height - 1; i >= 0;)
                                    {
                                        EncodeBit(ref bitmapCopy, binaryText, i, k, ref done);
                                        if (done) break;
                                        if (colorCounter == 3)
                                        {
                                            i--;
                                            colorCounter = 0;
                                        }

                                    }
                                }
                                break;
                        }

                        bitmapCopy.Save(selectedFileName.Substring(0, selectedFileName.LastIndexOf('.')) + "_encrypted.bmp", ImageFormat.Bmp);  //saving new image with hidden message
                        bitmapCopy.Dispose();

                        MessageBox.Show("Изображение сохранено");
                    }
                    else
                    {
                        MessageBox.Show("Введите текст");
                    }
                }
                else
                {
                    colorCounter = 0;
                    byte end = 255;
                    string result = "";
                    bool done = false;
                    int byteCounter = 0;

                    switch (Direction.SelectedIndex)
                    {
                        case 0:
                            for (int i = 0; i < bitmap.Height; i++)
                            {
                                if (done) break;
                                for (int k = 0; k < bitmap.Width;)
                                {
                                    DecodeBit(ref result, ref end, ref byteCounter, ref done, k, i);
                                    if (done) break;
                                    if (colorCounter == 3)
                                    {
                                        colorCounter = 0;
                                        k++;
                                    }
                                }
                            }
                            break;
                        case 1:
                            for (int i = bitmap.Height - 1; i >= 0; i--)
                            {
                                if (done) break;
                                for (int k = bitmap.Width - 1; k >= 0;)
                                {
                                    DecodeBit(ref result, ref end, ref byteCounter, ref done, k, i);
                                    if (done) break;
                                    if (colorCounter == 3)
                                    {
                                        colorCounter = 0;
                                        k--;
                                    }
                                }
                            }
                            break;
                        case 2:
                            for (int k = bitmap.Width - 1; k >= 0; k--)
                            {
                                if (done) break;
                                for (int i = 0; i < bitmap.Height;)
                                {
                                    DecodeBit(ref result, ref end, ref byteCounter, ref done, k, i);
                                    if (done) break;
                                    if (colorCounter == 3)
                                    {
                                        colorCounter = 0;
                                        i++;
                                    }
                                }
                            }
                            break;
                        case 3:
                            for (int k = 0; k < bitmap.Width; k++)
                            {
                                if (done) break;
                                for (int i = bitmap.Height - 1; i >= 0;)
                                {
                                    DecodeBit(ref result, ref end, ref byteCounter, ref done, k, i);
                                    if (done) break;
                                    if (colorCounter == 3)
                                    {
                                        colorCounter = 0;
                                        i--;
                                    }
                                }
                            }
                            break;
                    }

                    Text.Text = "Сообщение: " + BinaryToString(result);
                }
            }
            catch (NullReferenceException) when (bitmap == null)
            {
                MessageBox.Show("Выберите картинку");
            }
        }
    }
}
