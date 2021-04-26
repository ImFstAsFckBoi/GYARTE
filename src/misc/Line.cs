using System;
using Microsoft.Xna.Framework;


namespace GYARTE.misc
{
    public enum CoordinateAxis
    {
        X,
        Y
    }
    
    public struct Line
    {
        public Vector2 StartPoint;
        public Vector2 EndPoint;
        public Vector2 RawDirectionalVector;
        public Vector2 DirectonalVector;
        public Line(Vector2 startPoint, Vector2 directionalVector, float scalar) : this(startPoint, startPoint + directionalVector * scalar) { }
        public Line(float startX, float startY, float endX, float endY) : this(new Vector2(startX, startY), new Vector2(endX, endY)) { }
        public Line(Point start, Point end) : this(new Vector2(start.X, start.Y), new Vector2(end.X, end.Y)) { }
        public Line(Vector2 start, Vector2 end)
        {
            if (start == end)
                throw new ArgumentException("WTF ☠");

            StartPoint = start;
            EndPoint = end;
            
            RawDirectionalVector = new Vector2(end.X - start.X, end.Y - start.Y);
            float min = Math.Min(Math.Abs(RawDirectionalVector.X), Math.Abs(RawDirectionalVector.Y));

            if (min <= 1f)
            {
                DirectonalVector = RawDirectionalVector;
                return;
            }
                
            DirectonalVector = new Vector2(RawDirectionalVector.X / min, RawDirectionalVector.Y / min);
        }
        public bool Intersects(Rectangle rect)
        {
            float y1 = GetCoordsAtXorY(rect.X, CoordinateAxis.X).Y;

            if (Single.IsInfinity(y1)) goto Skip;                                               // Skippar om Infinity för lite snabbare
                                                                                                // Händer om Start X = End X
            float y2 = GetCoordsAtXorY(rect.Right, CoordinateAxis.X).Y;

            if ((y1 >= rect.Y && y1 <= rect.Bottom) || (y2 >= rect.Y && y2 <= rect.Bottom))
            {
                return true;
            }

            Skip:

            float x1 = GetCoordsAtXorY(rect.Y, CoordinateAxis.Y).X;                             // Behöver inte kolla för Infinty 
            float x2 = GetCoordsAtXorY(rect.Bottom, CoordinateAxis.Y).X;                        // för båda kan inte vara pga. 
                                                                                                // if (start == end) i constructor
            if ((x1 >= rect.X && x1 <= rect.Right) || (x2 >= rect.X && x2 <= rect.Right))
            {
                return true;
            }

            return false;
        }
        
        public Vector2 GetCoordsAtXorY(float value, CoordinateAxis inputValueAxis)
        {
            //Arigato Matte Spec :)

            // (X, Y) = (StartPoint) + [Directional Vector] * Scalar

            float t; //Scalar

            Vector2 coord;

            switch(inputValueAxis)
            {
               case CoordinateAxis.X:
                    t = ((value - StartPoint.X) / DirectonalVector.X);              // x = p0 + v.x * t =>
                                                                                    // t = (x - p0) / v.x
                    coord = StartPoint + DirectonalVector * t;                      
                    break;

                case CoordinateAxis.Y:
                    t = ((value - StartPoint.Y) / DirectonalVector.Y);              // y = p0 + v.y * t => 
                                                                                    // t = (y - p0) / v.y
                    coord = StartPoint + DirectonalVector * t;
                    break;

                default:
                    throw new Exception("HUR FAN?? 🚗🚓🚕🛺🚙🚌🚐🚎🚑🚒🚚🚛🚜🚘🚔🚖🚍");
                    
            }

            return coord;
        }
    }
}