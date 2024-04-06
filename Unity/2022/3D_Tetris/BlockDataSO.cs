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

        [Header("オブジェクトの幅が偶数かどうか")]
        public bool isEvenWidth;

        [Header("回転可能な周囲のオブジェクトとの距離")]
        public float rotLength;
    }

    public List<BlockData> blockDataList = new();
}