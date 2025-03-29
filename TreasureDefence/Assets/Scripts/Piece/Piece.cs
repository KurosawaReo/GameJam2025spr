using Gloval;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Piece : MonoBehaviour
{
    [Tooltip("HP残量")]
    int hp;

    [Tooltip("攻撃力")]
    int atk;

    [Tooltip("現在どのマスにいるのかをセット")]
    public Vector2Int currentPos;

    [Tooltip("駒のScriptableObjectをセット")]
    public EntityBase pieceData;

    [Tooltip("敵をステートを管理する用")]
    public EntityState currentState = EntityState.STOP;

    [Tooltip("グリッドマネージャーをセット")]
    GridManager gridManager;

    [Tooltip("GameManagerをセット")]
    GameManager gameManager;

    [Tooltip("攻撃の対象")]
    GameObject target;

    [Tooltip("1度しか処理しないところで使うフラグ")]
    bool isFirst;

    [Tooltip("検索に使う配列")]
    Vector2Int[] targetCheckList = {
        new Vector2Int(1,1),    // 左上
        new Vector2Int(0,1),    // 上
        new Vector2Int(-1,1),   // 右上
        new Vector2Int(-1,0),   // 右
        new Vector2Int(-1,-1),  // 右下
        new Vector2Int(0,-1),   // 下
        new Vector2Int(1,-1),   // 左下
        new Vector2Int(-1,0),   // 左
    };

    // Start is called before the first frame update
    void Start()
    {
        // 初期化
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case EntityState.STOP:
                StopState();
                break;

            case EntityState.MOVE:
                MoveState();
                break;

            case EntityState.ATTACK:
                AttackState();
                break;

            case EntityState.DEATH:
                DeathState();
                break;

            case EntityState.NONE:
                break;
        }
    }

    /// <summary>
    /// 初期化処理
    /// </summary>
    void Init()
    {
        isFirst = true;

        // HPと攻撃力をデータで初期化する
        hp = pieceData.hp;
        atk = pieceData.atk;

        // グリッドマネージャーを取得
        gridManager = FindObjectOfType<GridManager>();
        // ゲームマネージャーを取得
        gameManager = FindObjectOfType<GameManager>();
    }

    /// <summary>
    /// 攻撃を受けた時の処理
    /// </summary>
    /// <param name="dmg">受けたダメージ</param>
    public void Damage(int dmg)
    {
        hp -= dmg;

        // 死亡した場合
        if (hp <= 0)
        {
            hp = 0;
            var index = 0;

            for (var i = 0; i < gridManager.activePieceObjList.Count; i++)
            {
                if (gridManager.activePieceObjList[i] == gameObject)
                {
                    // 自分自身の場合
                    index = i;
                    break;
                }
            }

            // 取得したインデックスの要素を削除する
            gridManager.activePieceList.RemoveAt(index);
            gridManager.activePieceObjList.RemoveAt(index);

            // 駒のカウント-1
            gameManager.AddPlyPieceCnt(-1);

            Destroy(gameObject); // 自身を破棄する
        }
    }

    /// <summary>
    /// ステートがStopの時の処理
    /// </summary>
    void StopState()
    {
        var index = 0;
        for (var i = 0; i < gridManager.activePieceObjList.Count; i++)
        {
            if (gridManager.activePieceObjList[i] == gameObject)
            {
                // 自分自身の場合
                //print($"自分を見つけました");
                index = i;
                break;
            }
        }

        // 自分の周囲に敵がいるかを検索する
        for (var i = 0; i < targetCheckList.Length; i++)
        {
            for (var j = 0; j < gridManager.activeEnemyList.Count; j++)
            {
                //print($"targetCheckList[i]:{targetCheckList[i]}");
                //print($"gridManager.activePieceList[index] + targetCheckList[i]:{gridManager.activePieceList[index] + targetCheckList[i]}");
                //if (gridManager.activePieceList[index] + targetCheckList[i] == gridManager.activeEnemyList[j])
                //{
                //    print("自分の周囲に敵がいます");
                //}
            }
        }
    }

    /// <summary>
    /// ステートがMoveの時の処理
    /// </summary>
    void MoveState()
    {
    }

    /// <summary>
    /// ステートがAttackの時の処理
    /// </summary>
    void AttackState()
    {
        // 1度しか通らないようにする
        if (isFirst)
        {
            print("アタックステート内");
            // 攻撃処理を呼ぶ
            StartCoroutine(Attack());

            isFirst = false;
        }
    }

    /// <summary>
    /// ステートがDeathの時の処理
    /// </summary>
    void DeathState()
    {
        // 死亡処理
    }

    /// <summary>
    /// ステートを変更する時の処理
    /// </summary>
    public void ChangeState(EntityState newState)
    {
        // 変更後のステートが変更前と同じではないか確認する
        if (currentState == newState)
        {
            return;
        }

        print($"State changed: {currentState} → {newState}");
        currentState = newState;
    }

    /// <summary>
    /// 攻撃処理
    /// </summary>
    /// <returns></returns>
    IEnumerator Attack()
    {
        // 少しディレイさせたいので少し処理を待つ
        yield return Gl_Func.Delay(1f);

        // 実際に攻撃する
        print("攻撃");
        if (target != null)
        {
            target.GetComponent<Piece>().Damage(atk);
        }

        isFirst = true; // 攻撃のステートで使うのでフラグを戻しておく
        currentState = EntityState.STOP; // ステートをMoveに変更
    }
}
