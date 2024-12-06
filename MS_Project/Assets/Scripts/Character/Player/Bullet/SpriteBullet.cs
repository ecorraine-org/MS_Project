using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteBullet : Bullet
{
    [SerializeField, Header("スプライトレンダラー")]
    protected SpriteRenderer spriteRenderer;

    /// <param name="isFlipX">プレイヤーのスプライトが反転されたか</param>
    public virtual void Init(bool isFlipX)
    {
        base.Init();

        spriteRenderer.flipX = !isFlipX;

        if (isFlipX)
            shootDirec = new Vector3(1f, 0f, 0f);
        else
            shootDirec = new Vector3(-1f, 0f, 0f);
    }
}
