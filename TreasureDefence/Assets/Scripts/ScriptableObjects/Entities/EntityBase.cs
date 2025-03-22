using Gloval;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityBase : ScriptableObject
{
    [Header("基本情報系")]
    [Tooltip("エンティティのID")]
    public string entityId = "Entity-0000";

    [Tooltip("エンティティの名前")]
    public string entityName = "EntityName";

    [Tooltip("職業")]
    public JobBase job;

    [Tooltip("画像")]
    public Sprite sprite;

    [Tooltip("エンティティの盤面の現在地(2次元配列のインデックス)")]
    public int[] entityAddres = new int[Gl_Const.ENTITY_ADDRES_NUM];
}