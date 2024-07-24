namespace Runtime.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class GachaExtension
    {
        public static T RandomGachaWithWeight<T>(this List<T> elements, List<float> weights, int defaultElementIndex = 0)
        {
            // Validate input
            if (elements == null || weights == null || elements.Count != weights.Count || elements.Count == 0)
            {
                throw new ArgumentException("Invalid input");
            }

            // Normalize weights
            var sum               = weights.Sum();
            var normalizedWeights = weights.Select(w => w / sum).ToList();

            // Generate random number between 0 and 1
            var rnd          = new Random();
            var randomNumber = rnd.NextDouble();

            // Select element based on weights
            for (var i = 0; i < elements.Count; i++)
            {
                if (randomNumber < normalizedWeights[i])
                {
                    return elements[i];
                }

                randomNumber -= normalizedWeights[i];
            }

            return elements[defaultElementIndex];
        }

        public static List<T> RandomGachaWithWeight<T>(this List<T> elements, List<float> weights,int targetNumber, int defaultElementIndex = 0 )
        {
            if (elements.Count < targetNumber)
            {
                throw new ArgumentException("target number must <= element.count");
            }

            List<T>     poolItems  = elements;
            List<float> itemWeight = weights;
            List<T>      result     = new();
            for (int i = 0; i < targetNumber; i++)
            {
                var item = RandomGachaWithWeight(poolItems, itemWeight);
                itemWeight.RemoveAt(poolItems.IndexOf(item));
                poolItems.Remove(item);
                result.Add(item);
            }

            return result;

        }
    }
}