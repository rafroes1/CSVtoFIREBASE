using System;
using System.Collections.Generic;
using System.Text;
using Google.Cloud.Firestore;

namespace CSVtoFIREBASE
{
    class FirebaseConnection
    {
        FirestoreDb db;

        public FirebaseConnection(){
            string path = AppDomain.CurrentDomain.BaseDirectory + @"smart-financial-c2d8b-firebase-adminsdk-dcn7a-2c49d1f2ef.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
            db = FirestoreDb.Create("smart-financial-c2d8b");
        }

        void AddRandomIdDocument()
        {
            //specify collection
            CollectionReference collection = db.Collection("CollWithRandomIdDocuments");

            //create the data
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("firstname", "delete");
            data.Add("lastname", "this file field");
            data.Add("field", "test");

            //insert data in the collection
            collection.AddAsync(data);
            Console.WriteLine("Data inserted");
        }

        public void AddDocumentWithId(string collectionName, string documentId, Dictionary<string, object> data)
        {
            DocumentReference document = db.Collection(collectionName).Document(documentId);

            //insert data in the collection
            document.SetAsync(data);
        }

        async void QueryDocumentAsync()
        {
            //specify collection
            CollectionReference collection = db.Collection("CollWithRandomIdDocuments");

            // Query the collection for all documents where doc.firstname = vit.
            Query query = collection.WhereEqualTo("firstname", "vit");
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

            foreach (DocumentSnapshot queryResult in querySnapshot.Documents)
            {
                string lastName = queryResult.GetValue<string>("lastname");
                Console.WriteLine(lastName);
            }
        }

        async void UpdateOrAddField()
        {
            //specify collection and document id
            DocumentReference doc = db.Collection("CollWithRandomIdDocuments").Document("iwTvxfOn653zwETmMbZp");
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("status", "linda");

            DocumentSnapshot snap = await doc.GetSnapshotAsync();
            if (snap.Exists)
            {
                await doc.UpdateAsync(data);
            }
            Console.WriteLine("Field updated");
        }

        async void UpdateWithQuery()
        {
            //specify collection
            CollectionReference collection = db.Collection("CollWithRandomIdDocuments");

            // Query the collection for all documents where doc.firstname = "vit" and doc.status = "linda"
            Query query = collection.WhereEqualTo("firstname", "vit");
            query = query.WhereEqualTo("status", "linda");
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

            foreach (DocumentSnapshot queryResult in querySnapshot.Documents)
            {
                Dictionary<string, object> data = new Dictionary<string, object>();
                data.Add("status", "muito linda");

                DocumentReference doc = collection.Document(queryResult.Id);
                await doc.UpdateAsync(data);
                Console.WriteLine("Updated");
            }
        }

        async void DeleteDocument()
        {
            //specify collection
            CollectionReference collection = db.Collection("CollWithRandomIdDocuments");

            // Query the collection for all documents where doc.firstname = "delete" and doc.lastname = "this file"
            Query query = collection.WhereEqualTo("firstname", "delete");
            query = query.WhereEqualTo("lastname", "this file");
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

            foreach (DocumentSnapshot queryResult in querySnapshot.Documents)
            {
                DocumentReference doc = collection.Document(queryResult.Id);
                await doc.DeleteAsync();
                Console.WriteLine("Document deleted");
            }
        }

        async void DeleteDocumentField()
        {
            //specify collection
            CollectionReference collection = db.Collection("CollWithRandomIdDocuments");

            // Query the collection for all documents where doc.firstname = "delete" and doc.lastname = "this file field"
            Query query = collection.WhereEqualTo("firstname", "delete");
            query = query.WhereEqualTo("lastname", "this file field");
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

            foreach (DocumentSnapshot queryResult in querySnapshot.Documents)
            {
                Dictionary<string, object> data = new Dictionary<string, object>();
                data.Add("field", FieldValue.Delete);

                DocumentReference doc = collection.Document(queryResult.Id);
                await doc.UpdateAsync(data);
                Console.WriteLine("Field deleted");
            }
        }
    }
}
