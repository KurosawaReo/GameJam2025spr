/*
   - TestSingleton.cs -

   Unity���L�̃V���O���g���Ƃ����@�\.
   ����script���A�^�b�`���ꂽobj���A�V�[����ς��Ă��c�葱�����Ԃɂ���.

   [���ӓ_]
   ����script���A�^�b�`���������ł͋@�\���Ȃ�.
   �Q�[�������s���A�A�^�b�`���ꂽobj�̂���V�[���ɍs���ď��߂ăV���O���g���������.
*/
using UnityEngine;

/// <summary>
/// �V���O���g���p�N���X.
/// �V�[�����z���ĕۑ������������܂Ƃ߂�.
/// </summary>
public class TestSingleton : MonoBehaviour
{
    public static TestSingleton instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

