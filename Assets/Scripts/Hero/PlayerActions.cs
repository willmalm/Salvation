using InControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions
{
    private const float LEFT_STICK_DEAD_ZONE = 0.1f;
    private const float RIGHT_STICK_DEAD_ZONE = 0.1f;

    public static float MoveX
    {
        get
        {
            if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.A))
                return 0;
            else if (Input.GetKey(KeyCode.D))
                return 1;
            else if (Input.GetKey(KeyCode.A))
                return -1;

            float inputX = InputManager.ActiveDevice.GetControl(InputControlType.LeftStickX).Value;
            float inputY = InputManager.ActiveDevice.GetControl(InputControlType.LeftStickY).Value;

            if (inputX > LEFT_STICK_DEAD_ZONE || inputX < -LEFT_STICK_DEAD_ZONE || inputY > LEFT_STICK_DEAD_ZONE || inputY < -LEFT_STICK_DEAD_ZONE)
                return inputX;
            else
                return 0;
        }
    }

    public static float MoveY
    {
        get
        {
            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S))
                return 0;
            else if (Input.GetKey(KeyCode.W))
                return 1;
            else if (Input.GetKey(KeyCode.S))
                return -1;

            float inputX = InputManager.ActiveDevice.GetControl(InputControlType.LeftStickX).Value;
            float inputY = InputManager.ActiveDevice.GetControl(InputControlType.LeftStickY).Value;

            if (inputX > LEFT_STICK_DEAD_ZONE || inputX < -LEFT_STICK_DEAD_ZONE || inputY > LEFT_STICK_DEAD_ZONE || inputY < -LEFT_STICK_DEAD_ZONE)
                return inputY;
            else
                return 0;
        }
    }

    public static float RotateCameraX
    {
        get
        {
            return InputManager.ActiveDevice.GetControl(InputControlType.RightStickY).Value + InputManager.ActiveDevice.GetControl(InputControlType.TouchPadYAxis).Value;
        }
    }

    public static float RotateCameraY
    {
        get
        {
            return InputManager.ActiveDevice.GetControl(InputControlType.RightStickX).Value + InputManager.ActiveDevice.GetControl(InputControlType.TouchPadXAxis).Value;
        }
    }

    public static bool LightAttack
    {
        get
        {
            return InputManager.ActiveDevice.GetControl(InputControlType.RightBumper).WasPressed || Input.GetMouseButtonDown(0);
        }
    }

    public static bool Run
    {
        get
        {
            return InputManager.ActiveDevice.GetControl(InputControlType.Action2).IsPressed || Input.GetKey(KeyCode.LeftShift);
        }
    }

    public static bool Dash
    {
        get
        {
            return InputManager.ActiveDevice.GetControl(InputControlType.Action2).WasReleased || Input.GetKeyUp(KeyCode.LeftShift);
        }
    }

    public static bool Jump
    {
        get
        {
            return InputManager.ActiveDevice.GetControl(InputControlType.Action1).IsPressed || Input.GetKey(KeyCode.Space);
        }
    }

    public static bool LockOn
    {
        get
        {
            return InputManager.ActiveDevice.GetControl(InputControlType.RightStickButton).WasPressed || Input.GetMouseButtonDown(2);
        }
    }

    public static bool ChargeShot
    {
        get
        {
            return InputManager.ActiveDevice.GetControl(InputControlType.LeftBumper).WasPressed || Input.GetMouseButton(1);
        }
    }

    public static bool ReleaseShot
    {
        get
        {
            return InputManager.ActiveDevice.GetControl(InputControlType.LeftBumper).WasReleased || Input.GetMouseButtonUp(1);
        }
    }

    public static bool StartHeal
    {
        get
        {
            return InputManager.ActiveDevice.GetControl(InputControlType.Action3).WasPressed || Input.GetKeyDown(KeyCode.H);
        }
    }

    public static bool EndHeal
    {
        get
        {
            return InputManager.ActiveDevice.GetControl(InputControlType.Action3).WasReleased || Input.GetKeyUp(KeyCode.H);
        }
    }

    public static bool PauseGame
    {
        get
        {
            return InputManager.ActiveDevice.GetControl(InputControlType.Select).WasPressed || Input.GetKeyDown(KeyCode.Escape);
        }
    }
}
