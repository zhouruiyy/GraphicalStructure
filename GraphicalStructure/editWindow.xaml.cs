﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data;
using System.Collections;

namespace GraphicalStructure
{
    /// <summary>
    /// editWindow.xaml 的交互逻辑
    /// </summary>
    /// 
    public delegate void ChangeTextHandler();
    public delegate void ChangeShapeHandler(double radius,int a);
    public delegate void ChangeCoverLocation(Canvas c, StackPanel sp);

    public partial class editWindow : Window
    {
        public Components currentCom;
        public Components rightCom;
        public Components leftCom;
        private double leftLength;
        private double rightLength;

        public double left_X;
        public double pathWidth;

        private string shape;
        private string originShape;
        bool isFirstIn = true;

        public Canvas canvas;
        public StackPanel sp;

        public event ChangeTextHandler ChangeTextEvent;
        public event ChangeShapeHandler ChangeShapeEvent;
        public event ChangeShapeHandler ChangeShapeEvent2;
        public event ChangeShapeHandler ChangeShapeEvent3;
        public event ChangeCoverLocation ChangeCoverEvent;

        public string isLeftEndCap = "";
        public string isRightEndCap = "";

        public bool isComboxChanged = false;

        private static UseAccessDB accessDb;

        public DataSet ds;
        public DataTable dt;

        private Color color = new Color();

