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

        /// <summary>
        /// Returns our target object
        /// </summary>
        public WoWUnit Target => ObjectManager.Target;

        /// <summary>
        /// Returns our pet
        /// </summary>
        public WoWUnit Pet => ObjectManager.Pet;
    }
}
