﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Collections;
using System.Windows;
using System.Windows.Input;

namespace GraphicalStructure
{
    class ColorProc
    {
        // 保存 层与cover的映射关系
        public static Dictionary<PathFigure, Shape> PfToCoverMap = new Dictionary<PathFigure, Shape>();
        public static Dictionary<Shape, PathFigure> CoverToPfMap = new Dictionary<Shape, PathFigure>();
        public static Dictionary<Shape, Path> CoverToPathMap = new Dictionary<Shape, Path>();

        public static Canvas mainCanvas;

        // 添加层时，在canvas上对应位置（绝对坐标）添加cover，并作相应处理
        public static void processWhenAddLayer(Canvas canvas, StackPanel curSp,Path curPath, PathFigure pf, int isTop)
        {
            //获取当前添加层的坐标
            ArrayList arr = new ArrayList(getPathFigureCoordinate(canvas, curSp, curPath, pf, isTop));
            //添加cover 
            if (isTop == 0)
            {
                Path newPath = new Path();
                // Create a blue and a black Brush
                Random random = new Random();
                SolidColorBrush brush = new SolidColorBrush();
                brush.Color = Color.FromRgb((byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
                SolidColorBrush blackBrush = new SolidColorBrush();
                blackBrush.Color = Colors.Blue;

                newPath.Stroke = blackBrush;
                newPath.StrokeThickness = 1;
                newPath.Fill = brush;

                GeometryGroup geometryGroup = new GeometryGroup();
                geometryGroup.FillRule = FillRule.Nonzero;

                PathGeometry pg = new PathGeometry();
                PathFigure newpf = new PathFigure();
                newpf.StartPoint = new Point(((Point)arr[0]).X, ((Point)arr[0]).Y);

                if (pf.Segments[0] is LineSegment)
                {
                    LineSegment ls = new LineSegment();
                    ls.Point = new Point(((Point)arr[1]).X, ((Point)arr[1]).Y);
                    newpf.Segments.Add(ls);
                }
                else if (pf.Segments[0] is ArcSegment)
                {
                    ArcSegment ls = new ArcSegment();
                    ls.Point = new Point(((Point)arr[1]).X, ((Point)arr[1]).Y);
                    ls.Size = new Size(((ArcSegment)pf.Segments[0]).Size.Width, ((ArcSegment)pf.Segments[0]).Size.Height);
                    ls.SweepDirection = ((ArcSegment)pf.Segments[0]).SweepDirection;
                    newpf.Segments.Add(ls);
                }
                else
                {
                    PolyLineSegment ls = new PolyLineSegment();
                    for (int i = 0; i < ((PolyLineSegment)pf.Segments[0]).Points.Count - 1; i++)
                    {
                        double delta_x, delta_y;
                        delta_x = (((Point)arr[0]).X - pf.StartPoint.X);
                        delta_y = (((Point)arr[0]).Y - pf.StartPoint.Y);
                        ls.Points.Add(new Point(((PolyLineSegment)pf.Segments[0]).Points[i].X + delta_x, ((PolyLineSegment)pf.Segments[0]).Points[i].Y + delta_y));
                    }
                    newpf.Segments.Add(ls);
                }
                LineSegment ls2 = new LineSegment();
                ls2.Point = new Point(((Point)arr[2]).X, ((Point)arr[2]).Y);
                newpf.Segments.Add(ls2);

                if (pf.Segments[2] is LineSegment)
                {
                    LineSegment ls3 = new LineSegment();
                    ls3.Point = new Point(((Point)arr[3]).X, ((Point)arr[3]).Y);
                    newpf.Segments.Add(ls3);
                }
                else if (pf.Segments[2] is ArcSegment)
                {
                    ArcSegment ls3 = new ArcSegment();
                    ls3.Point = new Point(((Point)arr[3]).X, ((Point)arr[3]).Y);
                    ls3.Size = new Size(((ArcSegment)pf.Segments[2]).Size.Width, ((ArcSegment)pf.Segments[2]).Size.Height);
                    ls3.SweepDirection = ((ArcSegment)pf.Segments[2]).SweepDirection;
                    newpf.Segments.Add(ls3);
                }
                else
                {
                    PolyLineSegment ls3 = new PolyLineSegment();
                    for (int i = 0; i < ((PolyLineSegment)pf.Segments[2]).Points.Count - 1; i++)
                    {
                        double delta_x, delta_y;
                        delta_x = (((Point)arr[0]).X - pf.StartPoint.X);
                        delta_y = (((Point)arr[0]).Y - pf.StartPoint.Y);
                        ls3.Points.Add(new Point(((PolyLineSegment)pf.Segments[2]).Points[i].X + delta_x, ((PolyLineSegment)pf.Segments[2]).Points[i].Y + delta_y));
                    }
                    newpf.Segments.Add(ls3);
                }

                newpf.IsClosed = true;
                pg.Figures.Add(newpf);

                geometryGroup.Children.Add(pg);
                newPath.Data = geometryGroup;
                newPath.MouseDown += doubleClickOnCover;
                canvas.Children.Add(newPath);
                PfToCoverMap.Add(pf, newPath);
                CoverToPfMap.Add(newPath, pf);
                CoverToPathMap.Add(newPath, curPath);
            }
            else {
                Path newPath = new Path();
                // Create a blue and a black Brush
                Random random = new Random();
                SolidColorBrush brush = new SolidColorBrush();
                brush.Color = Color.FromRgb((byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
                SolidColorBrush blackBrush = new SolidColorBrush();
                blackBrush.Color = Colors.Blue;

                newPath.Stroke = blackBrush;
                newPath.StrokeThickness = 1;
                newPath.Fill = brush;

                GeometryGroup geometryGroup = new GeometryGroup();
                geometryGroup.FillRule = FillRule.Nonzero;

                PathGeometry pg = new PathGeometry();
                PathFigure newpf = new PathFigure();
                newpf.StartPoint = new Point(((Point)arr[0]).X, ((Point)arr[0]).Y);

                LineSegment ls1 = new LineSegment();
                ls1.Point = new Point(((Point)arr[1]).X, ((Point)arr[1]).Y);
                newpf.Segments.Add(ls1);

                if (pf.Segments[1] is LineSegment)
                {
                    LineSegment ls2 = new LineSegment();
                    ls2.Point = new Point(((Point)arr[2]).X, ((Point)arr[2]).Y);
                    newpf.Segments.Add(ls2);
                }
                else if (pf.Segments[1] is ArcSegment)
                {
                    ArcSegment ls2 = new ArcSegment();
                    ls2.Point = new Point(((Point)arr[2]).X, ((Point)arr[2]).Y);
                    ls2.Size = new Size(((ArcSegment)pf.Segments[1]).Size.Width, ((ArcSegment)pf.Segments[1]).Size.Height);
                    ls2.SweepDirection = ((ArcSegment)pf.Segments[1]).SweepDirection;
                    newpf.Segments.Add(ls2);
                }
                else
                {
                    PolyLineSegment ls2 = new PolyLineSegment();
                    for (int i = 0; i < ((PolyLineSegment)pf.Segments[1]).Points.Count - 1; i++)
                    {
                        double delta_x, delta_y;
                        delta_x = (((Point)arr[0]).X - pf.StartPoint.X);
                        delta_y = (((Point)arr[0]).Y - pf.StartPoint.Y);
                        ls2.Points.Add(new Point(((PolyLineSegment)pf.Segments[1]).Points[i].X + delta_x, ((PolyLineSegment)pf.Segments[1]).Points[i].Y + delta_y));
                    }
                    newpf.Segments.Add(ls2);
                }
                LineSegment ls3 = new LineSegment();
                ls3.Point = new Point(((Point)arr[3]).X, ((Point)arr[3]).Y);
                newpf.Segments.Add(ls3);

                if (pf.Segments.Count < 4 || pf.Segments[3] is LineSegment )
                {
                    LineSegment ls4 = new LineSegment();
                    ls4.Point = new Point(((Point)arr[0]).X, ((Point)arr[0]).Y);
                    newpf.Segments.Add(ls4);
                }
                else if (pf.Segments[3] is ArcSegment)
                {
                    ArcSegment ls4 = new ArcSegment();
                    ls4.Point = new Point(((Point)arr[0]).X, ((Point)arr[0]).Y);
                    ls4.Size = new Size(((ArcSegment)pf.Segments[3]).Size.Width, ((ArcSegment)pf.Segments[3]).Size.Height);
                    ls4.SweepDirection = ((ArcSegment)pf.Segments[3]).SweepDirection;
                    newpf.Segments.Add(ls4);
                }
                else
                {
                    PolyLineSegment ls4 = new PolyLineSegment();
                    for (int i = 0; i < ((PolyLineSegment)pf.Segments[3]).Points.Count - 1; i++)
                    {
                        double delta_x, delta_y;
                        delta_x = (((Point)arr[0]).X - pf.StartPoint.X);
                        delta_y = (((Point)arr[0]).Y - pf.StartPoint.Y);
                        ls4.Points.Add(new Point(((PolyLineSegment)pf.Segments[3]).Points[i].X + delta_x, ((PolyLineSegment)pf.Segments[3]).Points[i].Y + delta_y));
                    }
                    newpf.Segments.Add(ls4);
                }

                newpf.IsClosed = true;
                pg.Figures.Add(newpf);

                geometryGroup.Children.Add(pg);
                newPath.Data = geometryGroup;
                newPath.MouseDown += doubleClickOnCover;
                canvas.Children.Add(newPath);
                PfToCoverMap.Add(pf, newPath);
                CoverToPfMap.Add(newPath, pf);
                CoverToPathMap.Add(newPath, curPath);
            }
           
        }

        //获取PF对应cover path的坐标
        private static ArrayList getPathFigureCoordinate(Canvas canvas, StackPanel curSp, Path curPath, PathFigure pf, int isTop)
        {
            double getSpLeftDisToCan = Canvas.GetLeft(curSp);
            double getSpTopDisToCan = Canvas.GetTop(curSp);
            double __p = curSp.ActualHeight;
            double __q = curPath.ActualHeight;
            getSpTopDisToCan += (__p - __q) / 2;
            double y1, y2, y3, y4;
            if (isTop == 0)
            {
                //上层
                y1 = pf.StartPoint.Y;
                if (pf.Segments[0] is LineSegment)
                {
                    y2 = ((LineSegment)pf.Segments[0]).Point.Y;
                }
                else if (pf.Segments[0] is ArcSegment)
                {
                    y2 = ((ArcSegment)pf.Segments[0]).Point.Y;
                }
                else
                {
                    y2 = (((PolyLineSegment)pf.Segments[0]).Points[((PolyLineSegment)pf.Segments[0]).Points.Count - 1]).Y;
                }

                y3 = ((LineSegment)pf.Segments[1]).Point.Y;

                if (pf.Segments[2] is LineSegment)
                {
                    y4 = ((LineSegment)pf.Segments[2]).Point.Y;
                }
                else if (pf.Segments[2] is ArcSegment)
                {
                    y4 = ((ArcSegment)pf.Segments[2]).Point.Y;
                }
                else
                {
                    y4 = (((PolyLineSegment)pf.Segments[2]).Points[((PolyLineSegment)pf.Segments[2]).Points.Count - 1]).Y;
                }

                if (Math.Abs(y4 - y1) <= Math.Abs(y2 - y3)) {
                    getSpTopDisToCan -= Math.Abs(y2 - y3);
                }
                else {
                    getSpTopDisToCan -= Math.Abs(y4 - y1);
                }

                double getPathLeftDisToSp = 0;
                //double getPathTopDisToSp = (curSp.Height - (curPath.Height - 200))/2;
                int index = curSp.Children.IndexOf(curPath);
                if (index > 0)
                {
                    for (int i = 0; i < index; i++)
                    {
                        Path path = curSp.Children[i] as Path;
                        getPathLeftDisToSp += path.Width;
                    }
                }

                double left = getSpLeftDisToCan + getPathLeftDisToSp;
                double top = getSpTopDisToCan;// +getPathTopDisToSp;

                Point p1 = new Point(pf.StartPoint.X + left, pf.StartPoint.Y + top);
                Point p2;
                if (pf.Segments[0] is LineSegment)
                {
                    p2 = new Point(((LineSegment)pf.Segments[0]).Point.X + left, ((LineSegment)pf.Segments[0]).Point.Y + top);
                }
                else if (pf.Segments[0] is ArcSegment)
                {
                    p2 = new Point(((ArcSegment)pf.Segments[0]).Point.X + left, ((ArcSegment)pf.Segments[0]).Point.Y + top);
                }
                else
                {
                    p2 = new Point((((PolyLineSegment)pf.Segments[0]).Points[((PolyLineSegment)pf.Segments[0]).Points.Count - 1]).X + left, (((PolyLineSegment)pf.Segments[0]).Points[((PolyLineSegment)pf.Segments[0]).Points.Count - 1]).Y + top);
                }
                Point p3 = new Point(((LineSegment)pf.Segments[1]).Point.X + left, ((LineSegment)pf.Segments[1]).Point.Y + top);
                Point p4;
                if (pf.Segments[0] is LineSegment)
                {
                    p4 = new Point(((LineSegment)pf.Segments[2]).Point.X + left, ((LineSegment)pf.Segments[2]).Point.Y + top);
                }
                else if (pf.Segments[0] is ArcSegment)
                {
                    p4 = new Point(((ArcSegment)pf.Segments[2]).Point.X + left, ((ArcSegment)pf.Segments[2]).Point.Y + top);
                }
                else
                {
                    p4 = new Point((((PolyLineSegment)pf.Segments[2]).Points[((PolyLineSegment)pf.Segments[2]).Points.Count - 1]).X + left, (((PolyLineSegment)pf.Segments[2]).Points[((PolyLineSegment)pf.Segments[2]).Points.Count - 1]).Y + top);
                }
                ArrayList arrList = new ArrayList();
                arrList.Add(p1);
                arrList.Add(p2);
                arrList.Add(p3);
                arrList.Add(p4);

                return arrList;
            }
            else {
                //下层
                y1 = pf.StartPoint.Y;
                y2 = ((LineSegment)pf.Segments[0]).Point.Y;
                if (pf.Segments[1] is LineSegment)
                {
                    y3 = ((LineSegment)pf.Segments[1]).Point.Y;
                }
                else if (pf.Segments[1] is ArcSegment)
                {
                    y3 = ((ArcSegment)pf.Segments[1]).Point.Y;
                }
                else
                {
                    y3 = (((PolyLineSegment)pf.Segments[1]).Points[((PolyLineSegment)pf.Segments[1]).Points.Count - 1]).Y;
                }

                y4 = ((LineSegment)pf.Segments[2]).Point.Y;
               
                if (Math.Abs(y2 - y1) <= Math.Abs(y3 - y4))
                {
                    getSpTopDisToCan -= Math.Abs(y3 - y4);
                }
                else
                {
                    getSpTopDisToCan -= Math.Abs(y2 - y1);
                }
                double getPathLeftDisToSp = 0;
                //double getPathTopDisToSp = (curSp.Height - (curPath.Height - 200))/2;
                int index = curSp.Children.IndexOf(curPath);
                if (index > 0)
                {
                    for (int i = 0; i < index; i++)
                    {
                        Path path = curSp.Children[i] as Path;
                        getPathLeftDisToSp += path.Width;
                    }
                }

                double left = getSpLeftDisToCan + getPathLeftDisToSp;
                double top = getSpTopDisToCan;// +getPathTopDisToSp;

                Point p1 = new Point(pf.StartPoint.X + left, pf.StartPoint.Y + top);
                Point p2 = new Point(((LineSegment)pf.Segments[0]).Point.X + left, ((LineSegment)pf.Segments[0]).Point.Y + top);
                Point p3;
                if (pf.Segments[1] is LineSegment)
                {
                    p3 = new Point(((LineSegment)pf.Segments[1]).Point.X + left, ((LineSegment)pf.Segments[1]).Point.Y + top);
                }
                else if (pf.Segments[1] is ArcSegment)
                {
                    p3 = new Point(((ArcSegment)pf.Segments[1]).Point.X + left, ((ArcSegment)pf.Segments[1]).Point.Y + top);
                }
                else
                {
                    p3 = new Point((((PolyLineSegment)pf.Segments[1]).Points[((PolyLineSegment)pf.Segments[1]).Points.Count - 1]).X + left, (((PolyLineSegment)pf.Segments[1]).Points[((PolyLineSegment)pf.Segments[1]).Points.Count - 1]).Y + top);
                }
                Point p4 = new Point(((LineSegment)pf.Segments[2]).Point.X + left, ((LineSegment)pf.Segments[2]).Point.Y + top);
                
                ArrayList arrList = new ArrayList();
                arrList.Add(p1);
                arrList.Add(p2);
                arrList.Add(p3);
                arrList.Add(p4);

                return arrList;
            }
        }


        private static ArrayList getPathFigureCoordinateWhenChangeShapeOfLayer(Canvas canvas, StackPanel curSp, Path curPath, PathFigure pf, int isTop)
        {
            double getSpLeftDisToCan = Canvas.GetLeft(curSp);
            double getSpTopDisToCan = Canvas.GetTop(curSp);
            double __p = curSp.ActualHeight;
            double __q = curPath.ActualHeight;
            getSpTopDisToCan += (__p - __q) / 2;
            double y1, y2, y3, y4;
            if (isTop == 0)
            {
                //上层
                y1 = pf.StartPoint.Y;
                if (pf.Segments[0] is LineSegment)
                {
                    y2 = ((LineSegment)pf.Segments[0]).Point.Y;
                }
                else if (pf.Segments[0] is ArcSegment)
                {
                    y2 = ((ArcSegment)pf.Segments[0]).Point.Y;
                }
                else
                {
                    y2 = (((PolyLineSegment)pf.Segments[0]).Points[((PolyLineSegment)pf.Segments[0]).Points.Count - 1]).Y;
                }

                y3 = ((LineSegment)pf.Segments[1]).Point.Y;

                if (pf.Segments[2] is LineSegment)
                {
                    y4 = ((LineSegment)pf.Segments[2]).Point.Y;
                }
                else if (pf.Segments[2] is ArcSegment)
                {
                    y4 = ((ArcSegment)pf.Segments[2]).Point.Y;
                }
                else
                {
                    y4 = (((PolyLineSegment)pf.Segments[2]).Points[((PolyLineSegment)pf.Segments[2]).Points.Count - 1]).Y;
                }

                double getPathLeftDisToSp = 0;
                //double getPathTopDisToSp = (curSp.Height - (curPath.Height - 200))/2;
                int index = curSp.Children.IndexOf(curPath);
                if (index > 0)
                {
                    for (int i = 0; i < index; i++)
                    {
                        Path path = curSp.Children[i] as Path;
                        getPathLeftDisToSp += path.Width;
                    }
                }

                double left = getSpLeftDisToCan + getPathLeftDisToSp;
                double top = getSpTopDisToCan;// +getPathTopDisToSp;

                Point p1 = new Point(pf.StartPoint.X + left, pf.StartPoint.Y + top);
                Point p2;
                if (pf.Segments[0] is LineSegment)
                {
                    p2 = new Point(((LineSegment)pf.Segments[0]).Point.X + left, ((LineSegment)pf.Segments[0]).Point.Y + top);
                }
                else if (pf.Segments[0] is ArcSegment)
                {
                    p2 = new Point(((ArcSegment)pf.Segments[0]).Point.X + left, ((ArcSegment)pf.Segments[0]).Point.Y + top);
                }
                else
                {
                    p2 = new Point((((PolyLineSegment)pf.Segments[0]).Points[((PolyLineSegment)pf.Segments[0]).Points.Count - 1]).X + left, (((PolyLineSegment)pf.Segments[0]).Points[((PolyLineSegment)pf.Segments[0]).Points.Count - 1]).Y + top);
                }
                Point p3 = new Point(((LineSegment)pf.Segments[1]).Point.X + left, ((LineSegment)pf.Segments[1]).Point.Y + top);
                Point p4;
                if (pf.Segments[0] is LineSegment)
                {
                    p4 = new Point(((LineSegment)pf.Segments[2]).Point.X + left, ((LineSegment)pf.Segments[2]).Point.Y + top);
                }
                else if (pf.Segments[0] is ArcSegment)
                {
                    p4 = new Point(((ArcSegment)pf.Segments[2]).Point.X + left, ((ArcSegment)pf.Segments[2]).Point.Y + top);
                }
                else
                {
                    p4 = new Point((((PolyLineSegment)pf.Segments[2]).Points[((PolyLineSegment)pf.Segments[2]).Points.Count - 1]).X + left, (((PolyLineSegment)pf.Segments[2]).Points[((PolyLineSegment)pf.Segments[2]).Points.Count - 1]).Y + top);
                }
                ArrayList arrList = new ArrayList();
                arrList.Add(p1);
                arrList.Add(p2);
                arrList.Add(p3);
                arrList.Add(p4);

                return arrList;
            }
            else
            {
                //下层
                y1 = pf.StartPoint.Y;
                y2 = ((LineSegment)pf.Segments[0]).Point.Y;
                if (pf.Segments[1] is LineSegment)
                {
                    y3 = ((LineSegment)pf.Segments[1]).Point.Y;
                }
                else if (pf.Segments[1] is ArcSegment)
                {
                    y3 = ((ArcSegment)pf.Segments[1]).Point.Y;
                }
                else
                {
                    y3 = (((PolyLineSegment)pf.Segments[1]).Points[((PolyLineSegment)pf.Segments[1]).Points.Count - 1]).Y;
                }

                y4 = ((LineSegment)pf.Segments[2]).Point.Y;

                double getPathLeftDisToSp = 0;
                //double getPathTopDisToSp = (curSp.Height - (curPath.Height - 200))/2;
                int index = curSp.Children.IndexOf(curPath);
                if (index > 0)
                {
                    for (int i = 0; i < index; i++)
                    {
                        Path path = curSp.Children[i] as Path;
                        getPathLeftDisToSp += path.Width;
                    }
                }

                double left = getSpLeftDisToCan + getPathLeftDisToSp;
                double top = getSpTopDisToCan;// +getPathTopDisToSp;

                Point p1 = new Point(pf.StartPoint.X + left, pf.StartPoint.Y + top);
                Point p2 = new Point(((LineSegment)pf.Segments[0]).Point.X + left, ((LineSegment)pf.Segments[0]).Point.Y + top);
                Point p3;
                if (pf.Segments[1] is LineSegment)
                {
                    p3 = new Point(((LineSegment)pf.Segments[1]).Point.X + left, ((LineSegment)pf.Segments[1]).Point.Y + top);
                }
                else if (pf.Segments[1] is ArcSegment)
                {
                    p3 = new Point(((ArcSegment)pf.Segments[1]).Point.X + left, ((ArcSegment)pf.Segments[1]).Point.Y + top);
                }
                else
                {
                    p3 = new Point((((PolyLineSegment)pf.Segments[1]).Points[((PolyLineSegment)pf.Segments[1]).Points.Count - 1]).X + left, (((PolyLineSegment)pf.Segments[1]).Points[((PolyLineSegment)pf.Segments[1]).Points.Count - 1]).Y + top);
                }
                Point p4 = new Point(((LineSegment)pf.Segments[2]).Point.X + left, ((LineSegment)pf.Segments[2]).Point.Y + top);

                ArrayList arrList = new ArrayList();
                arrList.Add(p1);
                arrList.Add(p2);
                arrList.Add(p3);
                arrList.Add(p4);

                return arrList;
            }
        }

        // 删除层时，删除cover， 并作相应处理
        public static void processWhenDelLayer(Canvas canvas, PathFigure pf)
        {
            Shape _shape = PfToCoverMap[pf];
            CoverToPfMap.Remove(_shape);
            canvas.Children.Remove(_shape);
            PfToCoverMap.Remove(pf);

        }

        // 移动层时，同时移动cover， 并作相应处理
        public static void processWhenMoveLayer(Canvas canvas, StackPanel curSp)
        {
            int temp = 0;
            foreach (Shape shape in CoverToPathMap.Keys)
            {
                Path path = CoverToPathMap[shape];
                int index = curSp.Children.IndexOf(path);
                double leftPanel = 0;
                double leftCanvas = 0;
                double left;
                if (index > 0)
                {
                    for (int i = 0; i < index; i++)
                    {
                        leftPanel += ((Path)curSp.Children[i]).Width;
                    }
                }
                leftCanvas = Canvas.GetLeft(curSp);
                left = leftCanvas + leftPanel;

                GeometryGroup geometryGroup = (GeometryGroup)((Path)shape).Data;
                PathGeometry curPg = (PathGeometry)geometryGroup.Children[0];
                PathFigure curPf = curPg.Figures.ElementAt(0);

                curPf.StartPoint = new Point(left, curPf.StartPoint.Y);
                if (temp % 2 == 0)
                {
                    if (curPf.Segments[0] is LineSegment)
                    {
                        ((LineSegment)curPf.Segments[0]).Point = new Point(left + path.Width, ((LineSegment)curPf.Segments[0]).Point.Y);
                    }
                    else if (curPf.Segments[0] is ArcSegment)
                    {
                        ((ArcSegment)curPf.Segments[0]).Point = new Point(left + path.Width, ((ArcSegment)curPf.Segments[0]).Point.Y);
                    }
                    else
                    {
                        double offset = left - Canvas.GetLeft(shape);
                        for (int i = 0; i < ((PolyLineSegment)curPf.Segments[0]).Points.Count; i++)
                        {
                            ((PolyLineSegment)curPf.Segments[0]).Points[i] = new Point(((PolyLineSegment)curPf.Segments[0]).Points[i].X + offset, ((PolyLineSegment)curPf.Segments[0]).Points[i].Y);
                        }
                    }
                    ((LineSegment)curPf.Segments[1]).Point = new Point(left + path.Width, ((LineSegment)curPf.Segments[1]).Point.Y);
                    if (curPf.Segments[2] is LineSegment)
                    {
                        ((LineSegment)curPf.Segments[2]).Point = new Point(left, ((LineSegment)curPf.Segments[2]).Point.Y);
                    }
                    else if (curPf.Segments[2] is ArcSegment)
                    {
                        ((ArcSegment)curPf.Segments[2]).Point = new Point(left, ((ArcSegment)curPf.Segments[2]).Point.Y);
                    }
                    else
                    {
                        double offset = left - Canvas.GetLeft(shape);
                        for (int i = 0; i < ((PolyLineSegment)curPf.Segments[2]).Points.Count; i++)
                        {
                            ((PolyLineSegment)curPf.Segments[2]).Points[i] = new Point(((PolyLineSegment)curPf.Segments[2]).Points[i].X + offset, ((PolyLineSegment)curPf.Segments[2]).Points[i].Y);
                        }
                    }
                }
                else
                {
                    ((LineSegment)curPf.Segments[0]).Point = new Point(left, ((LineSegment)curPf.Segments[0]).Point.Y);
                    
                    if (curPf.Segments[1] is LineSegment)
                    {
                        ((LineSegment)curPf.Segments[1]).Point = new Point(left + path.Width, ((LineSegment)curPf.Segments[1]).Point.Y);
                    }
                    else if (curPf.Segments[1] is ArcSegment)
                    {
                        ((ArcSegment)curPf.Segments[1]).Point = new Point(left + path.Width, ((ArcSegment)curPf.Segments[1]).Point.Y);
                    }
                    else
                    {
                        double offset = left - Canvas.GetLeft(shape);
                        for (int i = 0; i < ((PolyLineSegment)curPf.Segments[1]).Points.Count; i++)
                        {
                            ((PolyLineSegment)curPf.Segments[1]).Points[i] = new Point(((PolyLineSegment)curPf.Segments[1]).Points[i].X + offset, ((PolyLineSegment)curPf.Segments[1]).Points[i].Y);
                        }
                    } 
                    ((LineSegment)curPf.Segments[2]).Point = new Point(left + path.Width, ((LineSegment)curPf.Segments[2]).Point.Y);
                    if (curPf.Segments[3] is LineSegment)
                    {
                        ((LineSegment)curPf.Segments[3]).Point = new Point(left, ((LineSegment)curPf.Segments[3]).Point.Y);
                    }
                    else if (curPf.Segments[3] is ArcSegment)
                    {
                        ((ArcSegment)curPf.Segments[3]).Point = new Point(left, ((ArcSegment)curPf.Segments[3]).Point.Y);
                    }
                    else
                    {
                        double offset = left - Canvas.GetLeft(shape);
                        for (int i = 0; i < ((PolyLineSegment)curPf.Segments[3]).Points.Count; i++)
                        {
                            ((PolyLineSegment)curPf.Segments[3]).Points[i] = new Point(((PolyLineSegment)curPf.Segments[3]).Points[i].X + offset, ((PolyLineSegment)curPf.Segments[3]).Points[i].Y);
                        }
                    } 
                }

                temp++;
            }
        }

        // 修改层的高度时，改变cover的高度
        // isLeft = 0表示改变left的高度
        // isTop = 0表示改变top cover的高度
        public static void processWhenChangeLayerHeight(PathFigure pf, int isLeft, int isTop) {
            if (isLeft == 0)
            {
                if (isTop == 0)
                {
                    double y1 = pf.StartPoint.Y;
                    double y2;
                    double changeValue = 0;
                    if (pf.Segments[2] is LineSegment)
                    {
                        y2 = ((LineSegment)pf.Segments[2]).Point.Y;
                    }
                    else if (pf.Segments[0] is ArcSegment)
                    {
                        y2 = ((ArcSegment)pf.Segments[2]).Point.Y;
                    }
                    else
                    {
                        y2 = ((PolyLineSegment)pf.Segments[2]).Points[((PolyLineSegment)pf.Segments[2]).Points.Count - 1].Y;
                    }

                    Shape coverPath = PfToCoverMap[pf];
                    PathGeometry coverPath_pg = (PathGeometry)((GeometryGroup)((Path)coverPath).Data).Children[0];
                    PathFigure cover_pf = coverPath_pg.Figures[0];
                    cover_pf.StartPoint = new Point(cover_pf.StartPoint.X, cover_pf.StartPoint.Y);
                    if (cover_pf.Segments[2] is LineSegment)
                    {
                        changeValue = ((LineSegment)cover_pf.Segments[2]).Point.Y - (cover_pf.StartPoint.Y + y2 - y1);
                        ((LineSegment)cover_pf.Segments[2]).Point = new Point(((LineSegment)cover_pf.Segments[2]).Point.X, cover_pf.StartPoint.Y + y2 - y1);

                    }
                    else if (cover_pf.Segments[0] is ArcSegment)
                    {
                        changeValue = ((LineSegment)cover_pf.Segments[2]).Point.Y - (cover_pf.StartPoint.Y + y2 - y1);
                        ((ArcSegment)cover_pf.Segments[2]).Point = new Point(((ArcSegment)cover_pf.Segments[2]).Point.X, cover_pf.StartPoint.Y + y2 - y1);
                    }
                    else
                    {
                        //y2 = ((PolyLineSegment)pf.Segments[2]).Points[((PolyLineSegment)pf.Segments[2]).Points.Count - 1].Y;

                        for (int i = 0; i < ((PolyLineSegment)pf.Segments[3]).Points.Count - 1; i++)
                        {
                            ((PolyLineSegment)pf.Segments[3]).Points[i] = new Point(((PolyLineSegment)pf.Segments[3]).Points[i].X, ((PolyLineSegment)pf.Segments[3]).Points[i].Y + y2 - y1);
                        }
                    }

                    /*
                    添加代码：遍历后边的covers，做相同变化
                    */
                    Shape curPath = CoverToPathMap[coverPath];
                    GeometryGroup gg = (GeometryGroup)((Path)curPath).Data;
                    // pg 是段的 PathGeometry
                    int indexOfPf = 0;
                    for (indexOfPf = 0; indexOfPf < gg.Children.Count; indexOfPf += 1)
                    {
                        PathGeometry _pg = (PathGeometry)gg.Children[indexOfPf];
                        PathFigure _pf = _pg.Figures[0];
                        if (_pf == pf)
                        {
                            break;
                        }
                    }
                    for (int i = indexOfPf + 2; i < gg.Children.Count; i += 2) {
                        PathGeometry _pg = (PathGeometry)gg.Children[i];
                        PathFigure _pf = _pg.Figures[0];
                        Shape _coverPath = PfToCoverMap[_pf];
                        PathGeometry _coverPath_pg = (PathGeometry)((GeometryGroup)((Path)_coverPath).Data).Children[0];
                        PathFigure _cover_pf = _coverPath_pg.Figures[0];
                        // 求_cover_pf的左侧高度

                        double _y1 = _cover_pf.StartPoint.Y;
                        double _y2 = 0;
                        if (_cover_pf.Segments[2] is LineSegment)
                        {
                            _y2 = ((LineSegment)_cover_pf.Segments[2]).Point.Y;
                        }
                        else if (_cover_pf.Segments[2] is ArcSegment)
                        {
                            _y2 = ((ArcSegment)_cover_pf.Segments[2]).Point.Y;
                        }
                        else
                        {
                            //
                        }
                        double offset = Math.Abs(_y2 - _y1) - Math.Abs(y2 - y1);
                        if (offset != 0) {
                            _cover_pf.StartPoint = new Point(_cover_pf.StartPoint.X, _cover_pf.StartPoint.Y + offset);

                            if (_cover_pf.Segments[2] is LineSegment)
                            {
                                ((LineSegment)_cover_pf.Segments[2]).Point = new Point(((LineSegment)_cover_pf.Segments[2]).Point.X, ((LineSegment)_cover_pf.Segments[2]).Point.Y + offset);
                            }
                            else if (_cover_pf.Segments[2] is ArcSegment)
                            {
                                ((ArcSegment)_cover_pf.Segments[2]).Point = new Point(((ArcSegment)_cover_pf.Segments[2]).Point.X, ((ArcSegment)_cover_pf.Segments[2]).Point.Y + offset);
                            }
                            else
                            {
                                //
                            }
                        }
                        else {
                            offset = -changeValue;
                            _cover_pf.StartPoint = new Point(_cover_pf.StartPoint.X, _cover_pf.StartPoint.Y + offset);

                            if (_cover_pf.Segments[2] is LineSegment)
                            {
                                ((LineSegment)_cover_pf.Segments[2]).Point = new Point(((LineSegment)_cover_pf.Segments[2]).Point.X, ((LineSegment)_cover_pf.Segments[2]).Point.Y + offset);
                            }
                            else if (_cover_pf.Segments[2] is ArcSegment)
                            {
                                ((ArcSegment)_cover_pf.Segments[2]).Point = new Point(((ArcSegment)_cover_pf.Segments[2]).Point.X, ((ArcSegment)_cover_pf.Segments[2]).Point.Y + offset);
                            }
                            else
                            {
                                //
                            }
                        }
                        
                    }
                }
                else {
                    double y1 = pf.StartPoint.Y;
                    double y2 = ((LineSegment)pf.Segments[0]).Point.Y;
                    double changeValue = 0;

                    Shape coverPath = PfToCoverMap[pf];
                    PathGeometry coverPath_pg = (PathGeometry)((GeometryGroup)((Path)coverPath).Data).Children[0];
                    PathFigure cover_pf = coverPath_pg.Figures[0];
                    cover_pf.StartPoint = new Point(cover_pf.StartPoint.X, cover_pf.StartPoint.Y);
                    changeValue = ((LineSegment)cover_pf.Segments[0]).Point.Y - (cover_pf.StartPoint.Y + y2 - y1);
                    ((LineSegment)cover_pf.Segments[0]).Point = new Point(((LineSegment)cover_pf.Segments[0]).Point.X, cover_pf.StartPoint.Y + y2 - y1);

                    /*
                   添加代码：遍历后边的covers，做相同变化
                   */
                    Shape curPath = CoverToPathMap[coverPath];
                    GeometryGroup gg = (GeometryGroup)((Path)curPath).Data;
                    // pg 是段的 PathGeometry
                    int indexOfPf = 0;
                    for (indexOfPf = 0; indexOfPf < gg.Children.Count; indexOfPf += 1)
                    {
                        PathGeometry _pg = (PathGeometry)gg.Children[indexOfPf];
                        PathFigure _pf = _pg.Figures[0];
                        if (_pf == pf)
                        {
                            break;
                        }
                    }
                    for (int i = indexOfPf + 2; i < gg.Children.Count; i += 2)
                    {
                        PathGeometry _pg = (PathGeometry)gg.Children[i];
                        PathFigure _pf = _pg.Figures[0];
                        Shape _coverPath = PfToCoverMap[_pf];
                        PathGeometry _coverPath_pg = (PathGeometry)((GeometryGroup)((Path)_coverPath).Data).Children[0];
                        PathFigure _cover_pf = _coverPath_pg.Figures[0];
                        // 求_cover_pf的左侧高度

                        double _y1 = _cover_pf.StartPoint.Y;
                        double _y2 = ((LineSegment)_cover_pf.Segments[0]).Point.Y;

                        double offset = Math.Abs(_y2 - _y1) - Math.Abs(y2 - y1);
                        if (offset != 0)
                        {
                            _cover_pf.StartPoint = new Point(_cover_pf.StartPoint.X, _cover_pf.StartPoint.Y - offset);
                            ((LineSegment)_cover_pf.Segments[0]).Point = new Point(((LineSegment)_cover_pf.Segments[0]).Point.X, ((LineSegment)_cover_pf.Segments[0]).Point.Y - offset);
                            if (_cover_pf.Segments.Count < 4 || _cover_pf.Segments[3] is LineSegment)
                            {
                                ((LineSegment)_cover_pf.Segments[3]).Point = new Point(_cover_pf.StartPoint.X, _cover_pf.StartPoint.Y);
                            }
                            else if (_cover_pf.Segments[3] is ArcSegment)
                            {
                                ((ArcSegment)_cover_pf.Segments[3]).Point = new Point(_cover_pf.StartPoint.X, _cover_pf.StartPoint.Y);
                            }
                        }
                        else {
                            offset = changeValue;
                            _cover_pf.StartPoint = new Point(_cover_pf.StartPoint.X, _cover_pf.StartPoint.Y - offset);
                            ((LineSegment)_cover_pf.Segments[0]).Point = new Point(((LineSegment)_cover_pf.Segments[0]).Point.X, ((LineSegment)_cover_pf.Segments[0]).Point.Y - offset);
                            if (_cover_pf.Segments.Count < 4 || _cover_pf.Segments[3] is LineSegment)
                            {
                                ((LineSegment)_cover_pf.Segments[3]).Point = new Point(_cover_pf.StartPoint.X, _cover_pf.StartPoint.Y);
                            }
                            else if (_cover_pf.Segments[3] is ArcSegment)
                            {
                                ((ArcSegment)_cover_pf.Segments[3]).Point = new Point(_cover_pf.StartPoint.X, _cover_pf.StartPoint.Y);
                            }
                        }
                        
                    }
                }
            }
            else {
                if (isTop == 0)
                {
                    double y1;
                    double y2 = ((LineSegment)pf.Segments[1]).Point.Y;
                    double changeValue = 0;

                    if (pf.Segments[0] is LineSegment)
                    {
                        y1 = ((LineSegment)pf.Segments[0]).Point.Y;
                    }
                    else if (pf.Segments[0] is ArcSegment)
                    {
                        y1 = ((ArcSegment)pf.Segments[0]).Point.Y;
                    }
                    else
                    {
                        y1 = ((PolyLineSegment)pf.Segments[0]).Points[((PolyLineSegment)pf.Segments[0]).Points.Count - 1].Y;
                    }

                    Shape coverPath = PfToCoverMap[pf];
                    PathGeometry coverPath_pg = (PathGeometry)((GeometryGroup)((Path)coverPath).Data).Children[0];
                    PathFigure cover_pf = coverPath_pg.Figures[0];
                    changeValue = ((LineSegment)cover_pf.Segments[1]).Point.Y - (((LineSegment)cover_pf.Segments[0]).Point.Y + y2 - y1);
                    ((LineSegment)cover_pf.Segments[1]).Point = new Point(((LineSegment)cover_pf.Segments[1]).Point.X, ((LineSegment)cover_pf.Segments[0]).Point.Y + y2 - y1);

                    /*
                   添加代码：遍历后边的covers，做相同变化
                   */

                    Shape curPath = CoverToPathMap[coverPath];
                    GeometryGroup gg = (GeometryGroup)((Path)curPath).Data;
                    // pg 是段的 PathGeometry
                    int indexOfPf = 0;
                    for (indexOfPf = 0; indexOfPf < gg.Children.Count; indexOfPf += 1)
                    {
                        PathGeometry _pg = (PathGeometry)gg.Children[indexOfPf];
                        PathFigure _pf = _pg.Figures[0];
                        if (_pf == pf)
                        {
                            break;
                        }
                    }
                    for (int i = indexOfPf + 2; i < gg.Children.Count; i += 2)
                    {
                        PathGeometry _pg = (PathGeometry)gg.Children[i];
                        PathFigure _pf = _pg.Figures[0];
                        Shape _coverPath = PfToCoverMap[_pf];
                        PathGeometry _coverPath_pg = (PathGeometry)((GeometryGroup)((Path)_coverPath).Data).Children[0];
                        PathFigure _cover_pf = _coverPath_pg.Figures[0];
                        // 求_cover_pf的左侧高度

                        double _y1 = 0;
                        double _y2 = ((LineSegment)_cover_pf.Segments[1]).Point.Y;
                        if (_cover_pf.Segments[0] is LineSegment)
                        {
                            _y1 = ((LineSegment)_cover_pf.Segments[0]).Point.Y;
                        }
                        else if (_cover_pf.Segments[0] is ArcSegment)
                        {
                            _y1 = ((ArcSegment)_cover_pf.Segments[0]).Point.Y;
                        }
                        else
                        {
                            //
                        }
                        double offset = Math.Abs(_y2 - _y1) - Math.Abs(y2 - y1);
                        if (offset == 0) {
                            offset = -changeValue;
                        }
                        ((LineSegment)_cover_pf.Segments[1]).Point = new Point(((LineSegment)_cover_pf.Segments[1]).Point.X, ((LineSegment)_cover_pf.Segments[1]).Point.Y + offset);
                        if (_cover_pf.Segments[0] is LineSegment)
                        {
                            ((LineSegment)_cover_pf.Segments[0]).Point = new Point(((LineSegment)_cover_pf.Segments[0]).Point.X, ((LineSegment)_cover_pf.Segments[0]).Point.Y + offset);
                        }
                        else if (_cover_pf.Segments[0] is ArcSegment)
                        {
                            ((ArcSegment)_cover_pf.Segments[0]).Point = new Point(((ArcSegment)_cover_pf.Segments[0]).Point.X, ((ArcSegment)_cover_pf.Segments[0]).Point.Y + offset);
                        }
                        else
                        {
                            //
                        }
                    }
                }
                else {
                    double y1;
                    double y2 = ((LineSegment)pf.Segments[2]).Point.Y;
                    double changeValue = 0;

                    if (pf.Segments[1] is LineSegment)
                    {
                        y1 = ((LineSegment)pf.Segments[1]).Point.Y;
                    }
                    else if (pf.Segments[1] is ArcSegment)
                    {
                        y1 = ((ArcSegment)pf.Segments[1]).Point.Y;
                    }
                    else
                    {
                        y1 = ((PolyLineSegment)pf.Segments[1]).Points[((PolyLineSegment)pf.Segments[1]).Points.Count - 1].Y;
                    }

                    Shape coverPath = PfToCoverMap[pf];
                    PathGeometry coverPath_pg = (PathGeometry)((GeometryGroup)((Path)coverPath).Data).Children[0];
                    PathFigure cover_pf = coverPath_pg.Figures[0];
                    changeValue = ((LineSegment)cover_pf.Segments[1]).Point.Y - (((LineSegment)cover_pf.Segments[2]).Point.Y + y1 - y2);
                    ((LineSegment)cover_pf.Segments[1]).Point = new Point(((LineSegment)cover_pf.Segments[1]).Point.X, ((LineSegment)cover_pf.Segments[2]).Point.Y + y1 - y2);

                    /*
                   添加代码：遍历后边的covers，做相同变化
                   */
                    Shape curPath = CoverToPathMap[coverPath];
                    GeometryGroup gg = (GeometryGroup)((Path)curPath).Data;
                    // pg 是段的 PathGeometry
                    int indexOfPf = 0;
                    for (indexOfPf = 0; indexOfPf < gg.Children.Count; indexOfPf += 1)
                    {
                        PathGeometry _pg = (PathGeometry)gg.Children[indexOfPf];
                        PathFigure _pf = _pg.Figures[0];
                        if (_pf == pf)
                        {
                            break;
                        }
                    }
                    for (int i = indexOfPf + 2; i < gg.Children.Count; i += 2)
                    {
                        PathGeometry _pg = (PathGeometry)gg.Children[i];
                        PathFigure _pf = _pg.Figures[0];
                        Shape _coverPath = PfToCoverMap[_pf];
                        PathGeometry _coverPath_pg = (PathGeometry)((GeometryGroup)((Path)_coverPath).Data).Children[0];
                        PathFigure _cover_pf = _coverPath_pg.Figures[0];
                        // 求_cover_pf的左侧高度

                        double _y2 = ((LineSegment)_cover_pf.Segments[2]).Point.Y;
                        double _y1 = 0;
                        if (_cover_pf.Segments[1] is LineSegment)
                        {
                            _y1 = ((LineSegment)_cover_pf.Segments[1]).Point.Y;
                        }
                        else if (_cover_pf.Segments[1] is ArcSegment)
                        {
                            _y1 = ((ArcSegment)_cover_pf.Segments[1]).Point.Y;
                        }
                        else
                        {
                            //
                        }
                        double offset = Math.Abs(_y2 - _y1) - Math.Abs(y2 - y1);
                        if (offset == 0) {
                            offset = changeValue;
                        }

                        ((LineSegment)_cover_pf.Segments[2]).Point = new Point(((LineSegment)_cover_pf.Segments[2]).Point.X, ((LineSegment)_cover_pf.Segments[2]).Point.Y - offset);
                        if (_cover_pf.Segments[1] is LineSegment)
                        {
                            ((LineSegment)_cover_pf.Segments[1]).Point = new Point(((LineSegment)_cover_pf.Segments[1]).Point.X, ((LineSegment)_cover_pf.Segments[1]).Point.Y - offset);
                        }
                        else if (_cover_pf.Segments[1] is ArcSegment)
                        {
                            ((ArcSegment)_cover_pf.Segments[1]).Point = new Point(((ArcSegment)_cover_pf.Segments[1]).Point.X, ((ArcSegment)_cover_pf.Segments[1]).Point.Y - offset);
                        }
                        else
                        {
                            //
                        }
                    }
                }
            }
        }

        /*
         * @TODO: 改变层的形状，由arc,poly变line，由line变arc,poly
         */
        public static void processWhenChangeLayerShape(Canvas canvas, StackPanel curSp, Path curPath) {
            GeometryGroup gg = (GeometryGroup)((Path)curPath).Data;
            int layerIndex = 0;
            foreach (PathGeometry pg in gg.Children) {
                if (layerIndex == 0) {
                    layerIndex++;
                    continue;
                }
                PathFigure pf = pg.Figures[0];
                if (layerIndex % 2 == 1)
                {
                    // 奇数时 isTop为0
                    ArrayList arr = new ArrayList(getPathFigureCoordinateWhenChangeShapeOfLayer(canvas, curSp, curPath, pf, 0));
                    Path coverPath = (Path)PfToCoverMap[pf];
                    GeometryGroup _gg = (GeometryGroup)((Path)coverPath).Data;
                    PathGeometry coverPg = (PathGeometry)(_gg.Children[0]);
                    PathFigure coverPf = coverPg.Figures[0];

                    if (pf.Segments[0] is LineSegment)
                    {
                        LineSegment ls = new LineSegment();
                        if (coverPf.Segments[0] is LineSegment)
                        {
                            ls.Point = new Point(((LineSegment)coverPf.Segments[0]).Point.X, ((LineSegment)coverPf.Segments[0]).Point.Y);
                        }
                        else if (coverPf.Segments[0] is ArcSegment)
                        {
                            ls.Point = new Point(((ArcSegment)coverPf.Segments[0]).Point.X, ((ArcSegment)coverPf.Segments[0]).Point.Y);
                        }
                        else {
                            // 需要添加
                        }
                        coverPf.Segments[0] = ls;
                    }
                    else if (pf.Segments[0] is ArcSegment)
                    {
                        ArcSegment aas = new ArcSegment();
                        if (coverPf.Segments[0] is LineSegment)
                        {
                            aas.Point = new Point(((LineSegment)coverPf.Segments[0]).Point.X, ((LineSegment)coverPf.Segments[0]).Point.Y);
                        }
                        else if (coverPf.Segments[0] is ArcSegment)
                        {
                            aas.Point = new Point(((ArcSegment)coverPf.Segments[0]).Point.X, ((ArcSegment)coverPf.Segments[0]).Point.Y);
                        }
                        else
                        {
                            // 需要添加
                        }
                        aas.Size = new Size(((ArcSegment)pf.Segments[0]).Size.Width, ((ArcSegment)pf.Segments[0]).Size.Height);
                        aas.SweepDirection = ((ArcSegment)pf.Segments[0]).SweepDirection;
                        coverPf.Segments[0] = aas;
                    }
                    else
                    {
                        PolyLineSegment ls = new PolyLineSegment();
                        for (int i = 0; i < ((PolyLineSegment)pf.Segments[0]).Points.Count - 1; i++)
                        {
                            double delta_x, delta_y;
                            delta_x = (((Point)arr[0]).X - pf.StartPoint.X);
                            delta_y = (((Point)arr[0]).Y - pf.StartPoint.Y);
                            ls.Points.Add(new Point(((PolyLineSegment)pf.Segments[0]).Points[i].X + delta_x, ((PolyLineSegment)pf.Segments[0]).Points[i].Y + delta_y));
                        }
                        coverPf.Segments[0] = ls;
                    }

                    if (pf.Segments[2] is LineSegment)
                    {
                        LineSegment ls = new LineSegment();
                        if (coverPf.Segments[2] is LineSegment)
                        {
                            ls.Point = new Point(((LineSegment)coverPf.Segments[2]).Point.X, ((LineSegment)coverPf.Segments[2]).Point.Y);
                        }
                        else if (coverPf.Segments[2] is ArcSegment)
                        {
                            ls.Point = new Point(((ArcSegment)coverPf.Segments[2]).Point.X, ((ArcSegment)coverPf.Segments[2]).Point.Y);
                        }
                        else
                        {
                            // 需要添加
                        }
                        coverPf.Segments[2] = ls;
                    }
                    else if (pf.Segments[2] is ArcSegment)
                    {
                        ArcSegment aas = new ArcSegment();
                        if (coverPf.Segments[2] is LineSegment)
                        {
                            aas.Point = new Point(((LineSegment)coverPf.Segments[2]).Point.X, ((LineSegment)coverPf.Segments[2]).Point.Y);
                        }
                        else if (coverPf.Segments[2] is ArcSegment)
                        {
                            aas.Point = new Point(((ArcSegment)coverPf.Segments[2]).Point.X, ((ArcSegment)coverPf.Segments[2]).Point.Y);
                        }
                        else
                        {
                            // 需要添加
                        }
                        aas.Size = new Size(((ArcSegment)pf.Segments[2]).Size.Width, ((ArcSegment)pf.Segments[2]).Size.Height);
                        aas.SweepDirection = ((ArcSegment)pf.Segments[2]).SweepDirection;
                        coverPf.Segments[2] = aas;
                    }
                    else
                    {
                        PolyLineSegment ls3 = new PolyLineSegment();
                        for (int i = 0; i < ((PolyLineSegment)pf.Segments[2]).Points.Count - 1; i++)
                        {
                            double delta_x, delta_y;
                            delta_x = (((Point)arr[0]).X - pf.StartPoint.X);
                            delta_y = (((Point)arr[0]).Y - pf.StartPoint.Y);
                            ls3.Points.Add(new Point(((PolyLineSegment)pf.Segments[2]).Points[i].X + delta_x, ((PolyLineSegment)pf.Segments[2]).Points[i].Y + delta_y));
                        }
                        coverPf.Segments[2] = ls3;
                    }

                }
                else {
                    // 偶数时 isTop为1
                    ArrayList arr = new ArrayList(getPathFigureCoordinateWhenChangeShapeOfLayer(canvas, curSp, curPath, pf, 1));
                    Path coverPath = (Path)PfToCoverMap[pf];
                    GeometryGroup _gg = (GeometryGroup)((Path)coverPath).Data;
                    PathGeometry coverPg = (PathGeometry)(_gg.Children[0]);
                    PathFigure coverPf = coverPg.Figures[0];
                    if (pf.Segments[1] is LineSegment)
                    {
                        LineSegment ls = new LineSegment();
                        if (coverPf.Segments[1] is LineSegment)
                        {
                            ls.Point = new Point(((LineSegment)coverPf.Segments[1]).Point.X, ((LineSegment)coverPf.Segments[1]).Point.Y);
                        }
                        else if (coverPf.Segments[1] is ArcSegment)
                        {
                            ls.Point = new Point(((ArcSegment)coverPf.Segments[1]).Point.X, ((ArcSegment)coverPf.Segments[1]).Point.Y);
                        }
                        else
                        {
                            // 需要添加
                        }
                        coverPf.Segments[1] = ls;
                    }
                    else if (pf.Segments[1] is ArcSegment)
                    {
                        ArcSegment aas = new ArcSegment();
                        if (coverPf.Segments[1] is LineSegment)
                        {
                            aas.Point = new Point(((LineSegment)coverPf.Segments[1]).Point.X, ((LineSegment)coverPf.Segments[1]).Point.Y);
                        }
                        else if (coverPf.Segments[1] is ArcSegment)
                        {
                            aas.Point = new Point(((ArcSegment)coverPf.Segments[1]).Point.X, ((ArcSegment)coverPf.Segments[1]).Point.Y);
                        }
                        else
                        {
                            // 需要添加
                        }
                        aas.Size = new Size(((ArcSegment)pf.Segments[1]).Size.Width, ((ArcSegment)pf.Segments[1]).Size.Height);
                        aas.SweepDirection = ((ArcSegment)pf.Segments[1]).SweepDirection;
                        coverPf.Segments[1] = aas;
                    }
                    else
                    {
                        PolyLineSegment ls = new PolyLineSegment();
                        for (int i = 0; i < ((PolyLineSegment)pf.Segments[1]).Points.Count - 1; i++)
                        {
                            double delta_x, delta_y;
                            delta_x = (((Point)arr[0]).X - pf.StartPoint.X);
                            delta_y = (((Point)arr[0]).Y - pf.StartPoint.Y);
                            ls.Points.Add(new Point(((PolyLineSegment)pf.Segments[1]).Points[i].X + delta_x, ((PolyLineSegment)pf.Segments[1]).Points[i].Y + delta_y));
                        }
                        coverPf.Segments[1] = ls;
                    }

                    if (pf.Segments.Count < 4 || pf.Segments[3] is LineSegment)
                    {
                        LineSegment ls = new LineSegment();
                        if (coverPf.Segments[3] is LineSegment)
                        {
                            ls.Point = new Point(((LineSegment)coverPf.Segments[3]).Point.X, ((LineSegment)coverPf.Segments[3]).Point.Y);
                        }
                        else if (coverPf.Segments[3] is ArcSegment)
                        {
                            ls.Point = new Point(((ArcSegment)coverPf.Segments[3]).Point.X, ((ArcSegment)coverPf.Segments[3]).Point.Y);
                        }
                        else
                        {
                            // 需要添加
                        }
                        coverPf.Segments[3] = ls;
                    }
                    else if (pf.Segments[3] is ArcSegment)
                    {
                        ArcSegment aas = new ArcSegment();
                        if (coverPf.Segments[3] is LineSegment)
                        {
                            aas.Point = new Point(((LineSegment)coverPf.Segments[3]).Point.X, ((LineSegment)coverPf.Segments[3]).Point.Y);
                        }
                        else if (coverPf.Segments[3] is ArcSegment)
                        {
                            aas.Point = new Point(((ArcSegment)coverPf.Segments[3]).Point.X, ((ArcSegment)coverPf.Segments[3]).Point.Y);
                        }
                        else
                        {
                            // 需要添加
                        }
                        aas.Size = new Size(((ArcSegment)pf.Segments[3]).Size.Width, ((ArcSegment)pf.Segments[3]).Size.Height);
                        aas.SweepDirection = ((ArcSegment)pf.Segments[3]).SweepDirection;
                        coverPf.Segments[3] = aas;
                    }
                    else
                    {
                        PolyLineSegment ls3 = new PolyLineSegment();
                        for (int i = 0; i < ((PolyLineSegment)pf.Segments[3]).Points.Count - 1; i++)
                        {
                            double delta_x, delta_y;
                            delta_x = (((Point)arr[0]).X - pf.StartPoint.X);
                            delta_y = (((Point)arr[0]).Y - pf.StartPoint.Y);
                            ls3.Points.Add(new Point(((PolyLineSegment)pf.Segments[3]).Points[i].X + delta_x, ((PolyLineSegment)pf.Segments[3]).Points[i].Y + delta_y));
                        }
                        coverPf.Segments[3] = ls3;
                    }
                }
                
                layerIndex++;
            }
        }

        // 真正的层点击更改颜色后，改变cover的颜色，并作相应处理
        public static void processWhenChangeLayerColor()
        {

        }

        // cover 的双击事件回调,传递到真正的层上，手动触发真正的层的双击事件
        public static void doubleClickOnCover(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Shapes.Path) {
                Path curPath = CoverToPathMap[sender as Path];
                // 触发事件 Img1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
                curPath.RaiseEvent(e);
            }
        }
    }
}