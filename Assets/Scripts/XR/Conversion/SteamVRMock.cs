using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SteamVRMock
{
    /// <summary>
    /// This is a namespace that is used to circumvent the SteamVR dependencies when compiling the game for Android.
    /// This does not provide any functionality.
    /// </summary>
    public class SteamVR_Input_Sources
    {
        public static SteamVR_Input_Sources LeftHand;
        public static SteamVR_Input_Sources RightHand;
        public static SteamVR_Input_Sources Any;
    }

    public class SteamVR_Action_Vibration
    {
        internal void Execute(int v1, float v2, float v3, float v4, object leftHand)
        {
            throw new NotImplementedException();
        }
    }

    public class SteamVR_Action_Skeleton : SteamVR_Action
    {
        internal Vector3 velocity;
        internal Vector3 angularVelocity;
    }

    public class SteamVR_Action_Boolean : SteamVR_Action
    {
        internal void AddOnStateDownListener(Action<SteamVR_Action_Boolean, SteamVR_Input_Sources> triggerTouchDown, SteamVR_Input_Sources handType)
        {
            throw new NotImplementedException();
        }

        internal void AddOnStateUpListener(Action<SteamVR_Action_Boolean, SteamVR_Input_Sources> triggerTouchUp, SteamVR_Input_Sources handType)
        {
            throw new NotImplementedException();
        }

        internal bool GetStateDown(SteamVR_Input_Sources handType)
        {
            throw new NotImplementedException();
        }
    }

    public class SteamVR_Action_Single : SteamVR_Action
    {
        internal void AddOnChangeListener(Action<SteamVR_Action_Single, SteamVR_Input_Sources, float, float> triggerValueChange, SteamVR_Input_Sources handType)
        {
            throw new NotImplementedException();
        }
    }

    public class SteamVR_Action_Vector2 : SteamVR_Action
    {
        internal void AddOnChangeListener(Action<SteamVR_Action_Vector2, SteamVR_Input_Sources, Vector2, Vector2> padTouchValueChange, SteamVR_Input_Sources handType)
        {
            throw new NotImplementedException();
        }
    }

    public class SteamVR_Action
    {

    }

    public class SteamVR_PlayArea : MonoBehaviour
    {

    }

    public class SteamVR_Behaviour_Pose : MonoBehaviour
    {

    }

    public class SteamVR_Input
    {
        internal static T GetAction<T>(string v)
        {
            throw new NotImplementedException();
        }
    }

    public class SteamVR_TrackedObject
    {
        public static EIndex EIndex { get; internal set; }
    }

    public class EIndex
    {
        public static object None;
    }


}
