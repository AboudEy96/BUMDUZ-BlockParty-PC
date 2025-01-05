using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IButtonClickMangment 
{

    void Play(GameObject button);
    void Test(GameObject button);
    void ChangeMode();
    void Profile();
    void Map();
    void Character();
    void Shop();
    void Settings();
}