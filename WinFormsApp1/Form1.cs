namespace WinFormsApp1 {
    public partial class Form1 : Form {
        
        public Form1() {
            InitializeComponent();
        }
        int x, y;
        private void button1_Click(object sender, EventArgs e) {
            if (openFileDialog1.ShowDialog() == DialogResult.OK) {
                pictureBox1.Image = Image.FromFile(openFileDialog1.FileName);
            }
        }

        private void button2_Click(object sender, EventArgs e) {
            if (openFileDialog1.ShowDialog() == DialogResult.OK) {
                Image img =Image.FromFile(openFileDialog1.FileName);
                if (img.Size != pictureBox1.Image.Size) {
                    MessageBox.Show(" артинка и карта должны быть идентичных размеров");
                }
                else pictureBox2.Image = img;
                
            }
        }

        private void button3_Click(object sender, EventArgs e) {

            do_nice(250, 250);
        }
        void do_nice(int x,int y) {
            const int range = 2;
            const int sensivity = 2;
            int[] light_pos = { x, y };
            if (pictureBox1.Image != null &&
                pictureBox2.Image != null) {
                Bitmap b1 = new Bitmap(pictureBox1.Image);
                Bitmap b2 = new Bitmap(pictureBox2.Image);
                Bitmap result = new Bitmap(b1.Width, b1.Height);
                for (int i = 1; i < b1.Width - 1; i++) {
                    for (int j = 1; j < b1.Height - 1; j++) {
                        int[] light_vect = { light_pos[0] - i, light_pos[1] - j };
                        double[] norm_light_vect = normalize(light_vect);
                        int x_grad = 0;
                        int y_grad = 0;
                        if (i - range > 0 && j - range > 0 && i + range < b1.Width && j + range < b1.Height) {
                            x_grad = (int)Math.Floor((double)((b2.GetPixel(i - range, j).R - b2.GetPixel(i + range, j).R)/sensivity))*sensivity;
                            y_grad = (int)Math.Floor((double)((b2.GetPixel(i, j - range).R - b2.GetPixel(i, j + range).R) / sensivity))* sensivity;
                        }
                        int[] bump_vector = { x_grad, y_grad };
                        double[] norm_bump_vect = normalize(bump_vector);
                        result.SetPixel(i, j, calc_bump(norm_light_vect, norm_bump_vect, b1.GetPixel(i, j)));
                    }
                }
                pictureBox3.Image = result;
            }
        }
        Color calc_bump(double[] light,double[] bump,Color p1) {
            const int sensivity = 2;
            double intensity = dot_product(light, bump)/2;
            int R = (int)Math.Round(p1.R + Math.Floor((double)(intensity * 128/sensivity))*sensivity);
            if (R > 255) R = 255;
            if(R<0) R = 0;
            int G = (int)Math.Round(p1.G + Math.Floor((double)(intensity * 128 / sensivity)) * sensivity);
            if (G > 255) G = 255;
            if (G < 0) G = 0;
            int B = (int)Math.Round(p1.B + Math.Floor((double)(intensity * 128 / sensivity)) * sensivity);
            if (B > 255)B = 255;
            if (B < 0) B= 0;
            //return Color.FromArgb((int)Math.Round(p1.R * intensity), (int)Math.Round(p1.G * intensity), (int)Math.Round(p1.B * intensity)); 
            return Color.FromArgb(R,G,B);


        }
        double[] normalize(int[] a) {
            double[] result = new double[a.Length];
            double length = Math.Sqrt(a.Sum((cmp) => { return Math.Pow(cmp, 2); }));
            for (int i = 0; i < a.Length; i++)
                if (length != 0)
                    result[i] = a[i] / length;
                else result[i] = 0;
            return result;
        }
        double dot_product(double[] x, double[] y) {
            double result = 0;
            for (int i = 0; i < x.Length; i++) { 
                result+=x[i]*y[i];
            }
            return result;
        }

        private void pictureBox1_Click(object sender, EventArgs e) {
            do_nice(x,y);
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e) {
            x = e.X;
            y = e.Y;
            label1.Text = x.ToString();
            label2.Text = y.ToString();
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e) {
           
        }
    }
}