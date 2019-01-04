using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum MenuState
{
    None,
    Main,
    LoadGame,
    Pause,
    Settings,
    Audio,
    Video,
    Keybindings,
    Controller,
    Count
}

public class UIManager : MonoBehaviour
{
    public static UIManager _instance;

    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<UIManager>();

            if (_instance == null)
                Debug.Log("Couldn't find a UI Manager in the scene.");

            return _instance;
        }
        set
        {
            _instance = value;
        }
    }

    public FSM fsm = new FSM();

    FSMState[] menuStates;

    public Animator menuAnimator;

    public int startState;
    public Transform menuParent;

    GameObject lastSelected;

    GameObject selectedButton;
    bool hovering;

    public RectTransform healthBar;
    public RectTransform staminaBar;
    public RectTransform penaltyBar;
    public RectTransform staminaDepletionBar;
    public RectTransform healthDepletionBar;
    public GameObject spiritBar;
    //public RectTransform spiritUnusedBar;
    public TextMeshProUGUI essenceText;

    Material spirit;

    public static float Stamina
    {
        get
        {
            return Instance.staminaBar.sizeDelta.x / 252.6f;
        }
        set
        {
            Instance.staminaBar.sizeDelta = new Vector2(252.6f * value, 40);
        }
    }

    public static float StaminaDepletion
    {
        get
        {
            return Instance.staminaDepletionBar.sizeDelta.x / 252.6f;
        }
        set
        {
            Instance.staminaDepletionBar.sizeDelta = new Vector2(252.6f * value, 40);
        }
    }

    public static float Health
    {
        get
        {
            return Instance.healthBar.sizeDelta.x / 512.6f;
        }
        set
        {
            Instance.healthBar.sizeDelta = new Vector2(512.6f * value, 40);
        }
    }

    public static float HealthDepletion
    {
        get
        {
            return Instance.healthDepletionBar.sizeDelta.x / 512.6f;
        }
        set
        {
            Instance.healthDepletionBar.sizeDelta = new Vector2(512.6f * value, 40);
        }
    }

    void Start ()
    {
        InitializeMenuStates();

        if (startState > 0)
            fsm.Start(menuStates[startState]);

        spirit = spiritBar.GetComponent<Image>().material;
    }

    private void Update()
    {
        fsm.Update();

        essenceText.text = SavedData.Instance.essence.ToString();

        if (hovering)
        {
            EventSystem.current.SetSelectedGameObject(selectedButton);
        }


        //spiritBar.sizeDelta = new Vector2((510f/3f) * (int)((SavedData.Instance.spirit / SavedData.Instance.spiritMax.Value)/(1f/3f)), 80);
        //spiritUnusedBar.sizeDelta = new Vector2(510 * (SavedData.Instance.spirit / SavedData.Instance.spiritMax.Value), 80);

        spirit.SetFloat("_Value", (int)(SavedData.Instance.spirit.Percentage / (1f / 3f)) / 3f);
        spirit.SetFloat("_Value2", SavedData.Instance.spirit.Percentage);

    }

    private void OnEnable()
    {
        EventManager.Subscribe("HealthChange", UpdateHealth);
        EventManager.Subscribe("StaminaChange", UpdateStamina);
        EventManager.Subscribe("SpiritChange", UpdateSpirit);
    }

    private void OnDisable()
    {
        
    }

    void UpdateHealth(object argument)
    {
        float percentage = (float)argument;
        healthBar.sizeDelta = new Vector2(512.6f * percentage, 40);
    }

    void UpdateStamina(object argument)
    {
        float percentage = (float)argument;
        staminaBar.sizeDelta = new Vector2(252.6f * percentage, 40);
    }

    void UpdateSpirit(object argument)
    {
        float percentage = (float)argument;
        spirit.SetFloat("_Value", (int)(percentage / (1f / 3f)) / 3f);
        spirit.SetFloat("_Value2", percentage);
    }

    void InitializeMenuStates()
    {
        menuStates = new FSMState[(int)MenuState.Count];

        menuStates[0] = new FSMState();
        menuStates[1] = new MenuMain(GetMenuObject(1));
        menuStates[2] = new MenuLoadGame(GetMenuObject(2));
        menuStates[4] = new MenuSettings(GetMenuObject(4));
        menuStates[5] = new MenuVideo(GetMenuObject(5));
    }

    public void MouseEnter(GameObject button)
    {
        //lastSelected = EventSystem.current.currentSelectedGameObject;
        selectedButton = button;
        hovering = true;
        //button.OnSelect(null);
    }

    public void MouseExit()
    {
        hovering = false;
    }

    private GameObject GetMenuObject(int index)
    {
        return menuParent.GetChild(index).gameObject;
    }

    public void ButtonContinue()
    {
        fsm.ExitState();
        GameManager.LoadGame(GameManager.Settings.lastSave);
    }

    public void ButtonUnpause()
    {

    }

    public void ButtonEnableMenu(int stateIndex)
    {
        //fsm.ChangeState(new MenuTransition(menuStates[stateIndex], menuAnimator));
        fsm.ChangeState(menuStates[stateIndex]);
    }

    public void ButtonExitGame()
    {
        Application.Quit();
    }

    public void ButtonLoadGame(int i)
    {
        fsm.ExitState();
        GameManager.LoadGame(i);
    }

    public void ButtonNewGame()
    {
        for (int i = 0; i < 5; i++)
        {
            if (!File.Exists("Assets/Resources/Save_" + i))
            {
                fsm.ExitState();
                GameManager.LoadGame(i);
                return;
            }
        }
    }

    public void ButtonReturn()
    {
        fsm.GoToPreviousState();
    }

    public void DropdownWindowMode(Dropdown changed)
    {
        GameManager.Settings.windowMode = (WindowMode)changed.value;

        if (GameManager.Settings.windowMode == WindowMode.Fullscreen)
            Screen.fullScreen = true;
        else
            Screen.fullScreen = false;
    }
}
