using System;
using System.ComponentModel;
using System.Drawing;
using LuaPlugin.Lua.Api;
using MoonSharp.Interpreter;
using robotManager.Helpful;
using wManager.Events;
using wManager.Wow.Class;
using wManager.Wow.Helpers;
using wManager.Wow.ObjectManager;

namespace LuaPlugin.Lua
{
    public class LuaEnv
    {
        public static bool IsLuaEnvActive;
        public static bool IsScriptLoaded;
        public static Script Script { get; private set; }

        /// <summary>
        /// Loads an Lua script
        /// </summary>
        /// <param name="path">Path to the script</param>
        public static void LoadScript(string path)
        {
            try
            {
                UserData.RegisterAssembly();

                UserData.RegisterType<Spell>();
                UserData.RegisterType<WoWLocalPlayer>();
                UserData.RegisterType<WoWPlayer>();
                UserData.RegisterType<WoWUnit>();
                UserData.RegisterType<SpellManager>();

                UserData.RegisterType<Vector3>();
                UserData.RegisterType<Color>();
                UserData.RegisterType<CancelEventArgs>();

                Script = new Script(CoreModules.Preset_SoftSandbox | CoreModules.LoadMethods)
                {
                    Options =
                    {
                        CheckThreadAccess = false,
                        DebugPrint = Logging.Write
                    }
                };

                Script.Globals["objectManager"] = new ObjectManagerApi();
                Script.Globals["spellManager"] = UserData.Create(typeof(SpellManager));
                Script.Globals["spell"] = UserData.Create(typeof(Spell));
                Script.Globals["color"] = UserData.Create(typeof(Color));
                Script.Globals["game"] = new GameApi();
                Script.Globals["draw"] = new DrawApi();

                IsLuaEnvActive = true;

                try
                {
                    Script.DoFile(path);
                    IsScriptLoaded = true;
                    CallFunctionSafe("onStart");
                    
                    //Fight Callbacks
                    FightEvents.OnFightLoop += OnFightLoop;
                    FightEvents.OnFightStart += OnFightStart;
                    FightEvents.OnFightEnd += OnFightEnd;
                    //Draw Callbacks
                    Radar3D.Pulse();
                    Radar3D.OnDrawEvent += OnDraw;
                }
                catch (InterpreterException ex)
                {
                    Logging.WriteError(ex.DecoratedMessage);
                }
            }
            catch (Exception ex)
            {
                Logging.WriteError("Error Setting up Lua Environment: " + ex.Message);
            }
        }

        /// <summary>
        /// Calls an Lua function safely(in case it doesn't exist)
        /// </summary>
        /// <param name="functionName">The function to call</param>
        /// <param name="args"></param>
        public static void CallFunctionSafe(string functionName, params object[] args)
        {

            if (!IsScriptLoaded) return;

            try
            {
                try
                {
                    CallFunction(Script.Globals.Get(functionName), args);
                }
                catch (ScriptRuntimeException ex)
                {
                    Logging.WriteError(ex.DecoratedMessage);
                }
            }
            catch (Exception ex)
            {
                Logging.WriteError("Error during the execution of '" + functionName + "': " + ex.Message);
            }
        }

        /// <summary>
        /// Used by "CallFunctionSafe"
        /// </summary>
        /// <param name="function"></param>
        /// <param name="args"></param>
        public static void CallFunction(DynValue function, params object[] args)
        {
            if (function.Type != DataType.Function) return;

            Script.Call(function, args);
        }

        /// <summary>
        /// OnFightloop callback
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="cancelable"></param>
        public static void OnFightLoop(WoWUnit unit, CancelEventArgs cancelable)
        {
            if (Conditions.InGameAndConnectedAndAliveAndProductStartedNotInPause)
                CallFunctionSafe("onFightLoop", unit, cancelable);
        }

        /// <summary>
        /// OnFightStart callback
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="cancelable"></param>
        public static void OnFightStart(WoWUnit unit, CancelEventArgs cancelable)
        {
            if (Conditions.InGameAndConnectedAndAliveAndProductStartedNotInPause)
                CallFunctionSafe("onFightStart", unit, cancelable);
        }

        /// <summary>
        /// OnFightEnd callback
        /// </summary>
        /// <param name="sig"></param>
        public static void OnFightEnd(ulong sig)
        {
            if (Conditions.InGameAndConnected && Conditions.ProductIsStartedNotInPause)
                CallFunctionSafe("onFightEnd", sig);
        }

        public static void OnDraw()
        {
            if (Conditions.InGameAndConnected && Conditions.ProductIsStartedNotInPause)
                CallFunctionSafe("onDraw");
        }
    }
}
