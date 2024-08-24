using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour, IAttacker {
    public Detector detector;
    public AimingController aimingController;
    public Shooter shooter;
    public Damage damage;
    public DamageStat damageStats;
    public TowerInfo info;
    void OnEnable() {
        // Suscribirse a los eventos del detector
        detector.OnObjectDetected.AddListener(aimingController.SetTarget);
        detector.OnObjectLost.AddListener(aimingController.SetTarget);
        aimingController.OnTargetSelected.AddListener(shooter.StartShoot);
        aimingController.OnTargetLost.AddListener(shooter.StopShoot);
    }

    void OnDisable() {
        // Desuscribirse de los eventos del detector
        detector.OnObjectDetected.RemoveListener(aimingController.SetTarget);
        detector.OnObjectLost.RemoveListener(aimingController.SetTarget);
        aimingController.OnTargetSelected.RemoveListener(shooter.StartShoot);
        aimingController.OnTargetLost.RemoveListener(shooter.StopShoot);
    }
    private void Start() {
        Init();
    }
    [ContextMenu("Init")]
    public void Init() {
        damage = new Damage(damageStats);
        if (detector != null) {
            detector.StartSearching();
        }
        if (aimingController != null) {
            aimingController.StartAiming();
        }
        if (aimingController != null) {
            shooter.Init(damage,this);
        }
       
    }

    public void RefreshInfo() {
        if (info.range == 0) {
            info.range = detector.detectionRadius;
        }
    }

    [Serializable]
    public class TowerInfo {
        public string id;
        public string name;
        public int price;
        public float range;
        public Sprite image;
    }
}
