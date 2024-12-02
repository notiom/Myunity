using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveManager
{
    // 创建一个接口
    void LoadData(GameData _data);
    void SaveData(ref GameData _data);
}
