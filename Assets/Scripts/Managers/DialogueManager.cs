using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager
{
    private const int MAX_STORED_COUNT = 5;
    private Dictionary<string, string> availableLines = new Dictionary<string, string>();
    private Queue<string> queuedLines;
    private Dictionary<string, Dictionary<string, string>> storedDialogue = new Dictionary<string, Dictionary<string, string>>();
    private List<string> orderedMapping = new List<string>();
    private GameObject dialogueObject;
    private Text textBox;
    private Image imageBox;

    private static DialogueManager instance;

    public static DialogueManager Instance
    {
        get
        {
            if (instance == null)
                instance = new DialogueManager();

            return instance;
        }
        set
        {
            instance = value;
        }
    }

    public DialogueManager()
    {
        dialogueObject = GameObject.FindGameObjectWithTag("DialogueBox");
        textBox = dialogueObject.GetComponentInChildren<Text>();
        imageBox = dialogueObject.GetComponentInChildren<Image>();
        //LoadFile("Assets/Resources/Text/English.xml");
        textBox.enabled = false;
        imageBox.enabled = false;
    }

    public void QueueLine(string lineName, string sender)
    {

    }

    public void SayLine(string fileName, string lineName, string sender)
    {
        if (!storedDialogue.ContainsKey(fileName))
        {
            storedDialogue.Add(fileName, LoadFromFile("Assets/Resources/Text/" + fileName));
            orderedMapping.Add(fileName);

            if (storedDialogue.Count > MAX_STORED_COUNT)
            {
                string oldFileName = orderedMapping[0];

                storedDialogue.Remove(oldFileName);
            }
        }

        if (!storedDialogue[fileName].ContainsKey(lineName))
        {
            Debug.LogError("DialogueManager: The line '" + lineName + "' could not be found.");
            return;
        }

        textBox.enabled = true;
        imageBox.enabled = true;
        textBox.text = storedDialogue[fileName][lineName];
        dialogueObject.SetActive(true);
        string[] contents = new string[2] { sender, lineName };
        EventManager.TriggerEvent("Dialogue", contents);
    }

    public void AdvanceDialogue()
    {

    }

    public Dictionary<string, string> LoadFromFile(string filePath)
    {
        XmlDocument document = new XmlDocument();
        document.Load(filePath);
        XmlElement root = document.DocumentElement;
        XmlNodeList nodes = root.SelectNodes("entry");

        //availableLines.Clear();

        Dictionary<string, string> dialogue = new Dictionary<string, string>();

        foreach (XmlNode node in nodes)
        {
            dialogue.Add(node.Attributes["name"].Value, node.InnerText);
        }

        return dialogue;
    }
}
   