        public editWindow()
        {
            InitializeComponent();

            accessDb = new UseAccessDB();
            accessDb.getConnection();
            ds = accessDb.SelectToDataSet("select * from material","material");
            dt = ds.Tables[0];
            ArrayList col = new ArrayList();
            foreach(DataRow row in dt.Rows)
            {
                Console.WriteLine(row[dt.Columns[1]]);
                col.Add(row[dt.Columns[1]]);
            }
            materialBox.ItemsSource = col;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (ChangeTextEvent != null)
            {
                ChangeTextEvent();
            }
            if (ChangeCoverEvent != null)
            {
                ChangeCoverEvent(canvas,sp);
            }

            leftCom = null;
            rightCom = null;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            leftD_Changed();
            rightD_Changed();
            //if (rightLength.ToString() != rightD.Text)
            //{
            //    rightD_Changed();
            //    if (rightCom != null)
            //    {
            //        currentCom = rightCom;
            //        leftD.Text = rightD.Text;
            //        leftD_Changed();
            //        leftLength = rightLength;
            //    }
            //}
            //if (leftLength.ToString() != leftD.Text)
            //{
            //    leftD_Changed();
            //    if (leftCom != null)
            //    {
            //        currentCom = leftCom;
            //        rightD.Text = leftD.Text;
            //        rightD_Changed();
            //        rightLength = leftLength;
            //    }
            //}
            

            if (Double.Parse(segmentWidth.Text) != pathWidth)
            {
                ChangePathWidth();
            }

            update_LayerCoordinate();
            ChangeColor();

            accessDb.closeDB();

            //changeLineToCurve();

            //SaveCubeXValue();

            ////改变形状

            double radius = 120;
            if (radiusText.Text != null && radiusText.Text != "")
            {
                radius = Double.Parse(radiusText.Text);
            }
            
            changeShape(radius);
            currentCom.radius = radius;
            
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        public void setComponent(Components c)
        {
            this.currentCom = c;

            showComponentInfo();
        }

        public void setRightComponent(Components c)
        {
            this.rightCom = c;
        }

        public void setLeftComponent(Components c)
        {
            this.leftCom = c;
        }

        //显示当前信息
        public void showComponentInfo()
        {
            leftX.Text = left_X.ToString();

            leftLength = Math.Abs(currentCom.startPoint.Y - currentCom.point2.Y);
            leftD.Text = leftLength.ToString();

            rightX.Text = (left_X + currentCom.point3.X).ToString();

            rightLength = Math.Abs(currentCom.point3.Y - currentCom.point4.Y);
            rightD.Text = rightLength.ToString();

            segmentWidth.Text = (Double.Parse(rightX.Text) - Double.Parse(leftX.Text)).ToString();
            pathWidth = Double.Parse(segmentWidth.Text);

            Console.WriteLine(currentCom.newPath.Fill);

            foreach (DataRow row in dt.Rows)
            {
                if (row[dt.Columns[3]].ToString() == currentCom.newPath.Fill.ToString())
                {
                    Console.WriteLine((row[dt.Columns[1]]));
                    materialBox.SelectedItem = row[dt.Columns[1]];
                    Console.WriteLine(Brushes.DarkSeaGreen);
                }
            }

            CubeXValue.Text = currentCom.cubeOffset.ToString();


            if (currentCom.isChangeOgive)
            {
                Ogive.IsChecked = true;
                InverseOgive.IsChecked = false;
                Cylinder.IsChecked = false;
                radiusText.Text = currentCom.radius.ToString();
            }
            else if (currentCom.isChangeIOgive)
            {
                Ogive.IsChecked = false;
                InverseOgive.IsChecked = true;
                Cylinder.IsChecked = false;
                radiusText.Text = currentCom.radius.ToString();
            }
            else
            {
                Ogive.IsChecked = false;
                InverseOgive.IsChecked = false;
                Cylinder.IsChecked = true;
            }

            if(currentCom.cubeOffset > 0)
            {
                RadioCube.IsChecked = true;
            }
        }

        public void leftD_Changed()
        {
            if (currentCom.geometryGroup.Children.Count <= 1)
            {
                if (Double.Parse(leftD.Text) != leftLength)
                {
                    if (Double.Parse(leftD.Text) > leftLength)
                    {
                        if (Double.Parse(leftD.Text) >= rightLength)
                        {
                            double temp = Double.Parse(leftD.Text) - leftLength;
                            currentCom.point2.Y += (temp - currentCom.startPoint.Y);
                            currentCom.point3.Y += (temp / 2.0 - currentCom.startPoint.Y);
                            currentCom.point4.Y += (temp / 2.0 - currentCom.startPoint.Y);
                            currentCom.startPoint.Y = 0;
                        }
                        else
                        {
                            double temp = Double.Parse(leftD.Text) - leftLength;
                            currentCom.startPoint.Y -= (temp / 2.0 + currentCom.point4.Y);
                            currentCom.point2.Y += (temp / 2.0 - currentCom.point4.Y);
                            currentCom.point3.Y -= currentCom.point4.Y;
                            currentCom.point4.Y = 0;
                        }
                    }
                    else
                    {
                        if (Double.Parse(leftD.Text) >= rightLength)
                        {
                            double temp = leftLength - Double.Parse(leftD.Text);
                            currentCom.point2.Y -= (temp + currentCom.startPoint.Y);
                            currentCom.point3.Y -= (temp / 2.0 + currentCom.startPoint.Y);
                            currentCom.point4.Y -= (temp / 2.0 + currentCom.startPoint.Y);
                            currentCom.startPoint.Y = 0;
                        }
                        else
                        {
                            double temp = leftLength - Double.Parse(leftD.Text);
                            currentCom.startPoint.Y += (temp / 2.0 - currentCom.point4.Y);
                            currentCom.point2.Y -= (temp / 2.0 + currentCom.point4.Y);
                            currentCom.point3.Y -= currentCom.point4.Y;
                            currentCom.point4.Y = 0;
                        }
                    }
                    currentCom.startPoint.Y += 100;
                    currentCom.point2.Y += 100;
                    currentCom.point3.Y += 100;
                    currentCom.point4.Y += 100;
                    currentCom.pf.StartPoint = currentCom.startPoint;
                    currentCom.ls.Point = currentCom.point2;
                    currentCom.ls2.Point = currentCom.point3;
                    currentCom.ls3.Point = currentCom.point4;
                    currentCom.ls4.Point = currentCom.startPoint;

                    double h1 = Math.Abs(currentCom.startPoint.Y - currentCom.point2.Y);
                    double h2 = Math.Abs(currentCom.point3.Y - currentCom.point4.Y);
                    double maxHeight = h1 > h2 ? h1 : h2;
                    if ((currentCom.point2.Y > currentCom.point3.Y ? currentCom.point2.Y : currentCom.point3.Y) > maxHeight)
                    {
                        currentCom.newPath.Height = (currentCom.point2.Y > currentCom.point3.Y ? currentCom.point2.Y : currentCom.point3.Y) + 100;
                    }
                    else
                    {
                        currentCom.newPath.Height = maxHeight + 100;
                        currentCom.height = maxHeight + 100;
                    }
                }
            }
            else
            {
                int geometryGroupCount = currentCom.geometryGroup.Children.Count;
                double layerHeight = 0;

                for (int i = geometryGroupCount-1; i >= 1; i -= 2)
                {
                    PathGeometry pg = currentCom.geometryGroup.Children[i] as PathGeometry;
                    PathFigure curPf = pg.Figures.ElementAt(0);

                    double leftHeight = Math.Abs(((LineSegment)(curPf.Segments[0])).Point.Y - curPf.StartPoint.Y);
                    double rightHeight = 0;
                    if (curPf.Segments[1] is LineSegment)
                    {
                        rightHeight = Math.Abs(((LineSegment)(curPf.Segments[1])).Point.Y - ((LineSegment)(curPf.Segments[2])).Point.Y);
                    }
                    else if (curPf.Segments[1] is ArcSegment)
                    {
                        rightHeight = Math.Abs(((ArcSegment)(curPf.Segments[1])).Point.Y - ((LineSegment)(curPf.Segments[2])).Point.Y);
                    }
                    else
                    {
                        rightHeight = Math.Abs(((PolyLineSegment)(curPf.Segments[1])).Points[((PolyLineSegment)(curPf.Segments[1])).Points.Count - 1].Y - ((LineSegment)(curPf.Segments[2])).Point.Y);
                    }
                    
                    double maxHeight = leftHeight > rightHeight ? leftHeight : rightHeight;
                    layerHeight += maxHeight;
                }
                

                if (Double.Parse(leftD.Text) != leftLength)
                {
                    if (Double.Parse(leftD.Text) > leftLength)
                    {
                        if (Double.Parse(leftD.Text) >= rightLength)
                        {
                            double temp = Double.Parse(leftD.Text) - leftLength;
                            currentCom.point2.Y += (temp - currentCom.startPoint.Y + layerHeight);
                            currentCom.point3.Y += (temp / 2.0 - currentCom.startPoint.Y + layerHeight);
                            currentCom.point4.Y += (temp / 2.0 - currentCom.startPoint.Y + layerHeight);
                            currentCom.startPoint.Y = 0 + layerHeight;
                        }
                        else
                        {
                            double temp = Double.Parse(leftD.Text) - leftLength;
                            currentCom.startPoint.Y -= (temp / 2.0 + currentCom.point4.Y - layerHeight);
                            currentCom.point2.Y += (temp / 2.0 - currentCom.point4.Y + layerHeight);
                            currentCom.point3.Y -= (currentCom.point4.Y - layerHeight);
                            currentCom.point4.Y = 0 + layerHeight;
                        }
                    }
                    else
                    {
                        if (Double.Parse(leftD.Text) >= rightLength)
                        {
                            double temp = leftLength - Double.Parse(leftD.Text);
                            currentCom.point2.Y -= (temp + currentCom.startPoint.Y - layerHeight);
                            currentCom.point3.Y -= (temp / 2.0 + currentCom.startPoint.Y - layerHeight);
                            currentCom.point4.Y -= (temp / 2.0 + currentCom.startPoint.Y - layerHeight);
                            currentCom.startPoint.Y = 0 + layerHeight;
                        }
                        else
                        {
                            double temp = leftLength - Double.Parse(leftD.Text);
                            currentCom.startPoint.Y += (temp / 2.0 - currentCom.point4.Y + layerHeight);
                            currentCom.point2.Y -= (temp / 2.0 + currentCom.point4.Y - layerHeight);
                            currentCom.point3.Y -= (currentCom.point4.Y - layerHeight);
                            currentCom.point4.Y = 0 + layerHeight;
                        }
                    }
                    currentCom.startPoint.Y += 100;
                    currentCom.point2.Y += 100;
                    currentCom.point3.Y += 100;
                    currentCom.point4.Y += 100;
                    currentCom.pf.StartPoint = currentCom.startPoint;
                    currentCom.ls.Point = currentCom.point2;
                    currentCom.ls2.Point = currentCom.point3;
                    currentCom.ls3.Point = currentCom.point4;
                    currentCom.ls4.Point = currentCom.startPoint;

                    double h1 = Math.Abs(currentCom.startPoint.Y - currentCom.point2.Y);
                    double h2 = Math.Abs(currentCom.point3.Y - currentCom.point4.Y);
                    double maxHeight = h1 > h2 ? h1 : h2;
                    if ((currentCom.point2.Y > currentCom.point3.Y ? currentCom.point2.Y : currentCom.point3.Y) > maxHeight)
                    {
                        currentCom.newPath.Height = (currentCom.point2.Y > currentCom.point3.Y ? currentCom.point2.Y : currentCom.point3.Y) + layerHeight + 100;
                    }
                    else
                    {
                        currentCom.newPath.Height = maxHeight + layerHeight + 100;
                        currentCom.height = maxHeight + layerHeight + 100;
                    }
                }
            }
            leftLength = Double.Parse(leftD.Text.ToString());
        }

        //更新层的坐标
        public void update_LayerCoordinate()
        {
            PathGeometry basePg = (PathGeometry)currentCom.geometryGroup.Children[0];
            PathFigure basePf = basePg.Figures.ElementAt(0);

            for (int i = 1; i < currentCom.geometryGroup.Children.Count;i += 2)
            {
                if (i == 1)
                {
                    PathGeometry topPg = (PathGeometry)currentCom.geometryGroup.Children[1];
                    PathFigure topPf = topPg.Figures.ElementAt(0);
                    double leftlayerHeight = 0;
                    double rightlayerHeight = 0;
                    if (topPf.Segments[0] is LineSegment)
                    {
                        leftlayerHeight = Math.Abs(((LineSegment)(topPf.Segments[2])).Point.Y - topPf.StartPoint.Y);
                        rightlayerHeight = Math.Abs(((LineSegment)(topPf.Segments[1])).Point.Y - ((LineSegment)(topPf.Segments[0])).Point.Y);
                    }
                    else if (topPf.Segments[0] is ArcSegment)
                    {
                        leftlayerHeight = Math.Abs(((ArcSegment)(topPf.Segments[2])).Point.Y - topPf.StartPoint.Y);
                        rightlayerHeight = Math.Abs(((LineSegment)(topPf.Segments[1])).Point.Y - ((ArcSegment)(topPf.Segments[0])).Point.Y);
                    }
                    else
                    {
                        leftlayerHeight = Math.Abs(((PolyLineSegment)(topPf.Segments[2])).Points[((PolyLineSegment)(topPf.Segments[2])).Points.Count - 1].Y - topPf.StartPoint.Y);
                        rightlayerHeight = Math.Abs(((LineSegment)(topPf.Segments[1])).Point.Y - ((PolyLineSegment)(topPf.Segments[0])).Points[((PolyLineSegment)(topPf.Segments[0])).Points.Count - 1].Y);
                    }
                    
                    //rightlayerHeight = Math.Abs(((LineSegment)(topPf.Segments[1])).Point.Y - ((LineSegment)(topPf.Segments[0])).Point.Y);

                    topPf.StartPoint = basePf.StartPoint;
                    if (topPf.Segments[0] is LineSegment)
                    {
                        ((LineSegment)(topPf.Segments[2])).Point = new Point(basePf.StartPoint.X, basePf.StartPoint.Y - leftlayerHeight);
                        ((LineSegment)(topPf.Segments[0])).Point = new Point(((LineSegment)basePf.Segments[2]).Point.X, ((LineSegment)basePf.Segments[2]).Point.Y);
                        ((LineSegment)(topPf.Segments[1])).Point = new Point(((LineSegment)basePf.Segments[2]).Point.X, ((LineSegment)basePf.Segments[2]).Point.Y - rightlayerHeight);
                    }
                    else if (topPf.Segments[0] is ArcSegment)
                    {
                        ((ArcSegment)(topPf.Segments[2])).Point = new Point(basePf.StartPoint.X, basePf.StartPoint.Y - leftlayerHeight);
                        ((ArcSegment)(topPf.Segments[0])).Point = new Point(((LineSegment)basePf.Segments[2]).Point.X, ((LineSegment)basePf.Segments[2]).Point.Y);
                        ((LineSegment)(topPf.Segments[1])).Point = new Point(((LineSegment)basePf.Segments[2]).Point.X, ((LineSegment)basePf.Segments[2]).Point.Y - rightlayerHeight);
                    }
                    else
                    {
                        
                    }
                    //((LineSegment)(topPf.Segments[2])).Point = new Point(basePf.StartPoint.X, basePf.StartPoint.Y - leftlayerHeight);
                    //((LineSegment)(topPf.Segments[0])).Point = new Point(((LineSegment)basePf.Segments[2]).Point.X, ((LineSegment)basePf.Segments[2]).Point.Y);
                    //((LineSegment)(topPf.Segments[1])).Point = new Point(((LineSegment)basePf.Segments[2]).Point.X, ((LineSegment)basePf.Segments[2]).Point.Y - rightlayerHeight);
                    ////((LineSegment)(topPf.Segments[3])).Point = basePf.StartPoint;

                    PathGeometry bottomPg = (PathGeometry)currentCom.geometryGroup.Children[2];
                    PathFigure bottomPf = bottomPg.Figures.ElementAt(0);

                    bottomPf.StartPoint = ((LineSegment)basePf.Segments[0]).Point;
                    if (bottomPf.Segments[1] is LineSegment)
                    {
                        ((LineSegment)(bottomPf.Segments[0])).Point = new Point(bottomPf.StartPoint.X, bottomPf.StartPoint.Y + leftlayerHeight);
                        ((LineSegment)(bottomPf.Segments[2])).Point = new Point(((LineSegment)basePf.Segments[1]).Point.X, ((LineSegment)basePf.Segments[1]).Point.Y);
                        ((LineSegment)(bottomPf.Segments[1])).Point = new Point(((LineSegment)basePf.Segments[1]).Point.X, ((LineSegment)basePf.Segments[1]).Point.Y + rightlayerHeight);
                    }
                    else if (bottomPf.Segments[1] is ArcSegment)
                    {
                        ((LineSegment)(bottomPf.Segments[0])).Point = new Point(bottomPf.StartPoint.X, bottomPf.StartPoint.Y + leftlayerHeight);
                        ((LineSegment)(bottomPf.Segments[2])).Point = new Point(((ArcSegment)basePf.Segments[1]).Point.X, ((ArcSegment)basePf.Segments[1]).Point.Y);
                        ((ArcSegment)(bottomPf.Segments[1])).Point = new Point(((ArcSegment)basePf.Segments[1]).Point.X, ((ArcSegment)basePf.Segments[1]).Point.Y + rightlayerHeight);
                        ((ArcSegment)(bottomPf.Segments[3])).Point = new Point(bottomPf.StartPoint.X, bottomPf.StartPoint.Y);
                    }
                    else
                    {

                    }
                    //((LineSegment)(bottomPf.Segments[0])).Point = new Point(bottomPf.StartPoint.X, bottomPf.StartPoint.Y + leftlayerHeight);
                    //((LineSegment)(bottomPf.Segments[2])).Point = new Point(((LineSegment)basePf.Segments[1]).Point.X, ((LineSegment)basePf.Segments[1]).Point.Y);
                    //((LineSegment)(bottomPf.Segments[1])).Point = new Point(((LineSegment)basePf.Segments[1]).Point.X, ((LineSegment)basePf.Segments[1]).Point.Y + rightlayerHeight);
                }
                else
                {
                    PathGeometry topPg = (PathGeometry)currentCom.geometryGroup.Children[i];
                    PathFigure topPf = topPg.Figures.ElementAt(0);
                    double leftlayerHeight = 0;// Math.Abs(((LineSegment)(topPf.Segments[2])).Point.Y - topPf.StartPoint.Y);
                    double rightlayerHeight = 0;// Math.Abs(((LineSegment)(topPf.Segments[1])).Point.Y - ((LineSegment)(topPf.Segments[0])).Point.Y);

                    if (topPf.Segments[0] is LineSegment)
                    {
                        leftlayerHeight = Math.Abs(((LineSegment)(topPf.Segments[2])).Point.Y - topPf.StartPoint.Y);
                        rightlayerHeight = Math.Abs(((LineSegment)(topPf.Segments[1])).Point.Y - ((LineSegment)(topPf.Segments[0])).Point.Y);
                    }
                    else if (topPf.Segments[0] is ArcSegment)
                    {
                        leftlayerHeight = Math.Abs(((ArcSegment)(topPf.Segments[2])).Point.Y - topPf.StartPoint.Y);
                        rightlayerHeight = Math.Abs(((LineSegment)(topPf.Segments[1])).Point.Y - ((ArcSegment)(topPf.Segments[0])).Point.Y);
                    }
                    else
                    {
                        leftlayerHeight = Math.Abs(((PolyLineSegment)(topPf.Segments[2])).Points[((PolyLineSegment)(topPf.Segments[2])).Points.Count - 1].Y - topPf.StartPoint.Y);
                        rightlayerHeight = Math.Abs(((LineSegment)(topPf.Segments[1])).Point.Y - ((PolyLineSegment)(topPf.Segments[0])).Points[((PolyLineSegment)(topPf.Segments[0])).Points.Count - 1].Y);
                    }

                    PathGeometry last_topPg = (PathGeometry)currentCom.geometryGroup.Children[i-2];
                    PathFigure last_topPf = last_topPg.Figures.ElementAt(0);


                    //((LineSegment)(topPf.Segments[2])).Point = new Point(topPf.StartPoint.X, topPf.StartPoint.Y - leftlayerHeight);
                    //((LineSegment)(topPf.Segments[0])).Point = ((LineSegment)(last_topPf.Segments[1])).Point;
                    //((LineSegment)(topPf.Segments[1])).Point = new Point(((LineSegment)(topPf.Segments[0])).Point.X, ((LineSegment)(topPf.Segments[0])).Point.Y - rightlayerHeight);
                    if (topPf.Segments[0] is LineSegment)
                    {
                        topPf.StartPoint = ((LineSegment)(last_topPf.Segments[2])).Point;
                        ((LineSegment)(topPf.Segments[2])).Point = new Point(topPf.StartPoint.X, topPf.StartPoint.Y - leftlayerHeight);
                        ((LineSegment)(topPf.Segments[0])).Point = ((LineSegment)(last_topPf.Segments[1])).Point;
                        ((LineSegment)(topPf.Segments[1])).Point = new Point(((LineSegment)(topPf.Segments[0])).Point.X, ((LineSegment)(topPf.Segments[0])).Point.Y - rightlayerHeight);
                    }
                    else if (topPf.Segments[0] is ArcSegment)
                    {
                        topPf.StartPoint = ((ArcSegment)(last_topPf.Segments[2])).Point;
                        ((ArcSegment)(topPf.Segments[2])).Point = new Point(topPf.StartPoint.X, topPf.StartPoint.Y - leftlayerHeight);
                        ((ArcSegment)(topPf.Segments[0])).Point = ((LineSegment)(last_topPf.Segments[1])).Point;
                        ((LineSegment)(topPf.Segments[1])).Point = new Point(((ArcSegment)(topPf.Segments[0])).Point.X, ((ArcSegment)(topPf.Segments[0])).Point.Y - rightlayerHeight);
                    }
                    else
                    {

                    }


                    PathGeometry bottomPg = (PathGeometry)currentCom.geometryGroup.Children[i + 1];
                    PathFigure bottomPf = bottomPg.Figures.ElementAt(0);

                    PathGeometry last_bottomPg = (PathGeometry)currentCom.geometryGroup.Children[i - 1];
                    PathFigure last_bottomPf = last_bottomPg.Figures.ElementAt(0);

                    bottomPf.StartPoint = ((LineSegment)last_bottomPf.Segments[0]).Point;
                    //((LineSegment)(bottomPf.Segments[0])).Point = new Point(bottomPf.StartPoint.X, bottomPf.StartPoint.Y + leftlayerHeight);
                    //((LineSegment)(bottomPf.Segments[2])).Point = new Point(((LineSegment)last_bottomPf.Segments[1]).Point.X, ((LineSegment)last_bottomPf.Segments[1]).Point.Y);
                    //((LineSegment)(bottomPf.Segments[1])).Point = new Point(((LineSegment)last_bottomPf.Segments[1]).Point.X, ((LineSegment)last_bottomPf.Segments[1]).Point.Y + rightlayerHeight);
                    if (bottomPf.Segments[1] is LineSegment)
                    {
                        ((LineSegment)(bottomPf.Segments[0])).Point = new Point(bottomPf.StartPoint.X, bottomPf.StartPoint.Y + leftlayerHeight);
                        ((LineSegment)(bottomPf.Segments[2])).Point = new Point(((LineSegment)last_bottomPf.Segments[1]).Point.X, ((LineSegment)last_bottomPf.Segments[1]).Point.Y);
                        ((LineSegment)(bottomPf.Segments[1])).Point = new Point(((LineSegment)last_bottomPf.Segments[1]).Point.X, ((LineSegment)last_bottomPf.Segments[1]).Point.Y + rightlayerHeight);
                    }
                    else if (bottomPf.Segments[1] is ArcSegment)
                    {
                        ((LineSegment)(bottomPf.Segments[0])).Point = new Point(bottomPf.StartPoint.X, bottomPf.StartPoint.Y + leftlayerHeight);
                        ((LineSegment)(bottomPf.Segments[2])).Point = new Point(((ArcSegment)last_bottomPf.Segments[1]).Point.X, ((ArcSegment)last_bottomPf.Segments[1]).Point.Y);
                        ((ArcSegment)(bottomPf.Segments[1])).Point = new Point(((ArcSegment)last_bottomPf.Segments[1]).Point.X, ((ArcSegment)last_bottomPf.Segments[1]).Point.Y + rightlayerHeight);
                        ((ArcSegment)(bottomPf.Segments[3])).Point = new Point(bottomPf.StartPoint.X, bottomPf.StartPoint.Y);
                    }
                    else
                    {

                    }
                }
            } 
        }

        //public void changeLineToCurve()
        //{
        //    GeometryGroup geometryGroup = currentCom.geometryGroup;
        //    PathGeometry curPg = (PathGeometry)geometryGroup.Children[0];
        //    PathFigure curPf = curPg.Figures.ElementAt(0);

        //    if (RaidusY.Text != "0" || RadiusX.Text != "0")
        //    {
        //        //初始化曲线
        //        ArcSegment as_bottom = new ArcSegment();
        //        if (curPf.Segments[1] is LineSegment)
        //        {
        //            as_bottom.Point = ((LineSegment)curPf.Segments[1]).Point;
        //        }
        //        else
        //        {
        //            as_bottom.Point = ((ArcSegment)curPf.Segments[1]).Point;
        //        }

        //        as_bottom.Size = new Size(Int32.Parse(RadiusX.Text), Int32.Parse(RaidusY.Text));
                
        //        ArcSegment as_top = new ArcSegment();
        //        as_top.Point = curPf.StartPoint;
        //        as_top.Size = new Size(Int32.Parse(RadiusX.Text), Int32.Parse(RaidusY.Text));

        //        //判断是顺时针还是逆时针
        //        if (clockWise.IsChecked == true)
        //        {
        //            as_bottom.SweepDirection = SweepDirection.Clockwise;
        //            as_top.SweepDirection = SweepDirection.Clockwise;
        //        }
        //        else
        //        {
        //            as_bottom.SweepDirection = SweepDirection.Counterclockwise;
        //            as_top.SweepDirection = SweepDirection.Counterclockwise;
        //        }
                
        //        //删除直线 添加曲线
        //        curPf.Segments[1] = as_bottom;
        //        curPf.Segments.Add(as_top);
        //    }
        //}

        public void rightD_Changed()
        {
            if (currentCom.geometryGroup.Children.Count <= 1)
            {
                if (Double.Parse(rightD.Text) != rightLength)
                {
                    if (Double.Parse(rightD.Text) >= rightLength)
                    {
                        if (Double.Parse(rightD.Text) <= leftLength)
                        {
                            double temp = Double.Parse(rightD.Text) - rightLength;
                            currentCom.point3.Y += temp / 2.0;
                            currentCom.point4.Y -= temp / 2.0;
                        }
                        else
                        {
                            double temp = Double.Parse(rightD.Text) - rightLength;
                            currentCom.startPoint.Y += (temp / 2.0 - currentCom.point4.Y);
                            currentCom.point2.Y += (temp / 2.0 - currentCom.point4.Y);
                            currentCom.point3.Y += (temp - currentCom.point4.Y);
                            currentCom.point4.Y = 0;

                            currentCom.startPoint.Y += 100;
                            currentCom.point2.Y += 100;
                            currentCom.point3.Y += 100;
                            currentCom.point4.Y += 100;
                        }
                    }
                    else
                    {
                        if (Double.Parse(rightD.Text) > leftLength)
                        {
                            double temp = rightLength - Double.Parse(rightD.Text);
                            currentCom.point2.Y -= temp / 2.0;
                            currentCom.point3.Y -= temp;
                            currentCom.startPoint.Y -= temp / 2.0;
                        }
                        else
                        {
                            double temp = rightLength - Double.Parse(rightD.Text);
                            currentCom.point2.Y -= currentCom.startPoint.Y;
                            currentCom.point3.Y -= (temp / 2.0 + currentCom.startPoint.Y);
                            currentCom.point4.Y += (temp / 2.0 - currentCom.startPoint.Y);
                            currentCom.startPoint.Y = 0;

                            currentCom.startPoint.Y += 100;
                            currentCom.point2.Y += 100;
                            currentCom.point3.Y += 100;
                            currentCom.point4.Y += 100;
                        }
                    }
                    
                    currentCom.pf.StartPoint = currentCom.startPoint;
                    currentCom.ls.Point = currentCom.point2;
                    //currentCom.setLs2(currentCom.point3);
                    ((LineSegment)currentCom.pf.Segments[1]).Point = new Point(currentCom.point3.X, currentCom.point3.Y);
                    //currentCom.ls2.Point = new Point(currentCom.point3.X,currentCom.point3.Y);
                    currentCom.ls3.Point = currentCom.point4;
                    currentCom.ls4.Point = currentCom.startPoint;

                    double h1 = Math.Abs(currentCom.startPoint.Y - currentCom.point2.Y);
                    double h2 = Math.Abs(currentCom.point3.Y - currentCom.point4.Y);
                    double maxHeight = h1 > h2 ? h1 : h2;
                    if ((currentCom.point2.Y > currentCom.point3.Y ? currentCom.point2.Y : currentCom.point3.Y) > maxHeight)
                    {
                        currentCom.newPath.Height = (currentCom.point2.Y > currentCom.point3.Y ? currentCom.point2.Y : currentCom.point3.Y) + 100;
                    }
                    else
                    {
                        currentCom.newPath.Height = maxHeight + 100;
                        currentCom.height = maxHeight + 100;
                    }
                }
            }
            else
            {
                int geometryGroupCount = currentCom.geometryGroup.Children.Count;
                double layerHeight = 0;

                for (int i = geometryGroupCount - 1; i >= 1; i -= 2)
                {
                    PathGeometry pg = currentCom.geometryGroup.Children[i] as PathGeometry;
                    PathFigure curPf = pg.Figures.ElementAt(0);

                    double leftHeight = Math.Abs(((LineSegment)(curPf.Segments[0])).Point.Y - curPf.StartPoint.Y);
                    double rightHeight = 0;// = Math.Abs(((LineSegment)(curPf.Segments[1])).Point.Y - ((LineSegment)(curPf.Segments[2])).Point.Y);
                    if (curPf.Segments[1] is LineSegment)
                    {
                        rightHeight = Math.Abs(((LineSegment)(curPf.Segments[1])).Point.Y - ((LineSegment)(curPf.Segments[2])).Point.Y);
                    }
                    else if (curPf.Segments[1] is ArcSegment)
                    {
                        rightHeight = Math.Abs(((ArcSegment)(curPf.Segments[1])).Point.Y - ((LineSegment)(curPf.Segments[2])).Point.Y);
                    }
                    else
                    {
                        rightHeight = Math.Abs(((PolyLineSegment)(curPf.Segments[1])).Points[((PolyLineSegment)(curPf.Segments[1])).Points.Count - 1].Y - ((LineSegment)(curPf.Segments[2])).Point.Y);
                    }
                    double maxHeight = leftHeight > rightHeight ? leftHeight : rightHeight;
                    layerHeight += maxHeight;
                }

                if (Double.Parse(rightD.Text) != rightLength)
                {
                    if (Double.Parse(rightD.Text) >= rightLength)
                    {
                        if (Double.Parse(rightD.Text) <= leftLength)
                        {
                            double temp = Double.Parse(rightD.Text) - rightLength;
                            currentCom.point3.Y += (temp / 2.0);
                            currentCom.point4.Y -= (temp / 2.0);

                        }
                        else
                        {
                            double temp = Double.Parse(rightD.Text) - rightLength;
                            currentCom.startPoint.Y += (temp / 2.0 - currentCom.point4.Y + layerHeight);
                            currentCom.point2.Y += (temp / 2.0 - currentCom.point4.Y + layerHeight);
                            currentCom.point3.Y += (temp - currentCom.point4.Y + layerHeight);
                            currentCom.point4.Y = 0 + layerHeight;
                        }
                    }
                    else
                    {
                        if (Double.Parse(rightD.Text) > leftLength)
                        {
                            double temp = rightLength - Double.Parse(rightD.Text);
                            currentCom.point2.Y -= (temp / 2.0 - layerHeight);
                            currentCom.point3.Y -= (temp -layerHeight);
                            currentCom.startPoint.Y -= (temp / 2.0 - layerHeight);
                            currentCom.point4.Y += layerHeight;
                        }
                        else
                        {
                            double temp = rightLength - Double.Parse(rightD.Text);
                            currentCom.point2.Y -= (currentCom.startPoint.Y - layerHeight);
                            currentCom.point3.Y -= (temp / 2.0 + currentCom.startPoint.Y - layerHeight);
                            currentCom.point4.Y += (temp / 2.0 - currentCom.startPoint.Y + layerHeight);
                            currentCom.startPoint.Y = 0 + layerHeight;
                        }
                    }
                    currentCom.startPoint.Y += 100;
                    currentCom.point2.Y += 100;
                    currentCom.point3.Y += 100;
                    currentCom.point4.Y += 100;
                    currentCom.pf.StartPoint = currentCom.startPoint;
                    currentCom.ls.Point = currentCom.point2;
                    currentCom.ls2.Point = currentCom.point3;
                    currentCom.ls3.Point = currentCom.point4;
                    currentCom.ls4.Point = currentCom.startPoint;

                    double h1 = Math.Abs(currentCom.startPoint.Y - currentCom.point2.Y);
                    double h2 = Math.Abs(currentCom.point3.Y - currentCom.point4.Y);
                    double maxHeight = h1 > h2 ? h1 : h2;
                    if ((currentCom.point2.Y > currentCom.point3.Y ? currentCom.point2.Y : currentCom.point3.Y) > maxHeight)
                    {
                        currentCom.newPath.Height = (currentCom.point2.Y > currentCom.point3.Y ? currentCom.point2.Y : currentCom.point3.Y) + layerHeight + 100;
                    }
                    else
                    {
                        currentCom.newPath.Height = maxHeight + layerHeight + 100;
                        currentCom.height = maxHeight + layerHeight + 100;
                    }
                }
            }
            rightLength = Double.Parse(rightD.Text.ToString());
        }

        public void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                string comValue = materialBox.SelectedItem.ToString();
                isComboxChanged = true;

                //遍历数据表，得到对应的颜色值
                foreach (DataRow row in dt.Rows)
                {
                    if (row[dt.Columns[1]].ToString() == comValue)
                    {
                        Console.WriteLine((row[dt.Columns[3]]));
                        color = (Color)ColorConverter.ConvertFromString(row[dt.Columns[3]].ToString());
                    }
                }
            }
            catch
            {
            }
        }

