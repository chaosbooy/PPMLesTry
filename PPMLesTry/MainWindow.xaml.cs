using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PPMLesTry
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string[] AvailableFormats = { ".png", ".jpg", ".gif", ".ppm", ".jpeg" };
        private string file = string.Empty;
        private BitmapImage image = new BitmapImage();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void FileDropped(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files.Length != 1)
            {
                ShowFile.Content = "Błąd: nie obsługiwane parę plików na raz.";
                ImageHolder.Source = null;
                image = new BitmapImage();
                return;
            }

            GetTheFile(Path.GetFullPath(files[0]));
        }

        private void GetTheFile(string name)
        {
            file = name;

            if (!AvailableFormats.Contains(Path.GetExtension(file)))
            {
                ShowFile.Content = "Błąd: plik ma nieobsługiwany format " + Path.GetExtension(file);
                ImageHolder.Source = null;
                return;
            }

            ShowFile.Content = Path.GetFileName(file);
            ImageHolder.Source = new BitmapImage(new Uri(file, UriKind.Absolute));
        }

        private void OpenExplorer(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "PNG|*.png|GIF|*.gif|JPG|*.jpg;*.jpeg|PPM|*.ppm|" + "All graphics available | *.png;*.jpg;*.jpeg;*.ppm;*.gif";
            bool? success = dialog.ShowDialog();
            
            if(success == true)
                GetTheFile(dialog.FileName);
        }

        private void Encode(object sender, RoutedEventArgs e)
        {
            string ppmPath = "converted.ppm";

            if (!Path.Exists(file)) return;
            else if (Path.GetExtension(file) == ".ppm")
            {
                File.Copy(file, ppmPath, true);
                return;
            }

            BitmapImage img = new BitmapImage(new Uri(file));

            int width = img.PixelWidth;
            int height = img.PixelHeight;
            byte[] pixelData = new byte[width * height * 3];

            FormatConvertedBitmap converted = new FormatConvertedBitmap(img, PixelFormats.Rgb24, null, 0);
            converted.CopyPixels(pixelData, width * 3, 0);

            using (StreamWriter writer = new StreamWriter(ppmPath))
            {
                writer.WriteLine("P3");
                writer.WriteLine($"{width} {height}");
                writer.WriteLine("255");

                for (int i = 0; i < pixelData.Length; i++)
                {
                    byte r = pixelData[i];
                    byte g = pixelData[i];
                    byte b = pixelData[i];

                    writer.WriteLine($"{r} {g} {b}");
                }
            }

        }
    }
}