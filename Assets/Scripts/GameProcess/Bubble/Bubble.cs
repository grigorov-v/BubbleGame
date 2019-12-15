using System.Linq;
using System.Collections.Generic;
using UnityEngine;

using Core.Events;

using GameProcess.Events;
using Controllers;
using NaughtyAttributes;

namespace GameProcess {
    public class Bubble : MonoBehaviour {
        public const string TAG_WALL  = "Wall";
        
        const string DEACTIVATE_ANIMATION_NAME = "BubbleBurst";
        const string TAG_TOP_WALL              = "TopWall";
        
        public static Dictionary<GameObject, Bubble> CacheBubbles {get; private set;}

        [SerializeField] string             _bubbleTag     = string.Empty;
        [SerializeField] List<BubbleReward> _bubbleRewards = new List<BubbleReward>();
        [SerializeField] ParticleSystem     _deactivateParticle = null;

        [SerializeField] [HideIf("IsHideBombRadius")]
        float _bombRadius = 1;

        List<Bubble> _connectedBubbles = new List<Bubble>();
        Animator     _animator         = null;
        bool         _init             = false;
        Rigidbody2D  _rigidbody        = null;
        float        _curForce         = 0;
        bool         _bubbleFromGun    = false;
      
        public BubbleReward ActiveBubbleReward {get; private set;}
        public bool         IsDeactivate       {get; private set;}

        public string BubbleTag {
            get {
                return _bubbleTag;
            }
        }

        bool IsHideBombRadius() {
            return _bubbleTag != "Bomb";
        }

        static void AddToCache(GameObject key, Bubble bubble) {
            if ( CacheBubbles == null ) {
                CacheBubbles = new Dictionary<GameObject, Bubble>();
            }

            if ( CacheBubbles.ContainsKey(key) ) {
                return;
            }

            CacheBubbles.Add(key, bubble);
        }

        static void RemoveToCache(GameObject key) {
            if ( CacheBubbles == null ) {
                return;
            }

            if ( !CacheBubbles.ContainsKey(key) ) {
                return;
            }

            CacheBubbles.Remove(key);
        }

        static Bubble FindToCache(GameObject key) {
            if ( !CacheBubbles.ContainsKey(key) ) {
                return null;
            }

            return CacheBubbles[key];
        }

        public static string GetRandomBubbleTag() {
            var levelInfo = ConfigsController.Instance.GetLevelInfo(0);
            if ( levelInfo == null ) {
                return string.Empty;
            }

            var bubbles = levelInfo.Bubbles;
            if ( bubbles == null ) {
                return string.Empty;
            }

            var rand = Random.Range(0, bubbles.Count);
            return bubbles[rand].Tag;
        }
       
        private void OnValidate() {
            if ( Application.isPlaying ) {
                return;
            }
            
            UpdateBubbleReward();
        }

        private void Awake() {
            Init();
        }