        public void ChangeColor()
        {
            if (RadioCube.IsChecked == true)
            {
            }
            else
            {
                SolidColorBrush brush = new SolidColorBrush();
                if (isLeftEndCap != "" && !isComboxChanged)
                {
                    color = (Color)ColorConverter.ConvertFromString(isLeftEndCap);
                }
                if (isRightEndCap != "" && !isComboxChanged)
                {
                    color = (Color)ColorConverter.ConvertFromString(isRightEndCap);
                }
                brush.Color = color;
                currentCom.newPath.Fill = brush;
            }
        }

        public void ChangePathWidth()
        {
            double tempNum = Double.Parse(segmentWidth.Text) - pathWidth;

            GeometryGroup geometryGroup = (GeometryGroup)currentCom.newPath.Data;
            Console.WriteLine(geometryGroup.Children.Count);

            //先将基本构件宽度增加
            PathGeometry curPg = (PathGeometry)geometryGroup.Children[0];
            PathFigure curPf = curPg.Figures.ElementAt(0);
            if (curPf.Segments[1] is LineSegment)
            {
                ((LineSegment)curPf.Segments[1]).Point = new Point(((LineSegment)curPf.Segments[1]).Point.X + tempNum, ((LineSegment)curPf.Segments[1]).Point.Y);
                currentCom.point3 = ((LineSegment)curPf.Segments[1]).Point;
                currentCom.point2 = new Point(curPf.StartPoint.X, ((LineSegment)curPf.Segments[1]).Point.Y);
                currentCom.startPoint = new Point(curPf.StartPoint.X, curPf.StartPoint.Y);
            }
            else if (curPf.Segments[1] is ArcSegment)
            {
                ((ArcSegment)curPf.Segments[1]).Point = new Point(((ArcSegment)curPf.Segments[1]).Point.X + tempNum, ((ArcSegment)curPf.Segments[1]).Point.Y);
                currentCom.point3 = ((ArcSegment)curPf.Segments[1]).Point;
                currentCom.point2 = new Point(curPf.StartPoint.X, ((ArcSegment)curPf.Segments[1]).Point.Y);
                currentCom.startPoint = new Point(curPf.StartPoint.X, curPf.StartPoint.Y);
            }
            else
            {
                MessageBox.Show("不能修改宽度！","警告");
            }
            ((LineSegment)curPf.Segments[2]).Point = new Point(((LineSegment)curPf.Segments[2]).Point.X + tempNum, ((LineSegment)curPf.Segments[2]).Point.Y);
            currentCom.point4 = ((LineSegment)curPf.Segments[2]).Point;
            
            

            //如果存在层，将层增加宽度  目前一层
            for (int i = 1; i < geometryGroup.Children.Count; i += 2)
            {
                //if (geometryGroup.Children.Count == 3)
                //{
                    PathGeometry top_Pg = (PathGeometry)geometryGroup.Children[i];
                    PathFigure top_Pf = top_Pg.Figures.ElementAt(0);

                    //判断直线还是曲线
                    if (top_Pf.Segments[0] is LineSegment)
                    {
                        ((LineSegment)top_Pf.Segments[0]).Point = new Point(((LineSegment)top_Pf.Segments[0]).Point.X + tempNum, ((LineSegment)top_Pf.Segments[0]).Point.Y);
                    }
                    else
                    {
                        ((ArcSegment)top_Pf.Segments[0]).Point = new Point(((ArcSegment)top_Pf.Segments[0]).Point.X + tempNum, ((ArcSegment)top_Pf.Segments[0]).Point.Y);
                    }

                    if (top_Pf.Segments[1] is LineSegment)
                    {
                        ((LineSegment)top_Pf.Segments[1]).Point = new Point(((LineSegment)top_Pf.Segments[1]).Point.X + tempNum, ((LineSegment)top_Pf.Segments[1]).Point.Y);
                    }
                    else
                    {
                        ((ArcSegment)top_Pf.Segments[1]).Point = new Point(((ArcSegment)top_Pf.Segments[1]).Point.X + tempNum, ((ArcSegment)top_Pf.Segments[1]).Point.Y);
                    }

                    PathGeometry bottom_Pg = (PathGeometry)geometryGroup.Children[i+1];
                    PathFigure bottom_Pf = bottom_Pg.Figures.ElementAt(0);

                    //判断直线还是曲线
                    if (bottom_Pf.Segments[1] is LineSegment)
                    {
                        ((LineSegment)bottom_Pf.Segments[1]).Point = new Point(((LineSegment)bottom_Pf.Segments[1]).Point.X + tempNum, ((LineSegment)bottom_Pf.Segments[1]).Point.Y);
                    }
                    else
                    {
                        ((ArcSegment)bottom_Pf.Segments[1]).Point = new Point(((ArcSegment)bottom_Pf.Segments[1]).Point.X + tempNum, ((ArcSegment)bottom_Pf.Segments[1]).Point.Y);
                    }

                    if (bottom_Pf.Segments[2] is LineSegment)
                    {
                        ((LineSegment)bottom_Pf.Segments[2]).Point = new Point(((LineSegment)bottom_Pf.Segments[2]).Point.X + tempNum, ((LineSegment)bottom_Pf.Segments[2]).Point.Y);
                    }
                    else
                    {
                        ((ArcSegment)bottom_Pf.Segments[2]).Point = new Point(((ArcSegment)bottom_Pf.Segments[2]).Point.X + tempNum, ((ArcSegment)bottom_Pf.Segments[2]).Point.Y);
                    }
                //}
            }

            currentCom.newPath.Width += tempNum;

        }

