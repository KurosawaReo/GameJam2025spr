using Gloval;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��̊��N���X(����, �G��킸)
/// </summary>
public abstract class EntityBase : ScriptableObject
{
    [Header("��{���n")]
    [Tooltip("�G���e�B�e�B��ID")]
    public string entityId = "Entity-0000";

    [Tooltip("�G���e�B�e�B�̖��O")]
    public string entityName = "EntityName";

    [Tooltip("�E��")]
    public JobBase job;

    [Tooltip("�摜")]
    public Sprite sprite;

    [Tooltip("HP")]
    public int hp;

    [Tooltip("�U����")]
    public int atk;
}