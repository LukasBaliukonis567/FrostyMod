using HarmonyLib;
using UnityEngine;

namespace MyFirstPlugin.Patches
{
    [HarmonyPatch(typeof(HoarderBugAI))]  // Ensure this is the correct class name
    internal class HoarderBugAIPatch
    {
        // Patching the field 'chitterSFX'
        [HarmonyPatch("Update")]  // This is the field name
        [HarmonyPostfix]  // This will be executed after the field is accessed/modified
        static void FrostySFX(ref AudioClip[] ___chitterSFX)
        {
            // Check if the new sound effect array is loaded
            if (Plugin.newSFX != null && Plugin.newSFX.Length > 0)
            {
                // Replace the chitterSFX with your custom sound effect
                ___chitterSFX = Plugin.newSFX;
                Plugin.Logger.LogInfo("HoarderBugAI chitter sound effect replaced with custom sound.");
            }
            else
            {
                Plugin.Logger.LogError("New sound effects are not loaded yet.");
            }
        }
    }
}
