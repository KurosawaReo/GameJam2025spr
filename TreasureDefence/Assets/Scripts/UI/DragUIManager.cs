using System;
using UnityEngine;

using Gloval;

/// <summary>
/// prefab���܂Ƃ߂��N���X.
/// </summary>
[Serializable]
public class UIPrefab
{
    //private�ϐ�.
    [SerializeField] GameObject m_test01;
    [SerializeField] GameObject m_test02;
    [SerializeField] GameObject m_test03;
    [SerializeField] GameObject m_test04;
    [Space]
    [SerializeField] GameObject m_inObj;

    //get, set.
    public GameObject test01 
    {
        get => m_test01;
    }
    public GameObject test02
    {
        get => m_test02;
    }
    public GameObject test03
    {
        get => m_test03;
    }
    public GameObject test04
    {
        get => m_test04;
    }
    public GameObject inObj
    {
        get => m_inObj;
    }
}

/// <summary>
/// �A�j���[�V�������܂Ƃ߂��N���X.
/// </summary>
[Serializable]
public class UIAnim
{
    //private�ϐ�.
    [SerializeField] GameObject m_batu;
    [SerializeField] GameObject m_circle;
    [Space]
    [SerializeField] GameObject m_inObj;

    //get, set.
    public GameObject batu
    {
        get => m_batu;
    }
    public GameObject circle
    {
        get => m_circle;
    }
    public GameObject inObj
    {
        get => m_inObj;
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
    [SerializeField] UIAnim   anim;

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
                var (x, y) = Gl_Func.WPosToBoardPos(mPos); //�{�[�h���W�ɕϊ�.
                bool isSucsess = false;

                //�Ֆʓ��ł���ꍇ.
                if (scptGridMng.IsInsideGrid(new Vector2Int(x, y)))
                {
                    //�ݒu�\�ȃ}�X�Ȃ�.
                    if (scptGridMng.grid[x, y].tileType == TileType.EMPTY ||
                        scptGridMng.grid[x, y].tileType == TileType.RIDE_OBSTACLE)
                    {
                        Gl_Func.PlaceOnBoard(nowActionObj, x, y); //�{�[�h���W�����ɐݒu.
                        scptGridMng.grid[x, y].entity = nowActionObj.GetComponent<Piece>().pieceData; // ScriptableObject��������擾
                        isSucsess = true;
                    }
                }

                //���������Ȃ�.
                if (isSucsess)
                {
                    //�A�j���[�V��������.
                    var objAnim = Instantiate(anim.circle, anim.inObj.transform);

                    //�{�[�h���W�����ɐݒu.
                    Gl_Func.PlaceOnBoard(nowActionObj, x, y);
                    Gl_Func.PlaceOnBoard(objAnim, x, y);
                }
                else
                {
                    //���̏�ɃA�j���[�V��������.
                    var objAnim = Instantiate(anim.batu, anim.inObj.transform);
                    objAnim.transform.position = mPos;

                    Destroy(nowActionObj); //obj�͏�������.
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
            case PlyAction.PIECE01: 
                nowActionObj = Instantiate(prfb.test01, prfb.inObj.transform); 
                break;

            case PlyAction.PIECE02: 
                nowActionObj = Instantiate(prfb.test02, prfb.inObj.transform); 
                break;

            case PlyAction.PIECE03: 
                nowActionObj = Instantiate(prfb.test03, prfb.inObj.transform); 
                break;

            case PlyAction.PIECE04:
                nowActionObj = Instantiate(prfb.test04, prfb.inObj.transform);
                break;
        }
    }
}