        private void CheckBox_Cube(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox)
            {
                CheckBox cb = sender as CheckBox;
                if (true == cb.IsChecked)
                {
                    CubeX.Visibility = Visibility.Visible;
                    CubeXValue.Visibility = Visibility.Visible;
                    CubeXUnit.Visibility = Visibility.Visible;
                }
                else
                {
                    CubeX.Visibility = Visibility.Hidden;
                    CubeXValue.Visibility = Visibility.Hidden;
                    CubeXUnit.Visibility = Visibility.Hidden;
                }
            }
        }

        private void SaveCubeXValue()
        {
            currentCom.cubeOffset = Double.Parse(CubeXValue.Text);
        }

        private void changeShape(double radius)
        {
            if (shape == "Cylinder" && (currentCom.isChangeOgive || currentCom.isChangeIOgive))
            {
                if (ChangeShapeEvent3 != null)
                {
                    ChangeShapeEvent3(120, 0);
                }
            }
            else if (shape == "Ogive" && currentCom.isChangeOgive == false)
            {
                if (ChangeShapeEvent != null)
                {
                    ChangeShapeEvent(radius, 0);
                }
            }
            else if (shape == "Inverst Ogive" && currentCom.isChangeIOgive == false)
            {
                if (ChangeShapeEvent2 != null)
                {
                    ChangeShapeEvent2(radius, 1);
                }
            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            shape = rb.Content.ToString();

            if (shape == "Cylinder")
            {
                radius.IsEnabled = false;
                radiusText.IsEnabled = false;
                radiusUnit.IsEnabled = false;
            }
            else if (shape == "Ogive")
            {
                radius.IsEnabled = true;
                radiusText.IsEnabled = true;
                radiusUnit.IsEnabled = true;
            }
            else
            {
                radius.IsEnabled = true;
                radiusText.IsEnabled = true;
                radiusUnit.IsEnabled = true;
            }
        }
    }
}
