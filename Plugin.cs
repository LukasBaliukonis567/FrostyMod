using BepInEx;
using BepInEx.Logging;
using UnityEngine; // Needed for AudioClip
using LCSoundTool;
using System.IO;
using UnityEngine.Networking;
using HarmonyLib;
using MyFirstPlugin.Patches;  

namespace MyFirstPlugin
{
    [BepInProcess("Lethal Company.exe")]
    [BepInDependency("CustomSounds", "1.0.0")]  // Ensure version matches exactly or adjust if needed
    [BepInDependency("LCSoundTool", "1.5.1")]
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        internal static new ManualLogSource Logger;
        public static AudioClip[] newSFX; // Static field to hold the new sound effects

        private void Awake()
        {
            Logger = base.Logger;
            Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");

            // Set up file path for the sound
            string modDirectory = Path.Combine(Paths.PluginPath, "MyFirstPlugin");
            string subFolder = "Sounds";
            string soundFile = "Frosties.wav";
            string filePath = Path.Combine(modDirectory, subFolder, soundFile);

            // Verify file exists before attempting to load it
            if (File.Exists(filePath))
            {
                StartCoroutine(LoadAndSetSoundEffect(filePath));
            }
            else
            {
                Logger.LogError("Sound file not found at path: " + filePath);
            }

            Harmony.CreateAndPatchAll(typeof(HoarderBugAIPatch));
        }

        private System.Collections.IEnumerator LoadAndSetSoundEffect(string filePath)
        {
            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(filePath, AudioType.WAV))
            {
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.Success)
                {
                    AudioClip loadedSound = DownloadHandlerAudioClip.GetContent(www);
                    Logger.LogInfo("New sound effect loaded successfully!");

                    // Store the sound effect in the static array
                    newSFX = new AudioClip[] { loadedSound };  // Assuming you just want one sound effect for simplicity
                }
                else
                {
                    Logger.LogError("Failed to load audio: " + www.error);
                }
            }
        }
    }
}
