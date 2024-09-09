using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;


public class CharacterEvents
    {
    // player damage and value
    public static UnityAction<GameObject, int> characterDamaged;

    // player healed and value
    public static UnityAction<GameObject, int> characterHealed;

}

