using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Sirenix.OdinInspector;  // Odin Inspectorの名前空間

public enum ClearConditionType
{
    HitAllTargets,// 全ターゲットを打つ
    HitSpecificPart,// 特定ターゲットを打つ
    WeakPointOnly,// 弱点のみを狙う
    HitCorrectTarget
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

    [Title("Mission Settings")]
    public string missionTitle;

    [Title("Game Settings")]
    public int maxArrowCount = 5;
    public int threeStarThreshold = 1;
    public int twoStarThreshold = 3;
    public float pointLightIntensity = 1.0f;
    public float targetVcamDuration = 3f;

    [Title("Clear Condition Settings")]
    [EnumToggleButtons]
    public ClearConditionType clearConditionType;

    // 特定のクリア条件に対してのみ表示される変数

    [ShowIf("@clearConditionType == ClearConditionType.HitSpecificPart || clearConditionType == ClearConditionType.WeakPointOnly")]
    [Tooltip("特定部位の名前を指定します")]
    public string specificPartName;   // 特定部位の名前

    [ShowIf("@clearConditionType == ClearConditionType.HitAllTargets || clearConditionType == ClearConditionType.HitCorrectTarget")]
    [Tooltip("制限時間を設定します")]
    public float timeLimit = 60f;

    [ShowIf("clearConditionType", ClearConditionType.HitCorrectTarget)]
    [Tooltip("ターゲットの画像を設定します")]
    public List<Sprite> targetPicture = new List<Sprite>();  // ターゲットの画像を保持

    [ShowIf("clearConditionType", ClearConditionType.HitCorrectTarget)]
    [Tooltip("配置するターゲットのプレハブを設定します")]
    public List<GameObject> targetPrefabs;  // 配置するターゲットのプレハブを保持

    [Title("Camera Settings")]
    public Vector3 targetCameraOffset;
}
