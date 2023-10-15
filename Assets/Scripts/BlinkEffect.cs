using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Creates a blinking effect between two sprites
[RequireComponent(typeof(SpriteRenderer))]
public class BlinkEffect : MonoBehaviour
{
    [Tooltip("Number of blinks per second when enabled")]
    [Range(0.01f, 30.0f)]
    public float BlinksPerSecond = 2.0f;

    [Tooltip("How long each blink to the other sprite lasts for")]
    [Range(0.0f, 30.0f)]
    public float BlinkOutDuration = 0.1f;

    [Tooltip("Blinking rate increases when blinking duration is less than this time")]
    [Range(0.0f, 10000.0f)]
    public float BlinkAccelerateThreshold = 4.0f;

    [Tooltip("Amount of time to spend at the max blink rate before the blink effect ends")]
    public float MaxBlinkRateDuration = 1.0f;

    public Sprite DefaultSprite;
    public Sprite BlinkInSprite;
    public Sprite BlinkOutSprite;

    private float _blinkDuration = 0.0f;
    private float _blinkOutTime = 0.0f;
    private float _nextBlinkTime = 0.0f;
    private float _blinkInterval;

    private SpriteRenderer _renderer;

    public void EnableBlink(float duration)
    {
        _blinkDuration = duration;
        _blinkOutTime = 0.0f;
        _nextBlinkTime = 0.0f;
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(DefaultSprite != null);
        _renderer = GetComponent<SpriteRenderer>();
        _blinkInterval = 1.0f / BlinksPerSecond;
    }

    // Update is called once per frame
    void Update()
    {
        if (_blinkDuration > 0.0f)
        {
            // Update all timers
            _blinkOutTime -= Time.deltaTime;
            _nextBlinkTime -= Time.deltaTime;
            _blinkDuration -= Time.deltaTime;

            // Set current sprite based on blinking state
            if (_blinkDuration <= 0.0f)
            {
                _renderer.enabled = true;
                _renderer.sprite = DefaultSprite;
            }
            else
            {
                if (_blinkOutTime > 0.0f)
                {
                    if (BlinkOutSprite != null)
                    {
                        _renderer.sprite = BlinkOutSprite;
                        _renderer.enabled = true;
                    }
                    else
                    {
                        _renderer.enabled = false;
                    }
                }
                else
                {
                    if (BlinkInSprite != null)
                    {
                        _renderer.sprite = BlinkInSprite;
                        _renderer.enabled = true;
                    }
                    else
                    {
                        _renderer.enabled = false;
                    }
                }
            }

            // Update blinking timer state for next frame
            if (_nextBlinkTime <= 0.0f)
            {
                _nextBlinkTime = _blinkInterval / (1.0f + Mathf.Min(
                        Mathf.Max(BlinkAccelerateThreshold - _blinkDuration, 0.0f),
                        BlinkAccelerateThreshold - MaxBlinkRateDuration
                    ));
                _blinkOutTime = BlinkOutDuration;
            }
        }
    }
}