        private void OnDestroy() {
            RemoveToCache(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other) {
            var bubble = FindToCache(other.gameObject);
            TryAddConnectedBubble(bubble);

            if ( _bubbleFromGun ) {
                TryDeactivateAllConnectedBubbles();  
            }
        }

        private void OnTriggerExit2D(Collider2D other) {
            var bubble = FindToCache(other.gameObject);
            TryRemoveConnectedBubble(bubble);
        }

        private void OnCollisionEnter2D(Collision2D other) {
            if ( other.gameObject.CompareTag(TAG_WALL) ) {
                var contactPoint = other.GetContact(0);
                var newDir = contactPoint.normal;
                SetForce(newDir, _curForce);
            } else if ( other.gameObject.CompareTag(gameObject.tag) || other.gameObject.CompareTag(TAG_TOP_WALL) ) {
                ResetPhysics();
                SetForce(Vector2.zero, 0);
            }
           
            EventManager.Fire(new PostBubbleCollision(this, other));
        }

        private void OnCollisionStay2D(Collision2D other) {
            if ( !other.gameObject.CompareTag(TAG_TOP_WALL) ) {
                return;
            }

            _rigidbody.velocity = Vector2.zero;
            _curForce = 0;
        }

        public Bubble Init() {
            if ( _init ) {
                return this;
            }

            AddToCache(gameObject, this);
            _animator = GetComponent<Animator>();

            UpdateBubbleReward();
            _rigidbody = GetComponent<Rigidbody2D>();
            ResetPhysics();
            _deactivateParticle.gameObject.SetActive(false);

            _init = true;
            return this;
        }
        
        public Bubble UpdateBubbleReward(string tag) {
            _bubbleTag = tag;
            UpdateBubbleReward();
            return this;
        }

        public Bubble SetParent(Transform parent) {
            transform.SetParent(parent);
            return this;
        }

        public Bubble SetLocalPosition(Vector2 localPosition) {
            transform.localPosition = localPosition;
            return this;
        }

        public Bubble PhysicsSimulated(bool isSimulated) {
            var isKinematic = !isSimulated;
            if ( _rigidbody.isKinematic == isKinematic ) {
                return this;
            }

            _rigidbody.isKinematic = isKinematic;
            return this;
        }

        public Bubble ResetPhysics() {
            _rigidbody.velocity = Vector2.zero;
            _rigidbody.gravityScale = -1;
            _rigidbody.angularDrag = 0.05f;
            return this;
        }

        public Bubble SetGunFlag(bool isActive) {
            _bubbleFromGun = isActive;
            return this;
        }

        public Bubble SetForce(Vector2 direction, float force) {
            _rigidbody.simulated = true;
            _curForce = force;
            _rigidbody.AddForce(direction * _curForce, ForceMode2D.Impulse);
            return this;
        }

        //Вызывается в Animation Events
        public void DeactivateBetweenAnimation() {
            if ( !IsDeactivate ) {
                return;
            }

            PlayRewardEffect();
            Destroy(gameObject);

            _deactivateParticle.gameObject.SetActive(true);
            _deactivateParticle.transform.SetParent(null);
            _deactivateParticle.Play();
            Destroy(_deactivateParticle.gameObject, 2);
        }

        void UpdateBubbleReward() {
            foreach (var bubbleReward in _bubbleRewards) {
                var isActive = (bubbleReward.BubbleTag == _bubbleTag);
                bubbleReward.gameObject.SetActive(isActive);

                if ( isActive ) {
                    ActiveBubbleReward = bubbleReward;
                }
            }
        }

        bool CheckConnectedBubble(Bubble bubble) {
            return _connectedBubbles.IndexOf(bubble) != -1;
        }

        bool CheckAllConnectedBubble(Bubble bubble) {
            if ( CheckConnectedBubble(bubble) ) {
                return true;
            }

            foreach ( var curBubble in _connectedBubbles ) {
                if ( curBubble.CheckConnectedBubble(bubble) ) {
                    return true;
                }
            }

            return false;
        }

        void TryAddConnectedBubble(Bubble bubble) {
            if ( !bubble ) {
                return;
            }

            if ( (bubble._bubbleTag != _bubbleTag) || (bubble == this) ) {
                return;
            }

            if ( CheckAllConnectedBubble(bubble) ) {
                return;
            }

            _connectedBubbles.Add(bubble);
        }

        void TryRemoveConnectedBubble(Bubble bubble) {
            if ( !bubble ) {
                return;
            }

            if ( !CheckConnectedBubble(bubble) ) {
                return;
            }

            _connectedBubbles.Remove(bubble);
        }

        void TryDeactivateAllConnectedBubbles() {
            var count = GetCountConnectedBubbles();
            if ( count < 3 ) {
                return;
            }

            RecursiveDeactivateConnectedBubbles(_connectedBubbles);

            if ( _connectedBubbles.Count > 0 ) {
                PlayDeactivateAnimation();
            }
        }

        void PlayDeactivateAnimation() {
            if ( IsDeactivate ) {
                return;
            }

            IsDeactivate = true;
            _animator.Play(DEACTIVATE_ANIMATION_NAME);

            if ( _bubbleTag == "Bomb" ) {
                PlayBomb();
            }
        }

        void PlayBomb() {
            foreach (var item in CacheBubbles) {
                var bubble = item.Value;
                var dist = Vector2.Distance(transform.position, bubble.transform.position);
                if ( (dist > _bombRadius) || (bubble == this) ) {
                    continue;
                }

                bubble.PlayDeactivateAnimation();
            }
        }

        void PlayRewardEffect() {
            if ( !ActiveBubbleReward ) {
                return;
            }

            var endPosAnim = ScoreTable.Instance.PointForRewardAnimation.position;
            ActiveBubbleReward.transform.SetParent(null);
            ActiveBubbleReward.PlayRewardAnimation(endPosAnim);
        }

        int GetCountConnectedBubbles() {
            var count = _connectedBubbles.Count;
            foreach ( var bub in _connectedBubbles ) {
                if ( !bub.gameObject.activeSelf ) {
                    continue;
                }

                if ( bub.IsDeactivate ) {
                    continue;
                }

                count += bub._connectedBubbles.Count;
            }

            return count;
        }

        static void RecursiveDeactivateConnectedBubbles(List<Bubble> bubbles) {
            foreach ( var bub in bubbles ) {
                if ( !bub ) {
                    continue;
                }
                
                if ( !bub.gameObject.activeSelf ) {
                    continue;
                }

                if ( bub.IsDeactivate ) {
                    continue;
                }

                bub.PlayDeactivateAnimation();
                RecursiveDeactivateConnectedBubbles(bub._connectedBubbles);
            }
        }

        void OnDrawGizmosSelected() {
            if ( _bubbleTag != "Bomb" ) {
                return;
            }

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _bombRadius);
        }
    }
}