using System.Collections.Generic;
using TMPEffects.CharacterData;
using TMPEffects.Components.Animator;
using TMPEffects.Databases;

namespace TMPEffects.TMPAnimations.HideAnimations
{
    public class DummyHideAnimation : TMPHideAnimation
    {
        public override void Animate(CharData cData, IAnimationContext context)
        {
            context.FinishAnimation(cData);
        }

        public override object GetNewCustomData()
        {
            return null;
        }

        public override void SetParameters(object customData, IDictionary<string, string> parameters,
            ITMPKeywordDatabase keywordDatabase)
        {
        }

        public override bool ValidateParameters(IDictionary<string, string> parameters, ITMPKeywordDatabase keywordDatabase)
        {
            return true;
        }
    }
}