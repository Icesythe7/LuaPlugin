using MoonSharp.Interpreter;
using wManager.Wow.ObjectManager;

namespace LuaPlugin.Lua.Api
{
    [MoonSharpUserData]
    public class ObjectManagerApi
    {
        /// <summary>
        /// Returns localplayer object
        /// </summary>
        public WoWLocalPlayer Me => ObjectManager.Me;

        public void testfunc()
        {
            //Me.
        }
    }
}
