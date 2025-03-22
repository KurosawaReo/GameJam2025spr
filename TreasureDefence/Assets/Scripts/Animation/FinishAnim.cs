using UnityEngine;

public class FinishAnim : MonoBehaviour
{
    /// <summary>
    /// アニメーションが終わったら実行.
    /// </summary>
    public void Finish()
    {
        Destroy(gameObject); //自身の消滅.
    }
}