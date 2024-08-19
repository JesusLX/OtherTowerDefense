using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyGroup  {
    public List<EnemyGroup.GroupStep> steps;
    public int groupLevel;
    
    [Serializable]
    public class GroupStep {
        public string key;
        public float time;
        public int count;
    }
}
