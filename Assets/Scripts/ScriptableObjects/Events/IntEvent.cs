using UnityEngine;

namespace BirdyFlap.Events
{
    /// <summary>
    /// GameEvent that carries an integer value.
    /// Useful for score updates, health changes, level changes, etc.
    /// </summary>
    [CreateAssetMenu(fileName = "New Int Event", menuName = "BirdyFlap/Events/Int Event")]
    public class IntEvent : GameEventWithData<int>
    {
    }
}
