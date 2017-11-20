using System.Drawing;
using MoonSharp.Interpreter;
using robotManager.Helpful;
using wManager.Wow.Helpers;

namespace LuaPlugin.Lua.Api
{
    [MoonSharpUserData]
    public class DrawApi
    {
        public void Line(Vector3 from, Vector3 to, Color color, int alpha = 255)
        {
            Radar3D.DrawLine(from, to, color, alpha);
        }

        public void Line(Vector3 from, float x2, float y2, float z2, Color color, int alpha = 255)
        {
            var to = new Vector3(x2, y2, z2);

            Radar3D.DrawLine(from, to, color, alpha);
        }

        public void Line(float x1, float y1, float z1, Vector3 to, Color color, int alpha = 255)
        {
            var from = new Vector3(x1, y1, z1);

            Radar3D.DrawLine(from, to, color, alpha);
        }

        public void Line(float x1, float y1, float z1, float x2, float y2, float z2, Color color, int alpha = 255)
        {
            var from = new Vector3(x1, y1, z1);
            var to = new Vector3(x2, y2, z2);

            Radar3D.DrawLine(from, to, color, alpha);
        }

        public void Circle(Vector3 center, float radius, Color color, bool filled = false, int alpha = 255)
        {
            Radar3D.DrawCircle(center, radius, color, filled, alpha);
        }

        public void Circle(float x1, float y1, float z1, float radius, Color color, bool filled = false, int alpha = 255)
        {
            var center = new Vector3(x1, y1, z1);

            Radar3D.DrawCircle(center, radius, color, filled, alpha);
        }

        public void Circle3D(Vector3 center, float radius, Color color, int quality = 360, int alpha = 255)
        {
            var segmentAngle = 2 * System.Math.PI / quality;

            for (var segment = 0; segment < quality; segment++)
            {
                var startX = center.X + (float)System.Math.Sin(segmentAngle * segment) * radius;
                var startY = center.Y + (float)System.Math.Cos(segmentAngle * segment) * radius;
                var endX = center.X + (float)System.Math.Sin(segmentAngle * (segment + 1)) * radius;
                var endY = center.Y + (float)System.Math.Cos(segmentAngle * (segment + 1)) * radius;

                Line(startX, startY, center.Z, endX, endY, center.Z, color, alpha);
            }
        }

        public void Text(dynamic txt, Vector3 pos, float size, Color color, int alpha = 255, FontFamily font = null)
        {
            var s = txt.ToString();

            Radar3D.DrawString(s, pos, size, color, alpha, font);
        }

        public void Text(dynamic txt, float x1, float y1, float z1, float size, Color color, int alpha = 255, FontFamily font = null)
        {
            var s = txt.ToString();
            var pos = new Vector3(x1, y1, z1);

            Radar3D.DrawString(s, pos, size, color, alpha, font);
        }
    }
}