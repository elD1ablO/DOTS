using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class SetupSpawner : MonoBehaviour
{
    [SerializeField] GameObject person;
    [SerializeField] int gridSize;
    [SerializeField] int spacing;
    [SerializeField] Vector2 speedRange = new Vector2(3, 7);
    [SerializeField] Vector2 lifetimeRange = new Vector2(20, 60);

    BlobAssetStore blob;
    void Start()
    {
        blob = new BlobAssetStore();
        var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, blob);
        var entity = GameObjectConversionUtility.ConvertGameObjectHierarchy(person, settings);
        var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        
        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                var instance = entityManager.Instantiate(entity);

                float3 position = new float3(x * spacing, 0, z * spacing);

                entityManager.SetComponentData(instance, new Translation { Value = position });                
                entityManager.SetComponentData(instance, new Destination { Value = position });

                float lifetime = UnityEngine.Random.Range(lifetimeRange.x, lifetimeRange.y);
                entityManager.SetComponentData(instance, new Lifetime { Value = lifetime });

                float speed = UnityEngine.Random.Range(speedRange.x, speedRange.y);
                entityManager.SetComponentData(instance, new MovementSpeed { moveSpeed = speed });
            }
        }
             
    }

    private void OnDestroy()
    {
        blob.Dispose();
    }
}
