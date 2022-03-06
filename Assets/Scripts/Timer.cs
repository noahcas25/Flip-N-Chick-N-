using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private GameObject _timerFill, _timerText;
    [SerializeField] private GameController _gameController;

    private int _startDuration, _remaining;
    
    // Start is called before the first frame update
    void Start() {
       _startDuration = 60;
       StartCoroutine(StartDelay(0));  
    }

    private void startTimer() {
        _remaining = _startDuration;
        StartCoroutine(UpdateTimer());
    }

    IEnumerator UpdateTimer() {
        while(_remaining >= 0) {
            UIUpdate();
            
            if(_remaining==0)
                break;
        
            _remaining--;
            yield return new WaitForSeconds(1f);
        }
        _gameController.GameOver();
    }

    public void AddTime(int extraTime) {
        this._remaining += extraTime;
        UIUpdate();
    } 

    private void UIUpdate() {
        _timerText.GetComponent<TMPro.TextMeshProUGUI>().text = "" + _remaining;
        _timerFill.GetComponent<Image>().fillAmount = Mathf.InverseLerp(0, _startDuration, _remaining);
    }

    IEnumerator StartDelay(float time) {
        yield return new WaitForSeconds(time);
        startTimer();
    }
}