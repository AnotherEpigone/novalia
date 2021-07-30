using GoRogue.MapGeneration;
using GoRogue;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;
using System.Collections.Generic;
using System.Linq;
using Troschuetz.Random;

namespace Novalia.Maps.Generation.Steps
{
    public class PangaeaGenerationStep : GenerationStep, INovaGenerationStep
    {
        private readonly IGenerator _rng;
        private readonly int _maxIterations;
        private readonly float _landThreshold;

        public PangaeaGenerationStep(IGenerator rng, int maxIterations, float landThreshold)
            : this("Continents", rng, maxIterations, landThreshold)
        {
        }
        public PangaeaGenerationStep(string componentTag, IGenerator rng, int maxIterations, float landThreshold)
        {
            ComponentTag = componentTag;
            _rng = rng;
            _maxIterations = maxIterations;
            _landThreshold = landThreshold;
        }

        public string ComponentTag { get; }

        protected override IEnumerator<object> OnPerform(GenerationContext context)
        {
            var continentsComponent = context.GetFirstOrNew<ISettableGridView<bool>>(
                () => new ArrayView<bool>(context.Width, context.Height),
                ComponentTag);

            while (!LandThresholdReached(continentsComponent))
            {
                var focusPoint = new Point(
                    _rng.Next(context.Width / 3, context.Width * 2 / 3),
                    _rng.Next(context.Height / 3, context.Height * 2 / 3));

                int iterations = _rng.Next(20, _maxIterations);
                for (int i = 0; i < iterations; i++)
                {
                    continentsComponent[focusPoint] = true;
                    if (continentsComponent.Contains(focusPoint.Translate(1, 0)))
                    {
                        continentsComponent[focusPoint.Translate(1, 0)] = true;
                    }

                    if (continentsComponent.Contains(focusPoint.Translate(0, 1)))
                    {
                        continentsComponent[focusPoint.Translate(0, 1)] = true;
                    }

                    focusPoint = AdjacencyRule.Cardinals
                        .Neighbors(focusPoint)
                        .Where(p => continentsComponent.Contains(p))
                        .ToList()
                        .RandomItem(_rng);
                }
            }

            yield break;
        }

        private bool LandThresholdReached(ISettableGridView<bool> map)
        {
            return (double)map.Positions().Where(t => map[t]).Count() / (double)map.Count > _landThreshold;
        }
    }
}
