using System.Collections.Generic;
using UnityEngine;

public class PoissonSampler
{
    private const int k = 30; // Maximum number of attempts before marking a sample as inactive.
    private readonly float cellSize;
    private readonly float radius2; // radius squared

    private readonly Rect rect;
    private readonly List<Vector2> activeSamples = new List<Vector2>();
    private readonly Vector2[,] grid;

    /// Create a sampler with the following parameters:
    /// 
    /// width:  each sample's x coordinate will be between [0, width]
    /// height: each sample's y coordinate will be between [0, height]
    /// radius: each sample will be at least `radius` units away from any other sample, and at most 2 * `radius`.
    public PoissonSampler(float width, float height, float radius)
    {
        rect = new Rect(0, 0, width, height);
        radius2 = radius * radius;
        cellSize = radius / Mathf.Sqrt(2);
        grid = new Vector2[Mathf.CeilToInt(width / cellSize),
            Mathf.CeilToInt(height / cellSize)];
    }

    /// Return a lazy sequence of samples. You typically want to call this in a foreach loop, like so:
    /// foreach (Vector2 sample in sampler.Samples()) { ... }
    public IEnumerable<Vector2> Samples()
    {
        // First sample is choosen randomly
        yield return AddSample(new Vector2(Random.value * rect.width, Random.value * rect.height));

        while (activeSamples.Count > 0)
        {
            // Pick a random active sample
            var i = (int) Random.value * activeSamples.Count;
            var sample = activeSamples[i];

            // Try `k` random candidates between [radius, 2 * radius] from that sample.
            var found = false;
            for (var j = 0; j < k; ++j)
            {
                var angle = 2 * Mathf.PI * Random.value;
                var r = Mathf.Sqrt(Random.value * 3 * radius2 +
                                   radius2); // See: http://stackoverflow.com/questions/9048095/create-random-number-within-an-annulus/9048443#9048443
                var candidate = sample + r * new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

                // Accept candidates if it's inside the rect and farther than 2 * radius to any existing sample.
                if (rect.Contains(candidate) && IsFarEnough(candidate))
                {
                    found = true;
                    yield return AddSample(candidate);
                    break;
                }
            }

            // If we couldn't find a valid candidate after k attempts, remove this sample from the active samples queue
            if (!found)
            {
                activeSamples[i] = activeSamples[activeSamples.Count - 1];
                activeSamples.RemoveAt(activeSamples.Count - 1);
            }
        }
    }

    private bool IsFarEnough(Vector2 sample)
    {
        var pos = new GridPos(sample, cellSize);

        var xmin = Mathf.Max(pos.x - 2, 0);
        var ymin = Mathf.Max(pos.y - 2, 0);
        var xmax = Mathf.Min(pos.x + 2, grid.GetLength(0) - 1);
        var ymax = Mathf.Min(pos.y + 2, grid.GetLength(1) - 1);

        for (var y = ymin; y <= ymax; y++)
        for (var x = xmin; x <= xmax; x++)
        {
            var s = grid[x, y];
            if (s != Vector2.zero)
            {
                var d = s - sample;
                if (d.x * d.x + d.y * d.y < radius2) return false;
            }
        }

        return true;

        // Note: we use the zero vector to denote an unfilled cell in the grid. This means that if we were
        // to randomly pick (0, 0) as a sample, it would be ignored for the purposes of proximity-testing
        // and we might end up with another sample too close from (0, 0). This is a very minor issue.
    }

    /// Adds the sample to the active samples queue and the grid before returning it
    private Vector2 AddSample(Vector2 sample)
    {
        activeSamples.Add(sample);
        var pos = new GridPos(sample, cellSize);
        grid[pos.x, pos.y] = sample;
        return sample;
    }

    /// Helper struct to calculate the x and y indices of a sample in the grid
    private struct GridPos
    {
        public readonly int x;
        public readonly int y;

        public GridPos(Vector2 sample, float cellSize)
        {
            x = (int) (sample.x / cellSize);
            y = (int) (sample.y / cellSize);
        }
    }
}