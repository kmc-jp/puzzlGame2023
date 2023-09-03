using System;

internal sealed class QualiaIntensity {
    internal event Action OnIntensityRateReached000Discretely;
    internal event Action OnIntensityRateReached025Discretely;
    internal event Action OnIntensityRateReached050Discretely;
    internal event Action OnIntensityRateReached075Discretely;
    internal event Action OnIntensityRateReached100Discretely;

    internal event Action OnIntensityRateThrough025UpwardDiscretely;
    internal event Action OnIntensityRateThrough050UpwardDiscretely;
    internal event Action OnIntensityRateThrough075UpwardDiscretely;

    internal event Action OnIntensityRateThrough025DownwardDiscretely;
    internal event Action OnIntensityRateThrough050DownwardDiscretely;
    internal event Action OnIntensityRateThrough075DownwardDiscretely;

    internal event Action OnIntensityRateReached000Continuously;
    internal event Action OnIntensityRateReached025Continuously;
    internal event Action OnIntensityRateReached050Continuously;
    internal event Action OnIntensityRateReached075Continuously;
    internal event Action OnIntensityRateReached100Continuously;

    internal event Action OnIntensityRateThrough025UpwardContinuously;
    internal event Action OnIntensityRateThrough050UpwardContinuously;
    internal event Action OnIntensityRateThrough075UpwardContinuously;

    internal event Action OnIntensityRateThrough025DownwardContinuously;
    internal event Action OnIntensityRateThrough050DownwardContinuously;
    internal event Action OnIntensityRateThrough075DownwardContinuously;

    private readonly float _minIntensity;
    private readonly float _maxIntensity;

    private float _intensity;

    internal float Intensity {
        get {
            return _intensity;
        }
    }

    internal float IntensityRate {
        get {
            float maxIntensityRange = _maxIntensity - _minIntensity;
            float intensityRange = _intensity - _minIntensity;
            float intensityRate = intensityRange / maxIntensityRange;
            return intensityRate;
        }
    }

    internal void IncreaseImmediately(float amount) {
        float intensityRateCache = IntensityRate;

        float intensityUpdated = _intensity + amount;
        if (intensityUpdated > _maxIntensity) {
            intensityUpdated = _maxIntensity;
        }
        _intensity = intensityUpdated;

        float intensityRateUpdated = IntensityRate;

        if (intensityRateCache < 0.25f && intensityRateUpdated >= 0.25f) {
            OnIntensityRateReached025Discretely();
            OnIntensityRateThrough025UpwardDiscretely();
        }
        if (intensityRateCache < 0.5f && intensityRateUpdated >= 0.5f) {
            OnIntensityRateReached050Discretely();
            OnIntensityRateThrough050UpwardDiscretely();
        }
        if (intensityRateCache < 0.75f && intensityRateUpdated >= 0.75f) {
            OnIntensityRateReached075Discretely();
            OnIntensityRateThrough075UpwardDiscretely();
        }
        if (intensityRateCache < 1f && intensityRateUpdated == 1f) {
            OnIntensityRateReached100Discretely();
        }
    }

    internal void DecreaseImmediately(float amount) {
        float intensityRateCache = IntensityRate;

        float intensityUpdated = _intensity - amount;
        if (intensityUpdated < _minIntensity) {
            intensityUpdated = _minIntensity;
        }
        _intensity = intensityUpdated;

        float intensityRateUpdated = IntensityRate;

        if (intensityRateCache > 0.75f && intensityRateUpdated <= 0.75f) {
            OnIntensityRateReached075Discretely();
            OnIntensityRateThrough075DownwardDiscretely();
        }
        if (intensityRateCache > 0.5f && intensityRateUpdated <= 0.5f) {
            OnIntensityRateReached050Discretely();
            OnIntensityRateThrough050DownwardDiscretely();
        }
        if (intensityRateCache > 0.25f && intensityRateUpdated <= 0.25f) {
            OnIntensityRateReached025Discretely();
            OnIntensityRateThrough025DownwardDiscretely();
        }
        if (intensityRateCache > 0f && intensityRateUpdated == 0f) {
            OnIntensityRateReached100Discretely();
        }
    }

    internal void IncreaseScheduled(float amount, float timeToReach) {

    }

    internal void DecreaseScheduled(float amount, float timeToReach) {

    }

    internal void EnableAutoRecovery() {

    }

    internal void DisableAutoRecovery() {

    }
}