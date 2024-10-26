using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteBullet : Bullet
{
    [SerializeField, Header("�X�v���C�g�����_���[")]
    private SpriteRenderer spriteRenderer;

    /// <param name="isFlipX">�v���C���[�̃X�v���C�g�����]���ꂽ��</param>
    public void Init(bool isFlipX)
    {
        base.Init();

        spriteRenderer.flipX = !isFlipX;

        if (isFlipX)
            shootDirec = new Vector3(1f, 0f, 0f);
        else
            shootDirec = new Vector3(-1f, 0f, 0f);
    }
}
