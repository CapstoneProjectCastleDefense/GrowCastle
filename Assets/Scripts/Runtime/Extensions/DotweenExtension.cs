namespace Runtime.Extensions
{
    using System;
    using System.Collections.Generic;
    using DG.Tweening;
    using UnityEngine;

    public static class DotweenExtension
    {
        public static Tween Fly(
            this Transform flyObj,
            Vector3 startPos,
            Vector3 endPos,
            int fragment,
            float duration,
            float delay,
            Vector3 VectorOrientation,
            Action finishCallback,
            Ease ease = Ease.Linear)
        {
            flyObj.gameObject.SetActive(true);
            List<Vector3> paths = new List<Vector3>();
            paths.Add(startPos);
            Vector3 AB      = endPos - startPos;
            Vector3 C_prime = startPos + AB / fragment;
            Vector3 C       = C_prime + Vector3.Normalize(Vector3.Cross(AB, VectorOrientation));

            paths.Add(C);
            paths.Add(endPos);

            if (finishCallback != null)
            {
                return flyObj.DOPath(paths.ToArray(), duration, PathType.CatmullRom).SetDelay(delay).SetOptions(false, AxisConstraint.Z).SetEase(ease)
                    .OnComplete(() => { finishCallback(); });
            }
            else
            {
                return flyObj.DOPath(paths.ToArray(), duration, PathType.CatmullRom).SetDelay(delay).SetOptions(false, AxisConstraint.Z).SetEase(ease);
            }
        }
        /// <summary>
        /// general fly object animation
        /// </summary>
        /// <param name="flyObj">obj to fly</param>
        /// <param name="startPos">start position</param>
        /// <param name="endPos">end position</param>
        /// <param name="v">curve orientation</param>
        /// <param name="fragment">curve highest/lowest point</param>
        /// <param name="flyDuration">fly duration</param>
        /// <param name="finishCallback">callback function to invoke when fly finish</param>
        /// <param name="ease">ease type</param>
        ///
    }
}