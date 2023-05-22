using System;
using System.Collections.Generic;
using UnityEngine;

public class EntityHandler : MonoBehaviour {
    private static EntityHandler Instance;

    public static EntityHandler GetInstance() {
        return Instance;
    }

    [SerializeField] private Transform entityPrefab;
    [SerializeField] private bool spawn = false;
    [SerializeField] private List<EntityData> aliveEntities = new List<EntityData>();
    private List<Player> playerClosedList = new List<Player>();

    // Event triggered when an entity is added
    public event Action<EntityData> OnEntityAdded;
    public event Action<Player> OnPlayerEntityAdded;

    // Event triggered when a character is spawned
    public event Action<Transform> OnEntitySpawned;
    public event Action<Player> OnPlayerEntitySpawned;

    private void Awake() {
        Instance = this;
    }
     
    private void Start() {

        foreach (EntityData entity in aliveEntities) {
            Debug.Log("Character: " + entity.entityTransform.name +
                ", Origin Position: " + entity.originPosition);
        }
    }

    private void Update() {
        foreach (EntityData entity in aliveEntities) {
            if (!entity.isSpawned && entity.shouldSpawn && spawn) {
                entity.isSpawned = true;
                SpawnEntity(entity.entityTransform, entity.originPosition);
            }

            if (entity.isSpawned) {
                if (entity.shouldBeActive) {
                    ActivateEntity(entity.entityTransform);
                } else {
                    DeactivateEntity(entity.entityTransform);
                }
            }

        }
    }

    private void SpawnEntity(Transform entity, Vector3 originPosition) {
        Transform characterEntity = Instantiate(entity, originPosition, Quaternion.identity);
        ActivateEntity(characterEntity);
        OnEntitySpawned?.Invoke(characterEntity);
    }

    private void ActivateEntity(Transform entity) {
        entity.gameObject.SetActive(true);
    }

    private void DeactivateEntity(Transform entity) {
        entity.gameObject.SetActive(false);
    }

    private void AddCharacter(Transform characterTransform, Vector3 originPosition) {
        EntityData characterData = new EntityData(characterTransform, originPosition,false,true, true);
        aliveEntities.Add(characterData);
        OnEntityAdded?.Invoke(characterData);
    }

    private void AddCharacterExisting(Player player, Vector3 originPosition) {
        EntityData characterData = new EntityData(player.transform, originPosition,true,true, true);
        aliveEntities.Add(characterData);
        OnPlayerEntityAdded?.Invoke(player);
    }



    





    // PUBLIC METHODS

    public List<EntityData> GetAliveEntities() {
        return aliveEntities;
    }

    public void FindAndAddPlayerEntities() {
        Player[] playerComponents = FindObjectsOfType<Player>();
        if (playerComponents != null) {
            foreach (Player playerComponent in playerComponents) {
                if (!playerClosedList.Contains(playerComponent)) {
                    if (playerComponent.isActiveAndEnabled) {
                        Debug.Log("Added Player: " + playerComponent.name);
                        playerClosedList.Add(playerComponent);
                        AddCharacterExisting(playerComponent, playerComponent.transform.position);

                    }
                }
                else {
                    Debug.Log("Player already found.");
                }
            }
        } else {
            Debug.Log("Could't find players.");
        }
    }

    // Custom class to hold character data
    [System.Serializable]
    public class EntityData {
        public Transform entityTransform;
        public Vector3 originPosition;
        public bool isSpawned;
        public bool shouldSpawn;
        public bool shouldBeActive;

        public EntityData(Transform entityTransform, Vector3 originPosition,bool isSpawned, bool shouldSpawn, bool shouldBeActive) {
            this.entityTransform = entityTransform;
            this.originPosition = originPosition;
            this.isSpawned = isSpawned;
            this.shouldSpawn = shouldSpawn;
            this.shouldBeActive = shouldBeActive;
        }
    }
}
