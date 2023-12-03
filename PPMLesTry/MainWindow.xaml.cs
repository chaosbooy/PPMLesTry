using System.Diagnostics;
using System.IO;
using System.Security.Cryptography.Xml;
using System.Windows;
using System.Windows.Media.Imaging;

namespace PPMLesTry
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string[] AvailableFormats = { ".png", ".jpg", ".gif", ".ppm" };
        private string file = string.Empty;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void GetTheFile(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;

            string[] files = (string[]) e.Data.GetData(DataFormats.FileDrop);
            file = Path.GetFullPath(files[0]);

            if(files.Length > 1)
            {
                ShowFile.Content = "Błąd: nie obsługiwane parę plików na raz.";
                ImageHolder.Source = null;
                return;
            }
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
            Process.Start("explorer.exe");
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

            using (StreamWriter writer = new StreamWriter(ppmPath))
            {
                writer.WriteLine("P3");
                writer.WriteLine($"{width} {height}");
                writer.WriteLine("255");

                int pixelIndex = 0;

                for(int y = 0; y < height; y++)
                {
                    for(int x = 0; x < width; x++)
                    {

                        writer.WriteLine($"255 255 255");
                    }
                }

            }
        }
    }
}