﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ZBuffer.Tools;
using ZBuffer.Shapes;
using System.Windows.Media.Media3D;
using System.Runtime.Serialization;
using ZBuffer.Affine_Transformation;

namespace ZBuffer
{
    //TODO Think about changing DataSerializer to XmlSerializer
    [DataContract]
    [KnownType(typeof(MFacet))]
    public class Scene
    {
        //TODO Refactor
        public WriteableBitmap Bitmap { get; set; }

        //TODO Make public?
        [DataMember]
        private List<MCommonPrimitive> Shapes { get; set; }
        [DataMember]
        private List<MCommonPrimitive> SelectedShapes { get; set; }
        private Tools.ZBuffer Buffer { get; set; }
        public Tools.Camera CurrentCamera { get; set; }


        public Scene(int width, int heigth, int z)
        {
            Buffer = new Tools.ZBuffer(width * heigth);

            CurrentCamera = new Tools.OrthographicCamera(-40, 40, -100, 100, -10, 0);
            //CurrentCamera = new Tools.PerspectiveCamera((float)Math.PI / 4, 16 / 9, 10, -10);

            Bitmap = new WriteableBitmap(width, heigth, 96, 96, PixelFormats.Bgra32, null);

            Shapes = new List<MCommonPrimitive>();

            SelectedShapes = new List<MCommonPrimitive>();
        }

        public WriteableBitmap Render()
        {
            new ShapeEditor().GetTransformedShapes(Shapes, CurrentCamera);

            List<MPoint> allPoints = GetAllPoints();

            new Painter().DrawSceneByPoints(this, allPoints);

            return Bitmap;
        }

        public void AddShape(MCommonPrimitive shape)
        {
            Shapes.Add(shape);
        }

        //TODO Rework
        public void RotateSelected(double angle)
        {
            //TODO remove this
            SelectedShapes = Shapes;

            var editor = new ShapeEditor();

            for (int i = 0; i < SelectedShapes.Count; ++i)
                editor.Move(SelectedShapes[i], 10, 0, 0);
        }

        //TODO Rework
        public void MoveSelected(float Sx, float Sy, float Sz)
        {
            //TODO remove this
            SelectedShapes = Shapes;

            var editor = new ShapeEditor();

            for (int i = 0; i < SelectedShapes.Count; ++i)
                editor.Move(SelectedShapes[i], Sx, Sy, Sz);
        }

        //TODO Rework
        public void ScaleSelected(float Sx, float Sy, float Sz)
        {
            //TODO remove this
            SelectedShapes = Shapes;

            var editor = new ShapeEditor();

            for (int i = 0; i < SelectedShapes.Count; ++i)
                editor.Scale(SelectedShapes[i], Sx, Sy, Sz);
        }

        private List<MPoint> GetAllPoints()
        {
            var allPoints = new List<MPoint>();

            foreach (MShape shape in Shapes)
            {
                allPoints.AddRange(shape.GetAllPoints());
            }
            //foreach(MShape shape in Shapes)
            //{
            //    foreach (MPoint point in shape.GetPoints())
            //        allPoints.Add(point);
            //}


            return allPoints;
        }
    }
}
