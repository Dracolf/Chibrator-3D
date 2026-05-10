using UnityEngine;

public static class EnemySpawnPositionHelper
{
    public static Vector3 GetSpawnPositionAroundPlayer(
        Transform playerTransform,
        float minSpawnDistance,
        float maxSpawnDistance,
        float spawnY,
        float mapHalfSize,
        float spawnBoundaryMargin,
        int maxAttempts = 50
    )
    {
        float minPosition = -mapHalfSize + spawnBoundaryMargin;
        float maxPosition = mapHalfSize - spawnBoundaryMargin;

        if (playerTransform == null)
        {
            return GetRandomPositionInsideMap(minPosition, maxPosition, spawnY);
        }

        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            Vector3 direction = GetBiasedDirectionAroundPlayer(playerTransform);
            float distance = Random.Range(minSpawnDistance, maxSpawnDistance);

            Vector3 candidatePosition = playerTransform.position + direction * distance;
            candidatePosition.y = spawnY;

            if (IsInsideMap(candidatePosition, minPosition, maxPosition))
            {
                return candidatePosition;
            }
        }

        // Fallback : si le joueur est vraiment trop collé à un bord,
        // on cherche une position random dans la map, mais assez loin du joueur.
        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            Vector3 candidatePosition = GetRandomPositionInsideMap(minPosition, maxPosition, spawnY);

            Vector2 playerPosition2D = new Vector2(playerTransform.position.x, playerTransform.position.z);
            Vector2 candidatePosition2D = new Vector2(candidatePosition.x, candidatePosition.z);

            if (Vector2.Distance(playerPosition2D, candidatePosition2D) >= minSpawnDistance)
            {
                return candidatePosition;
            }
        }

        // Dernier fallback très rare : position random dans la map.
        return GetRandomPositionInsideMap(minPosition, maxPosition, spawnY);
    }

    private static Vector3 GetBiasedDirectionAroundPlayer(Transform playerTransform)
    {
        float roll = Random.value;
        float angle;

        if (roll < 0.60f)
        {
            // 60% : devant le joueur
            angle = Random.Range(-75f, 75f);
        }
        else if (roll < 0.90f)
        {
            // 30% : sur les côtés
            bool leftSide = Random.value < 0.5f;

            if (leftSide)
            {
                angle = Random.Range(-140f, -75f);
            }
            else
            {
                angle = Random.Range(75f, 140f);
            }
        }
        else
        {
            // 10% : derrière le joueur
            bool backLeft = Random.value < 0.5f;

            if (backLeft)
            {
                angle = Random.Range(-180f, -140f);
            }
            else
            {
                angle = Random.Range(140f, 180f);
            }
        }

        Vector3 direction = Quaternion.Euler(0f, angle, 0f) * playerTransform.forward;
        direction.y = 0f;

        return direction.normalized;
    }

    private static bool IsInsideMap(Vector3 position, float minPosition, float maxPosition)
    {
        return position.x >= minPosition &&
               position.x <= maxPosition &&
               position.z >= minPosition &&
               position.z <= maxPosition;
    }

    private static Vector3 GetRandomPositionInsideMap(float minPosition, float maxPosition, float spawnY)
    {
        return new Vector3(
            Random.Range(minPosition, maxPosition),
            spawnY,
            Random.Range(minPosition, maxPosition)
        );
    }
}