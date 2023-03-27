using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace OppSwap
{
    public class GraphicsDrawable : IDrawable
    {

        public float Angle
        {
            get;
            set;
        }

        public static BindableProperty AngleProperty = BindableProperty.Create(nameof(Angle), typeof(Double), typeof(GraphicsView));
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {

            // Drawing code goes here
            canvas.StrokeColor = Colors.Red;
            canvas.StrokeSize = 6;
            Angle = -3.14f/2;
            canvas.DrawLine(0, 50,50 ,50);
            canvas.DrawLine(0, 0, 0, 100);
            canvas.DrawLine(100, 0, 100, 100);
            canvas.DrawLine(0, 100, 100, 100);
            canvas.StrokeColor= Colors.Blue;
            //canvas.DrawLine(50, 50, (float)(50 * Math.Cos(Angle)) + 50, (float)(50 * Math.Sin(Angle)) + 50);
            canvas.DrawLine((50 * (float)Math.Cos(Angle)) + 50, (50 * (float)Math.Sin(Angle)) + 50, 50, 50);

           
        }

        //(float)(100 * Math.Cos(Angle))
        //(float)(100 * Math.Sin(Angle))

    }
}
