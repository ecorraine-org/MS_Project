using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteBullet : Bullet
{
    [SerializeField, Header("スプライトレンダラー")]
    protected SpriteRenderer spriteRenderer;

    //ヒットリアクション情報
    protected AttackerParams attackerParams;

    /// <param name="_isFlipX">プレイヤーのスプライトが反転されたか</param>
    public virtual void Init(bool _isFlipX,AttackerParams _attackerParams)
    {
        base.Init();

        spriteRenderer.flipX = !_isFlipX;

        attackerParams = _attackerParams;

        if (_isFlipX)
            shootDirec = new Vector3(1f, 0f, 0f);
        else
            shootDirec = new Vector3(-1f, 0f, 0f);
    }
}
