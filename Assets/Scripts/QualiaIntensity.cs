using System.Collections;
using UnityEngine;

/// <summary>
/// Model layer to control Qualia Intensity.
/// </summary>
// CHANGED:
// - Deleted the feature to change update frequency (_updateTimesPerSecond before)
// - Used Coroutines to increase/decrease intensity gradually instead Tasks.
// - Renamed XXXScheduled to XXXLinearly.
internal sealed class QualiaIntensity : MonoBehaviour {
    // HACK:
    // _minIntensity and _maxIntensity are currently exposed as SerializedFields
    // but these parameters are generally thought to be connected to characters' characteristics (?)
    // so may have to be managed by ScriptableObject or other ways.

    [SerializeField] private float _minIntensity = 0f;
    [SerializeField] private float _maxIntensity = 100f;

    private float _intensity;

    private Coroutine _autoRecovering;

    /// <summary>
    /// The percentage within the range 0 to 1.
    /// </summary>
    // Unit test passed.
    internal float IntensityRate {
        get {
            float maxIntensityRange = _maxIntensity - _minIntensity;
            float intensityRange = _intensity - _minIntensity;
            float intensityRate = intensityRange / maxIntensityRange;
            return intensityRate;
        }
    }

    // Unit test passed.
    internal void IncreaseImmediately(float intensityToIncrease) {
#if UNITY_EDITOR
        if (intensityToIncrease <= 0) {
            Debug.LogAssertion($"{nameof(intensityToIncrease)} must be greater than 0.");
        }
#endif

        float intensityIncreased = _intensity + intensityToIncrease;
        if (intensityIncreased > _maxIntensity) {
            intensityIncreased = _maxIntensity;
        }
        _intensity = intensityIncreased;
    }

    // Unit test passed.
    internal void DecreaseImmediately(float intensityToDecrease) {
#if UNITY_EDITOR
        if (intensityToDecrease <= 0) {
            Debug.LogAssertion($"{nameof(intensityToDecrease)} must be greater than 0.");
        }
#endif

        float intensityDecreased = _intensity - intensityToDecrease;
        if (intensityDecreased < _minIntensity) {
            intensityDecreased = _minIntensity;
        }
        _intensity = intensityDecreased;
    }

    // Unit test passed.
    internal void IncreaseLinearly(float intensityToIncrease, float estimatedDuration) {
#if UNITY_EDITOR
        if (intensityToIncrease <= 0) {
            Debug.LogAssertion($"{nameof(intensityToIncrease)} must be greater than 0.");
        }
        if (estimatedDuration <= 0) {
            Debug.LogAssertion($"{nameof(estimatedDuration)} must be greater than 0.");
        }
#endif

        StartCoroutine(IncreaseLinearlyCoroutine(intensityToIncrease, estimatedDuration));
    }

    // Unit test passed.
    internal void DecreaseLinearly(float intensityToDecrease, float estimatedDuration) {
#if UNITY_EDITOR
        if (intensityToDecrease <= 0) {
            Debug.LogAssertion($"{nameof(intensityToDecrease)} must be greater than 0.");
        }
        if (estimatedDuration <= 0) {
            Debug.LogAssertion($"{nameof(estimatedDuration)} must be greater than 0.");
        }
#endif

        StartCoroutine(DecreaseLinearlyCoroutine(intensityToDecrease, estimatedDuration));
    }

    // Unit test passed.
    internal void EnableAutoRecovery(float intensityToIncreasePerSecond) {
        if (_autoRecovering != null) {
            return;
        }

        _autoRecovering = StartCoroutine(EnableAutoRecoveryCoroutine(intensityToIncreasePerSecond));
    }

    // Unit test passed.
    internal void DisableAutoRecovery() {
        if (_autoRecovering == null) {
            return;
        }

        StopCoroutine(_autoRecovering);
        _autoRecovering = null;
    }

    // Unit test passed.
    private IEnumerator IncreaseLinearlyCoroutine(float intensityToIncrease, float estimatedDuration) {
        float increasePerSecond = intensityToIncrease / estimatedDuration;

        float elapsed;
        for (elapsed = 0f; elapsed < estimatedDuration; elapsed += Time.deltaTime) {
            float increaseInOneFrame = increasePerSecond * Time.deltaTime;
            IncreaseImmediately(increaseInOneFrame);

            yield return null;
        }

        // FIXME:
        // This way of compensation is not exactly accurate to the extent
        // which there will be an error of about four decimal places.

        float elapsedInLastFrame = elapsed - Time.deltaTime;
        float remainingDuration = estimatedDuration - elapsedInLastFrame;
        float compensationIncrease = increasePerSecond * remainingDuration;
        IncreaseImmediately(compensationIncrease);
    }

    // Unit test passed.
    private IEnumerator DecreaseLinearlyCoroutine(float intensityToDecrease, float estimatedDuration) {
        float decreasePerSecond = intensityToDecrease / estimatedDuration;

        float elapsed;
        for (elapsed = 0f; elapsed < estimatedDuration; elapsed += Time.deltaTime) {
            float decreaseInOneFrame = decreasePerSecond * Time.deltaTime;
            DecreaseImmediately(decreaseInOneFrame);

            yield return null;
        }

        // FIXME:
        // This way of compensation is not exactly accurate to the extent
        // which there will be an error of about four decimal places.

        float elapsedInLastFrame = elapsed - Time.deltaTime;
        float remainingDuration = estimatedDuration - elapsedInLastFrame;
        float compensationDecrease = decreasePerSecond * remainingDuration;
        DecreaseImmediately(compensationDecrease);
    }

    // Unit test passed.
    private IEnumerator EnableAutoRecoveryCoroutine(float intensityToIncreasePerSecond) {
        while (true) {
            float increaseInOneFrame = intensityToIncreasePerSecond * Time.deltaTime;
            IncreaseImmediately(increaseInOneFrame);

            yield return null;
        }
    }

    private void Awake() {
#if UNITY_EDITOR
        if (_maxIntensity <= _minIntensity) {
            Debug.LogAssertion($"{nameof(_maxIntensity)} must be greater than {nameof(_minIntensity)}.");
        }
#endif

        _intensity = _maxIntensity;
    }
}