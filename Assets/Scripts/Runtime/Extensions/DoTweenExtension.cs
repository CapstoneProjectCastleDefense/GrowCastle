namespace Runtime.Extensions
{
    using System.Collections.Generic;
    using DG.Tweening;
    using UnityEngine;

    public static class DoTweenExtension
    {
        public static Tween Fly(
            this Transform flyObj,
            Vector3 startPos,
            Vector3 endPos,
            int fragment,
            float duration,
            float delay,
            Vector3 vectorOrientation,
            Ease ease = Ease.OutQuad)
        {
            flyObj.gameObject.SetActive(true);
            var paths   = new List<Vector3> { startPos };
            var AB      = endPos - startPos;
            var C_prime = startPos + AB / fragment;
            var C       = C_prime + Vector3.Normalize(Vector3.Cross(AB, vectorOrientation)) + Vector3.up;

            paths.Add(C);
            paths.Add(endPos);
            var lastPos = startPos;
            return flyObj.DOPath(paths.ToArray(), duration, PathType.CatmullRom)
                .SetDelay(delay)
                .SetOptions(false, AxisConstraint.Z)
                .SetEase(ease).OnUpdate(() =>
                {
                    var currentPos = flyObj.position;
                    var direction  = currentPos - lastPos;
                    var angle      = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    flyObj.rotation = Quaternion.Euler(0, 0, angle);
                    lastPos         = currentPos;
                });
        }
    }
}