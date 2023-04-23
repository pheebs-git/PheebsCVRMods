using ABI_RC.Core.Player;
using ABI_RC.Core.Savior;
using ABI_RC.Systems.IK.SubSystems;
using ActionMenu;
using HarmonyLib;
using MelonLoader;
using Pheebs.TrackingToggles;
using System.Collections.Generic;

[assembly: MelonInfo(typeof(TrackingToggles), "Tracking Toggles", "0.0.1", "Phoebe Potat")]
[assembly: MelonGame("Alpha Blend Interactive", "ChilloutVR")]
[assembly: MelonAdditionalDependencies("ActionMenu")]
namespace Pheebs.TrackingToggles
{
    public class TrackingToggles : MelonMod
    {
        private static bool _disableArms = false;
        private static bool _disableLegs = false;
        private Menu lib;

        public override void OnInitializeMelon()
        {
            lib = new Menu();
        }

        public class Menu : ActionMenuMod.Lib
        {
            protected override string modName => "Tracking Toggles";
            protected override List<MenuItem> modMenuItems()
            {
                return new List<MenuItem>()
                {
                    new MenuItem("Arms",BuildToggleItem("Arms",(x) => {_disableArms = !_disableArms; }),_disableArms),
                    new MenuItem("Legs",BuildToggleItem("Legs",(x) => {_disableLegs = !_disableLegs; }),_disableLegs),
                };
            }
        }

        public override void OnDeinitializeMelon()
        {
            _disableArms = false;
            _disableLegs = false;
        }

        public static void ToggleTracking()
        {
            if (CheckVR.Instance.hasVrDeviceLoaded)
            {
                //Arms
                BodySystem.TrackingLeftArmEnabled = !_disableArms;
                BodySystem.TrackingRightArmEnabled = !_disableArms;
                //Legs
                BodySystem.enableChestTracking = !_disableLegs;
                BodySystem.enableHipTracking = !_disableLegs;
                BodySystem.TrackingLeftLegEnabled = !_disableLegs;
                BodySystem.TrackingRightLegEnabled = !_disableLegs;
            }
        }

        [HarmonyPatch]
        private static class HarmonyPatches
        {
            [HarmonyPostfix]
            [HarmonyPatch(typeof(PlayerSetup), "Update")]
            public static void After_PlaySetup_SetToggle()
            {
                ToggleTracking();
            }
        }
    }
}
