using Gloval;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [Tooltip("�G���e�B�e�B�̔Ֆʂ̌��ݒn(2�����z��̃C���f�b�N�X)")]
    public int[] entityAddres = new int[Gl_Const.ENTITY_ADDRES_NUM];
}