using GoRogue.MapGeneration;
using SadRogue.Primitives.GridViews;
using System.Collections.Generic;
using Troschuetz.Random;

namespace Novalia.Maps.Generation.Steps
{
    public class LandFeatureGenerationStep : GenerationStep, INovaGenerationStep
    {
        private readonly IGenerator _rng;
        private readonly int _min;
        private readonly int _max;
        private readonly INovaGenerationStep _landGenerationStep;

        public LandFeatureGenerationStep(
            string componentTag,
            IGenerator rng,
            int min,
            int max,
            INovaGenerationStep landGenerationStep)
        {
            ComponentTag = componentTag;
            _rng = rng;
            _min = min;
            _max = max;
            _landGenerationStep = landGenerationStep;
        }

        public string ComponentTag { get; }

        protected override IEnumerator<object> OnPerform(GenerationContext context)
        {
            var featureComponent = context.GetFirstOrNew<ISettableGridView<bool>>(
                () => new ArrayView<bool>(context.Width, context.Height),
                ComponentTag);
            var landComponent = context.GetFirst<ISettableGridView<bool>>(_landGenerationStep.ComponentTag);

            var iterations = _rng.Next(_min, _max);
            for (int i = 0; i < iterations; i++)
            {
                var target = _rng.Next(featureComponent.Count);
                if (landComponent[target])
                {
                    featureComponent[target] = true;
                }
            }

            yield break;
        }
    }
}
