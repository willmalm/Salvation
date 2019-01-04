using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

public enum EntryName
{
    GREET1,
    GREET2
}

public class Dialogue
{
    public string fileName;
    public Dictionary<string, string> entries;
    public KeyValuePair<string, string> selectedEntry;

    public Dialogue(string fileName)
    {
        this.fileName = fileName;
        entries = new Dictionary<string, string>();
    }

    public string GetEntry(string name)
    {
        if (!entries.ContainsKey(name))
        {
            Debug.LogError("No dialogue entry '" + name.ToString() + "' found.");
            return null;
        }
        else
            return entries[name];
    }

    public void SaveDialogue()
    {
        string filePath = "Assets/Resources/DE_" + fileName + ".xml";

        if (!File.Exists(filePath))
        {
            FileStream fileStream = File.Create(filePath);
            fileStream.Close();
            fileStream.Dispose();
        }

        XmlDocument document = new XmlDocument();

        XmlElement root = document.CreateElement("entries");
        foreach (KeyValuePair<string, string> entry in entries)
        {
            XmlElement element = document.CreateElement("entry");
            element.SetAttribute("name", entry.Key.ToString());
            element.InnerText = entry.Value;
            root.AppendChild(element);
        }

        document.AppendChild(root);
        document.Save(filePath);
    }

    public void LoadDialogue()
    {
        string filePath = "Assets/Resources/DE_" + fileName + ".xml";
        if (!File.Exists(filePath))
            return;

        Dictionary<string, string> entries = new Dictionary<string, string>();

        XmlDocument document = new XmlDocument();
        document.Load(filePath);
        XmlElement root = document.DocumentElement;
        XmlNodeList nodes = root.SelectNodes("entry");

        foreach (XmlNode node in nodes)
        {
            string entryName = (string)Enum.Parse(typeof(EntryName), node.Attributes["name"].Value, true);
            entries.Add(node.Attributes["name"].Value, node.InnerText);
        }

        this.entries = entries;
    }
}
