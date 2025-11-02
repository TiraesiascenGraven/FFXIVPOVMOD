using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dalamud.Hooking;
using FFXIVClientStructs.FFXIV.Client.Game.Control;
using FFXIVClientStructs.FFXIV.Client.Game;
using Serilog;
using Dalamud.Logging;
using FFXIVClientStructs.FFXIV.Client.Game.Object;
using Hypostasis.Game.Structures;
using System;
using System.Numerics;
using Dalamud.Game.ClientState.Conditions;
using Dalamud.Game.ClientState.Objects.Types;
using FFXIVClientStructs.FFXIV.Client.Game;
using FFXIVClientStructs.FFXIV.Client.Game.Object;
using FFXIVClientStructs.FFXIV.Client.System.Framework;
using Hypostasis.Game.Structures;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Dalamud.Configuration;
using static FFXIVClientStructs.FFXIV.Client.UI.RaptureAtkHistory.Delegates;
using SamplePlugin.Windows;
using System.Diagnostics;
using static Lumina.Data.Parsing.Layer.LayerCommon;
using Dalamud.Hooking.Internal;

namespace SamplePlugin
{

    [HypostasisInjection]

    public static unsafe class HideBody
    {


        private static float CachedDefaultLookAtHeightOffset;
        private static GameObject* PrevCameraTarget;
        private static Vector3 PrevCameraTargetPosition;
        private static float InterpolatedHeight;

        private static float OffsetCalculatedX;
        private static float OffsetCalculatedZ;


        //CAMERA LOCATION + CAMERA OFFSET
        private static void GetCameraPositionDetour(GameCamera* camera, GameObject* target, Vector3* position, Bool swapPerson)
        {
            //Log.Information($"CAMERA DETOUR");
            var newPos = Common.GetBoneWorldPosition(target, (uint)Configuration.BoneToBind) + ((Vector3)target->Position - PrevCameraTargetPosition); ///26 Head 


            /// -------------------- Camera XYZ Offset --------------------
            /// 

            float PlayerYaw = target->Rotation;

            float cosY = MathF.Cos(PlayerYaw);
            float sinY = MathF.Sin(PlayerYaw);

            Vector3 rotatedOffset = new Vector3(
                Configuration.OffsetX * sinY + Configuration.OffsetZ * cosY,
                Configuration.OffsetY,
                Configuration.OffsetX * cosY - Configuration.OffsetZ * sinY
            );

            Vector3 OffsetPos = new Vector3(Configuration.OffsetX+OffsetCalculatedX, Configuration.OffsetY, Configuration.OffsetZ+OffsetCalculatedZ);

            newPos = rotatedOffset + newPos;

            camera->minFoV = MainWindow.arrowOffset;


            /// -------------------- CAMERA DIRECTION --------------------
            /// how tf do I get bone rotations, do later? 

            ///Camera Left/Right
            //camera->currentHRotation = 22;

            //Camera Up/Down
            //camera->currentVRotation = 22;


            /// -------------------- FINAL CAMERA --------------------
            /// Fixes the weird camera issues with PrevCameraTargetPosition

            PrevCameraTargetPosition = target->Position;
            *position = newPos;

        }



        public static void Initialize()
        {
            if (Common.CameraManager == null || !Common.IsValid(Common.CameraManager->worldCamera) || !Common.IsValid(Common.InputData))
                throw new ApplicationException("Failed to validate core structures!");

            var vtbl = Common.CameraManager->worldCamera->VTable;
            vtbl.getCameraPosition.CreateHook(GetCameraPositionDetour);

            Service.Log.Information($"===CALLED INIT===");

        }
        public static void Dispose() 
        {
            //Cammy DOESNT do this but it stops the errors appearing? 
            Common.CameraManager->worldCamera->VTable.getCameraPosition.Hook.Disable();
            Common.CameraManager->worldCamera->VTable.getCameraPosition.Hook.Dispose();

        }
    }
}
