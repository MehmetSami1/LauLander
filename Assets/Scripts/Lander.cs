using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Lander : MonoBehaviour
{

    private const float GRAVITY_NORMAL = 0.7f;
    public static Lander Instance { get; private set; }

    private Rigidbody2D landerRigidbody2D;

    public event EventHandler OnUpForce;
    public event EventHandler OnRightForce;
    public event EventHandler OnLeftForce;
    public event EventHandler OnBeforeForce;
    public event EventHandler OnCoinPickup;
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    
    public event EventHandler<OnLandedEventArgs> OnLanded;

    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }
    public class OnLandedEventArgs : EventArgs
    {
        public LandingType landingType;
        public int score;
        public float dotVector;
        public float landingSpeed;
        public float scoreMultiplier;
    }


    public enum LandingType
    {
        Success,
        wrongLandingArea,
        TooSteepAngle,
        TooFastLanding,
    }

    public enum State
    {
        WaitingToStart,
        Normal,
        GameOver,
    }


    public event EventHandler OnAfterFuelEnd;

    private float fuelAmount;
    private float fuelAmountMax= 10f;

    private State state;
    private void Awake()    
    {
        Instance = this;

        fuelAmount = fuelAmountMax;
        state = State.WaitingToStart;

        landerRigidbody2D=GetComponent<Rigidbody2D>();
        landerRigidbody2D.gravityScale = 0f;
    }

    private void FixedUpdate()
    {
        OnBeforeForce?.Invoke(this, EventArgs.Empty);

        switch (state)
        {
            default:
            case State.WaitingToStart:
                if (Keyboard.current.wKey.isPressed || Keyboard.current.aKey.isPressed || Keyboard.current.dKey.isPressed)
                {
                    landerRigidbody2D.gravityScale = GRAVITY_NORMAL;
                    SetState(State.Normal);
                }

                break;
            case State.Normal:

                if (fuelAmount <= 0f)
                {
                    OnAfterFuelEnd?.Invoke(this, EventArgs.Empty);
                    return;
                }

                if (Keyboard.current.wKey.isPressed || Keyboard.current.aKey.isPressed || Keyboard.current.dKey.isPressed)
                {
                    ConsumeFuel();
                }

                if (Keyboard.current.wKey.isPressed)
                {
                    float force = 700f;
                    landerRigidbody2D.AddForce(force * transform.up * Time.deltaTime);
                    OnUpForce?.Invoke(this, EventArgs.Empty);
                }
                if (Keyboard.current.aKey.isPressed)
                {
                    float turnSpeed = +100f;
                    landerRigidbody2D.AddTorque(turnSpeed * Time.deltaTime);
                    OnLeftForce?.Invoke(this, EventArgs.Empty);
                }
                if (Keyboard.current.dKey.isPressed)
                {
                    float turnSpeed = -100f;
                    landerRigidbody2D.AddTorque(turnSpeed * Time.deltaTime);
                    OnRightForce?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GameOver:
                break;
        }
        

    }

    private void OnCollisionEnter2D(Collision2D collider2D)
    {

        if (!collider2D.gameObject.TryGetComponent(out LandingPad landingPad))
        {
            Debug.Log("Zemine çarptın");
            OnLanded?.Invoke(this, new OnLandedEventArgs
            {
                landingType = LandingType.wrongLandingArea,
                dotVector = 0f,
                landingSpeed = 0f,
                scoreMultiplier = 0f,
                score = 0,
            });
            SetState(State.GameOver);
            return;
        }
      


        float softLandingVelocityMagnitude = 4f;
        float relativeVelocityMagnitude = collider2D.relativeVelocity.magnitude;
        if (relativeVelocityMagnitude > softLandingVelocityMagnitude)
        {
            Debug.Log(" Landed too hard");
            OnLanded?.Invoke(this, new OnLandedEventArgs
            {
                landingType = LandingType.TooFastLanding,
                dotVector = 0f,
                landingSpeed = relativeVelocityMagnitude,
                scoreMultiplier =0f,
                score = 0,
            });
            SetState(State.GameOver);
            return;
        }

        float dotVector = Vector2.Dot(Vector2.up, transform.up);
        float minDotVector=.90f;
        if (dotVector < minDotVector)
        {
            Debug.Log("böyle inilir mi amk");
            OnLanded?.Invoke(this, new OnLandedEventArgs
            {
                landingType = LandingType.TooSteepAngle,
                dotVector = dotVector,
                landingSpeed = relativeVelocityMagnitude,
                scoreMultiplier =0f,
                score =0,
            });
            SetState(State.GameOver);
            return;
        }

        Debug.Log("Seccesfull Landing");

        float maxScoreAmountLandingAngle = 100f;
        float scoreDotVectorMultiplier = 10f;
        float landingAngleScore = maxScoreAmountLandingAngle - Mathf.Abs(dotVector - 1f) * scoreDotVectorMultiplier * maxScoreAmountLandingAngle;

        float maxScoreAmountLandingSpeed = 100f;
        float landingSpeedScore = (softLandingVelocityMagnitude - relativeVelocityMagnitude) * maxScoreAmountLandingSpeed;

        Debug.Log("LandingAngleScore: " + landingAngleScore);
        Debug.Log("LandingSpeedScore: " + landingSpeedScore);

        int score = Mathf.RoundToInt((landingAngleScore + landingSpeedScore) * landingPad.GetScoreMultiplier());
       
        Debug.Log("Score: "+ score);
        OnLanded?.Invoke(this, new OnLandedEventArgs
        {
            landingType=LandingType.Success,
            dotVector=dotVector,
            landingSpeed=relativeVelocityMagnitude,
            scoreMultiplier=landingPad.GetScoreMultiplier(),
            score =score,

        });
        SetState(State.GameOver);
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.TryGetComponent(out FuelPickup fuelPickup))
        {
            float addFuelAmmont = 10f;
            fuelAmount += addFuelAmmont;
            if (fuelAmount > fuelAmountMax)
            {
                fuelAmount = fuelAmountMax;
            }
            fuelPickup.DestroySelf();  
        }

        if (collider2D.gameObject.TryGetComponent(out CoinPickup coinPickup))
        {
            coinPickup.DestroySelf();
            OnCoinPickup?.Invoke(this, EventArgs.Empty);

        }
    }

    private void SetState(State state)
    {
        this.state = state;
        OnStateChanged?.Invoke(this,new OnStateChangedEventArgs
        {
            state=state,
        });
    }
    private void ConsumeFuel() {
        float fuelConsumptionAmount = 1f;
        fuelAmount -= fuelConsumptionAmount * Time.deltaTime;
    }
    public float GetFuelAmountNormalized()
    {
        return fuelAmount / fuelAmountMax; 
    }
    public float GetFuel()
    {
        return fuelAmount; 
    }
    public float GetSpeedX()
    {
        return landerRigidbody2D.linearVelocityX;
    }
    public float GetSpeedY()
    {
        return landerRigidbody2D.linearVelocityY;
    }

    



}
