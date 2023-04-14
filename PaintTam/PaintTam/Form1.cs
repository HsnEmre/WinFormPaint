using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;




namespace PaintTam
{
    public partial class Form1 : Form
    {
        Bitmap bm;
        Graphics g;
        bool ciz = false;
        Point px, py;
        Pen p = new Pen(Color.Black, 1);
        Pen sil = new Pen(Color.White, 10);
        int index;
        int x, y, Sx, Sy, Cx, Cy;
        ColorDialog cd = new ColorDialog();
        Color new_color;

        public Form1()
        {
            InitializeComponent();
            this.Width = 1500;
            this.Height = 1000;
            /* Piksel verileri tarafından tanımlanan görüntülerle çalışmak için kullanılan bir nesnedir.*/
            bm = new Bitmap(picB.Width, picB.Height);
            g = Graphics.FromImage(bm);//bitmap den gelen resim
            g.Clear(Color.White);
            picB.Image = bm;

        }

        private void picB_MouseDown(object sender, MouseEventArgs e)
        {
            //kullanıcı tuvale tıkladığında bool true olucak maous kordinatından çizmeye başlicak
            //tıklama noktası yani başlangıç noktası=py

            ciz = true;
            py = e.Location;

            //mause basıldığı zaman çizim için xy kordinatları
            Cx = e.X;
            Cy = e.Y;

        }

        private void picB_MouseMove(object sender, MouseEventArgs e)
        {
            /*bool değeri doğruysa ve index 1 olduğu zaman fare hareketiyle çizme işlemi*/
            if (ciz)
            {
                if (index == 1)
                {
                    px = e.Location;
                    g.DrawLine(p, px, py);//kalem başlangıç bitiş
                    py = px;

                }
                if (index == 2)
                {
                    px = e.Location;
                    g.DrawLine(sil, px, py);//kalem başlangıç bitiş
                    py = px;

                }
            }
            picB.Refresh();

            //fare hareket ederkenki başlangıç ve bitiş noktaları
            x = e.X;
            y = e.Y;
            Sx = e.X - Cx;
            Sy = e.Y - Cy;


        }

        private void picB_MouseUp(object sender, MouseEventArgs e)
        {
            //tıklama bittiğinde işlem false
            ciz = false;

            Sx = x - Cx;
            Sy = y - Cy;

            //fare basılı değil index 3 olduğu zaman daire çizme
            if (index == 3)
            {
                /*Bir Koordinat çifti, yükseklik ve genişlik ile belirtilen bir sınırlayıcı
                 dikdörtgenle tanımlanan bir elips çizer.*/
                //aşırı yükleme mevzusu
                g.DrawEllipse(p, Cx, Cy, Sx, Sy);
            }
            if (index == 4)
            {
                g.DrawRectangle(p, Cx, Cy, Sx, Sy);
            }
            if (index == 5)
            {
                g.DrawLine(p, Cx, Cy, Sx, Sy);
            }
        }


        private void picB_Paint(object sender, PaintEventArgs e)
        {
            /*mause basılıysa yani ciz ture ise şekilleri çizerken aynı anda ne çizdiğimi 
             * nasıl şekillendireceğimi görmem için gerekli metot
             */
            Graphics g = e.Graphics;


            if (ciz)
            {
                if (index == 3)
                {
                    /*Bir Koordinat çifti, yükseklik ve genişlik ile belirtilen bir sınırlayıcı
                     dikdörtgenle tanımlanan bir elips çizer.*/
                    //aşırı yükleme mevzusu
                    g.DrawEllipse(p, Cx, Cy, Sx, Sy);
                }
                if (index == 4)
                {
                    g.DrawRectangle(p, Cx, Cy, Sx, Sy);
                }
                if (index == 5)
                {
                    g.DrawLine(p, Cx, Cy, Sx, Sy);
                }
            }
        }

        private void btn_color_Click(object sender, EventArgs e)
        {
            cd.ShowDialog();
            new_color = cd.Color;
            pic_color.BackColor = cd.Color;
            p.Color = cd.Color;
        }
        static Point Set_Point(PictureBox pb, Point pt)
        {
            //renk paleti görüntü noktasını ayarlama
            float px = 1f * pb.Image.Width / pb.Width;
            float py = 1f * pb.Image.Height / pb.Height;
            return new Point((int)(pt.X * px), (int)(pt.Y * py));
        }

        private void btn_clear_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void color_picker_MouseClick(object sender, MouseEventArgs e)
        {
            Point point = Set_Point(color_picker, e.Location);
            pic_color.BackColor = ((Bitmap)color_picker.Image).GetPixel(point.X, point.Y);
            new_color = pic_color.BackColor;
            p.Color = pic_color.BackColor;
        }

        private void btn_pencil_Click(object sender, EventArgs e)
        {
            //kalem e basılınca index bir olarak ayarladım ve mause move olayına index i yolladım çizme işlemi başlatabilirm
            index = 1;
        }

        private void btn_eraser_Click(object sender, EventArgs e)
        {
            index = 2;
        }

        private void picB_MouseClick(object sender, MouseEventArgs e)
        {
            if (index == 7)
            {
                Point point = Set_Point(picB, e.Location);
                fill(bm, point.X, point.Y, new_color);
            }
        }

        private void Btn_fill_Click(object sender, EventArgs e)
        {
            index = 7;
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog();
            sfd.Filter = "Image(.jpg)|*.jpg|(*.*|*.*";
            if (sfd.ShowDialog()==DialogResult.OK)
            {
                Bitmap btm = bm.Clone(new Rectangle(0, 0, picB.Width, picB.Height),bm.PixelFormat);
                btm.Save(sfd.FileName, ImageFormat.jpeg);
            }
        }

        private void btn_ellipse_Click(object sender, EventArgs e)
        {
            index = 3;
        }
        private void btn_rect_Click(object sender, EventArgs e)
        {
            index = 4;
        }
        private void btn_line_Click(object sender, EventArgs e)
        {
            index = 5;
        }

        private void btn_clear_Click(object sender, EventArgs e)
        {
            g.Clear(Color.White);
            picB.Image = bm;
            index = 0;
        }

        private void validate(Bitmap bm, Stack<Point> sp, int x, int y, Color old_color, Color new_color)
        {

            Color cx = bm.GetPixel(x, y);
            if (cx == old_color)
            {
                sp.Push(new Point(x, y));
                bm.SetPixel(x, y, new_color);
            }
        }

        public void fill(Bitmap bm, int x, int y, Color new_clr)
        {
            Color old_color = bm.GetPixel(x, y);
            Stack<Point> pixel = new Stack<Point>();
            pixel.Push(new Point(x, y));
            bm.SetPixel(x, y, new_clr);
            if (old_color == new_clr) return;

            while (pixel.Count > 0)
            {
                Point pt = (Point)pixel.Pop();
                if (pt.X >= 0 && pt.Y > 0 && pt.X < bm.Width - 1 && pt.Y < bm.Height - 1)
                {
                    validate(bm, pixel, pt.X - 1, pt.Y, old_color, new_clr);
                    validate(bm, pixel, pt.X, pt.Y - 1, old_color, new_clr);
                    validate(bm, pixel, pt.X + 1, pt.Y, old_color, new_clr);
                    validate(bm, pixel, pt.X, pt.Y + 1, old_color, new_clr);
                }
            }


        }

    }

}
