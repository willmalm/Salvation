using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSelection : MonoBehaviour
{
    GameObject lastSelected;
    Button button;

    private void Start()
    {
        button = GetComponent<Button>();
    }

    void OnMouseEnter()
    {
        lastSelected = EventSystem.current.currentSelectedGameObject;
        EventSystem.current.SetSelectedGameObject(gameObject);
        button.OnSelect(null);
    }

    void OnMouseExit()
    {
        EventSystem.current.SetSelectedGameObject(lastSelected);
        button.OnSelect(null);
    }
}
