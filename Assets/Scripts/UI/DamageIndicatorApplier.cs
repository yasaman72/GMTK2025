using System.Collections;
using TMPro;
using UnityEngine;

public class DamageIndicatorApplier : MonoBehaviour
{
    [Space]
    [SerializeField] private GameObject _damageIndicator;
    [SerializeField] private Transform _origin;
    [SerializeField] private float _destroyAfter = 1f;
    [SerializeField] private Vector2 _spawnHorizontalRange = new Vector2(-.5f, .5f);

    public void ShowDamageIndicator(int damage)
    {
        if (_damageIndicator == null) return;
        Vector2 origin = (Vector2)_origin.position + (Vector2.one * Random.Range(_spawnHorizontalRange.x, _spawnHorizontalRange.y));
        GameObject damageIndicator = Instantiate(_damageIndicator, origin, Quaternion.identity);
        damageIndicator.GetComponentInChildren<TextMeshProUGUI>().text = damage.ToString();
        damageIndicator.GetComponent<Animator>().speed = 1f / _destroyAfter;
        StartCoroutine(DestroyDamageIndicator(damageIndicator));
    }

    private IEnumerator DestroyDamageIndicator(GameObject damageIndicator)
    {
        yield return new WaitForSeconds(_destroyAfter);
        Destroy(damageIndicator);
    }
}
