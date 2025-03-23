using System;
using UnityEngine;

using Gloval;

/// <summary>
/// prefab���܂Ƃ߂��N���X.
/// </summary>
[Serializable]
public class PiecePrefab
{
    //private�ϐ�.
    [SerializeField] GameObject m_piece01;
    [SerializeField] GameObject m_piece02;
    [SerializeField] GameObject m_piece03;
    [SerializeField] GameObject m_piece04;
    [Space]
    [SerializeField] GameObject m_inObj;

    //get, set.
    public GameObject piece01 
    {
        get => m_piece01;
    }
    public GameObject piece02
    {
        get => m_piece02;
    }
    public GameObject piece03
    {
        get => m_piece03;
    }
    public GameObject piece04
    {
        get => m_piece04;
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
    [SerializeField] GameObject m_dotCircle;
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
    public GameObject dotCircle
    {
        get => m_dotCircle;
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
    [SerializeField] GameManager scptGameMng;

    [Header("- prefab -")]
    [SerializeField] PiecePrefab prfb;
    [SerializeField] UIAnim      anim;

    GameObject nowActionObj; //���݂̃A�N�V������obj.

    void Update()
    {
        //�܂����u����Ȃ�.
        if (!scptGameMng.IsPlyPieceMax())
        {
            DragUI();
        }
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
                //�ݒu�\�A�j���[�V��������.
                EraseSetAble(); 

                var (x, y) = Gl_Func.WPosToBoardPos(mPos); //�{�[�h���W�ɕϊ�.

                //�ݒu�ł���Ȃ�.
                if (scptGridMng.CanPlacePiece(x, y))
                {
                    DragSucsess(x, y); //��������.
                }
                else
                {
                    DragFailure(mPos); //���s����.
                }

                nowActionObj = null; //�������삵�Ȃ�����obj�f�[�^��j��.
            }
        }
    }

    /// <summary>
    /// �h���b�O����.
    /// </summary>
    /// <param name="x">�{�[�h���Wx</param>
    /// <param name="y">�{�[�h���Wy</param>
    private void DragSucsess(int x, int y)
    {
        //ScriptableObject��������擾.
        scptGridMng.grid[x, y].entity = nowActionObj.GetComponent<Piece>().pieceData;
        //��J�E���g+1
        scptGameMng.AddPlyPieceCnt(1);

        //�����A�j���[�V����.
        var objAnim = Instantiate(anim.circle, anim.inObj.transform);

        scptGridMng.activePieceList.Add(new Vector2Int(x, y)); // �ݒu������̈ʒu��ێ�����
        scptGridMng.activePieceObjList.Add(nowActionObj); // �ݒu������̃I�u�W�F�N�g��ێ�����

        //�{�[�h��ɔz�u.
        Gl_Func.PlaceOnBoard(nowActionObj, x, y); //����obj.                    
        Gl_Func.PlaceOnBoard(objAnim, x, y);      //�A�j���[�V����obj.
    }
    /// <summary>
    /// �h���b�O���s.
    /// </summary>
    /// <param name="mPos">�}�E�X���W</param>
    private void DragFailure(Vector2 mPos)
    {
        //���s�A�j���[�V����.
        var obj = Instantiate(anim.batu, anim.inObj.transform);
        obj.transform.position = mPos;

        Destroy(nowActionObj); //obj�͏�������.
    }

    /// <summary>
    /// �eAction�{�^������񋓑̂𑗂�p.
    /// </summary>
    public void SendPlyAction(PlyAction _plyAction)
    {
        //�܂����u����Ȃ�.
        if (!scptGameMng.IsPlyPieceMax())
        { 
            //�A�N�V������.
            switch (_plyAction)
            {
                case PlyAction.PIECE01: 
                    nowActionObj = Instantiate(prfb.piece01, prfb.inObj.transform); 
                    break;

                case PlyAction.PIECE02: 
                    nowActionObj = Instantiate(prfb.piece02, prfb.inObj.transform); 
                    break;

                case PlyAction.PIECE03: 
                    nowActionObj = Instantiate(prfb.piece03, prfb.inObj.transform); 
                    break;

                case PlyAction.PIECE04:
                    nowActionObj = Instantiate(prfb.piece04, prfb.inObj.transform);
                    break;
            }

            DisSetAble();
        }
    }

    /// <summary>
    /// �ݒu�\�}�X�̕\��.
    /// </summary>
    private void DisSetAble()
    {
        //�S�}�X���[�v.
        for (int i = 0; i < scptGridMng.width; i++) {
            for (int j = 0; j < scptGridMng.height; j++) {

                //�ݒu�\�Ȃ�.
                if (scptGridMng.CanPlacePiece(i, j))
                {
                    var objAnim = Instantiate(anim.dotCircle, anim.inObj.transform);

                    //�{�[�h��ɔz�u.
                    Gl_Func.PlaceOnBoard(objAnim, i, j);
                }
            }
        }
    }
    /// <summary>
    /// �ݒu�\�}�X����.
    /// </summary>
    private void EraseSetAble()
    {
        //�ݒu�\�T�C����obj�S�擾.
        var setAbles = GameObject.FindGameObjectsWithTag("set_able");
        //�S�ď���.
        foreach (var i in setAbles)
        {
            Destroy(i);
        }
    }
}
