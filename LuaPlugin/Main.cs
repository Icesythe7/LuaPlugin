using robotManager.Helpful;
using wManager.Wow.Helpers;
using wManager.Wow.ObjectManager;
using System;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using LuaPlugin.Lua;
using wManager.Plugin;

// ReSharper disable once CheckNamespace
public class Main : IPlugin
{
    private readonly string _path = Environment.CurrentDirectory + "\\Scripts\\";

    public void Initialize()
    {
        Logging.Write("[LuaPlugin] Started.");

        Start();
    }

    public void Dispose()
    {
        LuaEnv.CallFunctionSafe("onStop");

        Radar3D.Stop();

        Logging.Write("[LuaPlugin] Disposed.");
    }
    public void Settings()
    {
        GetSettings.ToForm();
        GetSettings.Save();
    }


    public void Start()
    {
        try
        {
            LuaEnv.LoadScript(_path + GetSettings.FileName);
        }
        catch (Exception ex)
        {
            Logging.WriteError(ex.Message);
        }

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