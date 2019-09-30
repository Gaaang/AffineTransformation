using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AffineTransformations
{
    public partial class Form1 : Form
    {
        private Graphics g;
        private PointF clickedPoint = new PointF(0, 0); //точка по щелчку
        private Point[] edge = new Point[2]; //ребро
        private Point[] polygon = new Point[0];//полигон
        private Pen penColor = Pens.Black;
        private Point startPoint = Point.Empty, endPoint = Point.Empty;



        public Form1()
        {
            InitializeComponent();
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(pictureBox1.Image);
            g.Clear(Color.White);
            chosenPointTb.Text = string.Format("X: {0} Y: {1}", clickedPoint.X, clickedPoint.Y);
        }

        //при нажатии на мышь
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (edgeChk.Checked)//если выбрали отрезок
                {
                    startPoint = e.Location;
                }
                else if (polyChk.Checked && polygon.Length == 0)//если выбрали полигон
                {
                    startPoint = e.Location;
                    Array.Resize(ref polygon, 1);
                    polygon[polygon.Length - 1] = startPoint;
                }
            }
        }

        //при движении мыши
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (edgeChk.Checked)
                {
                    endPoint = e.Location;
                    pictureBox1.Invalidate();
                }
                else if (polyChk.Checked)
                {
                    endPoint = e.Location;
                    pictureBox1.Invalidate();
                }
            }
        }

        //отпускаем мышь
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (edgeChk.Checked)//если выбран отрезок
                {
                    edge[edge.Length - 2] = new Point(startPoint.X, startPoint.Y);
                    edge[edge.Length - 1] = new Point(endPoint.X, endPoint.Y);
                    Array.Resize(ref edge, edge.Length + 2);
                }
                else if (polyChk.Checked)//если выбран полигон
                {
                    Array.Resize(ref polygon, polygon.Length + 1);
                    polygon[polygon.Length - 1] = endPoint;
                    startPoint = endPoint;
                }
                else if (pointChk.Checked) //задаем координаты точки
                {
                    clickedPoint = e.Location;
                    chosenPointTb.Text = string.Format("X: {0} Y: {1}", e.Location.X, e.Location.Y);
                }
            }
            pictureBox1.Invalidate();
        }


        //делаем невозможным одновременный выбор режимов рисования полигона,отрезка и точки
        private void pointChk_CheckedChanged(object sender, EventArgs e)
        {
            if (pointChk.Checked)
            {
                polyChk.Checked = false;
                edgeChk.Checked = false;

            }
        }
        private void lineChk_CheckedChanged(object sender, EventArgs e)
        {
            if (edgeChk.Checked)
            {
                polyChk.Checked = false;
                pointChk.Checked = false;
            }
        }
        private void polyChk_CheckedChanged(object sender, EventArgs e)
        {
            if (polyChk.Checked)
            {
                edgeChk.Checked = false;
                pointChk.Checked = false;
            }
        }

        //прорисовка
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            for (int i = 0; i < edge.Length; i += 2)//рисуем все ребра
            {
                e.Graphics.DrawLine(penColor, edge[i], edge[i + 1]);
            }
            if (polygon.Length > 1)
                e.Graphics.DrawPolygon(penColor, polygon);
            e.Graphics.DrawLine(penColor, startPoint, endPoint);
            e.Graphics.DrawEllipse(penColor, clickedPoint.X - 1, clickedPoint.Y - 1, 3, 3);
        }

        //очистить экран и сбросить настройки
        private void clearBtn_Click(object sender, EventArgs e)
        {
            g.Clear(Color.White);
            pictureBox1.Invalidate();
            Array.Clear(edge, 0, edge.Length);
            Array.Clear(polygon, 0, polygon.Length);
            Array.Resize(ref edge, 2);
            Array.Resize(ref polygon, 0);
            clickedPoint.X = 0f; clickedPoint.Y = 0f;
            startPoint = endPoint = Point.Empty;
        }



    }
}
