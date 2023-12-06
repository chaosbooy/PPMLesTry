using System.IO;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PPMLesTry.Coders
{
    internal class Encoder
    {
        BitmapImage originalImage = new BitmapImage();
        FormatConvertedBitmap ppmImage = new FormatConvertedBitmap();
        BitmapImage encodedImage = new BitmapImage();

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

        public string SavePPMImage(string direction, string name)
        {
            try
            {
                name = name.Trim();
                if (!Path.Exists(direction) && direction != string.Empty) throw new Exception("ścieżka");
                if (name == string.Empty) return "nazwa";
                if (ppmImage == new FormatConvertedBitmap() || originalImage.UriSource == null) throw new Exception("pusto");

                int width = ppmImage.PixelWidth;
                int height = ppmImage.PixelHeight;
                byte[] pixelData = new byte[width * height * 3];

                ppmImage.CopyPixels(pixelData, width * 3, 0);

                using (StreamWriter writer = new StreamWriter(direction + name + ".ppm"))
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
            catch (Exception ex)
            {
                return ex.Message;
            }

            return string.Empty;
        }

        public void EncodeMessage(string message)
        {
            if (ppmImage == new FormatConvertedBitmap() || originalImage.UriSource == null) return;

            int width = ppmImage.PixelWidth;
            int height = ppmImage.PixelHeight;
            byte[] pixelData = new byte[width * height * 3];
            uint pixelAmount = Convert.ToUInt32(pixelData.Length / 3);
            uint space = Convert.ToUInt32(pixelAmount / message.Length);
            uint offset = Convert.ToUInt32(pixelAmount % message.Length);
            string offsetBinary = Convert.ToString(offset, 2).PadLeft(30, '0');
            byte[] messageBinary = Encoding.UTF8.GetBytes(message);

            ppmImage.CopyPixels(pixelData, width * 3, 0);

            for(uint i = 3; i < 33; i++)
                pixelData[i] = (byte) offsetBinary[Convert.ToInt32(i) - 3];

            for (uint i = 33; i < pixelData.Length; i += space * 3)
            {
                if (--offset >= 0) i++;
                pixelData[i] = (byte)offsetBinary[Convert.ToInt32(i) - 3];
            }
        }
    }
}