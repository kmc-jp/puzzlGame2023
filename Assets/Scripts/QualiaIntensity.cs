using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

internal sealed class QualiaIntensity {
    private enum IntensityUpdatePattern {
        Discreate,
        Continuous,
    }

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

    internal static int _updateTimesPerSecond;

    internal static int UpdateTimesPerSecond {
        get {
            return _updateTimesPerSecond;
        }
        set {
            _updateTimesPerSecond = value;
        }
    }

    static QualiaIntensity() {
        _updateTimesPerSecond = 60;
    }

    private readonly float _minIntensity;
    private readonly float _maxIntensity;

    private readonly List<CancellationTokenSource> _updateScheduleCtsList;

    private float _intensity;

    private CancellationTokenSource _autoRecoveryCts;

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

    internal QualiaIntensity(float minIntensity, float maxIntensity, float intensity) {
        _minIntensity = minIntensity;
        _maxIntensity = maxIntensity;

        _updateScheduleCtsList = new List<CancellationTokenSource>();

        _intensity = intensity;
    }

    ~QualiaIntensity() {
        foreach (var updateScheduleCts in  _updateScheduleCtsList) {
            updateScheduleCts.Cancel();
            updateScheduleCts.Dispose();
        }

        if (_autoRecoveryCts != null) {
            _autoRecoveryCts.Cancel();
            _autoRecoveryCts.Dispose();
        }
    }

    internal void IncreaseImmediately(float amount) {
        IncreasePatternChecked(amount, IntensityUpdatePattern.Discreate);
    }

    internal void DecreaseImmediately(float amount) {
        DecreasePatternChecked(amount, IntensityUpdatePattern.Discreate);
    }

    internal void IncreaseScheduled(float amount, float timeToReach) {
        var updateScheduleCts = new CancellationTokenSource();
        
        _updateScheduleCtsList.Add(updateScheduleCts);

        _ = Task.Run(() => IncreaseScheduledAsync(amount, timeToReach), updateScheduleCts.Token);
        updateScheduleCts.Dispose();

        _updateScheduleCtsList.Remove(updateScheduleCts);
    }

    internal void DecreaseScheduled(float amount, float timeToReach) {
        var updateScheduleCts = new CancellationTokenSource();

        _updateScheduleCtsList.Add(updateScheduleCts);

        _ = Task.Run(() => DecreaseScheduledAsync(amount, timeToReach), updateScheduleCts.Token);

        _updateScheduleCtsList.Remove(updateScheduleCts);
    }

    internal void EnableAutoRecovery(float amountPerSecond) {
        if (_autoRecoveryCts != null) {
            return;
        }

        _autoRecoveryCts = new CancellationTokenSource();
        _ = Task.Run(() => EnableAutoRecoveryAsync(amountPerSecond), _autoRecoveryCts.Token);
    }

    internal void DisableAutoRecovery() {
        if (_autoRecoveryCts == null) {
            return;
        }

        _autoRecoveryCts.Cancel();
        _autoRecoveryCts.Dispose();
        _autoRecoveryCts = null;
    }

    private void IncreasePatternChecked(float amount, IntensityUpdatePattern pattern) {
        float intensityRateCache = IntensityRate;

        float intensityUpdated = _intensity + amount;
        if (intensityUpdated > _maxIntensity) {
            intensityUpdated = _maxIntensity;
        }
        _intensity = intensityUpdated;

        float intensityRateUpdated = IntensityRate;

        switch (pattern) {
            case IntensityUpdatePattern.Discreate:
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
                break;
            case IntensityUpdatePattern.Continuous:
                if (intensityRateCache < 0.25f && intensityRateUpdated >= 0.25f) {
                    OnIntensityRateReached025Continuously();
                    OnIntensityRateThrough025UpwardContinuously();
                }
                if (intensityRateCache < 0.5f && intensityRateUpdated >= 0.5f) {
                    OnIntensityRateReached050Continuously();
                    OnIntensityRateThrough050UpwardContinuously();
                }
                if (intensityRateCache < 0.75f && intensityRateUpdated >= 0.75f) {
                    OnIntensityRateReached075Continuously();
                    OnIntensityRateThrough075UpwardContinuously();
                }
                if (intensityRateCache < 1f && intensityRateUpdated == 1f) {
                    OnIntensityRateReached100Continuously();
                }
                break;
        }
    }

    private void DecreasePatternChecked(float amount, IntensityUpdatePattern pattern) {
        float intensityRateCache = IntensityRate;

        float intensityUpdated = _intensity - amount;
        if (intensityUpdated < _minIntensity) {
            intensityUpdated = _minIntensity;
        }
        _intensity = intensityUpdated;

        float intensityRateUpdated = IntensityRate;

        switch (pattern) {
            case IntensityUpdatePattern.Discreate:
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
                    OnIntensityRateReached000Discretely();
                }
                break;
            case IntensityUpdatePattern.Continuous:
                if (intensityRateCache > 0.75f && intensityRateUpdated <= 0.75f) {
                    OnIntensityRateReached075Continuously();
                    OnIntensityRateThrough075DownwardContinuously();
                }
                if (intensityRateCache > 0.5f && intensityRateUpdated <= 0.5f) {
                    OnIntensityRateReached050Continuously();
                    OnIntensityRateThrough050DownwardContinuously();
                }
                if (intensityRateCache > 0.25f && intensityRateUpdated <= 0.25f) {
                    OnIntensityRateReached025Continuously();
                    OnIntensityRateThrough025DownwardContinuously();
                }
                if (intensityRateCache > 0f && intensityRateUpdated == 0f) {
                    OnIntensityRateReached000Continuously();
                }
                break;
        }
    }

    private async void IncreaseScheduledAsync(float amount, float timeToReach) {
        int totalIncreaseCount = (int)(timeToReach * _updateTimesPerSecond);

        int secondToMillisecond = 1000;
        int increaseInterval = (int)(1f / _updateTimesPerSecond * secondToMillisecond);

        float amountPerSecond = amount / timeToReach;
        float amountPerUpdate = amountPerSecond / _updateTimesPerSecond;

        for (int increaseCount = 0; increaseCount < totalIncreaseCount; increaseCount++) {
            IncreasePatternChecked(amountPerUpdate, IntensityUpdatePattern.Continuous);

            await Task.Delay(increaseInterval);
        }
    }

    private async void DecreaseScheduledAsync(float amount, float timeToReach) {
        int totalDecreaseCount = (int)(timeToReach * _updateTimesPerSecond);

        int secondToMillisecond = 1000;
        int decreaseInterval = (int)(1f / _updateTimesPerSecond * secondToMillisecond);

        float amountPerSecond = amount / timeToReach;
        float amountPerUpdate = amountPerSecond / _updateTimesPerSecond;

        for (int decreaseCount = 0; decreaseCount < totalDecreaseCount; decreaseCount++) {
            DecreasePatternChecked(amountPerUpdate, IntensityUpdatePattern.Continuous);

            await Task.Delay(decreaseInterval);
        }
    }

    private async void EnableAutoRecoveryAsync(float amountPerSecond) {
        int secondToMillisecond = 1000;
        int increaseInterval = (int)(1f / _updateTimesPerSecond * secondToMillisecond);

        float amountPerUpdate = amountPerSecond / _updateTimesPerSecond;

        while (true) {
            IncreasePatternChecked(amountPerUpdate, IntensityUpdatePattern.Continuous);

            await Task.Delay(increaseInterval);
        }
    }
}