using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurnBaseGame
{
    public class CharacterSelector : MonoBehaviour
    {
        private Character selectedCharacter;

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0)) // Change this if condition based on your input method
            {
                // Create a ray from the mouse position
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Change Input.mousePosition based on your input method

                RaycastHit hitInfo;

                bool hitAnyCharacter = false;

                // Perform the raycast
                if (Physics.Raycast(ray, out hitInfo))
                {
                    // Check if the raycast hit an object
                    if (hitInfo.collider != null)
                    {
                        // Access the GameObject that was hit
                        GameObject hitObject = hitInfo.collider.gameObject;

                        Debug.Log("hit " + hitObject.name);
                        if (hitObject.tag == "Character")
                        {
                            Character character = hitObject.GetComponent<Character>();

                            ProcessCharacterSelection(character);

                            hitAnyCharacter = true;
                        }
                    }
                }

                if (!hitAnyCharacter)
                {
                    UnSelectCurrentCharacter();
                }
            }
        }

        private void ProcessCharacterSelection(Character character)
        {
            if (selectedCharacter == null)
            {
                character.HighLight();

                selectedCharacter = character;

                return;
            }

            if (selectedCharacter == character)
            {
                UnSelectCurrentCharacter();

                return;
            }

            UnSelectCurrentCharacter();
            character.HighLight();

            selectedCharacter = character;
        }

        private void UnSelectCurrentCharacter()
        {
            selectedCharacter.UnHighLight();
            selectedCharacter = null;
        }
    }
}
