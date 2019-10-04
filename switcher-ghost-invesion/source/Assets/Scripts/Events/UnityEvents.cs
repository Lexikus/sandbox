using UnityEngine;
using UnityEngine.Events;

[System.Serializable] public class TransformEvent : UnityEvent<Transform> { }
[System.Serializable] public class CharacterInputKeyEvent : UnityEvent<CharacterInput.InputKey> { }
[System.Serializable] public class CharacterInputMovementEvent : UnityEvent<Vector2, CharacterInput.InputKey> { }
[System.Serializable] public class EnemyAnimationEvent : UnityEvent<EnemyAnimation.AnimationType> { }
[System.Serializable] public class HealthEvent : UnityEvent<int> { }

[System.Serializable] public class NetworkDataEvent : UnityEvent<string> { }
[System.Serializable] public class NetworkConnectEvent : UnityEvent<int> { }
[System.Serializable] public class NetworkDisconnectEvent : UnityEvent<int> { }