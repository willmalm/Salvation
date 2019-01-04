using UnityEngine;

public class Condition : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;
    public int currentStamina;
    public int maxStamina;
    public bool invincible;
    public bool canHeal;

    public GameObject healthBarUI;
    RectTransform healthBarRect;
    RectTransform healthFilledRect;
    Camera mainCamera;

    public int CurrentHealth
    {
        get { return currentHealth; }
        set
        {
            currentHealth = value;
            healthBarUI.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector3((float)currentHealth / maxHealth * 150, 10);

            if (currentHealth > maxHealth)
                currentHealth = maxHealth;
            else if (currentHealth <= 0)
            {
                Destroy(healthBarUI);
                Destroy(gameObject);
            }
        }
    }

    public int MaxHealth
    {
        get { return maxHealth; }
        set
        {
            currentHealth = value;

            if (currentHealth > maxHealth)
                currentHealth = maxHealth;
        }
    }

    void OnGUI()
    {
    }

    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        GameObject canvas = GameObject.FindGameObjectWithTag("Canvas");

        healthBarUI = Instantiate(healthBarUI, canvas.transform);
        healthBarRect = healthBarUI.GetComponent<RectTransform>();
        healthFilledRect = healthBarUI.transform.GetChild(1).GetComponent<RectTransform>();
    }

    void Update()
    {

    }

    private void LateUpdate()
    {
        healthBarRect.position = mainCamera.WorldToScreenPoint(transform.position + new Vector3(0, 4, 0));
        healthFilledRect.sizeDelta = new Vector2((CurrentHealth / (float)MaxHealth) * 150f, 10);
    }

    void OnEnable()
    {
        EventManager.Subscribe("damage", TakeDamage, gameObject);
    }

    void OnDisable()
    {
        EventManager.Unsubscribe("damage", TakeDamage);
    }

    void TakeDamage(object argument)
    {
        DamageInstance damageInstance = (DamageInstance)argument;
        CurrentHealth -= damageInstance.damage;
    }
}
