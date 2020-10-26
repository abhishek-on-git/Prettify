using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Prettify
{
    public partial class Prettify : Form
    {
        Boolean isImported = false;
        Image image;
        public Prettify()
        {
            InitializeComponent();
        }

        private void importImage()
        {
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                image = Image.FromFile(openFileDialog1.FileName);
                pictureBox1.Image = image;
                isImported = true;
            }
            else
            {
                MessageBox.Show("Unable to import image. Try again later.");
            }
        }

        private void resetImage()
        {
            if (!isImported)
            {
                return;
            }
            pictureBox1.Image = image;
            isImported = true;
        }

        private void saveImage()
        {
            if (!isImported)
            {
                MessageBox.Show("No Image to save. Open an image to be able to save it");
                return;
            }
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Images|*.png;*bmp;*jpg;*";
            ImageFormat format = ImageFormat.Png;
            DialogResult result = saveDialog.ShowDialog();
            if(result == DialogResult.OK)
            {
                string extension = Path.GetExtension(saveDialog.FileName);
                switch (extension)
                {
                    case ".bmp":
                        format = ImageFormat.Bmp;
                        break;
                    case ".jpg":
                        format = ImageFormat.Jpeg;
                        break;
                }
                pictureBox1.Image.Save(saveDialog.FileName, format);
            }
            else
            {
                MessageBox.Show("Couldn't save your image");
            }
        }

        private float[][] pink_glow_Matrix = new float[][]
        {
            new float[]{.393f, .349f, .272f+1.3f, 0, 0},
            new float[]{.769f, .686f+0.5f, .534f, 0, 0},
            new float[]{.189f+2.3f, .168f, .131f, 0, 0},
            new float[]{0, 0, 0, 1, 0},
            new float[]{0, 0, 0, 0, 1}
        };

        private float[][] winter_Matrix = new float[][]
        {
            new float[]{.393f, .349f+0.5f, .272f, 0, 0},
            new float[]{.189f, .168f, .131f+0.5f, 0, 0},
            new float[]{.189f, .168f, .131f+0.5f, 0, 0},
            new float[]{0, 0, 0, 1, 0},
            new float[]{0, 0, 0, 0, 1}
        };

        private float[][] lavender_Matrix = new float[][]
        {
            new float[]{.393f+0.3f, .349f, .272f, 0, 0},
            new float[]{.769f, .686f+0.2f, .534f, 0, 0},
            new float[]{.189f, .168f, .131f+0.9f, 0, 0},
            new float[]{0, 0, 0, 1, 0},
            new float[]{0, 0, 0, 0, 1}
        };

        private float[][] winter_Matrix_2 = new float[][]
        {
            new float[]{1, 0, 0, 0, 0},
            new float[]{0, 1, 0, 0, 0},
            new float[]{0, 0, 1, 0, 0},
            new float[]{0, 0, 0, 1, 0},
            new float[]{0, 0, 1, 0, 1}
        };

        private float[][] red_Filter = new float[][]
        {
            new float[]{.393f, .349f, .272f, 0, 0},
            new float[]{.769f, .686f, .534f, 0, 0},
            new float[]{.189f, .168f, .131f, 0, 0},
            new float[]{0, 0, 0, 1, 0},
            new float[]{0, 0, 0, 0, 1}
        };

        private float[][] freezing_cold_Filter = new float[][]
        {
            new float[]{1+0.3f, 0, 0, 0, 0},
            new float[]{0, 1+0f, 0, 0, 0},
            new float[]{0, 0, 1+2f, 0, 0},
            new float[]{0, 0, 0, 1, 0},
            new float[]{0, 0, 0, 0, 1}
        };

        private float[][] flash_Filter = new float[][]
        {
            new float[]{1+0.9f, 0, 0, 0, 0},
            new float[]{0, 1+1.5f, 0, 0, 0},
            new float[]{0, 0, 1+1.3f, 0, 0},
            new float[]{0, 0, 0, 1, 0},
            new float[]{0, 0, 0, 0, 1}
        };

        private float[][] foggy_Filter = new float[][]
        {
            new float[]{1+0.3f, 0, 0, 0, 0},
            new float[]{0, 1+0.7f, 0, 0, 0},
            new float[]{0, 0, 1+1.3f, 0, 0},
            new float[]{0, 0, 0, 1, 0},
            new float[]{0, 0, 0, 0, 1}
        };

        private float[][] grayscale_Filter = new float[][]
        {
            new float[]{0.299f, 0.299f, 0.299f, 0, 0},
            new float[]{0.587f, 0.587f, 0.587f, 0, 0},
            new float[]{0.114f, 0.114f, 0.114f, 0, 0},
            new float[]{0, 0, 0, 1, 0},
            new float[]{0, 0, 0, 0, 0}
        };

        void filter1()
        {
            if (!isImported)
            {
                MessageBox.Show("No image to apply filter to");
                return;
            }
            else
            {
                Image img = pictureBox1.Image;
                Bitmap bmpInverted = new Bitmap(img.Width, img.Height);
                ImageAttributes ia = new ImageAttributes();
                ColorMatrix cmPicture = new ColorMatrix(pink_glow_Matrix);
                ia.SetColorMatrix(cmPicture);
                Graphics g = Graphics.FromImage(bmpInverted);
                g.DrawImage(img, new Rectangle(0, 0, img.Width, img.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, ia);
                g.Dispose();
                pictureBox1.Image = bmpInverted;
            }
        }

        void filter(int which)
        {
            if (!isImported)
            {
                MessageBox.Show("No image to apply filter to");
                return;
            }
            else
            {
                float[][] filter = null;
                switch (which)
                {
                    case 1:
                        filter = pink_glow_Matrix;
                        break;
                    case 2:
                        filter = winter_Matrix;
                        break;
                    case 3:
                        filter = lavender_Matrix;
                        break;
                    case 4:
                        filter = foggy_Filter;
                        break;
                    case 5:
                        filter = red_Filter;
                        break;
                    case 6:
                        filter = flash_Filter;
                        break;
                    case 7:
                        filter = freezing_cold_Filter;
                        break;
                    case 8:
                        filter = grayscale_Filter;
                        break;

                }
                Image img = pictureBox1.Image;
                Bitmap bmpInverted = new Bitmap(img.Width, img.Height);
                ImageAttributes ia = new ImageAttributes();
                ColorMatrix cmPicture = new ColorMatrix(filter);
                ia.SetColorMatrix(cmPicture);
                Graphics graphics = Graphics.FromImage(bmpInverted);
                graphics.DrawImage(img, new Rectangle(0, 0, img.Width, img.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, ia);
                graphics.Dispose();
                pictureBox1.Image = bmpInverted;

            }
        }

        void hue()
        {
            if (!isImported)
            {
                MessageBox.Show("Please import an image");
                redTracker.Value = 0;
                greenTracker.Value = 0;
                blueTracker.Value = 0;
                return;
            }

            resetImage();
            brightnessTracker.Value = 0;
            saturationTracker.Value = 0;
            contrastTracker.Value = 0;
            float red = redTracker.Value * 0.1f;
            float green = greenTracker.Value * 0.1f;
            float blue = blueTracker.Value * 0.1f;

            Image img = pictureBox1.Image;
            Bitmap bmpInverted = new Bitmap(img.Width, img.Height);
            ImageAttributes ia = new ImageAttributes();
            ColorMatrix cmPicture = new ColorMatrix(
                new float[][]{
                    new float[]{1+red, 0, 0, 0, 0},
                    new float[]{0, 1+green, 0, 0, 0},
                    new float[]{0, 0, 1+blue, 0, 0},
                    new float[]{0, 0, 0, 1, 0},
                    new float[]{0, 0, 0, 0, 1}
                });
            ia.SetColorMatrix(cmPicture);
            Graphics g = Graphics.FromImage(bmpInverted);
            g.DrawImage(img, new Rectangle(0, 0, img.Width, img.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, ia);
            g.Dispose();
            pictureBox1.Image = bmpInverted;
        }

        void adjustBrightness()
        {
            if (!isImported)
            {
                MessageBox.Show("Please import an image");
                return;
            }

            resetImage();
            float brightness = brightnessTracker.Value * 0.1f;

            Image img = pictureBox1.Image;
            Bitmap bmpInverted = new Bitmap(img.Width, img.Height);
            ImageAttributes ia = new ImageAttributes();
            ColorMatrix cmPicture = new ColorMatrix(
                new float[][]{
                    new float[] {1 + brightness, 0, 0, 0, 0},
                    new float[] {0, 1 + brightness, 0, 0, 0},
                    new float[] {0, 0, 1 + brightness, 0, 0},
                    new float[] {0, 0, 0, 1, 0},
                    new float[] {0, 0, 0, 0, 1},
                });
            ia.SetColorMatrix(cmPicture);
            Graphics g = Graphics.FromImage(bmpInverted);
            g.DrawImage(img, new Rectangle(0, 0, img.Width, img.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, ia);
            g.Dispose();
            pictureBox1.Image = bmpInverted;
        }

        private void adjustPictureParams()
        {
            if (!isImported)
            {
                MessageBox.Show("Please import an image");
                return;
            }

            resetImage();
            float r = redTracker.Value * 0.1f;
            float g = greenTracker.Value * 0.1f;
            float b = blueTracker.Value * 0.1f;
            float c = 1 + contrastTracker.Value * 0.1f;
            float t = 0.5f * (1.0f - c);
            float br = brightnessTracker.Value * 0.1f;
            float s = saturationTracker.Value * 0.1f;
            float lumR = 0.3086f;
            float lumG = 0.6094f;
            float lumB = 0.0820f;
            float sr = (1 - s) * lumR;
            float sg = (1 - s) * lumG;
            float sb = (1 - s) * lumB;

            Image img = pictureBox1.Image;
            Bitmap bmpInverted = new Bitmap(img.Width, img.Height);
            ImageAttributes ia = new ImageAttributes();
            ColorMatrix cmPicture = new ColorMatrix(
                new float[][]{
                    new float[] {c*(sr + s), c*sr, c*sr, 0, 0},
                    new float[] {c*sg, c*(sg + s), c*sg, 0, 0},
                    new float[] {c*sb, c*sb, c*(sb + s), 0, 0},
                    new float[] {0, 0, 0, 1, 0},
                    new float[] {t+br, t+br, t+br, 0, 1},
                }
            );
            ia.SetColorMatrix(cmPicture);
            Graphics graphics = Graphics.FromImage(bmpInverted);
            graphics.DrawImage(img, new Rectangle(0, 0, img.Width, img.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, ia);
            graphics.Dispose();
            pictureBox1.Image = bmpInverted;
        }

        void adjustContrast()
        {
            if (!isImported)
            {
                MessageBox.Show("Please import an image");
                return;
            }

            resetImage();
            float c = 1 + contrastTracker.Value * 0.1f;
            float t = 0.5f * (1.0f - c);

            Image img = pictureBox1.Image;
            Bitmap bmpInverted = new Bitmap(img.Width, img.Height);
            ImageAttributes ia = new ImageAttributes();
            ColorMatrix cmPicture = new ColorMatrix(
                new float[][]{
                    new float[] {c, 0, 0, 0, 0},
                    new float[] {0, c, 0, 0, 0},
                    new float[] {0, 0, c, 0, 0}, 
                    new float[] {0, 0, 0, 1, 0},
                    new float[] {t, t, t, 0, 1},
                }
            );
            ia.SetColorMatrix(cmPicture);
            Graphics g = Graphics.FromImage(bmpInverted);
            g.DrawImage(img, new Rectangle(0, 0, img.Width, img.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, ia);
            g.Dispose();
            pictureBox1.Image = bmpInverted;
        }

        private void button10_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            importImage();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            importImage();
        }

        private void button10_Click_1(object sender, EventArgs e)
        {
            saveImage();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            resetImage();
            filter(1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            resetImage();
            filter(2);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            resetImage();
            filter(3);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            resetImage();
            filter(4);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            resetImage();
            filter(5);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            resetImage();
            filter(6);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            resetImage();
            filter(7);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            resetImage();
            filter(8);
        }

        private void redTracker_ValueChanged(object sender, EventArgs e)
        {
            hue();
        }

        private void blueTracker_ValueChanged(object sender, EventArgs e)
        {
            hue();
        }

        private void greenTracker_ValueChanged(object sender, EventArgs e)
        {
            hue();
        }

        private void brightnessTracker_ValueChanged(object sender, EventArgs e)
        {
            adjustPictureParams();
        }

        private void contrastTracker_ValueChanged(object sender, EventArgs e)
        {
            adjustPictureParams();
        }

        private void saturationTracker_ValueChanged(object sender, EventArgs e)
        {
            adjustPictureParams();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (!isImported)
            {
                MessageBox.Show("No image to reset");
                return;
            }
            resetImage();
        }

        private void button12_Click(object sender, EventArgs e)
        {

        }
    }
}
