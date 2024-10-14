using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[System.Serializable]
public class StageObjectData
{
    public AssetReference assetReference; // アセットの参照
    public Vector3 position;              // 配置位置
    public Quaternion rotation;           // 配置回転
    public Vector3 scale = Vector3.one;   // 配置スケール
}

[CreateAssetMenu(fileName = "NewStageData", menuName = "Stage Data")]
public class StageData : ScriptableObject
{
    public List<StageObjectData> objects;     // ステージに配置するオブジェクトのリスト
    public int maxArrowCount = 5;             // ステージで使用可能な矢の本数
    public int threeStarThreshold = 1;        // 3つ星のためのミス許容数
    public int twoStarThreshold = 3;          // 2つ星のためのミス許容数
    public float pointLightIntensity = 1.0f;  // ポイントライトの明るさ
}
