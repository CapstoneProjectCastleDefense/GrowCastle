namespace Runtime.Extensions
{
    using System;
    using System.Collections.Generic;
    using DG.Tweening;
    using UnityEngine;

    public static class DoTweenExtension
    {
        /// <summary>
        /// general fly object animation
        /// </summary>
        /// <param name="flyObj">obj to fly</param>
        /// <param name="startPos">start position</param>
        /// <param name="endPos">end position</param>
        /// <param name="fragment">curve highest/lowest point</param>
        /// <param name="duration">fly duration</param>
        /// <param name="vectorOrientation"></param>
        /// <param name="finishCallback">callback function to invoke when fly finish</param>
        /// <param name="ease">ease type</param>
        /// <param name="delay"></param>
        public static Tween Fly(
            this Transform flyObj,
            Vector3 startPos,
            Vector3 endPos,
            int fragment,
            float duration,
            float delay,
            Vector3 vectorOrientation,
            Action finishCallback,
            Ease ease = Ease.OutQuad)
        {
            flyObj.gameObject.SetActive(true);
            var paths   = new List<Vector3> { startPos };
            var AB      = endPos - startPos;
            var C_prime = startPos + AB / fragment;
            var C       = C_prime + Vector3.Normalize(Vector3.Cross(AB, vectorOrientation));

            paths.Add(C);
            paths.Add(endPos);

            return flyObj.DOPath(paths.ToArray(), duration, PathType.CatmullRom).SetDelay(delay).SetOptions(false, AxisConstraint.Z).SetEase(ease)
                .OnComplete(() => { finishCallback?.Invoke(); });
        }
    }
}