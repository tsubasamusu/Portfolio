using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "BlockDataSO", menuName = "Create BlockDataSO")]
public class BlockDataSO : ScriptableObject
{
    public enum BlockName
    {
        I,
        J,
        L,
        O,
        S,
        T,
        Z
    }

    [Serializable]
    public class BlockData
    {
        public BlockName name;

        public GameObject prefab;

        public Sprite sprite;

        [Header("�I�u�W�F�N�g�̕����������ǂ���")]
        public bool isEvenWidth;

        [Header("��]�\�Ȏ��͂̃I�u�W�F�N�g�Ƃ̋���")]
        public float rotLength;
    }

    public List<BlockData> blockDataList = new();
}