using System.Collections;
using System.Collections.Generic;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine;

public class TestFirebaseDB : MonoBehaviour
{
    DatabaseReference reference;

    // Start is called before the first frame update
    void Start()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://testfirebase-24b33.firebaseio.com/");
// Get the root reference location of the database.
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        
        writeNewUser("_aUser", "name", "some email");
    }

    private void writeNewUser(string userId, string name, string email)
    {
        User user = new User(name, email);
        string json = JsonUtility.ToJson(user);

        reference.Child("values").Child(userId).SetRawJsonValueAsync(json);
    }

    public class User
    {
        public string username;
        public string email;

        public User()
        {
        }

        public User(string username, string email)
        {
            this.username = username;
            this.email = email;
        }
    }
}