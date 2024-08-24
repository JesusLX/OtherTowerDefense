using System;
using System.Threading;

public class Countdown {
    public Action OnTimeOut;
    public Action<float> OnTimeUpdate;

    private float remainingTime;
    private bool isCountingDown;
    private Timer timer;

    public Countdown(float time) {
        remainingTime = time;
    }

    public void StartCountdown() {
        if (isCountingDown)
            return;

        isCountingDown = true;

        // Configurar un Timer para que llame a Update cada 100ms (o lo que consideres apropiado)
        timer = new Timer(TimerCallback, null, 0, 100);
    }

    public void StopCountdown() {
        isCountingDown = false;
        timer?.Dispose();
    }

    private void TimerCallback(object state) {
        // Calcular deltaTime en C# puro (100ms)
        float deltaTime = 0.1f;
        Update(deltaTime);
    }

    private void Update(float deltaTime) {
        if (isCountingDown) {
            remainingTime -= deltaTime;
            OnTimeUpdate?.Invoke(remainingTime);

            if (remainingTime <= 0) {
                StopCountdown();
                remainingTime = 0;
                OnTimeOut?.Invoke();
            }
        }
    }

    public bool IsCountingDown() {
        return isCountingDown;
    }

    public void UpdateRemainingTime(float time) {
        remainingTime = time;
    }

    public float GetRemainingTime() {
        return remainingTime;
    }
}
