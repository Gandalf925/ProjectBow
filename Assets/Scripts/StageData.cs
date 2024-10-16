using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public enum ClearConditionType
{
    HitAllTargets,// 全ターゲットを打つ
    HitSpecificPart,// 特定ターゲットを打つ
    TimeLimit,// 時間内に打つ
    WeakPointOnly// 弱点のみを狙う
}

[System.Serializable]
public class StageObjectData
{
    public AssetReference assetReference;
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale = Vector3.one;
}

[CreateAssetMenu(fileName = "NewStageData", menuName = "Stage Data")]
public class StageData : ScriptableObject
{
    public List<StageObjectData> objects;
    public string missionTitle;
    public int maxArrowCount = 5;
    public int threeStarThreshold = 1;
    public int twoStarThreshold = 3;
    public float pointLightIntensity = 1.0f;

    public ClearConditionType clearConditionType;

    // クリア条件の追加データ
    public string specificPartName;   // 特定部位の名前
    public float timeLimit = 60f;
    public int requiredHitsInRow = 3;
    public List<GameObject> orderedTargets; // HitInOrder用
    public Sprite stageImage; // ステージ画像
}
