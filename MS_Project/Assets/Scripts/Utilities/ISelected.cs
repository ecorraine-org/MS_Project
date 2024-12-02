using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 選ばれる処理インタフェース
/// </summary>
public interface ISelected
{
    /// <summary>
    /// 選ばれる処理
    /// </summary>
    void Selected();

    /// <summary>
    /// 選ばれてない処理
    /// </summary>
    void UnSelected();
}
