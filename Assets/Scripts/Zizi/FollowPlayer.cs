using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField]
    private Transform _player;

    [SerializeField]
    private float _avgSpeed = 3f;

    private void Update()
    {
        if (_player == null)
            return;

        Vector3 direction = (_player.position - transform.position).normalized;
        transform.position += direction * Random.Range(_avgSpeed-1, _avgSpeed+1) * Time.deltaTime;
    }
}