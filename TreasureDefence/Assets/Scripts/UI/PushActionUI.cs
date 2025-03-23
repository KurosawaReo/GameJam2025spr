using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using Gloval;

/// <summary>
/// �{�^���ɋ��ʂł���script.
/// </summary>
public class PushActionUI : MonoBehaviour
{
    [Header("- script -")]
    [SerializeField] DragUIManager scptDragUI;

    [Header("- value -")]
    [SerializeField] PlyAction action;

    void Awake()
    {
        /*
           ���ӓ_:

           1.using UnityEngine.EventSystems;�̓������K�v
           2.Unity���EventTrigger�̃R���|�[�l���g��ǉ�����K�v������(null�G���[�ɂȂ�)
        */

        EventTrigger.Entry ev = new EventTrigger.Entry();         //�C�x���g�쐬.
        ev.eventID = EventTriggerType.PointerDown;                //���̃C�x���g��.
        ev.callback.AddListener((data) => { MouseClickDown(); }); //���s���������֐��̓o�^.

        //�R���|�[�l���g�擾.
        var cmpEv = GetComponent<EventTrigger>();
        //�R���|�[�l���g�����݂���Ȃ�.
        if (cmpEv != null)
        {
            cmpEv.triggers.Add(ev); //script��Ŕ��f.
        }
        else
        {
            Debug.LogError($"[Error] {gameObject.name} ��EventTrigger�R���|�[�l���g��ǉ����Ă�������");
        }
    }

    /// <summary>
    /// �}�E�X�̃N���b�N�������m�p.
    /// </summary>
    public void MouseClickDown()
    {
        scptDragUI.SendPlyAction(action);
    }
}
