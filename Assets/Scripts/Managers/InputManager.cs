//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class InputManager : MonoBehaviour
//{
//	private static InputManager _instance;

//    public static InputManager Instance
//    {
//        get
//        {
//            if (_instance == null)
//                _instance = FindObjectOfType<InputManager>();

//            if (_instance == null)
//                Debug.Log("Couldn't find an Input Manager in the scene.");

//            return _instance;
//        }
//        set
//        {
//            _instance = value;
//        }
//    }

//    private Dictionary<string, object> inputs = new Dictionary<string, object>();

//    public static bool GetBool(string inputName)
//    {
//        UpdateInput();
//        if (!Instance.inputs.ContainsKey(inputName))
//        {
//            Debug.LogError("Could not find input: " + inputName);
//            return false;
//        }

//        return (bool)Instance.inputs[inputName];
//    }

//    public static float GetFloat(string inputName)
//    {
//        UpdateInput();
//        if (!Instance.inputs.ContainsKey(inputName))
//        {
//            Debug.LogError("Could not find input: " + inputName);
//            return 0;
//        }

//        return (float)Instance.inputs[inputName];
//    }

//    public static void SetInput(string inputName, object value)
//    {
//        if (!Instance.inputs.ContainsKey(inputName))
//            Instance.inputs.Add(inputName, value);
//        else
//            Instance.inputs[inputName] = value;
//    }

//    public static void UpdateInput()
//    {
//        SetInput("MoveX", Input.GetAxis("Horizontal"));
//        SetInput("MoveZ", Input.GetAxis("Vertical"));
//        SetInput("CameraX", Input.GetAxis("RightHorizontal"));
//        SetInput("CameraY", Input.GetAxis("RightVertical"));
//        SetInput("Run", Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.LeftShift));
//        SetInput("Dash", Input.GetKeyUp("joystick button 1") || Input.GetKeyUp(KeyCode.LeftShift));
//        SetInput("LockOn", Input.GetKeyDown("joystick button 9") || Input.GetKeyDown(KeyCode.T));
//        SetInput("LightAttack", Input.GetKeyDown("joystick button 5") || Input.GetMouseButtonDown(0));
//        SetInput("ChargeHeal", Input.GetKey("joystick button 2") || Input.GetKey(KeyCode.H));
//        SetInput("EndHeal", Input.GetKeyUp("joystick button 2") || Input.GetKeyUp(KeyCode.H));
//        SetInput("Sneak", Input.GetKey(KeyCode.LeftControl));
//        SetInput("Jump", Input.GetKeyDown("joystick button 0") || Input.GetKeyDown(KeyCode.Space));
//        SetInput("ChargeShot", Input.GetKey("joystick button 4") || Input.GetKey(KeyCode.F));
//        SetInput("ReleaseShot", Input.GetKeyUp("joystick button 4") || Input.GetKeyUp(KeyCode.F));
//    }
//}
