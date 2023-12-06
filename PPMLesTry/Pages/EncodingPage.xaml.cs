using Microsoft.Win32;
using PPMLesTry.Coders;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PPMLesTry.Pages
{
    /// <summary>
    /// Logika interakcji dla klasy EncodingPage.xaml
    /// </summary>
    public partial class EncodingPage : Page
    {
        private string[] AvailableFormats = { ".png", ".jpg", ".gif", ".ppm", ".jpeg" };
        private string file = string.Empty;
        private string message = string.Empty;
        private BitmapImage image = new BitmapImage();

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
            image = new BitmapImage(new Uri(file, UriKind.Absolute));
            ImageHolder.Source = image;
        }

        private void OpenExplorer(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "All graphics available | *.png;*.jpg;*.jpeg;*.ppm;*.gif" + "PNG|*.png|GIF|*.gif|JPG|*.jpg;*.jpeg|PPM|*.ppm|";
            bool? success = dialog.ShowDialog();

            if (success == true)
                GetTheFile(dialog.FileName);
        }

        private void ChangeMessage(object sender, System.Windows.Input.KeyEventArgs e)
        {
            message = MessageTxt.Text;
        }

        private void Encode(object sender, RoutedEventArgs e)
        {
            if (image.UriSource == null)
            {
                ShowFile.Content = "Nie podano żadnego zdjęcia";
                return;
            }
            Encoder imageEncoder = new Encoder(image);

            string result = imageEncoder.SavePPMImage("", "converted");
            if (result != string.Empty)
                ShowFile.Content = result;

            imageEncoder.EncodeMessage(message);
        }
    }
}
