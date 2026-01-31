using Godot;

namespace DiegoG.Godot.Common;

public static class TransformExtensions
{
    /*
    extension(Transform3D trans)
    {
        /// <summary>
        /// Returns a transform interpolated between this transform and another transform by a given weight (on the range of 0.0 to 1.0).
        /// </summary>
        /// <param name="other">The other transform</param>
        /// <param name="rotationWeight">A Vector containing values on the range of 0.0 to 1.0, representing the amount of interpolation for the rotation</param>
        /// <param name="scaleWeight">A Vector containing values on the range of 0.0 to 1.0, representing the amount of interpolation for the scale</param>
        /// <param name="originWeight">A Vector containing values on the range of 0.0 to 1.0, representing the amount of interpolation for the origin</param>
        /// <returns>The interpolated transform.</returns>
        public Transform3D InterpolateWith(Transform3D other, float rotationWeight, float scaleWeight, float originWeight)
            => trans.InterpolateWith(other, new Vector3(rotationWeight, scaleWeight, originWeight));
        
        /// <summary>
        /// Returns a transform interpolated between this transform and another transform by a given weight (on the range of 0.0 to 1.0).
        /// </summary>
        /// <param name="other">The other transform</param>
        /// <param name="weight">A Vector containing values on the range of 0.0 to 1.0, representing the amount of interpolation for each transform. X: Rotation, Y: Scale, Z: Origin</param>
        /// <returns>The interpolated transform.</returns>
        public Transform3D InterpolateWith(Transform3D other, Vector3 weight)
        {
            Vector3 scale1 = trans.Basis.Scale;
            Quaternion rotationQuaternion1 = trans.Basis.GetRotationQuaternion().Normalized();
            Vector3 origin1 = trans.Origin;
            Vector3 scale2 = other.Basis.Scale;
            Quaternion rotationQuaternion2 = other.Basis.GetRotationQuaternion().Normalized();
            Vector3 origin2 = other.Origin;
            Transform3D transform3D = new Transform3D();
            Quaternion quaternion = rotationQuaternion1.Slerp(rotationQuaternion2, weight.X).Normalized();
            Vector3 scale3 = scale1.Lerp(scale2, weight.Y);
            SetBasisQuaternionScale(ref transform3D.Basis, quaternion, scale3);
            transform3D.Origin = origin1.Lerp(origin2, weight.Z);
            return transform3D;
        }
    }
    
    public static void SetBasisQuaternionScale(ref Basis bs, in Quaternion q, Vector3 scale)
    {
        SetBasisDiagonal(ref bs, scale);
        RotateBasis(ref bs, q);
    }

    public static void SetBasisDiagonal(ref Basis bs, Vector3 diagonal)
    {
        bs.Row0 = new Vector3(diagonal.X, 0.0f, 0.0f);
        bs.Row1 = new Vector3(0.0f, diagonal.Y, 0.0f);
        bs.Row2 = new Vector3(0.0f, 0.0f, diagonal.Z);
    }

    public static void RotateBasis(ref Basis bs, Quaternion quaternion) => bs = new Basis(quaternion) * bs;
    //*/
}