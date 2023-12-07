using Microsoft.Win32;
using PPMLesTry.Coders;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

namespace PPMLesTry.Pages
{
    /// <summary>
    /// Logika interakcji dla klasy EncodingPage.xaml
    /// </summary>
    public partial class EncodingPage : Page
    {
        private string[] AvailableFormats = { ".png", ".jpg", ".gif", ".jpeg"};
        private string file = string.Empty;
        private string message = string.Empty;
        private BitmapImage image = new BitmapImage();
        private BitmapImage encodedImage = new BitmapImage();

        public EncodingPage()
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
                image = new BitmapImage();
                return;
            }

            ShowFile.Content = Path.GetFileName(file);
            if(Path.GetExtension(file) == ".ppm")
            {
                using (StreamReader sr = new StreamReader(file))
                {
                    string format = sr.ReadLine(); // Read the PPM format (P3)
                    string dimensions = sr.ReadLine(); // Read dimensions (width height)
                    string maxValue = sr.ReadLine(); // Read the maximum color value (usually 255)

                    string[] dimensionParts = dimensions.Split(' ');
                    int width = int.Parse(dimensionParts[0]);
                    int height = int.Parse(dimensionParts[1]);

                    image = new BitmapImage();
                    image.BeginInit();
                    image.DecodePixelWidth = width;
                    image.DecodePixelHeight = height;
                    image.UriSource = new Uri(file, UriKind.RelativeOrAbsolute);
                    image.EndInit();
                }
            }
            else
                 image = new BitmapImage(new Uri(file, UriKind.Absolute));
            ImageHolder.Source = image;
            encodedImage = new BitmapImage();
            BeforeEncode.Source = new BitmapImage();
            AfterEncode.Source = new BitmapImage();
            Downloader.Visibility = Visibility.Collapsed;
        }

        private void OpenExplorer(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "All graphics available | *.png;*.jpg;*.jpeg;*.gif|" + "PNG|*.png|GIF|*.gif|JPG|*.jpg;*.jpeg";
            bool? success = dialog.ShowDialog();

            if (success == true)
                GetTheFile(dialog.FileName);
        }

        private void ChangeMessage(object sender, System.Windows.Input.KeyEventArgs e)
        {
            message = MssgBox.Text;
            MssgTxt.Foreground = new SolidColorBrush(Colors.DarkGray);
        }

        

        private void Encode(object sender, RoutedEventArgs e)
        {
            if (image.UriSource == null)
            {
                ShowFile.Content = "Nie podano żadnego zdjęcia";
                return;
            } 
            if (message == string.Empty)
            {
                MssgTxt.Foreground = new SolidColorBrush(Colors.Red);
                return;
            }

            Encoder imageEncoder = new Encoder(image);
            encodedImage = imageEncoder.EncodeMessage(message, WhereStore.g, Path.GetExtension(file));

            BeforeEncode.Source = image;
            AfterEncode.Source = encodedImage;
            Downloader.Visibility = Visibility.Visible;
        }
    }
}
