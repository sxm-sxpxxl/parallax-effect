using System;
using UnityEngine;

namespace Sxm.ParallaxEffect
{
    [Serializable]
    public class ParallaxLayer
    {
        public const string RectTransformName = nameof(rectTransform);
        public const string ParallaxWeight = nameof(parallaxWeight);
    
        public RectTransform RectTransform => rectTransform;
        
        [SerializeField] private RectTransform rectTransform = null;
        [SerializeField, Range(0f, 1f)] private float parallaxWeight = 0f;

        public void TranslateWithParallax(Vector3 translation, ParallaxEffectApplicator.MotionSource motionSource)
        {
            float actualWeight = motionSource == ParallaxEffectApplicator.MotionSource.Camera ? parallaxWeight : 1f - parallaxWeight;
            Vector3 actualTranslation = actualWeight * translation.x * Vector3.right;
            
            rectTransform.localPosition += actualTranslation;
        }

        public void Translate(Vector3 translation)
        {
            Vector3 actualTranslation = translation.x * Vector3.right;
            rectTransform.localPosition += actualTranslation;
        }
    }
}
