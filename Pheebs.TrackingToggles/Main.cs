using ABI_RC.Core.Player;
using ABI_RC.Core.Savior;
using ABI_RC.Systems.IK.SubSystems;
using ActionMenu;
using HarmonyLib;
using MelonLoader;
using Pheebs.TrackingToggles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: MelonInfo(typeof(TrackingToggles), "Tracking Toggles", "0.0.1", "Phoebe Potat")]
[assembly: MelonGame("Alpha Blend Interactive", "ChilloutVR")]
[assembly: MelonAdditionalDependencies("ActionMenu")]
namespace Pheebs.TrackingToggles
{
    public class TrackingToggles : MelonMod
    {
        private static bool _disableLeftArm = false;
        private static bool _disableRightArm = false;
        private static bool _disableBothArms = false;
        private Menu lib;

        public override void OnInitializeMelon()
        {
            lib = new Menu();
        }

        public class Menu : ActionMenuMod.Lib
        {
            protected override string modName => "Ik Toggle";
            protected override List<MenuItem> modMenuItems()
            {
                return new List<MenuItem>()
                {
                    new MenuItem("Toggle Left Arm",BuildToggleItem("Left",(x) => {_disableLeftArm = !_disableLeftArm; }),_disableLeftArm),
                    new MenuItem("Toggle Right Arm",BuildToggleItem("Right",(x) => {_disableRightArm = !_disableRightArm; }),_disableRightArm),
                    new MenuItem("Toggle L/R",BuildToggleItem("L/R",(x) => {_disableBothArms = !_disableBothArms; }),_disableBothArms),
                };
            }
        }

        public override void OnDeinitializeMelon()
        {
            _disableLeftArm = false;
            _disableRightArm = false;
            _disableBothArms = false;
        }

        public static void ToggleTracking()
        {
            if (CheckVR.Instance.hasVrDeviceLoaded)
            {
                if (!_disableBothArms)
                {
                    BodySystem.TrackingLeftArmEnabled = !_disableLeftArm;
                    BodySystem.TrackingRightArmEnabled = !_disableRightArm;
                }
                else
                {
                    BodySystem.TrackingLeftArmEnabled = !_disableBothArms;
                    BodySystem.TrackingRightArmEnabled = !_disableBothArms;
                }
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
