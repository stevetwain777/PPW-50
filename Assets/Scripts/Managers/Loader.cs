﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour {

    public GameObject audioManager;

	// Use this for initialization
	void Awake () {
        LoadManager<AudioManager>();
	}

    private GameObject PrefabForManager<T>() {
        if (typeof(T) == typeof(AudioManager)) {
            return audioManager;
        }
        return null;
    }

    public T LoadManager<T>()
        where T : Object {
        T manager = FindObjectOfType<T>();
        if (manager == null) {
            GameObject prefab = PrefabForManager<T>();
            manager = Instantiate(prefab).GetComponent<T>();
        }

        return manager;
    }
}
