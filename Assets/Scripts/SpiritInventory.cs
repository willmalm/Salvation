using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;
using System;

public class SpiritInventory : MonoBehaviour
{
    public bool[] equippedSpirits;

    public bool[] availableSpirits;

    SavedData savedData;

	void Start ()
    {     
        savedData = SavedData.Instance;
        equippedSpirits = savedData.equippedSpirits;
        availableSpirits = savedData.availableSpirits;
        availableSpirits[1] = true;
        availableSpirits[2] = true;
        EquipSpirit(1);
        EquipSpirit(2);

    }
	
	void Update ()
    {

	}

    public void EquipSpirit(int spiritID)
    {
        if (!availableSpirits[spiritID] || equippedSpirits[spiritID])
            return;

        List<SpiritEffect> cardEffects = GetSpiritEffects(spiritID);
        foreach (SpiritEffect cardEffect in cardEffects)
        {
            SavedData.GetValue<CharacterStat>(cardEffect.affectedStat).AddModifier(cardEffect.statModifier);
        }

        equippedSpirits[spiritID] = true;
    }

    public void UnequipSpirit(int spiritID)
    {
        if (!equippedSpirits[spiritID])
            return;

        List<SpiritEffect> cardEffects = GetSpiritEffects(spiritID);
        foreach (SpiritEffect cardEffect in cardEffects)
        {
            SavedData.GetValue<CharacterStat>(cardEffect.affectedStat).RemoveModifiers(cardEffect.statModifier.sourceName);
        }

        equippedSpirits[spiritID] = false;
    }

    private void LoadData()
    {
        equippedSpirits = SavedData.Instance.equippedSpirits;
    }

    private void SaveData()
    {

    }

    private List<SpiritEffect> GetSpiritEffects(int spiritID)
    {
        string filePath = "Assets/Resources/Spirits/Spirit_" + spiritID;

        if (!File.Exists(filePath))
            return null;

        XmlDocument document = new XmlDocument();
        document.Load(filePath);
        XmlElement root = document.DocumentElement;

        XmlNodeList nodes = root.SelectNodes("effect");

        List<SpiritEffect> spiritEffects = new List<SpiritEffect>();

        foreach (XmlNode node in nodes)
        {
            string[] splitLine = node.InnerText.Split(' ');

            StatModifier statModifier = new StatModifier();
            statModifier.sourceName = "Spirit_" + spiritID;
            statModifier.type = (ModifierType) Enum.Parse(typeof(ModifierType), splitLine[1], true);
            statModifier.value = float.Parse(splitLine[2]);

            SpiritEffect spiritEffect = new SpiritEffect();
            spiritEffect.affectedStat = splitLine[0];
            spiritEffect.statModifier = statModifier;

            spiritEffects.Add(spiritEffect);
        }

        return spiritEffects;
    }
}
