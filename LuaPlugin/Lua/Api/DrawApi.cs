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