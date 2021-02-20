using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

// Represents a building in the city
public class Building : MonoBehaviour
{

    private Combat combat;

    private void Start()
    {
        combat = GetComponent<Combat>();

        Debug.Assert(combat);
    }

}
