using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Spatial;

namespace LineCrossTest
{
    public partial class Form1 : Form
    {
        // SIZE
        private int ClientWidth = 600;
        private int ClientHeight = 600;
        private int HalfWidth = 300;
        private int HalfHeight = 300;

        // VECTOR
        Point2 pointA;
        Point2 pointB;

        Point2 pointC;
        Point2 pointD;

        public Form1()
        {
            InitializeComponent();
        }

        public Point TransformToLeftHand(Point2 point)
        {
            Matrix2 transformMat = new Matrix2(1, 0, 0, -1, 0, 0);
            Point2 newPoint = Matrix2.TransformPoint(transformMat, point);
            Point2 transformPoint = new Point2(ClientWidth * 0.5f, ClientHeight * 0.5f);
            Point2 resultPoint = newPoint + transformPoint;

            return new Point((int)resultPoint.X, (int)resultPoint.Y);
        }
        private void DrawBaseLine(Graphics g)
        {
            Pen basePen = new Pen(Color.White);
            basePen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot;
            basePen.Width = 1;

            Point2 startY = new Point2(0, -HalfHeight);
            Point2 endY = new Point2(0, HalfHeight);
            Point2 startX = new Point2(-HalfWidth, 0);
            Point2 endX = new Point2(HalfHeight, 0);

            g.DrawLine(basePen, TransformToLeftHand(startY),
                TransformToLeftHand(endY));
            g.DrawLine(basePen, TransformToLeftHand(startX),
                TransformToLeftHand(endX));
        }
        private void DrawBaseVector(Graphics g)
        {
            Pen pen = new Pen(Color.Black);
            pen.Width = 3;

            pointA = new Point2(-250, 0);
            pointB = new Point2(150, 0);

            pointC = new Point2(-60, 100);
            pointD = new Point2(50, -50);

            g.DrawLine(pen, TransformToLeftHand(pointA), TransformToLeftHand(pointB));
            g.DrawLine(pen, TransformToLeftHand(pointC), TransformToLeftHand(pointD));
        }
        private void CalcCrossPoint(Graphics g)
        {
            Pen calcedPen = new Pen(Color.Blue);
            calcedPen.Width = 2;

            Vector2 lineA2C = new Vector2(pointC.X - pointA.X, pointC.Y - pointA.Y);
            Point2 lineA2CEnd = new Point2(pointA.X + lineA2C.X, pointA.Y + lineA2C.Y);

            Vector2 lineA2B = new Vector2(pointA.X - pointB.X, pointA.Y - pointB.Y);
            Vector2 a2bNormal = lineA2B.Normalize();
            double a2cScala = lineA2C.Dot(a2bNormal);
            Vector2 lineA2E = a2cScala * a2bNormal;

            Point2 pointE = new Point2(pointA.X + lineA2E.X, pointA.Y + lineA2E.Y);
            Vector2 lineC2E = new Vector2(pointE.X - pointC.X, pointE.Y - pointC.Y);
            Vector2 c2eNormal = lineC2E.Normalize();

            Vector2 lineC2D = new Vector2(pointD.X - pointC.X, pointD.Y - pointC.Y);
            double c2dScala = lineC2D.Dot(c2eNormal);

            Vector2 lineC2F = c2dScala * c2eNormal;
            Point2 pointF = new Point2(pointC.X + lineC2F.X, pointC.Y + lineC2F.Y);

            double c2eLength = lineC2E.Magnitude();
            double c2fLength = lineC2F.Magnitude();
            double c2eRatio = c2eLength / c2fLength;

            Vector2 crossPointVec = lineC2D * c2eRatio;
            Point2 crossPoint = new Point2(pointC.X + crossPointVec.X, pointC.Y + crossPointVec.Y);

            g.DrawLine(calcedPen, TransformToLeftHand(pointA),TransformToLeftHand(lineA2CEnd));
            g.DrawLine(calcedPen, TransformToLeftHand(pointA), TransformToLeftHand(pointE));
            g.DrawLine(calcedPen, TransformToLeftHand(pointC), TransformToLeftHand(pointE));
            g.DrawLine(new Pen(Color.Red, 2), TransformToLeftHand(pointE), TransformToLeftHand(pointF));
            g.DrawLine(new Pen(Color.Green, 2), TransformToLeftHand(pointC), TransformToLeftHand(crossPoint));
        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            DrawBaseLine(e.Graphics);
            DrawBaseVector(e.Graphics);
            CalcCrossPoint(e.Graphics);
        }
    }
}
