using Godot;
using Godot.Collections;

namespace DiegoG.Godot.Common;

public readonly record struct RayCastResult(
    Vector3 IntersectionPoint,
    Vector3 IntersectionNormal,
    Variant Collider,
    int ColliderId,
    int FaceIndex,
    Rid IntersectingObjectRid,
    int ShapeIndex
)
{
    public static class RayCastQueryDictionaryKeys
    {
        public static Variant Position { get; } = "position";
        public static Variant Collider { get; } = "collider";
        public static Variant ColliderId { get; } = "collider_id";
        public static Variant Normal { get; } = "normal";
        public static Variant FaceIndex { get; } = "face_index";
        public static Variant Rid { get; } = "rid";
        public static Variant Shape { get; } = "shape";
    }
    
    public static RayCastResult Empty => default;

    public static RayCastResult FromDictionary(Dictionary? dict)
        => dict is not null && dict.Count > 0
            ? new RayCastResult(
                IntersectionPoint: dict[RayCastQueryDictionaryKeys.Position].AsVector3(),
                IntersectionNormal: dict[RayCastQueryDictionaryKeys.Normal].AsVector3(),
                Collider: dict[RayCastQueryDictionaryKeys.Collider],
                ColliderId: dict[RayCastQueryDictionaryKeys.ColliderId].AsInt32(),
                FaceIndex: dict[RayCastQueryDictionaryKeys.FaceIndex].AsInt32(),
                IntersectingObjectRid: dict[RayCastQueryDictionaryKeys.Rid].AsRid(),
                ShapeIndex: dict[RayCastQueryDictionaryKeys.Shape].AsInt32()
            )
            : default;
}
