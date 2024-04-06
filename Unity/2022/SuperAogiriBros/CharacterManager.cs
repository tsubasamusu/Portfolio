using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterManager : MonoBehaviour
{
    public enum CharaName
    {
        Tamako,
        Mashiro
    }

    public enum KeyType
    {
        Up, Down,
        Right,
        Left
    }

    [Serializable]
    public class CharaData
    {
        public CharaName charaName;

        public KeyType keyType;

        public KeyCode key;
    }

    public List<CharaData> charaDataList = new();

    public KeyCode GetCharacterControllerKey(CharaName charaName, KeyType keyType)
    {
        return charaDataList.Find(x => x.charaName == charaName && x.keyType == keyType).key;
    }

    [Serializable]
    public class CharacterClassData
    {
        public CharaName charaName;

        public NPCController npcController;

        public Tsubasa.CharacterController characterController;

        public CharacterHealth characterHealth;
    }

    public List<CharacterClassData> characterClassDataList = new();

    public NPCController GetNpcController(CharaName charaName)
    {
        return characterClassDataList.Find(x => x.charaName == charaName).npcController;
    }

    public Tsubasa.CharacterController GetCharacterController(CharaName charaName)
    {
        return characterClassDataList.Find(x => x.charaName == charaName).characterController;
    }

    public CharacterHealth GetCharacterHealth(CharaName charaName)
    {
        return characterClassDataList.Find(x => x.charaName == charaName).characterHealth;
    }
}