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

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ChooseImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Изображение";
            dlg.InitialDirectory = @"C:\Users\Nikita\Desktop";
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
            if (bitmap != null)
            {
                if ((bool)Encrypt.IsChecked)
                {
                    if (Text.Text.Length > 0) 
                    { 
                        Bitmap bitmapConvert = new Bitmap(bitmap);
                        bitmapConvert.Save(selectedFileName.Substring(0, selectedFileName.LastIndexOf('.')) + "_encrypted_tmp.bmp", ImageFormat.Bmp);
                        bitmapConvert.Dispose();
                        Bitmap copycopy = new Bitmap(selectedFileName.Substring(0, selectedFileName.LastIndexOf('.')) + "_encrypted_tmp.bmp");
                        Bitmap bitmapCopy = new Bitmap(copycopy);
                        copycopy.Dispose();

                        string binaryText = StringToBinary(Text.Text) + "00000000";
                        charCounter = binaryText.Length - 1;
                        colorCounter = 0;
                        bool done = false;

                        switch (Direction.SelectedIndex)
                        {
                            case 0:
                                for (int i = 0; i < bitmap.Height; i++)
                                {
                                    if (done) break;
                                    for (int k = 0; k < bitmap.Width;)
                                    {
                                        char currentColor = Colors.SelectionBoxItem.ToString().ToUpper()[colorCounter];
                                        System.Drawing.Color currentPixel = bitmapCopy.GetPixel(k, i);

                                        if (binaryText[binaryText.Length - charCounter - 1] == '0' &&
                                            Convert.ToInt32(currentPixel.GetType().GetProperty(currentColor.ToString()).GetValue(currentPixel, null)) % 2 == 1)
                                        {
                                            ChangeColor(currentColor, ref currentPixel);
                                            bitmapCopy.SetPixel(k, i, currentPixel);
                                        }
                                        else if (binaryText[binaryText.Length - charCounter - 1] == '1' &&
                                            Convert.ToInt32(currentPixel.GetType().GetProperty(currentColor.ToString()).GetValue(currentPixel, null)) % 2 == 0)
                                        {
                                            ChangeColor(currentColor, ref currentPixel);
                                            bitmapCopy.SetPixel(k, i, currentPixel);

                                        }

                                        charCounter--;
                                        if (charCounter == 0)
                                        {
                                            done = true;
                                            break;
                                        }

                                        colorCounter++;
                                        if (colorCounter == 3)
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
                                        char currentColor = Colors.SelectionBoxItem.ToString().ToUpper()[colorCounter];
                                        System.Drawing.Color currentPixel = bitmapCopy.GetPixel(k, i);

                                        if (binaryText[binaryText.Length - charCounter - 1] == '0' &&
                                            Convert.ToInt32(currentPixel.GetType().GetProperty(currentColor.ToString()).GetValue(currentPixel, null)) % 2 == 1)
                                        {
                                            ChangeColor(currentColor, ref currentPixel);
                                            bitmapCopy.SetPixel(k, i, currentPixel);
                                        }
                                        else if (binaryText[binaryText.Length - charCounter - 1] == '1' &&
                                            Convert.ToInt32(currentPixel.GetType().GetProperty(currentColor.ToString()).GetValue(currentPixel, null)) % 2 == 0)
                                        {
                                            ChangeColor(currentColor, ref currentPixel);
                                            bitmapCopy.SetPixel(k, i, currentPixel);

                                        }

                                        charCounter--;
                                        if (charCounter == 0)
                                        {
                                            done = true;
                                            break;
                                        }

                                        colorCounter++;
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
                                        char currentColor = Colors.SelectionBoxItem.ToString().ToUpper()[colorCounter];
                                        System.Drawing.Color currentPixel = bitmapCopy.GetPixel(k, i);

                                        if (binaryText[binaryText.Length - charCounter - 1] == '0' &&
                                            Convert.ToInt32(currentPixel.GetType().GetProperty(currentColor.ToString()).GetValue(currentPixel, null)) % 2 == 1)
                                        {
                                            ChangeColor(currentColor, ref currentPixel);
                                            bitmapCopy.SetPixel(k, i, currentPixel);
                                        }
                                        else if (binaryText[binaryText.Length - charCounter - 1] == '1' &&
                                            Convert.ToInt32(currentPixel.GetType().GetProperty(currentColor.ToString()).GetValue(currentPixel, null)) % 2 == 0)
                                        {
                                            ChangeColor(currentColor, ref currentPixel);
                                            bitmapCopy.SetPixel(k, i, currentPixel);

                                        }

                                        charCounter--;
                                        if (charCounter == 0)
                                        {
                                            done = true;
                                            break;
                                        }

                                        colorCounter++;
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
                                        char currentColor = Colors.SelectionBoxItem.ToString().ToUpper()[colorCounter];
                                        System.Drawing.Color currentPixel = bitmapCopy.GetPixel(k, i);

                                        if (binaryText[binaryText.Length - charCounter - 1] == '0' &&
                                            Convert.ToInt32(currentPixel.GetType().GetProperty(currentColor.ToString()).GetValue(currentPixel, null)) % 2 == 1)
                                        {
                                            ChangeColor(currentColor, ref currentPixel);
                                            bitmapCopy.SetPixel(k, i, currentPixel);
                                        }
                                        else if (binaryText[binaryText.Length - charCounter - 1] == '1' &&
                                            Convert.ToInt32(currentPixel.GetType().GetProperty(currentColor.ToString()).GetValue(currentPixel, null)) % 2 == 0)
                                        {
                                            ChangeColor(currentColor, ref currentPixel);
                                            bitmapCopy.SetPixel(k, i, currentPixel);

                                        }

                                        charCounter--;
                                        if (charCounter == 0)
                                        {
                                            done = true;
                                            break;
                                        }

                                        colorCounter++;
                                        if (colorCounter == 3)
                                        {
                                            i--;
                                            colorCounter = 0;
                                        }

                                    }
                                }
                                break;
                        }

                        System.Drawing.Color test = bitmapCopy.GetPixel(0, 0);

                        bitmapCopy.Save(selectedFileName.Substring(0, selectedFileName.LastIndexOf('.')) + "_encrypted.bmp", ImageFormat.Bmp);
                        bitmapCopy.Dispose();

                        if (System.IO.File.Exists(selectedFileName.Substring(0, selectedFileName.LastIndexOf('.')) + "_encrypted_tmp.bmp"))
                            System.IO.File.Delete(selectedFileName.Substring(0, selectedFileName.LastIndexOf('.')) + "_encrypted_tmp.bmp");
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
                                            break;
                                        }
                                    }
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
                                            break;
                                        }
                                    }
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
                                            break;
                                        }
                                    }
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
                                            break;
                                        }
                                    }
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
            else
            {
                MessageBox.Show("Выберите картинку");
            }
        }
    }
}
