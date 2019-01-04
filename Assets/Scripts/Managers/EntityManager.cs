using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager
{
    public static EntityManager _instance;

    public static EntityManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new EntityManager();

            return _instance;
        }
        set
        {
            _instance = value;
        }
    }

    List<CharacterAbstract> characters = new List<CharacterAbstract>();

    private Transform _player;

    public static Transform Player
    {
        get
        {
            if (Instance._player == null)
                Instance._player = GameObject.Find("Player").transform;

            return Instance._player;
        }
    }


    public static void AddCharacter(CharacterAbstract character)
    {
        if (character)
            Instance.characters.Add(character);
    }

    public static void RemoveCharacter(CharacterAbstract character)
    {
        if (character)
            Instance.characters.Remove(character);
    }

    public static List<CharacterAbstract> GetCharacters()
    {
        return Instance.characters;
    }
}
