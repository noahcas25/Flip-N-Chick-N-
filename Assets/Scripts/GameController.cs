using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _chickenScream;
    [SerializeField] private AudioClip _collect;
    [SerializeField] private Transform _chickenAnimator;

    // UI
    [SerializeField] private GameObject _UIRed, _UIGreen;
    [SerializeField] private GameObject _gameOverCanvas;
    [SerializeField] private TextMeshProUGUI _eggCountText;
    [SerializeField] private TextMeshProUGUI _eggCountEnd;

    private int _eggsCaught;
    private bool _flipped;
    private bool _canFlip = true;

    private void Awake() => _eggsCaught = 0;

    public void FlipGravity() {
        if(!_canFlip) return;

        StartCoroutine(FlipCoroutine());

        if(_flipped) {
            Physics.gravity = new Vector3(0, -9.81f, 0);
            StartCoroutine(FlipDelay());     
            _flipped = false;
            _UIGreen.SetActive(false);
            _UIRed.SetActive(true);
        }
        else {
            Physics.gravity = new Vector3(0, 9.81f, 0);
            StartCoroutine(FlipBackDelay());
            _flipped = true;
            _UIGreen.SetActive(true);
            _UIRed.SetActive(false);
        }
    }

    public void ChickenHit() { 
        _audioSource.PlayOneShot(_chickenScream);
        Respawn();
    }

    public void Respawn() {
        Physics.gravity = new Vector3(0, -9.81f, 0);

        if(!_flipped) return;

        _chickenAnimator.Rotate(0, 0, -180);
        _flipped = false;
        _UIGreen.SetActive(false);
        _UIRed.SetActive(true);  
    }

    public void IncreaseEggsCaught() {
        _eggsCaught++;
        _audioSource.PlayOneShot(_collect);
        _eggCountText.text = _eggsCaught + "";

        if(_eggsCaught>=12) 
            Win();
    }

    private void Win() {
        Respawn();
        SceneManager.LoadScene("WinScene");
        // _winCanvas.SetActive(true);
    }

    public void GameOver() {
        _gameOverCanvas.SetActive(true);
        _eggCountEnd.text = _eggsCaught + "";
        Time.timeScale = 0f;
    }

    public void Restart() {
        Time.timeScale = 1f;
        Respawn();  
        SceneManager.LoadScene("GameScene");
    }

    public void Quit() => Application.Quit();

     private IEnumerator FlipDelay() {
        yield return new WaitForSeconds(0.3f);
        _chickenAnimator.Rotate(0, 0, 180); 
     }

      private IEnumerator FlipBackDelay() {
        yield return new WaitForSeconds(0.3f);
        _chickenAnimator.Rotate(0, 0, -180); 
     }

     private IEnumerator FlipCoroutine() {
        _canFlip = false;
        yield return new WaitForSeconds(1f);
        _canFlip = true;
    }
}
