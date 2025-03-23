using Gloval;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 駒の基底クラス(味方, 敵問わず)
/// </summary>
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

    [Tooltip("HP")]
    public int hp;

    [Tooltip("攻撃力")]
    public int atk;
}