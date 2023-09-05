using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

internal sealed class QualiaIntensity : IDisposable {
    // Corrected the typo.
    private enum IntensityUpdatePattern {
        Discrete,
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

    public void Dispose() {
        foreach (var updateScheduleCts in _updateScheduleCtsList) {
            updateScheduleCts.Cancel();
        }

        if (_autoRecoveryCts != null) {
            _autoRecoveryCts.Cancel();
        }
    }

    internal void IncreaseImmediately(float amount) {
        IncreasePatternChecked(amount, IntensityUpdatePattern.Discrete);
    }

    internal void DecreaseImmediately(float amount) {
        DecreasePatternChecked(amount, IntensityUpdatePattern.Discrete);
    }

    internal async void IncreaseScheduled(float amount, float timeToReach) {
        var updateScheduleCts = new CancellationTokenSource();
        _updateScheduleCtsList.Add(updateScheduleCts);

        try {
            await IncreaseScheduledAsync(amount, timeToReach, updateScheduleCts.Token);
            _updateScheduleCtsList.Remove(updateScheduleCts);
        }
        catch (OperationCanceledException) {

        }
        finally {
            updateScheduleCts.Dispose();
        }
    }

    internal async void DecreaseScheduled(float amount, float timeToReach) {
        var updateScheduleCts = new CancellationTokenSource();

        try {
            await DecreaseScheduledAsync(amount, timeToReach, updateScheduleCts.Token);
            _updateScheduleCtsList.Remove(updateScheduleCts);
        }
        catch (OperationCanceledException) {

        }
        finally {
            updateScheduleCts.Dispose();
        }
    }

    internal void EnableAutoRecovery(float amountPerSecond) {
        if (_autoRecoveryCts != null) {
            return;
        }

        _autoRecoveryCts = new CancellationTokenSource();

        try {
            EnableAutoRecoveryAsync(amountPerSecond, _autoRecoveryCts.Token);
        }
        catch (OperationCanceledException) {

        }
        finally {
            _autoRecoveryCts.Dispose();
            _autoRecoveryCts = null;
        }
    }

    internal void DisableAutoRecovery() {
        if (_autoRecoveryCts == null) {
            return;
        }

        _autoRecoveryCts.Cancel();
    }

    private void IncreasePatternChecked(float amount, IntensityUpdatePattern pattern) {
        float intensityRateCache;
        float intensityRateUpdated;

        lock (null) {
            intensityRateCache = IntensityRate;

            float intensityUpdated = _intensity + amount;
            if (intensityUpdated > _maxIntensity) {
                intensityUpdated = _maxIntensity;
            }
            _intensity = intensityUpdated;

            intensityRateUpdated = IntensityRate;
        }

        switch (pattern) {
            case IntensityUpdatePattern.Discrete:
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
        float intensityRateCache;
        float intensityRateUpdated;

        lock (null) {
            intensityRateCache = IntensityRate;

            float intensityUpdated = _intensity - amount;
            if (intensityUpdated < _minIntensity) {
                intensityUpdated = _minIntensity;
            }
            _intensity = intensityUpdated;

            intensityRateUpdated = IntensityRate;
        }

        switch (pattern) {
            case IntensityUpdatePattern.Discrete:
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

    private async Task IncreaseScheduledAsync(float amount, float timeToReach, CancellationToken ct) {
        try {
            int totalIncreaseCount;
            int increaseInterval;
            float amountPerUpdate;

            lock (null) {
                totalIncreaseCount = (int)(timeToReach * _updateTimesPerSecond);

                int secondToMillisecond = 1000;
                increaseInterval = (int)(1f / _updateTimesPerSecond * secondToMillisecond);

                float amountPerSecond = amount / timeToReach;
                amountPerUpdate = amountPerSecond / _updateTimesPerSecond;
            }

            for (int increaseCount = 0; increaseCount < totalIncreaseCount; increaseCount++) {
                IncreasePatternChecked(amountPerUpdate, IntensityUpdatePattern.Continuous);

                await Task.Delay(increaseInterval, ct);
            }

            // Error compensation.
            float totalIncreaseCountDecimalPortion = timeToReach * _updateTimesPerSecond - totalIncreaseCount;
            float errorCompensation = amountPerUpdate * totalIncreaseCountDecimalPortion;
            IncreasePatternChecked(errorCompensation, IntensityUpdatePattern.Continuous);
        }
        catch (OperationCanceledException) {
            throw;
        }
    }

    private async Task DecreaseScheduledAsync(float amount, float timeToReach, CancellationToken ct) {
        try {
            int totalDecreaseCount;
            int decreaseInterval;
            float amountPerUpdate;

            lock (null) {
                totalDecreaseCount = (int)(timeToReach * _updateTimesPerSecond);

                int secondToMillisecond = 1000;
                decreaseInterval = (int)(1f / _updateTimesPerSecond * secondToMillisecond);

                float amountPerSecond = amount / timeToReach;
                amountPerUpdate = amountPerSecond / _updateTimesPerSecond;
            }
            
            for (int decreaseCount = 0; decreaseCount < totalDecreaseCount; decreaseCount++) {
                DecreasePatternChecked(amountPerUpdate, IntensityUpdatePattern.Continuous);

                await Task.Delay(decreaseInterval, ct);
            }

            // Error compensation.
            float totalDecreaseCountDecimalPortion = timeToReach * _updateTimesPerSecond - totalDecreaseCount;
            float errorCompensation = amountPerUpdate * totalDecreaseCountDecimalPortion;
            DecreasePatternChecked(errorCompensation, IntensityUpdatePattern.Continuous);
        }
        catch (OperationCanceledException) {
            throw;
        }
    }

    private async void EnableAutoRecoveryAsync(float amountPerSecond, CancellationToken ct) {
        try {
            float amountPerUpdate;
            int increaseInterval;

            lock (null) {
                int secondToMillisecond = 1000;
                increaseInterval = (int)(1f / _updateTimesPerSecond * secondToMillisecond);

                amountPerUpdate = amountPerSecond / _updateTimesPerSecond;
            }

            while (true) {
                IncreasePatternChecked(amountPerUpdate, IntensityUpdatePattern.Continuous);

                await Task.Delay(increaseInterval, ct);
            }
        }
        catch (OperationCanceledException) {
            throw;
        }
    }
}