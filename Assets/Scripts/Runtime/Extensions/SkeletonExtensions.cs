

using System.Linq;
using Spine;
using Spine.Unity;

public static class SkeletonExtensions
{
    public static void UpdateSkeletonHeadAndBody(this Skeleton skeleton, string headSkinName = "", string bodySkinName = "")
        {
            var skeletonData  = skeleton.Data;
            var characterSkin = new Skin("character-base");

            if (!string.IsNullOrEmpty(bodySkinName)) characterSkin.AddSkin(skeletonData.FindSkin(bodySkinName));
            if (!string.IsNullOrEmpty(headSkinName)) characterSkin.AddSkin(skeletonData.FindSkin(headSkinName));

            skeleton.SetSkin(characterSkin);
            skeleton.SetSlotsToSetupPose();
        }

        public static void ChangeSkeletonDataAsset(this SkeletonGraphic skeleton, SkeletonDataAsset skeletonDataAsset, string animationName = null)
        {
            if (string.IsNullOrEmpty(animationName))
            {
                var animations = skeletonDataAsset.GetAnimationStateData().SkeletonData.Animations;
                animationName = animations.First().Name;
            }

            skeleton.Clear();
            skeleton.skeletonDataAsset = skeletonDataAsset;
            skeleton.SetAnimation(animationName);
            skeleton.SetAllDirty();
        }

        public static void ChangeSkeletonDataAsset(this SkeletonAnimation skeleton, SkeletonDataAsset skeletonDataAsset, string animationName = null)
        {
            if (string.IsNullOrEmpty(animationName))
            {
                var animations = skeletonDataAsset.GetAnimationStateData().SkeletonData.Animations;
                animationName = animations.First().Name;
            }

            skeleton.ClearState();
            skeleton.skeletonDataAsset = skeletonDataAsset;
            skeleton.Initialize(true);
            skeleton.SetAnimation(animationName);
        }

        public static void SetAnimation(this      SkeletonGraphic   skeleton, string animationName, bool loop = true) { skeleton.AnimationState.SetAnimation(0, animationName, loop); }
        public static void SetAnimation(this      SkeletonAnimation skeleton, string animationName, bool loop = true) { skeleton.AnimationState.SetAnimation(0, animationName, loop); }
        public static void SetEmptyAnimation(this SkeletonAnimation skeleton) { skeleton.AnimationState.SetEmptyAnimation(0, 0); }
}