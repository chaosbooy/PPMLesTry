using Microsoft.Win32;
using System.IO;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PPMLesTry.Coders
{
    internal enum WhereStore
    {
        r = 0,
        g = 1,
        b
    };

    internal class Encoder
    {
        BitmapImage originalImage = new BitmapImage();
        FormatConvertedBitmap ppmImage = new FormatConvertedBitmap();
        BitmapImage encodedImage = new BitmapImage();
        private string[] AvailableFormats = { ".png", ".jpg", ".gif", ".jpeg" };

        public Encoder()
        {
            originalImage = new BitmapImage();
            encodedImage = new BitmapImage();
            ppmImage = new FormatConvertedBitmap();
        }

        public Encoder(BitmapImage source)
        {
            originalImage = source;
            encodedImage = new BitmapImage();
            ppmImage = new FormatConvertedBitmap();
            CreatePPMImage();
        }

        private void CreatePPMImage()
        {
            if (originalImage.UriSource == null) return;
                
            ppmImage = new FormatConvertedBitmap(originalImage, PixelFormats.Rgb24, null, 0);
        }

        public void SavePPMImage(string absolutePath)
        {
            if (!Path.Exists(absolutePath)) throw new Exception("Error (LP): Inapropriate file name or direction");
            if (ppmImage == new FormatConvertedBitmap() || originalImage.UriSource == null) throw new Exception("Error (LP): No files that can be saved");

            int width = ppmImage.PixelWidth;
            int height = ppmImage.PixelHeight;
            byte[] pixelData = new byte[width * height * 3];

            ppmImage.CopyPixels(pixelData, width * 3, 0);

            using (StreamWriter writer = new StreamWriter(Path.GetPathRoot(absolutePath) + Path.GetFileNameWithoutExtension(absolutePath) + ".ppm"))
            {
                writer.WriteLine("P3");
                writer.WriteLine($"{width} {height}");
                writer.WriteLine("255");

                for (int i = 0; i < pixelData.Length; i++)
                {
                    byte r = pixelData[i];
                    byte g = pixelData[++i];
                    byte b = pixelData[++i];

                    writer.WriteLine($"{r} {g} {b}");
                }
            }
        }

        public void SaveEncodedImage(string absolutePath)
        {
            if (!Path.Exists(absolutePath)) throw new Exception("Error (LP): Inapropriate file name or direction");
            if (encodedImage.UriSource == null) throw new Exception("Error (LP): No files that can be saved");

        }

        public BitmapImage EncodeMessage(string message, WhereStore where, string type)
        {
            if (ppmImage == new FormatConvertedBitmap() || originalImage.UriSource == null) throw new Exception("Error (LP): no image to encode into!");
            if (message == string.Empty) throw new Exception("Error (LP): no message to encode!");
            if (!AvailableFormats.Contains(type)) throw new Exception("Error (LP): not in available image formats!");

            int width = ppmImage.PixelWidth;
            int height = ppmImage.PixelHeight;
            byte[] pixelData = new byte[width * height * 3];
            uint pixelAmount = Convert.ToUInt32(pixelData.Length / 3);
            uint space = Convert.ToUInt32(pixelAmount / message.Length);
            uint offset = Convert.ToUInt32(pixelAmount % message.Length);
            string offsetBinary = Convert.ToString(offset, 2).PadLeft(30, '0');
            byte[] messageBinary = Encoding.UTF8.GetBytes(message);

            for(int i = 0; i < 3; i++)
            {
                var data = Convert.ToString(pixelData[i], 2);
                if (i != (int)where)
                    data = data.Substring(0, data.Length - 1) + '0';
                else
                    data = data.Substring(0, data.Length - 1) + '1';

                pixelData[i] = Convert.ToByte(data);
            }

            
            for(uint i = 3; i < 33; i++)
            {
                var data = Convert.ToString(pixelData[i], 2);
                pixelData[i] = Convert.ToByte(data.Substring(0, data.Length - 1) + offsetBinary[Convert.ToInt32(i) - 3]);
            }

            uint j = 0;
            for (uint i = 33; i < pixelData.Length; i += space * 3, j++)
            {
                var data = Convert.ToString(pixelData[i], 2);

                if (--offset >= 0) i++;
                    pixelData[i] = (byte)offsetBinary[Convert.ToInt32(i) - 3];

                pixelData[i] = Convert.ToByte(data.Substring(0,data.Length - 1) + messageBinary[j]);
            }

            var src = BitmapSource.Create(width, height, originalImage.DpiX, originalImage.DpiY, originalImage.Format, originalImage.Palette, pixelData, width * 3);
            BitmapEncoder encoder = new BmpBitmapEncoder();

            switch(type)
            {
                case ".jpg": case ".jpeg":
                    encoder = new JpegBitmapEncoder();
                    break;
                case ".png":
                    encoder = new PngBitmapEncoder();
                    break;
                case ".gif":
                    encoder = new GifBitmapEncoder();
                    break;
            }

            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Frames.Add(BitmapFrame.Create(src));
                encoder.Save(ms);

                ms.Position = 0;
                encodedImage.BeginInit();
                encodedImage.StreamSource = ms;
                encodedImage.EndInit();
            }

            return encodedImage;
        }
    }
}