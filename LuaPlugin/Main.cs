using robotManager.Helpful;
using System.Threading;
using robotManager.Products;
using wManager.Wow.Enums;
using wManager.Wow.Helpers;
using wManager.Wow.ObjectManager;
using System;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using LuaPlugin.Lua;

public class Main : wManager.Plugin.IPlugin
{
    private bool _isLaunched;
    private readonly string _path = Environment.CurrentDirectory + "\\Scripts\\";

    public void Initialize()
    {
        Logging.Write("[LuaPlugin] Started.");

        _isLaunched = true;

        WatchForEvents();

        PluginLoop();
    }

    public void Dispose()
    {

        _isLaunched = false;

        Logging.Write("[LuaPlugin] Disposed.");
    }
    public void Settings()
    {
        GetSettings.ToForm();
        GetSettings.Save();
    }


    public void PluginLoop()
    {
        try
        {
            LuaEnv.LoadScript(_path + GetSettings.FileName);
        }
        catch (Exception ex)
        {
            Logging.WriteError(ex.Message);
        }

        while (Products.IsStarted && _isLaunched)
        {
            if (Products.InPause) continue;

            LuaEnv.CallFunctionSafe("onTick");

            Thread.Sleep(10);
        }

    }

    public static void WatchForEvents()
    {
        EventsLuaWithArgs.OnEventsLuaWithArgs += (id, args) =>
        {
            if (id == LuaEventsId.UNIT_MANA)
            { 
                LuaEnv.CallFunctionSafe("onManaChanged");
            }
            if (id == LuaEventsId.CHAT_MSG_SYSTEM)
            {
                Logging.Write("We Received a System Message.");
            }
        };

    }

    private static PluginSettings GetSettings
    {
        get
        {
            try
            {
                if (PluginSettings.CurrentSetting == null)
                    PluginSettings.Load();
                return PluginSettings.CurrentSetting;
            }
            catch (Exception ex)
            {
                Logging.WriteError(ex.Message);
            }
            return new PluginSettings();
        }
    }

    [Serializable]
    public class PluginSettings : Settings
    {

        public PluginSettings()
        {
            FileName = "test.lua";
        }

        [Setting]
        [Category("General Settings")]
        [DisplayName("Script Name")]
        [Description("Enter the name of the script to load.")]
        public string FileName { get; set; }

        public static PluginSettings CurrentSetting { get; set; }

        public bool Save()
        {
            try
            {
                return Save(AdviserFilePathAndName("LuaPlugin", ObjectManager.Me.Name + "." + Usefuls.RealmName));
            }
            catch (Exception ex)
            {
                Logging.WriteError("LuaPlugin > Save(): " + ex.Message);
                return false;
            }
        }

        public static bool Load()
        {
            try
            {
                if (File.Exists(AdviserFilePathAndName("LuaPlugin", ObjectManager.Me.Name + "." + Usefuls.RealmName)))
                {
                    CurrentSetting =
                        Load<PluginSettings>(AdviserFilePathAndName("LuaPlugin",
                                                                      ObjectManager.Me.Name + "." + Usefuls.RealmName));
                    return true;
                }
                CurrentSetting = new PluginSettings();
            }
            catch (Exception ex)
            {
                Logging.WriteError("LuaPlugin > Load(): " + ex.Message);
                return false;
            }
            return false;
        }
    }

}