using MoonSharp.Interpreter;

namespace LuaPlugin.Lua.Api
{
    [MoonSharpUserData]
    public class GameApi
    {
        /// <summary>
        /// Prints a local message to the ingame chat
        /// </summary>
        /// <param name="msg">The message to send to the chat(automatically converted to a string)</param>
        /// <param name="color">(optional)Hex color code to color the text</param>
        public void Print(dynamic msg, string color = "00FF00")
        {
            var s = "|cff" + color + msg.ToString();

            wManager.Wow.Helpers.Lua.LuaDoString("DEFAULT_CHAT_FRAME:AddMessage('" + s + "')");
        }
    }
}
