using System;
using UnityEngine;

using Gloval;

/// <summary>
/// Board�Ɏg��prefab�܂Ƃ�.
/// </summary>
[Serializable]
public class UIPrefab
{
    //private�ϐ�.
    [SerializeField] GameObject m_test01;
    [SerializeField] GameObject m_test02;
    [SerializeField] GameObject m_test03;
    [Space]
    [SerializeField] GameObject m_inObj;

    //get, set.
    public GameObject test01 
    {
        get => m_test01;
        set => m_test01 = value; 
    }
    public GameObject test02
    {
        get => m_test02;
        set => m_test02 = value;
    }
    public GameObject test03
    {
        get => m_test03;
        set => m_test03 = value;
    }
    public GameObject inObj
    {
        get => m_inObj;
        set => m_inObj = value;
    }
}

/// <summary>
/// UI�̑���.
/// </summary>
public class DragUIManager : MonoBehaviour
{
    [Header("- script -")]
    [SerializeField] GridManager scptGridMng;

    [Header("- prefab -")]
    [SerializeField] UIPrefab prfb;

    GameObject nowActionObj; //���݂̃A�N�V������obj.

    void Update()
    {
        DragUI();
    }

    /// <summary>
    /// �h���b�O����.
    /// </summary>
    private void DragUI()
    {
        //����obj�������(=�}�E�X�N���b�N��)
        if (nowActionObj != null)
        {
            var mPos = Gl_Func.GetMousePos();       //�}�E�X���W�擾.
            nowActionObj.transform.position = mPos; //�ړ�.

            //�}�E�X�{�^���𗣂����u��.
            if (Input.GetMouseButtonUp(0))
            {
                var (x, y) = Gl_Func.WPosToBPos(mPos); //�{�[�h���W�ɕϊ�.

                //�Ֆʓ��ł���ꍇ.
                if (x >= 0 && x < scptGridMng.width &&
                    y >= 0 && y < scptGridMng.height)
                {
                    //�ݒu�\�ȃ}�X�Ȃ�.
                    if (scptGridMng.grid[x, y].tileType == TileType.EMPTY ||
                        scptGridMng.grid[x, y].tileType == TileType.RIDE_OBSTACLE)
                    {
                        Gl_Func.ObjPlaceOnBoard(nowActionObj, x, y); //�{�[�h���W�����ɐݒu.
                    }
                    else
                    {
                        //TODO:�A�j���[�V���������.<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                        Destroy(nowActionObj);
                    }
                }
                else
                {
                    Destroy(nowActionObj);
                }

                nowActionObj = null; //�������삵�Ȃ�����obj�f�[�^��j��.
            }
        }
    }

    /// <summary>
    /// �eAction�{�^������񋓑̂𑗂�p.
    /// </summary>
    public void SendPlyAction(PlyAction _plyAction)
    {
        //�A�N�V������.
        switch (_plyAction)
        {
            case PlyAction.TEST01: 
                nowActionObj = Instantiate(prfb.test01, prfb.inObj.transform); 
                break;

            case PlyAction.TEST02: 
                nowActionObj = Instantiate(prfb.test02, prfb.inObj.transform); 
                break;

            case PlyAction.TEST03: 
                nowActionObj = Instantiate(prfb.test03, prfb.inObj.transform); 
                break;
        }
    }
}
